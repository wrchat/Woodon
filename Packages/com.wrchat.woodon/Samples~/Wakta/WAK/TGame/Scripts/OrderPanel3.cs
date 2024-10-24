using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.WAK.TGame
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class OrderPanel3 : MBase
	{
		[field: Header("_" + nameof(OrderPanel3))]
		[SerializeField] private GameObject obejctParent;
		private OrderBox3[] orderBoxes;
		private CanvasGroup canvasGroup;

		[UdonSynced, FieldChangeCallback(nameof(IsShowing))] private bool isShowing;
		public bool IsShowing
		{
			get => isShowing;
			set
			{
				isShowing = value;
				OnShowingChange();
			}
		}

		private void OnShowingChange()
		{
			canvasGroup.alpha = isShowing ? 1 : 0;
			canvasGroup.interactable = isShowing;
			obejctParent.SetActive(isShowing);
		}
		
		public void Init(TGameManager gameManager)
		{
			MSFXManager sfxManager = GetComponentInChildren<MSFXManager>(true);
			orderBoxes = GetComponentsInChildren<OrderBox3>(true);
			for (int i = 0; i < orderBoxes.Length; i++)
				orderBoxes[i].Init(this, gameManager, sfxManager, i);
			canvasGroup = GetComponent<CanvasGroup>();
			OnShowingChange();
		}

		public void UpdateUI()
		{
			foreach (OrderBox3 orderBox in orderBoxes)
				orderBox.UpdateUI();
		}

		public void Show()
		{
			SetOwner();
			IsShowing = true;
			RequestSerialization();
		}

		public void Hide()
		{
			SetOwner();
			IsShowing = false;
			RequestSerialization();

			foreach (OrderBox3 orderBox in orderBoxes)
				orderBox.Hide();
		}
	}
}