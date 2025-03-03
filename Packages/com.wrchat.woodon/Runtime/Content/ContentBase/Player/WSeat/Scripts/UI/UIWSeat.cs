using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIWSeat : WBase
	{
		[Header("_" + nameof(UIWSeat))]
		[SerializeField] private TextMeshProUGUI[] indexTexts;
		[SerializeField] private UISeatData[] seatDataUIs;

		private ContentManager contentManager;
		private WSeat wSeat;

		public void Init(ContentManager contentManager, WSeat mSeat)
		{
			this.contentManager = contentManager;
			this.wSeat = mSeat;
		}

		public void UpdateUI()
		{
			int index = wSeat.Index;
			foreach (TextMeshProUGUI seatIndexText in indexTexts)
				seatIndexText.text = index.ToString();

			foreach (UISeatData seatDataUI in seatDataUIs)
				seatDataUI.UpdateUI(contentManager, wSeat);
		}
	}
}