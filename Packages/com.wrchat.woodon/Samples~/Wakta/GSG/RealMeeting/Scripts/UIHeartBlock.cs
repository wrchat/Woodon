using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.ISD.GSG.RealMeeting
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIHeartBlock : MBase
	{
		private Image[] hearts;

		public void Init()
		{
			hearts = GetComponentsInChildren<Image>(true);
		}

		public void UpdateUI(Sprite sprite, int count)
		{
			foreach (Image heart in hearts)
			{
				heart.sprite = sprite;
			}

			for (int i = 0; i < hearts.Length; i++)
			{
				hearts[i].enabled = i < count;
			}
		}
	}
}