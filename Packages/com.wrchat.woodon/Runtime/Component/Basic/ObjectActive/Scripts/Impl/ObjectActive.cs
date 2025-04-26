using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	// Toggle 개념을 통해 `게임 오브젝트`를 활성화/비활성화 하는 것
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class ObjectActive : ActiveToggle
	{
		[Header("_" + nameof(ObjectActive))]
		[SerializeField] private GameObject[] activeObjects;
		[SerializeField] private GameObject[] disableObjects;

		protected override void UpdateActive()
		{
			WDebugLog($"{nameof(UpdateActive)}");

			foreach (GameObject activeObject in activeObjects)
			{
				if (activeObject == null)
				{
					continue;
				}

				activeObject.SetActive(Active);
			}

			foreach (GameObject disableObject in disableObjects)
			{
				if (disableObject == null)
				{
					continue;
				}

				disableObject.SetActive(!Active);
			}
		}

#if UNITY_EDITOR
		public void SetActiveObjects(GameObject[] activeObjects)
		{
			this.activeObjects = activeObjects;
		}
#endif
	}
}