
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	public class SansList : UdonSharpBehaviour
	{
		[SerializeField] private GameObject all;
		[SerializeField] private string[] sansList;

		private void Start()
		{
			foreach (var sans in sansList)
			{
				if ((Networking.LocalPlayer.displayName == sans) ||
					(Networking.LocalPlayer.displayName.Contains(sans)))
				{
					all.SetActive(false);
					Sans();
				}
			}
		}

		public void Sans()
		{
			for (int i = 0; i < 100; i++)
				SendCustomEventDelayedFrames(nameof(Sans), 1);
		}
	}
}