using UnityEngine.UI;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.RusukBar
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UISettingButton : MBase
	{
		[field: SerializeField] public ToggleType ToggleObjectType { get; private set; }
		[SerializeField] private Image enableState;

		private RB3_UICanvas canvasManager;

		public void Init(RB3_UICanvas canvasManager)
		{
			this.canvasManager = canvasManager;
		}

		public void UpdateUI(bool active)
		{
			enableState.color = MColorUtil.GetGreenOrRed(active);
		}

		public void Click()
		{
			canvasManager.ToggleObject(ToggleObjectType);
		}
	}
}