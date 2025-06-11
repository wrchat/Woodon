using TMPro;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIContentController : UIContent
	{
		[Header("_" + nameof(UIContentController))]
		[SerializeField] private TextMeshProUGUI curStateText;
		[SerializeField] private string[] stateToStringOverride;

		public virtual void PrevState()
		{
			contentManager.SetContentStatePrev();
		}

		public virtual void NextState()
		{
			contentManager.SetContentStateNext();
		}

		public override void UpdateUI()
		{
			WDebugLog($"{nameof(UpdateUI)}");

			// Override가 불가능하다면
			if ((contentManager.ContentState < 0) || (contentManager.ContentState >= stateToStringOverride.Length))
			{
				curStateText.text = contentManager.GetContentStateString();
			}
			else
			{
				curStateText.text = stateToStringOverride[contentManager.ContentState];
			}
		}

		#region HorribleEvents
		public void SetState(int state)
		{
			contentManager.SetContentState(state);
		}
		public void SetState0() => SetState(0);
		public void SetState1() => SetState(1);
		public void SetState2() => SetState(2);
		public void SetState3() => SetState(3);
		public void SetState4() => SetState(4);
		public void SetState5() => SetState(5);
		public void SetState6() => SetState(6);
		public void SetState7() => SetState(7);
		public void SetState8() => SetState(8);
		public void SetState9() => SetState(9);
		public void SetState10() => SetState(10);
		#endregion
	}
}