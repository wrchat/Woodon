using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	// TODO: 슬라이버 보정을 IncreaseAmount, DecreaseAmount로 해야할지
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMValueSlider : MValueFollower
	{
		[Header("_" + nameof(UIMValueSlider))]
		[SerializeField] private Slider slider;
		[SerializeField] private Slider fakeSlider;

		[Header("_" + nameof(UIMValueSlider) + " - Options")]
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

			mValue.RegisterListener(this, nameof(UpdateSlider));
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

			int newValue = mValue.MinValue + (int)(slider.value * (mValue.MaxValue - mValue.MinValue));
			if (mValue.Value != newValue)
			{
				if (logDetail)
					WDebugLog($"{nameof(OnSliderValueChanged)} = {newValue}");
				else
					WDebugLog($"{nameof(OnSliderValueChanged)}");

				mValue.SetValue(newValue);
			}
		}

		public void UpdateSlider()
		{
			int curFakeSliderValue = mValue.MinValue + (int)(fakeSlider.value * (mValue.MaxValue - mValue.MinValue));
			if (mValue.Value != curFakeSliderValue)
				fakeSlider.value = (float)(mValue.Value - mValue.MinValue) / (mValue.MaxValue - mValue.MinValue);

			if (IsOwner(mValue.gameObject))
				return;

			int curSliderValue = mValue.MinValue + (int)(slider.value * (mValue.MaxValue - mValue.MinValue));
			if (mValue.Value != curSliderValue)
			{
				forceChange = true;

				if (logDetail)
					WDebugLog($"{nameof(UpdateSlider)} = {mValue.Value}");
				else
					WDebugLog($"{nameof(UpdateSlider)}");

				slider.value = (float)(mValue.Value - mValue.MinValue) / (mValue.MaxValue - mValue.MinValue);
			}
		}

		private void Update()
		{
			if (isSliderPressed != null)
				isSliderPressed.SetValue(sliderAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pressed"));
		}

		public override void SetMValue(MValue mValue)
		{
			WDebugLog($"{nameof(SetMValue)} : {mValue}");

			if (this.mValue != null)
				this.mValue.UnregisterListener(this, nameof(UpdateSlider));

			this.mValue = mValue;
			Init();
		}
	}
}