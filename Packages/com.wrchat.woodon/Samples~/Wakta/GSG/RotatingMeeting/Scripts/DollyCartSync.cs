using Cinemachine;
using TMPro;
using UnityEngine;
using WRC.Woodon;

namespace Mascari4615.Project.ISD.GSG.RotatingMeeting
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class DollyCartSync : MBase
	{
		[Header("_" + nameof(DollyCartSync))]
		[SerializeField] private CinemachineDollyCart cinemachineDollyCart;
		[SerializeField] private RotatingMeetingManager rotatingMeetingManager;
		[SerializeField] private TextMeshPro asd;

		public float Position { get; private set; }

		public void SetPosition(float newPosition)
		{
			Position = newPosition;
			asd.text = Position.ToString();

			cinemachineDollyCart.m_Position = Position;

			bool isManager = IsOwner(rotatingMeetingManager.gameObject);
			cinemachineDollyCart.enabled = isManager;
			if (isManager)
				SetOwner();
		}
	}
}