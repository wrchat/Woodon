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

		[ContextMenu(nameof(UpdateObjectByScore))]
		public void UpdateObjectByScore()
		{
			if (mScore)
				SetObject(mScore.Score);
		}

		public void SetObject(int targetIndex)
		{
			for (int i = 0; i < objectList.Length; i++)
			{
				if (objectList[i])
					objectList[i].SetActive(i == targetIndex);
			}
		}

		#region HorribleEvents
		[ContextMenu(nameof(SetObject0))]
		public void SetObject0() => SetObject(0);
		public void SetObject1() => SetObject(1);
		public void SetObject2() => SetObject(2);
		#endregion
	}
}