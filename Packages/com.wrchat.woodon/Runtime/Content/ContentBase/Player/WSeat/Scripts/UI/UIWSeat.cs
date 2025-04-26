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
		[SerializeField] private WInt contentState; // ContentManager로부터 받아오는 값 입니다. - KarmoDDrine 250427

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

			if (contentState == null)
				return;
			contentState.SetValue(contentManager.ContentState);
		}
	}
}