using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace Mascari4615
{
	public class SyncedSlider : MBase
	{
		[Header("_" + nameof(SyncedSlider))]
		[SerializeField] private Slider slider;

		// [SerializeField] private Transform seat;
		[SerializeField] private TextMeshProUGUI valueText;
		[SerializeField] private TextMeshProUGUI ownerText;

		[SerializeField] private float minValue = 0;
		[SerializeField] private float maxValue = 1;
		private float diff;

		[field: UdonSynced(UdonSyncMode.Smooth)]
		public float CurValue { get; private set; } = 0;

		public float CalcValue => minValue + (diff * CurValue);

		private void Start()
		{
			diff = maxValue - minValue;

			if (Networking.IsMaster)
			{
				// originValue = seat.transform.localPosition.y;
				CurValue = 0;
				slider.value = CurValue;
			}
		}

		private void Update()
		{
			if (IsOwner()) CurValue = slider.value;

			slider.value = CurValue;

			var owner = Networking.GetOwner(gameObject);
			ownerText.text = $"{owner.playerId} : {owner.displayName}";

			// Vector3 newPos = seat.transform.localPosition;
			// newPos.y = originValue + ((slider.value - .5f) * 10f);
			// seat.transform.localPosition = newPos;
			// valueText.text = newPos.y.ToString();

			valueText.text = CalcValue.ToString();
		}

		public void _SetOwner()
		{
			SetOwner();
		}
	}
}