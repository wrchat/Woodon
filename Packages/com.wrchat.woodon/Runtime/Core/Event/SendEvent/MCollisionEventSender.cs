using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WRC.Woodon
{
	public class MCollisionEventSender : WEventPublisher
	{
		[Header("_" + nameof(MCollisionEventSender))]
		[SerializeField] private GameObject ownerObject;

		[SerializeField] private string[] keyStrings;
		[SerializeField] private bool whenTargetObjectNameContainsKey;
		[SerializeField] private bool whenTargetObjectNameEqualsKey;

		[SerializeField] private GameObject[] keyObjects;

		protected bool CheckCondition(GameObject other)
		{
			// 1. Check OwnerObject
			if (ownerObject != null && IsOwner(ownerObject) == false)
				return false;

			bool isKeyObject = false;

			// 2. Check KeyObjects
			foreach (GameObject targetObject in keyObjects)
				if (other.gameObject == targetObject)
				{
					isKeyObject = true;
					break;
				}

			// 3. Check KeyStrings
			foreach (string keyString in keyStrings)
				if ((whenTargetObjectNameContainsKey && other.gameObject.name.Contains(keyString)) ||
					(whenTargetObjectNameEqualsKey && other.gameObject.name.Equals(keyString)))
				{
					isKeyObject = true;
					break;
				}

			return isKeyObject;
		}
	}
}