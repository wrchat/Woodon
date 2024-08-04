using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMColor : MBase
	{
		[Header("_" + nameof(UIMColor))]
		[SerializeField] private MColor mColor;

		[SerializeField] private Image syncedColorImage;
		[SerializeField] private Image previewColorImage;

		[SerializeField] private Slider rSlider;
		[SerializeField] private Slider gSlider;
		[SerializeField] private Slider bSlider;

		[SerializeField] private TextMeshProUGUI rText;
		[SerializeField] private TextMeshProUGUI gText;
		[SerializeField] private TextMeshProUGUI bText;
		
		private void Start()
		{
			Init();
		}

		private void Init()
		{
			mColor.RegisterListener(this, nameof(UpdateSyncedColorUI));
		}

		public void UpdateSyncedColorUI()
		{
			Color curColor = mColor.Value;
			syncedColorImage.color = curColor;

			rSlider.value = curColor.r;
			gSlider.value = curColor.g;
			bSlider.value = curColor.b;

			rText.text = (curColor.r * 255f).ToString();
			gText.text = (curColor.g * 255f).ToString();
			bText.text = (curColor.b * 255f).ToString();
		}

		public void UpdatePreviewColorUI()
		{
			previewColorImage.color = new Color(rSlider.value, gSlider.value, bSlider.value);
		}

		public void SetColor()
		{
			mColor.SetColor(new Color(rSlider.value, gSlider.value, bSlider.value));
		}

		public void SetColorOrigin()
		{
			mColor.SetColor(mColor.OriginColor);
		}
	}
}