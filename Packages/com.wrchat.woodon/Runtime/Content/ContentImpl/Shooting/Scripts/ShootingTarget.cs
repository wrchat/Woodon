using UdonSharp;
using UnityEngine;
using VRC.Udon;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class ShootingTarget : WBase
	{
		[SerializeField] private ShootingManager shootingManager;
		[SerializeField] private UdonSharpBehaviour[] targetUdonBehaviours;
		[SerializeField] private string[] eventNames;

		public void Ahh()
		{
			Debug.Log($"{gameObject.name} : {nameof(Ahh)}");

			for (var i = 0; i < targetUdonBehaviours.Length; i++)
				targetUdonBehaviours[i].SendCustomEvent(eventNames[i]);
		}
	}
}