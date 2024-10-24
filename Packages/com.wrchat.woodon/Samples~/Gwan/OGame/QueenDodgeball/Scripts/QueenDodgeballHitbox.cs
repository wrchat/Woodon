using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using WRC.Woodon;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class QueenDodgeballHitbox : MBase
    {
        [Header("_" + nameof(QueenDodgeballHitbox))]
        private const int QUEEN = 0;
        private const int GUARD = 1;

        [SerializeField] private TeamType teamType;
        [SerializeField] private int hitPlayerIndex;
        
        [SerializeField] private QueenDodgeballManager queenDodgeballManager;
        [SerializeField] private GameObject ball;
        
        [Header("_TargetPlayer")]
        // [SerializeField] private MPlayerSync mPlayerSync;
        [SerializeField] private MTargetTeamManager teamManager;
        [SerializeField] private MTarget mTarget;
        // [SerializeField] private bool useMTarget;

        private bool IsLocalPlayerOwner =>
            (queenDodgeballManager.IsCurGame) &&
            // (useMTarget ?
            (mTarget.TargetPlayerID == Networking.LocalPlayer.playerId);
             //   : (mPlayerSync.PlayerID == Networking.LocalPlayer.playerId));

        private bool IsLocalPlayerBallOwner => IsOwner(ball);

        public void CalcHit()
        {
            MDebugLog($"{nameof(CalcHit)} : {nameof(teamType)} = {teamType}, {nameof(hitPlayerIndex)} = {hitPlayerIndex}");

            if (!IsLocalPlayerBallOwner)
            {
                MDebugLog("IsBallOwner == false");
                return;
            }

			TeamType localPlayerTeamType = teamManager.GetTargetPlayerTeamType();                                    
            MDebugLog($"{localPlayerTeamType}");
            if (localPlayerTeamType == teamType || localPlayerTeamType == TeamType.None)
            {
                MDebugLog($"Invalid TeamType {localPlayerTeamType}");
                return;
            }

			int localPlayerTeamIndex = teamManager.MTargetTeams[(int)localPlayerTeamType].GetTargetPlayerIndex();

            // 필터링
            {
                // 여왕이 공 던진 경우
                if (localPlayerTeamIndex == QUEEN)
                {
                }
                // 일반, 수호무사가 공 던진 경우
                else // if (localPlayerTeamIndex == GUARD)
                {
                    // 수호모사는 안죽음 ㄷㄷ (여왕 공에만 쥬금)
                    if (hitPlayerIndex == GUARD)
                    {
						int normalCount = 0;
                        for (int i = 2; i < teamManager.MTargetTeams[(int)teamType].mTargets.Length; i++)
                            if (teamManager.MTargetTeams[(int)teamType].mTargets[hitPlayerIndex].TargetPlayerID !=
                                NONE_INT)
                                normalCount++;

                        if (normalCount != 0)
                            return;
                    }
                }
            }

            MDebugLog("Send Out");
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Out));
        }

        // 맞은 사람 아웃시키기
        public void Out()
        {
            MDebugLog(nameof(Out));

            if (!IsLocalPlayerOwner)
                return;

			TeamType localPlayerTeamType = teamManager.GetTargetPlayerTeamType();

            Transform respawnPos = localPlayerTeamType == TeamType.A
                ? queenDodgeballManager.aRespawn
                : queenDodgeballManager.bRespawn;
            
            Networking.LocalPlayer.TeleportTo(respawnPos.position, respawnPos.rotation);
            // mPlayerSync.ClearPlayer();
            
            // TDOO : 탈락 위치
            // TODO : 싱크에서 제외 시킬 것인가?
            // TODO : 파티클 (등의 효과)
        }
    }
}