using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	// TODO: 슬라이더 보정을 IncreaseAmount, DecreaseAmount로 해야할지
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIWIntSlider : WIntFollower
	{
		[Header("_" + nameof(UIWIntSlider))]
		[SerializeField] private Slider slider;
		[SerializeField] private Slider fakeSlider;

		[Header("_" + nameof(UIWIntSlider) + " - Options")]
		[SerializeField] private bool logDetail = false;
		[SerializeField] private WBool isSliderPressed;
		private Animator sliderAnimator;

		private bool forceChange = false;

		private void Start()
		{
			Init();
		}

		public void Init()
		{
			slider.value = 0;
			fakeSlider.value = 0;

			if (isSliderPressed != null)
				sliderAnimator = slider.GetComponent<Animator>();

			wInt.RegisterListener(this, nameof(UpdateSlider));
			UpdateSlider();
		}

		public void OnSliderValueChanged()
		{
			WDebugLog($"{nameof(OnSliderValueChanged)}");

			if (forceChange)
			{
				forceChange = false;
				return;
			}

			int newValue = wInt.MinValue + (int)(slider.value * (wInt.MaxValue - wInt.MinValue));
			if (wInt.Value != newValue)
			{
				if (logDetail)
					WDebugLog($"{nameof(OnSliderValueChanged)} = {newValue}");
				else
					WDebugLog($"{nameof(OnSliderValueChanged)}");

				wInt.SetValue(newValue);
			}
		}

		public void UpdateSlider()
		{
			int curFakeSliderValue = wInt.MinValue + (int)(fakeSlider.value * (wInt.MaxValue - wInt.MinValue));
			if (wInt.Value != curFakeSliderValue)
				fakeSlider.value = (float)(wInt.Value - wInt.MinValue) / (wInt.MaxValue - wInt.MinValue);

			if (IsOwner(wInt.gameObject))
				return;

			int curSliderValue = wInt.MinValue + (int)(slider.value * (wInt.MaxValue - wInt.MinValue));
			if (wInt.Value != curSliderValue)
			{
				forceChange = true;

				if (logDetail)
					WDebugLog($"{nameof(UpdateSlider)} = {wInt.Value}");
				else
					WDebugLog($"{nameof(UpdateSlider)}");

				slider.value = (float)(wInt.Value - wInt.MinValue) / (wInt.MaxValue - wInt.MinValue);
			}
		}

		private void Update()
		{
			if (isSliderPressed != null)
				isSliderPressed.SetValue(sliderAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pressed"));
		}

		public override void SetWInt(WInt wInt)
		{
			WDebugLog($"{nameof(SetWInt)} : {wInt}");

			if (this.wInt != null)
				this.wInt.UnregisterListener(this, nameof(UpdateSlider));

			this.wInt = wInt;
			Init();
		}
	}
}