using TMPro;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	// 현재 위치 정보를 잠깐 표시해주는 UI (포켓몬스터 202번 도로 진입하면 텍스트 뜨는 것처럼)
	// KarmoDDrine - 2025-09-30. 23:20
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class PosInfo : WBase
	{
		[SerializeField] private TextMeshProUGUI posText;
		[SerializeField] private WAnimator animator;

		public void ShowPosInfo(string posName)
		{
			if (posText != null)
				posText.text = posName;

			if (animator != null)
				animator.SetTrigger_L0();
		}
	}
}