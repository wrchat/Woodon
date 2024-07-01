using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static Mascari4615.MUtil;

namespace Mascari4615
{
	public class MBase : UdonSharpBehaviour
	{
		public const string DEBUG_PREFIX = "DEBUG";

		public const char DATA_PACK_SEPARATOR = '_';
		public const char DATA_SEPARATOR = '@';

		public const int NONE_INT = -1;
		public const string NONE_STRING = "SANS";
		public const string TRUE_STRING = "TRUE";
		public const string FALSE_STRING = "FALSE";

		[Header("_" + nameof(MBase))]
		[SerializeField] protected bool DEBUG = true;

		protected void MDebugLog(string log = NONE_STRING)
		{
			// To Not Log Error When Player Left The World.
			if (IsNotOnline())
				return;

			if (DEBUG)
				Debug.Log($"{DEBUG_PREFIX}, {Networking.LocalPlayer.playerId}, {gameObject.name} : {log}");
		}

		protected void SetOwner(GameObject targetObject = null)
		{
			if (targetObject == null)
				targetObject = gameObject;

			if (IsOwner(targetObject) == false)
				Networking.SetOwner(Networking.LocalPlayer, targetObject);
		}

		protected bool IsOwner(GameObject targetObject = null)
		{
			if (IsNotOnline())
				return false;

			if (targetObject == null)
				targetObject = gameObject;

			return Networking.LocalPlayer.IsOwner(targetObject);
		}
	}
}