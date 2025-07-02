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
		[SerializeField] protected TextMeshProUGUI[] indexTexts;
		[SerializeField] protected UISeatData[] seatDataUIs;
		[SerializeField] protected WInt contentState; // ContentManager로부터 받아오는 값 입니다. - KarmoDDrine 250427

		protected ContentManager contentManager;
		protected WSeat wSeat;

		public virtual void Init(ContentManager contentManager, WSeat wSeat)
		{
			WDebugLog($"{nameof(Init)}: {wSeat.Index}");

			this.contentManager = contentManager;
			this.wSeat = wSeat;
		}

		public virtual void UpdateUI()
		{
			WDebugLog($"{nameof(UpdateUI)}: {wSeat.Index}");

			int index = wSeat.Index;
			foreach (TextMeshProUGUI seatIndexText in indexTexts)
				seatIndexText.text = index.ToString();

			foreach (UISeatData seatDataUI in seatDataUIs)
				seatDataUI.UpdateUI(contentManager, wSeat);

			if (contentState == null)
				return;
			contentState.SetValue(contentManager.ContentState);
		}

		public void SetTargetPlayerLocalPlayer()
			=> wSeat.SetTargetPlayerLocalPlayer();
	}
}