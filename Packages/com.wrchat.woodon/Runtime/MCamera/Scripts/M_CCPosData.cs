using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class M_CCPosData : UdonSharpBehaviour
	{
		private Transform[] ccPos;

		private void Start()
		{
			Init();
		}

		public Transform IndexOf(int index)
		{
			if (ccPos == null)
				Init();
			return ccPos[index];
		}

		public int Length()
		{
			if (ccPos == null)
				Init();
			return ccPos.Length;
		}

		private void Init()
		{
			ccPos = new Transform[transform.childCount];
			for (int ci = 0; ci < transform.childCount; ci++)
				ccPos[ci] = transform.GetChild(ci);
		}
	}
}