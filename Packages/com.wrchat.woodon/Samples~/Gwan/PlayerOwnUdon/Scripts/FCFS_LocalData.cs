using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using WRC.Woodon;
using static WRC.Woodon.MUtil;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class FCFS_LocalData : MBase
	{
		[UdonSynced]
		[FieldChangeCallback(nameof(LocalData))]
		private string localData = NONE_STRING;

		public PlayerOwnUdon PlayerOwnUdon { get; private set; }

		public string LocalData
		{
			get => localData;
			set
			{
				localData = value;
				OnLocalDataChange();
			}
		}

		public int MyTimeByMilliseconds { get; private set; } = NONE_INT;

		public string SubData { get; private set; } = NONE_STRING;

		private void Start()
		{
			PlayerOwnUdon = transform.parent.GetComponent<PlayerOwnUdon>();
		}

		private void OnLocalDataChange()
		{
			if (LocalData == NONE_STRING)
			{
				MyTimeByMilliseconds = NONE_INT;
				SubData = NONE_STRING;
			}
			else
			{
				string[] s = LocalData.Split(new[] { DATA_SEPARATOR }, StringSplitOptions.None);

				MyTimeByMilliseconds = int.Parse(s[0]);
				SubData = s[1];
			}

			Debug.Log(
				$"{gameObject.name} : {nameof(OnLocalDataChange)}, {nameof(MyTimeByMilliseconds)} = {MyTimeByMilliseconds}");
		}

		public void RecordCurTime()
		{
			RecordCurTime_();
		}

		public void RecordCurTimeOnce()
		{
			RecordCurTime_(true);
		}

		public void RecordCurTime_(bool recordOnce = false, string subData = NONE_STRING)
		{
			Debug.Log(
				$"{gameObject.name} : {nameof(RecordCurTime_)}, {nameof(recordOnce)} = {recordOnce}, {nameof(subData)} = {subData}");

			if (PlayerOwnUdon.OwnerID != Networking.LocalPlayer.playerId)
				return;

			if (recordOnce && MyTimeByMilliseconds != NONE_INT)
				return;

			SetOwner();
			LocalData = $"{Networking.GetServerTimeInMilliseconds()}{DATA_SEPARATOR}{subData}";
			RequestSerialization();
		}

		public void ClearTime()
		{
			if (PlayerOwnUdon.OwnerID != Networking.LocalPlayer.playerId)
				return;

			SetOwner();
			LocalData = NONE_STRING;
			RequestSerialization();
		}

		public void ForceClearTime()
		{
			SetOwner();
			LocalData = NONE_STRING;
			RequestSerialization();
		}

		public override void OnPlayerLeft(VRCPlayerApi player)
		{
			if (IsLocalPlayer(player)) ClearTime();
		}
	}
}