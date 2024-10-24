using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	public class TestCollision : MBase
	{
		[UdonSynced, FieldChangeCallback(nameof(TestInt))] private int testInt;
		public int TestInt
		{
			get => testInt;
			set
			{
				testInt = value;
				OnTestIntChange();
			}
		}

		private void OnTestIntChange()
		{
			MDebugLog(testInt.ToString());
		}

		private void Start()
		{
			MDebugLog(nameof(Start));
			OnTestIntChange();
		}

		public override void Interact()
		{
			SetOwner();
			TestInt++;
			RequestSerialization();
		}

		private void OnCollisionEnter(Collision collision)
		{
			Debug.Log($"{nameof(OnCollisionEnter)}, collision.gameobject.name = {collision.gameObject.name}");
		}

		private void OnCollisionExit(Collision collision)
		{
			Debug.Log($"{nameof(OnCollisionExit)}, collision.gameobject.name = {collision.gameObject.name}");
		}

		private void OnCollisionStay(Collision collision)
		{
			Debug.Log($"{nameof(OnCollisionStay)}, collision.gameobject.name = {collision.gameObject.name}");
		}

		private void OnParticleCollision(GameObject other)
		{
			Debug.Log($"{nameof(OnParticleCollision)}, other.name = {other.name}");
		}

		private void OnParticleTrigger()
		{
			Debug.Log($"{nameof(OnParticleTrigger)}");
		}

		private void OnTriggerEnter(Collider other)
		{
			Debug.Log($"{nameof(OnTriggerEnter)}, other.name = {other.name}");
		}

		private void OnTriggerExit(Collider other)
		{
			Debug.Log($"{nameof(OnTriggerExit)}, other.name = {other.name}");
		}

		private void OnTriggerStay(Collider other)
		{
			Debug.Log($"{nameof(OnTriggerStay)}, other.name = {other.name}");
		}
	}
}