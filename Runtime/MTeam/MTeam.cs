using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MTeam : MEventSender
	{
		[Header("_" + nameof(MTeam))]
		public TeamType TeamType;
		public MPlayerSync[] mPlayerSyncs;

		[SerializeField] private MTeamManager mTeamManager;

		public void PlayerChanged(int index)
		{
			MDebugLog($"{nameof(PlayerChanged)} : Index = {index}");

			SendEvents();
			mTeamManager.PlayerChanged(TeamType, index, mPlayerSyncs[index].PlayerID);
		}

		public bool IsTargetPlayerTeam(VRCPlayerApi targetPlayer = null)
		{
			if (targetPlayer == null)
				targetPlayer = Networking.LocalPlayer;

			foreach (var mPlayerSync in mPlayerSyncs)
				if (mPlayerSync.PlayerID == targetPlayer.playerId)
					return true;

			return false;
		}

		public int GetTargetPlayerIndex(VRCPlayerApi targetPlayer = null)
		{
			if (targetPlayer == null)
				targetPlayer = Networking.LocalPlayer;

			for (var i = 0; i < mPlayerSyncs.Length; i++)
				if (mPlayerSyncs[i].PlayerID == targetPlayer.playerId)
					return i;

			return NONE_INT;
		}

		public void PlayerChanged0()
		{
			PlayerChanged(0);
		}

		public void PlayerChanged1()
		{
			PlayerChanged(1);
		}

		public void PlayerChanged2()
		{
			PlayerChanged(2);
		}

		public void PlayerChanged3()
		{
			PlayerChanged(3);
		}

		public void PlayerChanged4()
		{
			PlayerChanged(4);
		}

		public void PlayerChanged5()
		{
			PlayerChanged(5);
		}

		public void PlayerChanged6()
		{
			PlayerChanged(6);
		}

		public void PlayerChanged7()
		{
			PlayerChanged(7);
		}

		public void PlayerChanged8()
		{
			PlayerChanged(8);
		}

		public void PlayerChanged9()
		{
			PlayerChanged(9);
		}
	}
}