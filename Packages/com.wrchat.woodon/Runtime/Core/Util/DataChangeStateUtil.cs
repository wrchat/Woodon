namespace WRC.Woodon
{
	public class DataChangeStateUtil
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
	}
}