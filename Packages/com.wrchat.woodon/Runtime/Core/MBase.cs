using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static WRC.Woodon.MUtil;

namespace WRC.Woodon
{
	public abstract class MBase : UdonSharpBehaviour
	{
		public const string DEBUG_PREFIX = "WRChat";

		public const char DATA_PACK_SEPARATOR = '_';
		public const char DATA_SEPARATOR = '@';

		public const int NONE_INT = -1;
		public const string NONE_STRING = "SANS";
		public const string TRUE_STRING = "TRUE";
		public const string FALSE_STRING = "FALSE";

		[Header("_" + nameof(MBase))]
		[SerializeField] protected bool DEBUG = false;

		// 아래 메서드들은 확장 메서드들로 만들까 했는데, 일반 메서드와 크게 성능상 차이가 없다고 함.
		// this.MDebugLog(); 같이 쓰기 번거로워지기만 할 것 같아서 그냥 일반 메서드로 둠.

		// 정적 메소드로 만들더라도 마찬가지로 MBase.MDebugLog(); 같이 써야 하거나,
		// 대신 위에 using static WRC.MBase; 를 써주면 되는데,
		// 클래스 이름 : MBase 로 상속 받는 것이 더 빠르고 편하기도 하고, 상속 받을 때만의 이점이 있어서 (아래 참고) 패스

		// MBase를 직접 상속받게 한다면,
		// gameObject로 현재 이 클래스가 속한 인스턴스의 gameObject를 가져올 수 있기 때문에,
		// SetOwner(gameObject); 같은 경우 SetOwner(); 로 축약할 수 있음. IsOwner()도 마찬가지.

		// DEBUG 옵션도 딱히 대체 방법이 생각나지 않아서, 직접 상속 받게하는 것이 여러모로 편하고 좋아보임.

		protected void MDebugLog(string log, LogType logType = LogType.Log)
		{
			// To Not Log Error When Player Left The World.
			if (IsNotOnline())
				return;

			if (DEBUG == false)
				return;

			string formattedLog = $@"" +
				$@"[<color=#3a9e00>{DEBUG_PREFIX}</color>] " +
				$@"[<color=#fc6203>{Networking.LocalPlayer.playerId}</color>] " +
				$@"<color=#00a6ff>{gameObject.name}</color> {log}";

			if (logType == LogType.Log)
				Debug.Log(formattedLog);
			else if (logType == LogType.Warning)
				Debug.LogWarning(formattedLog);
			else if (logType == LogType.Error)
				Debug.LogError(formattedLog);
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