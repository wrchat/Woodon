using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using WRC.Woodon;

namespace Mascari4615.Project.RusukBar
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class RB3_UICanvas : UdonSharpBehaviour
	{
		[Header("_" + nameof(RB3_UICanvas))]
		[SerializeField] private bool isOverlayCanvas;
		private UISettingButton[] settingButtons;

		[SerializeField] private Transform[] uiPositions;

		private RB3_GameManager gameManager;

		public void Init()
		{
			gameManager = GameObject.Find("GameManager").GetComponent<RB3_GameManager>();
		
			settingButtons = GetComponentsInChildren<UISettingButton>(true);
			foreach (UISettingButton uiSettingButton in settingButtons)
				uiSettingButton.Init(this);
		}

		public void ResetAllPos()
		{
			gameManager.ResetAllPos();
		}

		public void ResetCocktailPos()
		{
			gameManager.ResetCocktailPos();
		}

		public void ResetPizzaPos()
		{
			gameManager.ResetPizzaPos();
		}

		public void ResetChessPos()
		{
			gameManager.ResetChessPos();
		}

		public void ResetPens_Global()
		{
			gameManager.ResetPens_Global();
		}

		public void ResetPens()
		{
			gameManager.ResetPens();
		}

		public void ClearPens_Global()
		{
			gameManager.ClearPens_Global();
		}

		public void ClearPens()
		{
			gameManager.ClearPens();
		}

		public void ToggleObject(ToggleType objectType)
		{
			gameManager.ToggleObject(objectType);
		}

		public void UpdateToggleUI(bool[] bools)
		{
			foreach (UISettingButton uiSettingButton in settingButtons)
				uiSettingButton.UpdateUI(bools[(int)uiSettingButton.ToggleObjectType]);
		}

		private void Update()
		{
			if (MUtil.IsNotOnline())
				return;

			UpdateUIPos();
		}

		private void UpdateUIPos()
		{
			if (uiPositions == null || uiPositions.Length == 0)
				return;

			int nearestPosIndex = 0;

			Vector3 localPlayerPosition = Networking.LocalPlayer.GetPosition();
			float minDistance = float.MaxValue;

			for (int i = 0; i < uiPositions.Length; i++)
			{
				float distance = Vector3.Distance(uiPositions[i].position, localPlayerPosition);
				if (distance < minDistance)
				{
					minDistance = distance;
					nearestPosIndex = i;
				}
			}

			transform.position = uiPositions[nearestPosIndex].position;
			transform.rotation = uiPositions[nearestPosIndex].rotation;
		}
	}
}