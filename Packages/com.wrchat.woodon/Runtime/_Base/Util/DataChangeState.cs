namespace WRC.Woodon
{
	public enum DataChangeState
	{
		None,
		Equal,
		NotEqual,
		Greater,
		Less,
	}

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
	}
}