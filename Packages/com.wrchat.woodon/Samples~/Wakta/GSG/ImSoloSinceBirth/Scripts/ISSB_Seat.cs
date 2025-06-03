using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.ISD.GSG.ImSoloSinceBirth
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class ISSB_Seat : WBase
	{
		[SerializeField] private Button heartButton;
		[SerializeField] private Button brokenHeartButton;

		[SerializeField] private Color enabledColor;
		[SerializeField] private Color disabledColor;

		[SerializeField] private Button submitButton;

		[SerializeField] private WBool heartBool_Local;
		[field: SerializeField] public WBool HeartBool { get; private set; }

		private void Start()
		{
			Init();
			UpdateHeartButtonUI();
		}

		private void Init()
		{
			heartBool_Local.RegisterListener(this, nameof(UpdateHeartButtonUI));
		}

		public void UpdateHeartButtonUI()
		{
			heartButton.image.color = heartBool_Local.Value ? enabledColor : disabledColor;
			brokenHeartButton.image.color = !heartBool_Local.Value ? enabledColor : disabledColor;
		}

		public void UpdateSubmitButtonUI()
		{
			submitButton.interactable = heartBool_Local.Value != HeartBool.Value;
		}

		public void Sumbit()
		{
			HeartBool.SetValue(heartBool_Local.Value);
		}
	}
}