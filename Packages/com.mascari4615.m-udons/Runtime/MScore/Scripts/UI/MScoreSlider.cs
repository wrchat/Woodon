using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MScoreSlider : MBase
	{
		[Header("_" + nameof(MScoreSlider))]
		[SerializeField] private MScore mScore;
		[SerializeField] private Slider slider;
		[SerializeField] private Slider fakeSlider;

		[Header("_" + nameof(MScoreSlider) + " - Options")]
		[SerializeField] private bool logDetail = false;
		[SerializeField] private CustomBool isSliderPressed;
		private Animator sliderAnimator;

		bool forceChange = false;

		private void Start() => Init();

		public void Init()
		{
			mScore.RegisterListener(this, nameof(UpdateSlider));
			
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

			int newValue = (int)(slider.value * mScore.MaxScore);
			if (mScore.Value != newValue)
			{
				if (logDetail)
					MDebugLog($"{nameof(OnSliderValueChanged)} = {newValue}");
				else
					MDebugLog($"{nameof(OnSliderValueChanged)}");

				mScore.SetValue(newValue);
			}
		}

		public void UpdateSlider()
		{
			int curFakeSliderValue = (int)(fakeSlider.value * mScore.MaxScore);
			if (mScore.Value != curFakeSliderValue)
				fakeSlider.value = (float)mScore.Value / mScore.MaxScore;

			if (IsOwner(mScore.gameObject))
				return;

			int curSliderValue = (int)(slider.value * mScore.MaxScore);
			if (mScore.Value != curSliderValue)
			{
				forceChange = true;

				if (logDetail)
					MDebugLog($"{nameof(UpdateSlider)} = {mScore.Value}");
				else
					MDebugLog($"{nameof(UpdateSlider)}");

				slider.value = (float)mScore.Value / mScore.MaxScore;
			}
		}

		private void Update()
		{
			if (isSliderPressed != null)
				isSliderPressed.SetValue(sliderAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pressed"));
		}
	}
}