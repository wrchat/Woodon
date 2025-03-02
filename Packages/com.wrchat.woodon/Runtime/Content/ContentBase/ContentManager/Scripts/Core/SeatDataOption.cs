using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class SeatDataOption : WBase
	{
		[field: Header("_" + nameof(SeatDataOption))]

		// 이름
		[field: SerializeField] public string Name { get; private set; } = ContentManager.TurnDataString;

		// 옵션
		[Header("_" + nameof(SeatDataOption) + "_Option")]
		public int DefaultValue = NONE_INT;
		public string[] DataToString = new string[0];
		public bool ResetWhenOwnerChange = true;

		// 데이터가 요소로 사용되는 경우
		[Header("_" + nameof(SeatDataOption) + "_Element")]
		public bool IsElement = false;
		public Sprite[] DataToSprites = new Sprite[0];
		public Sprite DataNoneSprite = null;
	}
}