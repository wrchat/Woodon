using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMSeat : WBase
	{
		[Header("_" + nameof(UIMSeat))]
		[SerializeField] private TextMeshProUGUI[] indexTexts;
		[SerializeField] private UISeatData[] seatDataUIs;

		private ContentManager contentManager;
		private MSeat mSeat;

		public void Init(ContentManager contentManager, MSeat mSeat)
		{
			this.contentManager = contentManager;
			this.mSeat = mSeat;
		}

		public void UpdateUI()
		{
			int index = mSeat.Index;
			foreach (TextMeshProUGUI seatIndexText in indexTexts)
				seatIndexText.text = index.ToString();

			foreach (UISeatData seatDataUI in seatDataUIs)
				seatDataUI.UpdateUI(contentManager, mSeat);
		}
	}
}