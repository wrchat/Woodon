using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public abstract class ActiveList : MBase
	{
		[Header("_" + nameof(ActiveList))]
		[SerializeField] private int defaultValue;

		[Header("_" + nameof(ActiveList) + " - Options")]
		[SerializeField] protected MValue mValue;
		[SerializeField] protected ActiveListOption option;
		[SerializeField] protected int targetIndex = NONE_INT;

		private int _value;
		public int Value
		{
			get => _value;
			private set
			{
				_value = value;
				UpdateActive();
			}
		}

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			SetValue(defaultValue);

			if (mValue != null)
			{
				InitMValueMinMax();
				mValue.RegisterListener(this, nameof(UpdateActiveByMValue));
			}

			UpdateActive();
		}

		/// <summary>
		/// 대상이 되는 요소들의 수를 바탕으로 mValue.SetMinMaxValue
		/// </summary>
		protected abstract void InitMValueMinMax();

		/// <summary>
		/// ActiveListOption에 대하여 각 케이스 별로 구현
		/// </summary>
		/// <param name="value"></param>
		protected abstract void UpdateActive();

		public void SetValue(int newValue)
		{
			MDebugLog($"{nameof(SetValue)}({newValue})");
		
			if (mValue != null)
			{
				mValue.SetValue(newValue);
			}
			else
			{
				Value = newValue;
			}
		}

		public void UpdateActiveByMValue()
		{
			if (mValue)
				Value = mValue.Value;
		}

		public void SetMValue(MValue mValue)
		{
			this.mValue = mValue;
			InitMValueMinMax();
			UpdateActiveByMValue();
		}

		#region HorribleEvents
		[ContextMenu(nameof(SetValue) + "N")]
		public void SetValue0() => SetValue(0);
		public void SetValue1() => SetValue(1);
		public void SetValue2() => SetValue(2);
		public void SetValue3() => SetValue(3);
		public void SetValue4() => SetValue(4);
		public void SetValue5() => SetValue(5);
		public void SetValue6() => SetValue(6);
		public void SetValue7() => SetValue(7);
		public void SetValue8() => SetValue(8);
		public void SetValue9() => SetValue(9);
		#endregion
	}
}