using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static Mascari4615.MUtil;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MTargetUI : MBase
	{
		[Header("_" + nameof(MTargetUI))]
		[SerializeField] private MTarget mTarget;
		[SerializeField] private RectTransform background;
		[SerializeField] private GameObject noneButton;
		[SerializeField] private TextMeshProUGUI[] targetPlayerTexts;
		[SerializeField] private TextMeshProUGUI localPlayerUI;

		[Header("_" + nameof(MTargetUI) + " - Options")]
		[SerializeField] private bool printPlayerID = true;

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

			localPlayerUI.text = $"LocalPlayer ID : {Networking.LocalPlayer.playerId}";

			mTarget.RegisterListener(this, nameof(UpdateUI));
		}

		public void UpdateUI()
		{
			SetNoneButton(mTarget.UseNone);
			UpdateTargetPlayerUI(mTarget.CurTargetPlayerID);
			UpdatePlayerList(GetPlayers());
		}

		private void SetNoneButton(bool active)
		{
			noneButton.SetActive(active);
			background.sizeDelta = new Vector2(background.sizeDelta.x, 400 + (active ? 40 : 0));
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

		private void UpdatePlayerList(VRCPlayerApi[] players)
		{
			for (int i = 0; i < playerSelectButtons.Length; i++)
			{
				if (i >= players.Length)
				{
					playerSelectButtons[i].UpdateUI(NONE_STRING);
					playerSelectButtons[i].gameObject.SetActive(false);
					mTarget.PlayerIDBuffer[i] = -1;
				}
				else
				{
					playerSelectButtons[i].UpdateUI($"{players[i].playerId}\n{players[i].displayName}");
					playerSelectButtons[i].gameObject.SetActive(true);
					mTarget.PlayerIDBuffer[i] = players[i].playerId;
				}
			}
		}

		// ---- ---- ---- ----

		public void SetNone()
		{
			mTarget.SetNone();
		}

		public void SelectPlayer(int index)
		{
			MDebugLog($"{nameof(SelectPlayer)} : {index}");
			mTarget.SelectPlayer(index);
		}
	}
}