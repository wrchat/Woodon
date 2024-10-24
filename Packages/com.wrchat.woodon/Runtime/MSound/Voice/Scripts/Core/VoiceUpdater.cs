using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	// 베이스가 되는 추상 클래스
	// 이 클래스를 상속받아서 UpdateVoice()를 구현해야 함
	public abstract class VoiceUpdater : MBase
	{
		protected VoiceManager voiceManager;

		[SerializeField] protected MBool enable;
		[SerializeField] protected bool usePrevData;

		public virtual void Init(VoiceManager voiceManager)
		{
			this.voiceManager = voiceManager;
		}

		public abstract void UpdateVoice();

		public void SetEnable(bool enable)
		{
			if (this.enable == null)
				return;

			this.enable.SetValue(enable);
		}
	}
}