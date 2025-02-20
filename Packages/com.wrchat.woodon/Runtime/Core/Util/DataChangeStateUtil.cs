namespace WRC.Woodon
{
	public static class DataChangeStateUtil
	{
		public static DataChangeState GetChangeState(int origin, int value)
		{
			if (origin == value)
				return DataChangeState.Equal;
			if (origin < value)
				return DataChangeState.Greater;
			if (origin > value)
				return DataChangeState.Less;

			return DataChangeState.None;
		}

		public static DataChangeState GetChangeState(bool origin, bool value)
		{
			if (origin == value)
				return DataChangeState.Equal;
			if (origin != value)
				return DataChangeState.NotEqual;

			return DataChangeState.None;
		}

		public static bool IsDataChanged(DataChangeState changeState) =>
			changeState != DataChangeState.None &&
			changeState != DataChangeState.Equal;

		public static string ToFriendlyString(this DataChangeState state)
		{
			switch (state)
			{
				case DataChangeState.None:
					return "None";
				case DataChangeState.Equal:
					return "Equal";
				case DataChangeState.NotEqual:
					return "NotEqual";
				case DataChangeState.Greater:
					return "Greater";
				case DataChangeState.Less:
					return "Less";
				default:
					return "ERROR";
			}
		}
	}
}