using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MValueSlider : MBase
	{
		[Header("_" + nameof(MValueSlider))]
		[SerializeField] private MValue mValue;
		[SerializeField] private Slider slider;
		[SerializeField] private Slider fakeSlider;

		[Header("_" + nameof(MValueSlider) + " - Options")]
		[SerializeField] private bool logDetail = false;
		[SerializeField] private CustomBool isSliderPressed;
		private Animator sliderAnimator;

		bool forceChange = false;

		private void Start() => Init();

		public void Init()
		{
			mValue.RegisterListener(this, nameof(UpdateSlider));
			
			slider.value = 0;
			fakeSlider.value = 0;

			if (isSliderPressed != null)
				sliderAnimator = slider.GetComponent<Animator>();
		}

		public void OnSliderValueChanged()
		{
			MDebugLog($"{nameof(OnSliderValueChanged)}");

			if (forceChange)
			{
				forceChange = false;
				return;
			}

			int newValue = (int)(slider.value * mValue.MaxValue);
			if (mValue.Value != newValue)
			{
				if (logDetail)
					MDebugLog($"{nameof(OnSliderValueChanged)} = {newValue}");
				else
					MDebugLog($"{nameof(OnSliderValueChanged)}");

				mValue.SetValue(newValue);
			}
		}

		public void UpdateSlider()
		{
			int curFakeSliderValue = (int)(fakeSlider.value * mValue.MaxValue);
			if (mValue.Value != curFakeSliderValue)
				fakeSlider.value = (float)mValue.Value / mValue.MaxValue;

			if (IsOwner(mValue.gameObject))
				return;

			int curSliderValue = (int)(slider.value * mValue.MaxValue);
			if (mValue.Value != curSliderValue)
			{
				forceChange = true;

				if (logDetail)
					MDebugLog($"{nameof(UpdateSlider)} = {mValue.Value}");
				else
					MDebugLog($"{nameof(UpdateSlider)}");

				slider.value = (float)mValue.Value / mValue.MaxValue;
			}
		}

		private void Update()
		{
			if (isSliderPressed != null)
				isSliderPressed.SetValue(sliderAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pressed"));
		}
	}
}