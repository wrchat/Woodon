using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using WRC.Woodon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class PlayerOwnUdon : MBase
	{
		[UdonSynced] public bool isSynced;
		private UdonSharpBehaviour[] childUdons;
		private int index = NONE_INT;

		[UdonSynced]
		[FieldChangeCallback(nameof(OwnerID))]
		private int ownerID = NONE_INT;

		private PlayerOwnUdonManager ownUdonManager;

		[UdonSynced]
		[FieldChangeCallback(nameof(SomeData))]
		private string someData = NONE_STRING;

		public int Index
		{
			get
			{
				if (index == NONE_INT)
					index = int.Parse(gameObject.name.Split(' ')[1].Trim(new char[] { '(', ')' }));

				return index;
			}
		}

		public int OwnerID
		{
			get => ownerID;
			set
			{
				ownerID = value;
				OnOwnerChanged();
			}
		}

		public string SomeData
		{
			get => someData;
			set
			{
				someData = value;
				OnSomeDataChanged();
			}
		}

		public VRCPlayerApi OwnerAPI { get; private set; }

		private void Start()
		{
			ownUdonManager = GameObject.Find(nameof(PlayerOwnUdonManager)).GetComponent<PlayerOwnUdonManager>();
			childUdons = GetComponentsInChildren<UdonSharpBehaviour>();

			// OnStateChanged();
			OnOwnerChanged();
			OnSomeDataChanged();
		}

		private void LateUpdate()
		{
			// OnActivateChanged();
		}

		public void Init()
		{
			SetOwner();
			isSynced = true;
			RequestSerialization();
		}

		public void OnOwnerChanged()
		{
			// Debug.Log($"{gameObject.name} : {nameof(OnOwnerChanged)}, {nameof(OwnerIndex)} = {OwnerIndex}");

			VRCPlayerApi playerAPI = VRCPlayerApi.GetPlayerById(ownerID);
			OwnerAPI = playerAPI;
		}

		public void OnSomeDataChanged()
		{
		}

		public void SetOwnerNull()
		{
			SetOwner();
			OwnerID = NONE_INT;
			SomeData = NONE_STRING;
			RequestSerialization();
		}

		public void SendEvnetToChildUdons(string eventName)
		{
			Debug.Log($"{gameObject.name} : {nameof(SendEvnetToChildUdons)}, {nameof(eventName)} = {eventName}");

			foreach (UdonSharpBehaviour childUdon in childUdons)
			{
				if (childUdon == this)
					continue;

				childUdon.SendCustomEvent(eventName);
			}
		}

		public void SetStatus(int ownerIndex, string someData)
		{
			SetOwner();
			OwnerID = ownerIndex;
			SomeData = someData;
			RequestSerialization();
		}

		public override void OnPlayerLeft(VRCPlayerApi player)
		{
			if (VRCPlayerApi.GetPlayerById(OwnerID) == null)
				if (IsOwner())
				{
					SetOwnerNull();
					RequestSerialization();
				}
		}
	}
}