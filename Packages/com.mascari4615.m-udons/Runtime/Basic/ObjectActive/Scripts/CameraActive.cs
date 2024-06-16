using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class CameraActive : MBase
	{
		[SerializeField] private Camera[] cameras;
		[SerializeField] private Image[] buttonUIImages;
		[SerializeField] private TextMeshProUGUI[] buttonUITexts;
		[SerializeField] private bool defaultActive;
		[SerializeField] private CustomBool customBool;

		private bool active;

		public bool Active
		{
			get => active;
			private set
			{
				active = value;
				OnActiveChange();
			}
		}

		private void Start()
		{
			if (customBool == null)
			{
				Active = defaultActive;
			}
			else
			{

			}

			OnActiveChange();
		}

		public void SetActive(bool targetActive)
		{
			MDebugLog($"{nameof(SetActive)}({targetActive})");

			Active = targetActive;
		}

		[ContextMenu(nameof(ToggleActive))]
		public void ToggleActive() => SetActive(!Active);
	
		[ContextMenu(nameof(SetActiveTrue))]
		public void SetActiveTrue() => SetActive(true);
		
		[ContextMenu(nameof(SetActiveFalse))]
		public void SetActiveFalse() => SetActive(false);

		[ContextMenu(nameof(UpdateValue))]
		public void UpdateValue()
		{
			if (customBool)
				SetActive(customBool.Value);
		}

		private void OnActiveChange()
		{
			MDebugLog($"{nameof(OnActiveChange)}");

			foreach (Image i in buttonUIImages)
				i.color = MColorUtil.GetGreenOrRed(Active);

			foreach (TextMeshProUGUI t in buttonUITexts)
				t.color = MColorUtil.GetGreenOrRed(Active);

			foreach (Camera camera in cameras)
				camera.enabled = Active;
		}
	}
}