using UdonSharp;
using UnityEngine;
using static WRC.Woodon.MUtil;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public abstract class MTurnBaseManager : ContentManager
	{
		[field: Header("_" + nameof(MTurnBaseManager))]

		[field: Header("_" + nameof(MTurnBaseManager) + "_Data")]
		[field: SerializeField] public int DefaultData { get; private set; } = 0;
		[field: SerializeField] public string[] DataToString { get; protected set; }
		[field: SerializeField] public bool ResetDataWhenOwnerChange { get; private set; }
		[field: SerializeField] public bool UseDataSprites { get; private set; }
		[field: SerializeField] public bool IsDataElement { get; private set; }
		[field: SerializeField] public Sprite[] DataSprites { get; protected set; }
		[field: SerializeField] public Sprite DataNoneSprite { get; protected set; }

		[field: Header("_" + nameof(MTurnBaseManager) + "_TurnData")]
		[field: SerializeField] public int DefaultTurnData { get; private set; } = 0;
		[field: SerializeField] public string[] TurnDataToString { get; protected set; }
		[field: SerializeField] public bool ResetTurnDataWhenOwnerChange { get; private set; }
		[field: SerializeField] public bool UseTurnDataSprites { get; private set; }
		[field: SerializeField] public bool IsTurnDataElement { get; private set; }
		[field: SerializeField] public Sprite[] TurnDataSprites { get; protected set; }
		[field: SerializeField] public Sprite TurnDataNoneSprite { get; protected set; }

		public int GetMaxTurnData()
		{
			int maxTurnData = 0;

			foreach (MTurnSeat mTurnSeat in MSeats)
				maxTurnData = Mathf.Max(maxTurnData, mTurnSeat.TurnData);

			return maxTurnData;
		}

		public MTurnSeat[] GetMaxTurnDataSeats()
		{
			int maxTurnData = GetMaxTurnData();
			int maxTurnDataCount = 0;
			MTurnSeat[] maxTurnDataSeats = new MTurnSeat[MSeats.Length];

			foreach (MTurnSeat mTurnSeat in MSeats)
			{
				if (mTurnSeat.TurnData == maxTurnData)
				{
					maxTurnDataSeats[maxTurnDataCount] = mTurnSeat;
					maxTurnDataCount++;
				}
			}

			MDataUtil.ResizeArr(ref maxTurnDataSeats, maxTurnDataCount);

			return maxTurnDataSeats;
		}
	}
}
