using TMPro;
using UdonSharp;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.WAK.TGame
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class TGameOverlayUI : MBase
	{
		private LeaderBoardPanel leaderBoardPanel;
		[SerializeField] private TextMeshProUGUI curRoundText;
		[SerializeField] private GameObject playerOverlayUI;
		[SerializeField] private GameObject timeBlock;
		[SerializeField] private Animator killLog;
		[SerializeField] private TextMeshProUGUI killLogText;
		[SerializeField] private Image killLogImage;
		[SerializeField] private TextMeshProUGUI timeText;
		[field: SerializeField] public InfoPopupModule InfoPopupModule { get; set; }

		[SerializeField] private Color red;
		private TGameManager gameManager;

		public void Init(TGameManager gameManager)
		{
			this.gameManager = gameManager;
			leaderBoardPanel = GetComponentInChildren<LeaderBoardPanel>(true);
			leaderBoardPanel.Init(gameManager);
		}

		public void UpdateUI()
		{
			curRoundText.text = $"{gameManager.Data.CurRound + 1} 라운드";
		}

		public void UpdateLeaderBoard()
		{
			leaderBoardPanel.UpdateUI();
		}

		public void KillLog(int playerNumber)
		{
			killLogText.text = gameManager.GetPlayerName(playerNumber, false);
			killLogImage.sprite = gameManager.GetPlayerImage(playerNumber);
			killLog.SetTrigger("POP");
		}

		public void UpdateTimeText(float time)
		{
			// timeText.text = t <= 0 ? "-" : Mathf.CeilToInt(t + 1).ToString();
			timeBlock.SetActive(time > 0);
			timeText.text = ((int)time).ToString(); // t.ToString("F1");
			timeText.color = MColorUtil.GetColorByBool(time <= 5, red, MColorPreset.White);
		}
	}
}