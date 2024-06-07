
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MScoreSlider : MBase
	{
		[SerializeField] private Slider slider;
		[SerializeField] private Slider fakeSlider;
		[SerializeField] private MScore mScore;

		public void Init()
		{
			slider.value = 0;
			fakeSlider.value = 0;
		}

		public void OnSliderValueChanged()
		{
			int newValue = (int)(slider.value * mScore.maxScore);
			if (mScore.Score != newValue)
			{
				MDebugLog($"{nameof(OnSliderValueChanged)} = {newValue}");
				mScore.SetScore(newValue);
			}
		}

		public void UpdateSlider()
		{
			int curValue = (int)(fakeSlider.value * mScore.maxScore);
			if (mScore.Score != curValue)
				fakeSlider.value = (float)mScore.Score / mScore.maxScore;
		}
	}
}