using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using WRC.Woodon;

namespace Mascari4615.Project.RusukBar
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UISelectPlayListButton : MBase
	{
		[SerializeField] private Image cur;
		[SerializeField] private Image playListCover;

		private UIMusicPlayer musicPlayerUI;
		private int index;


		public void Init(UIMusicPlayer musicPlayerUI, int index)
		{
			this.musicPlayerUI = musicPlayerUI;
			this.index = index;
		}

		public void UpdateUI(MusicPlaylist musicPlayList, int curPlayListIndex)
		{
			playListCover.sprite = musicPlayList.Cover;
			cur.gameObject.SetActive(index == curPlayListIndex);
		}

		public void Click()
		{
			musicPlayerUI.SelectPlayList(index);
		}
	}
}