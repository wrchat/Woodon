using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class ObjectActiveList : MBase
	{
		[Header("_" + nameof(ObjectActiveList))]
		[SerializeField] private GameObject[] objectList;
		[SerializeField] private MScore mScore;

		private void Start()
		{
			mScore.SetMinMaxScore(0, objectList.Length - 1);
		}

		public void UpdateObjectByScore()
		{
			if (mScore)
				SetObject(mScore.Score);
		}

		public void SetObject(int targetIndex)
		{
			for (int oi = 0; oi < objectList.Length; oi++)
				objectList[oi].SetActive(oi == targetIndex);
		}

		public void SetObject0() => SetObject(0);
		public void SetObject1() => SetObject(1);
		public void SetObject2() => SetObject(2);
	}
}