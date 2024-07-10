using UdonSharp;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class SyncedBool : CustomBool
	{
		//[Header("_" + nameof(SyncedBool))]
		//[SerializeField] private UdonBehaviour[] targetUdonsAtSetTime;

		//[SerializeField] private string[] trueEventNamesAtSetTime;
		//[SerializeField] private string[] falseEventNamesAtSetTime;

		//[SerializeField] private string[] trueEventNames;
		//[SerializeField] private string[] falseEventNames;

		[UdonSynced(), FieldChangeCallback(nameof(SyncValue))]
		private bool _syncValue;
		public bool SyncValue
		{
			get => _syncValue;
			private set
			{
				_syncValue = value;
				OnValueChange();
			}
		}

		private bool _onceChanged = false;

		protected override void Start()
		{
			if (Networking.IsMaster)
			{
				if (!_onceChanged)
				{
					SetValue(defaultValue);
					RequestSerialization();
				}
			}

			OnValueChange();

			// base.Start();
		}

		protected override void OnValueChange()
		{
			MDebugLog($"{nameof(OnValueChange)}");

			// Value가 Event 호출보다 먼저 설정되어야함
			// Event에서 CustomBool.Value에 접근할 수 있기 때문
			_value = SyncValue;

			_onceChanged = true;

			UpdateUI();
			SendEvents();
		}

		public override void SetValue(bool newValue)
		{
			MDebugLog($"{nameof(SetValue)}({newValue})");

			// Value가 Event 호출보다 먼저 설정되어야함
			// Event에서 CustomBool.Value에 접근할 수 있기 때문
			_value = newValue;

			SetOwner();
			SyncValue = newValue;
			RequestSerialization();

			//for (var i = 0; i < targetUdonsAtSetTime.Length; i++)
			//	targetUdonsAtSetTime[i]
			//		.SendCustomEvent((newValue ? trueEventNamesAtSetTime[i] : falseEventNamesAtSetTime[i]));
		}

		public override void ToggleValue() => SetValue(!SyncValue);
	}
}