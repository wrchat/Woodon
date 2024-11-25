using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMTarget : MTargetFollower
	{
		[Header("_" + nameof(UIMTarget))]
		[SerializeField] private TextMeshProUGUI[] targetPlayerTexts;
		[SerializeField] private GameObject noneButton;
		[SerializeField] private TextMeshProUGUI localPlayerUI;

		[Header("_" + nameof(UIMTarget) + " - Options")]
		[SerializeField] private bool printPlayerID = true;

		private readonly int[] playerIDBuffer = new int[80];
		private UIMTargetPlayerSelectButton[] playerSelectButtons;

		// ---- ---- ---- ----

		private void Start()
		{
			Init();
		}
	
		private void Init()
		{
			playerSelectButtons = transform.GetComponentsInChildren<UIMTargetPlayerSelectButton>(true);
			for (int i = 0; i < playerSelectButtons.Length; i++)
				playerSelectButtons[i].Init(this, i);

			if (localPlayerUI != null)
				localPlayerUI.text = $"LocalPlayer ID : {Networking.LocalPlayer.playerId}";

			if (mTarget != null)
				mTarget.RegisterListener(this, nameof(UpdateUI));

			UpdateUI();
			UpdatePlayerIDBuffer();
		}

		public void UpdateUI()
		{
			if (mTarget == null)
				return;
			
			SetNoneButton(mTarget.UseNone);
			UpdateTargetPlayerUI(mTarget.TargetPlayerID);
		}

		private void SetNoneButton(bool active)
		{
			if (noneButton != null)
				noneButton.SetActive(active);
			// RectTransform rectTransform = GetComponent<RectTransform>();
			// rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 400 + (active ? 40 : 0));
		}

		private void UpdateTargetPlayerUI(int curTargetPlayerID)
		{
			string s = "-";

			if (curTargetPlayerID != NONE_INT)
			{
				VRCPlayerApi targetPlayer = VRCPlayerApi.GetPlayerById(curTargetPlayerID);

				if (targetPlayer != null)
				{
					s = "";
					if (printPlayerID)
						s = $"{curTargetPlayerID} : ";
					s += $"{VRCPlayerApi.GetPlayerById(curTargetPlayerID).displayName}";
				}
			}

			foreach (TextMeshProUGUI targetPlayerText in targetPlayerTexts)
				targetPlayerText.text = s;
		}

		public void UpdatePlayerIDBuffer()
		{
			VRCPlayerApi[] players = MUtil.GetPlayers();

			if (players.Length != VRCPlayerApi.GetPlayerCount())
			{
				SendCustomEventDelayedSeconds(nameof(UpdatePlayerIDBuffer), .3f);
				return;
			}

			for (int i = 0; i < playerSelectButtons.Length; i++)
			{
				if (i >= players.Length)
				{
					playerSelectButtons[i].UpdateUI(NONE_STRING);
					playerSelectButtons[i].gameObject.SetActive(false);
					playerIDBuffer[i] = -1;
				}
				else
				{
					playerSelectButtons[i].UpdateUI($"{players[i].playerId}\n{players[i].displayName}");
					playerSelectButtons[i].gameObject.SetActive(true);
					playerIDBuffer[i] = players[i].playerId;
				}
			}
		}

		public override void OnPlayerJoined(VRCPlayerApi player)
		{
			UpdatePlayerIDBuffer();
		}

		public override void OnPlayerLeft(VRCPlayerApi player)
		{
			UpdatePlayerIDBuffer();
		}

		// ---- ---- ---- ----

		public void SetNone()
		{
			mTarget.SetTargetNone();
		}

		public void SelectPlayer(int index)
		{
			MDebugLog($"{nameof(SelectPlayer)} : {index}");
			mTarget.SetTarget(playerIDBuffer[index]);
		}

		public override void SetMTarget(MTarget mTarget)
		{
			if (this.mTarget != null)
				this.mTarget.UnregisterListener(this, nameof(UpdateUI));

			this.mTarget = mTarget;
			Init();
		}
	}
}