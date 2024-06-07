
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class RB3_UICanvas : UdonSharpBehaviour
	{
		[SerializeField] private Canvas canvas;
		[SerializeField] private Transform[] uiPositions;

		private void Update()
		{
			UpdateUIPos();
		}

		private void UpdateUIPos()
		{
			if (uiPositions == null || uiPositions.Length == 0)
				return;

			int nearestPosIndex = 0;

			Vector3 localPlayerPosition = Networking.LocalPlayer.GetPosition();
			float minDistance = float.MaxValue;

			for (int i = 0; i < uiPositions.Length; i++)
			{
				float distance = Vector3.Distance(uiPositions[i].position, localPlayerPosition);
				if (distance < minDistance)
				{
					minDistance = distance;
					nearestPosIndex = i;
				}
			}

			transform.position = uiPositions[nearestPosIndex].position;
			transform.rotation = uiPositions[nearestPosIndex].rotation;
		}
	}
}