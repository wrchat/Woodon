using UdonSharp;
using UnityEngine;
using WRC.Woodon;

namespace Mascari4615.Project.RusukBar
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MusicPlaylist : MBase
	{
		[field: SerializeField] public Sprite Cover { get; private set; }
		[field: SerializeField] public string PlaylistName { get; private set; }
		[field: SerializeField] public MusicData[] MusicDatas { get; private set; }

		// MusicData 요소 반대로
		[ContextMenu("Reverse MusicDatas")]
		public void ReverseMusicDatas()
		{
			for (int i = 0; i < MusicDatas.Length / 2; i++)
			{
				MusicData temp = MusicDatas[i];
				MusicDatas[i] = MusicDatas[MusicDatas.Length - i - 1];
				MusicDatas[MusicDatas.Length - i - 1] = temp;
			}
		}
	}
}