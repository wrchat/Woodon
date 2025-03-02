using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public abstract class ActiveList : WBase
	{
		[Header("_" + nameof(ActiveList))]
		[SerializeField] private int defaultValue;

		[Header("_" + nameof(ActiveList) + " - Options")]
		[SerializeField] protected WInt wInt; // defaultValue를 무시합니다.
		[SerializeField] private bool setWIntMinMax = true;
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
			if (wInt != null)
			{
				if (setWIntMinMax)
					InitWIntMinMax();
				wInt.RegisterListener(this, nameof(UpdateActiveByWInt));
				UpdateActiveByWInt();
			}
			else
			{
				SetValue(defaultValue);
			}

			UpdateActive();
		}

		/// <summary>
		/// 대상이 되는 요소들의 수를 바탕으로 wInt.SetMinMaxValue
		/// </summary>
		protected abstract void InitWIntMinMax();

		/// <summary>
		/// ActiveListOption에 대하여 각 케이스 별로 구현
		/// </summary>
		/// <param name="value"></param>
		protected abstract void UpdateActive();

		public void SetValue(int newValue)
		{
			WDebugLog($"{nameof(SetValue)}({newValue})");
		
			if (wInt != null)
			{
				wInt.SetValue(newValue);
			}
			else
			{
				Value = newValue;
			}
		}

		public void UpdateActiveByWInt()
		{
			if (wInt)
				Value = wInt.Value;
		}

		// TODO: WIntFollowers
		public void SetWInt(WInt wInt)
		{
			this.wInt = wInt;

			if (setWIntMinMax)
				InitWIntMinMax();
			UpdateActiveByWInt();
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