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

		bool forceChange = false;

		private void Start()
		{
			mScore.RegisterListener(this, nameof(UpdateSlider));
			Init();
		}

		public void Init()
		{
			slider.value = 0;
			fakeSlider.value = 0;
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
			if (isSliderPressed)
			{
				Animator sliderAnimator = slider.GetComponent<Animator>();
				isSliderPressed.SetValue(sliderAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pressed"));
			}
		}
	}
}