namespace WRC.Woodon
{
	public enum MPathCartEvent
	{
		StartPath = 0,
		StartFowardPath = 1,
		StartBackwardPath = 2,
		
		EndPath = 10,
		EndFowardPath = 11,
		EndBackwordPath = 12,
		
		// 중간에 멈추는
		// 중간에 멈췄다가 다시 움직이는
		// 중간에 방향을 바꾸는
	}
}