using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using WRC.Woodon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class Flag : MBase
	{
		[Header("_" + nameof(Flag))]
		[SerializeField]
		private ChickenFightManager chickenFightManager;

		[SerializeField] private MTargetTeamManager teamManager;
		[SerializeField] private Timer shieldDuration;
		[SerializeField] private Timer shieldCoolTime;
		[SerializeField] private MeshRenderer shieldMesh;

		[SerializeField] private GameObject uiObject;
		[SerializeField] private TextMeshProUGUI curStateText;
		[SerializeField] private Image timeBar;
		[SerializeField] private float lerpValue = 5;

		[Header("_TargetPlayer")]
		// [SerializeField] private MPlayerSync mPlayerSync;
		[SerializeField]
		private MTarget mTarget;

		[SerializeField] private MTarget mTargetHorse;
		// [SerializeField] private bool useMTarget;

		private bool IsLocalPlayerOwner =>
			(chickenFightManager.IsCurGame) &&
			// (useMTarget ?
			(mTarget.TargetPlayerID == Networking.LocalPlayer.playerId);
		// : (mPlayerSync.PlayerID == Networking.LocalPlayer.playerId));

		private void Update()
		{
			shieldMesh.enabled = shieldDuration.ExpireTime != NONE_INT;

			UpdateUI();

			if (Input.GetKeyDown(KeyCode.E))
				if (!Networking.LocalPlayer.IsUserInVR())
					TryUseShield();
		}

		private void UpdateUI()
		{
			if (IsLocalPlayerOwner)
			{
				if (!uiObject.activeSelf)
					uiObject.SetActive(true);

				if (shieldDuration.ExpireTime != NONE_INT)
				{
					curStateText.text = "방어 모드 !";
					timeBar.color = Color.blue;

					float targetAmount =
						(float)((shieldDuration.ExpireTime - Networking.GetServerTimeInMilliseconds()) / 100) /
						shieldDuration.TimeByDecisecond;
					timeBar.fillAmount = Mathf.Lerp(timeBar.fillAmount, targetAmount, Time.deltaTime * lerpValue);
				}
				else if (shieldCoolTime.ExpireTime != NONE_INT)
				{
					curStateText.text = "쿨타임...";
					timeBar.color = Color.yellow;
					float targetAmount =
						(float)((shieldCoolTime.ExpireTime - Networking.GetServerTimeInMilliseconds()) / 100) /
						(shieldCoolTime.TimeByDecisecond - shieldDuration.TimeByDecisecond);
					timeBar.fillAmount = Mathf.Lerp(timeBar.fillAmount, targetAmount, Time.deltaTime * lerpValue);
				}
				else
				{
					curStateText.text = "방어 모드 사용 가능";
					timeBar.color = Color.green;
					timeBar.fillAmount = 1;
				}
			}
			else
			{
				if (uiObject.activeSelf)
					uiObject.SetActive(false);
			}
		}


		public void TryUseShield()
		{
			MDebugLog(nameof(TryUseShield));

			if (IsLocalPlayerOwner)
				if (shieldCoolTime.ExpireTime == NONE_INT)
				{
					shieldDuration.StartTimer();
					shieldCoolTime.StartTimer();
				}
		}

		public override void Interact()
		{
			Touch();
		}

		// 오너가 아닌 사람이 아웃 시키는 것
		public void Touch()
		{
			MDebugLog(nameof(Touch));

			if (IsLocalPlayerOwner)
				return;

			if (mTarget.TargetPlayerID == NONE_INT)
				return;

			TeamType localPlayerTeamType = teamManager.GetTargetPlayerTeamType();
			// var ownerPlayerTeamType = teamManager.GetTargetPlayerTeamType(mPlayerSync.playerAPI);
			TeamType ownerPlayerTeamType = teamManager.GetTargetPlayerTeamType();

			if (localPlayerTeamType == TeamType.None)
				return;

			if (localPlayerTeamType == ownerPlayerTeamType)
				return;

			if (shieldDuration.ExpireTime != NONE_INT)
				return;

			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Out));
		}

		// 오너가 아웃 당하는 것
		public void Out()
		{
			MDebugLog(nameof(Out));

			if (IsLocalPlayerOwner || (mTargetHorse.TargetPlayerID == Networking.LocalPlayer.playerId))
			{
				Networking.LocalPlayer.TeleportTo(chickenFightManager.respawnPos.position,
					chickenFightManager.respawnPos.rotation);
				// mPlayerSync.ClearPlayer();

				// TDOO : 탈락 위치
				// TODO : 싱크에서 제외 시킬 것인가?
			}
		}

		// CHECK : VR 시야 괜찮은지? (모자가 가리는지, 말 위에 탔을 때 어지러운지 등)
		// CHECK : 기마병끼리 서로 터치할 수 있는지
	}
}