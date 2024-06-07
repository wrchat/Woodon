using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MTargetUI : MBase
	{
		[Header("_" + nameof(MTargetUI))]
		[SerializeField] private RectTransform background;
		[SerializeField] private GameObject noneButton;
		[SerializeField] private TextMeshProUGUI targetPlayerUI;
		[SerializeField] private TextMeshProUGUI localPlayerUI;

		[SerializeField] private Transform buttonsParent;
		[SerializeField] private bool printPlayerID = true;

		private MTarget mTarget;
		private GameObject[] playerSelectButtons;
		private TextMeshProUGUI[] playerSelectButtonTexts;

		private void Start()
		{
			var childCount = buttonsParent.childCount;
			playerSelectButtons = new GameObject[childCount];
			playerSelectButtonTexts = new TextMeshProUGUI[childCount];

			for (var i = 0; i < buttonsParent.childCount; i++)
			{
				playerSelectButtons[i] = buttonsParent.GetChild(i).gameObject;
				playerSelectButtonTexts[i] = buttonsParent.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>();
			}

			localPlayerUI.text = $"LocalPlayer ID : {Networking.LocalPlayer.playerId}";
		}

		public void SetMTarget(MTarget mTarget)
		{
			this.mTarget = mTarget;
		}

		public void SetNoneButton(bool active)
		{
			noneButton.SetActive(active);
			background.sizeDelta = new Vector2(background.sizeDelta.x, 400 + (active ? 40 : 0));
		}

		public void UpdateTargetPlayerUI(int curTargetPlayerID)
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

			targetPlayerUI.text = s;
		}

		public void UpdatePlayerList()
		{
			var players = Players;

			if (players.Length != VRCPlayerApi.GetPlayerCount())
			{
				SendCustomEventDelayedSeconds(nameof(UpdatePlayerList), .3f);
				return;
			}

			for (var i = 0; i < playerSelectButtons.Length; i++)
				if (i >= players.Length)
				{
					playerSelectButtons[i].SetActive(false);
					mTarget.PlayerIDBuffer[i] = -1;
					playerSelectButtonTexts[i].text = NONE_STRING;
				}
				else
				{
					playerSelectButtons[i].SetActive(true);
					mTarget.PlayerIDBuffer[i] = players[i].playerId;
					playerSelectButtonTexts[i].text = $"{players[i].playerId}\n{players[i].displayName}";
				}
		}

		public void SetNone()
		{
			mTarget.SetNone();
		}

		public void SelectPlayer(int index)
		{
			mTarget.SelectPlayer(index);
		}

		#region SelectPlayer

		public void SelectPlayer1()
		{
			SelectPlayer(1);
		}

		public void SelectPlayer2()
		{
			SelectPlayer(2);
		}

		public void SelectPlayer3()
		{
			SelectPlayer(3);
		}

		public void SelectPlayer4()
		{
			SelectPlayer(4);
		}

		public void SelectPlayer5()
		{
			SelectPlayer(5);
		}

		public void SelectPlayer6()
		{
			SelectPlayer(6);
		}

		public void SelectPlayer7()
		{
			SelectPlayer(7);
		}

		public void SelectPlayer8()
		{
			SelectPlayer(8);
		}

		public void SelectPlayer9()
		{
			SelectPlayer(9);
		}

		public void SelectPlayer10()
		{
			SelectPlayer(10);
		}

		public void SelectPlayer11()
		{
			SelectPlayer(11);
		}

		public void SelectPlayer12()
		{
			SelectPlayer(12);
		}

		public void SelectPlayer13()
		{
			SelectPlayer(13);
		}

		public void SelectPlayer14()
		{
			SelectPlayer(14);
		}

		public void SelectPlayer15()
		{
			SelectPlayer(15);
		}

		public void SelectPlayer16()
		{
			SelectPlayer(16);
		}

		public void SelectPlayer17()
		{
			SelectPlayer(17);
		}

		public void SelectPlayer18()
		{
			SelectPlayer(18);
		}

		public void SelectPlayer19()
		{
			SelectPlayer(19);
		}

		public void SelectPlayer20()
		{
			SelectPlayer(20);
		}

		public void SelectPlayer21()
		{
			SelectPlayer(21);
		}

		public void SelectPlayer22()
		{
			SelectPlayer(22);
		}

		public void SelectPlayer23()
		{
			SelectPlayer(23);
		}

		public void SelectPlayer24()
		{
			SelectPlayer(24);
		}

		public void SelectPlayer25()
		{
			SelectPlayer(25);
		}

		public void SelectPlayer26()
		{
			SelectPlayer(26);
		}

		public void SelectPlayer27()
		{
			SelectPlayer(27);
		}

		public void SelectPlayer28()
		{
			SelectPlayer(28);
		}

		public void SelectPlayer29()
		{
			SelectPlayer(29);
		}

		public void SelectPlayer30()
		{
			SelectPlayer(30);
		}

		public void SelectPlayer31()
		{
			SelectPlayer(31);
		}

		public void SelectPlayer32()
		{
			SelectPlayer(32);
		}

		public void SelectPlayer33()
		{
			SelectPlayer(33);
		}

		public void SelectPlayer34()
		{
			SelectPlayer(34);
		}

		public void SelectPlayer35()
		{
			SelectPlayer(35);
		}

		public void SelectPlayer36()
		{
			SelectPlayer(36);
		}

		public void SelectPlayer37()
		{
			SelectPlayer(37);
		}

		public void SelectPlayer38()
		{
			SelectPlayer(38);
		}

		public void SelectPlayer39()
		{
			SelectPlayer(39);
		}

		public void SelectPlayer40()
		{
			SelectPlayer(40);
		}

		public void SelectPlayer41()
		{
			SelectPlayer(41);
		}

		public void SelectPlayer42()
		{
			SelectPlayer(42);
		}

		public void SelectPlayer43()
		{
			SelectPlayer(43);
		}

		public void SelectPlayer44()
		{
			SelectPlayer(44);
		}

		public void SelectPlayer45()
		{
			SelectPlayer(45);
		}

		public void SelectPlayer46()
		{
			SelectPlayer(46);
		}

		public void SelectPlayer47()
		{
			SelectPlayer(47);
		}

		public void SelectPlayer48()
		{
			SelectPlayer(48);
		}

		public void SelectPlayer49()
		{
			SelectPlayer(49);
		}

		public void SelectPlayer50()
		{
			SelectPlayer(50);
		}

		public void SelectPlayer51()
		{
			SelectPlayer(51);
		}

		public void SelectPlayer52()
		{
			SelectPlayer(52);
		}

		public void SelectPlayer53()
		{
			SelectPlayer(53);
		}

		public void SelectPlayer54()
		{
			SelectPlayer(54);
		}

		public void SelectPlayer55()
		{
			SelectPlayer(55);
		}

		public void SelectPlayer56()
		{
			SelectPlayer(56);
		}

		public void SelectPlayer57()
		{
			SelectPlayer(57);
		}

		public void SelectPlayer58()
		{
			SelectPlayer(58);
		}

		public void SelectPlayer59()
		{
			SelectPlayer(59);
		}

		public void SelectPlayer60()
		{
			SelectPlayer(60);
		}

		public void SelectPlayer61()
		{
			SelectPlayer(61);
		}

		public void SelectPlayer62()
		{
			SelectPlayer(62);
		}

		public void SelectPlayer63()
		{
			SelectPlayer(63);
		}

		public void SelectPlayer64()
		{
			SelectPlayer(64);
		}

		public void SelectPlayer65()
		{
			SelectPlayer(65);
		}

		public void SelectPlayer66()
		{
			SelectPlayer(66);
		}

		public void SelectPlayer67()
		{
			SelectPlayer(67);
		}

		public void SelectPlayer68()
		{
			SelectPlayer(68);
		}

		public void SelectPlayer69()
		{
			SelectPlayer(69);
		}

		public void SelectPlayer70()
		{
			SelectPlayer(70);
		}

		public void SelectPlayer71()
		{
			SelectPlayer(71);
		}

		public void SelectPlayer72()
		{
			SelectPlayer(72);
		}

		public void SelectPlayer73()
		{
			SelectPlayer(73);
		}

		public void SelectPlayer74()
		{
			SelectPlayer(74);
		}

		public void SelectPlayer75()
		{
			SelectPlayer(75);
		}

		public void SelectPlayer76()
		{
			SelectPlayer(76);
		}

		public void SelectPlayer77()
		{
			SelectPlayer(77);
		}

		public void SelectPlayer78()
		{
			SelectPlayer(78);
		}

		public void SelectPlayer79()
		{
			SelectPlayer(79);
		}

		public void SelectPlayer80()
		{
			SelectPlayer(80);
		}

		#endregion
	}
}