using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using WRC.Woodon;

namespace Mascari4615.Project.RusukBar
{
	public enum ToggleType
	{
		PostProcess = 0,
		Bell,
		Mike,
		Piano,
		Drum,
		Guitar,
		Collider,
		Music,
	}

	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class RB3_GameManager : MBase
	{
		[Header("_" + nameof(RB3_GameManager))]
		public RB3_UICanvas[] uis;
		private bool isVR = false;

		[Header("Objects")]
		[SerializeField] private GameObject[] toggleObjects;
		[SerializeField] private Collider[] colliders;

		[Header("Objects")]
		[SerializeField] private Transform cocktailsParent;
		private VRC_Pickup[] cocktailPickups;

		[SerializeField] private Transform pizzaParent;
		private VRC_Pickup[] pizzaPickups;

		[SerializeField] private Transform chessParent;
		private VRC_Pickup[] chessPickups;

		[SerializeField] private ObjectActive mirror;
		[SerializeField] private ObjectActive mirrorQuility;
		// private int curVRUIPosIndex = -1;

		private UdonSharpBehaviour[] qvPens;

		private void Start() => Init();

		private void Init()
		{
			MDebugLog(nameof(Init));

			qvPens = GameObject.Find("Pens").GetComponentsInChildren<UdonSharpBehaviour>();

			foreach (RB3_UICanvas ui in uis)
				ui.Init();
			uis[0].gameObject.SetActive(false);
			// mainUI.GetComponent<Animator>().keepAnimatorControllerStateOnDisable = true;

			isVR = Networking.LocalPlayer.IsUserInVR();
			if (isVR)
				ToggleObject(ToggleType.Collider);

			cocktailPickups = cocktailsParent.GetComponentsInChildren<VRC_Pickup>();
			pizzaPickups = pizzaParent.GetComponentsInChildren<VRC_Pickup>();
			chessPickups = chessParent.GetComponentsInChildren<VRC_Pickup>();

			UpdateStuff();
		}

		private void Update()
		{
			if (isVR)
				return;

			if (Input.GetKeyDown(KeyCode.Tab))
			{
				// foreach (CanvasManager item in canvasManagers)
				// 	item.ChangeCocktail();
				uis[0].gameObject.SetActive(true);
				uis[0].Init();
			}
			else if (Input.GetKeyUp(KeyCode.Tab))
			{
				// foreach (CanvasManager item in canvasManagers)
				// 	item.ChangeCocktail();
				uis[0].gameObject.SetActive(false);
				uis[0].Init();
			}
		}

		private void UpdateStuff()
		{
			foreach (RB3_UICanvas ui in uis)
			{
				bool[] bools = new bool[100];

				bools[(int)ToggleType.PostProcess] = toggleObjects[(int)ToggleType.PostProcess].activeSelf;
				bools[(int)ToggleType.Bell] = toggleObjects[(int)ToggleType.Bell].activeSelf;
				bools[(int)ToggleType.Mike] = toggleObjects[(int)ToggleType.Mike].activeSelf;
				bools[(int)ToggleType.Piano] = toggleObjects[(int)ToggleType.Piano].activeSelf;
				bools[(int)ToggleType.Drum] = toggleObjects[(int)ToggleType.Drum].activeSelf;
				bools[(int)ToggleType.Guitar] = toggleObjects[(int)ToggleType.Guitar].activeSelf;
				bools[(int)ToggleType.Collider] = colliders[0].enabled;

				ui.UpdateToggleUI(bools);
			}
		}

		public void ToggleMirror()
		{
			mirror.ToggleActive();
		}

		public void ToggleMirrorQuility()
		{
			mirrorQuility.ToggleActive();
		}

		public void ToggleObject(ToggleType objectType)
		{
			if (objectType == ToggleType.Collider)
			{
				foreach (Collider collider in colliders)
				{
					if (collider == null)
						continue;
					collider.enabled = !collider.enabled;
				}
			}
			else
			{
				GameObject target = toggleObjects[(int)objectType];
				target.SetActive(!target.activeSelf);
			}

			UpdateStuff();
		}

		public void ResetAllPos()
		{
			ResetCocktailPos();
			ResetPizzaPos();
			ResetChessPos();
		}

		public void ResetCocktailPos() => ResetPos(cocktailPickups);
		public void ResetPizzaPos() => ResetPos(pizzaPickups);
		public void ResetChessPos() => ResetPos(chessPickups);

		private void ResetPos(VRC_Pickup[] pickups)
		{
			foreach (VRC_Pickup pickup in pickups)
			{
				SetOwner(pickup.gameObject);

				pickup.Drop();
				pickup.transform.position = Vector3.down * 444f;
			}
		}

		public void ResetPens_Global()
		{
			if (Networking.LocalPlayer.isMaster)
				SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ResetPens));
		}

		public void ResetPens()
		{
			foreach (UdonSharpBehaviour pen in qvPens)
				pen.SendCustomEvent("_Respawn");
		}

		public void ClearPens_Global()
		{
			if (Networking.LocalPlayer.isMaster)
				SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ClearPens));
		}

		public void ClearPens()
		{
			foreach (UdonSharpBehaviour pen in qvPens)
				pen.SendCustomEvent("_Clear");
		}
	}
}