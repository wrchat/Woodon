
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	// 베이스가 되는 추상 클래스
	// 이 클래스를 상속받아서 UpdateVoice()를 구현해야 함
	public class VoiceUpdater : MBase
	{
		[SerializeField] protected CustomBool enable;
		[SerializeField] protected bool usePrevData;

		public virtual void UpdateVoice() { }

		public void SetEnable(bool enable)
		{
			if (this.enable)
				this.enable.SetValue(enable);
		}
	}
}