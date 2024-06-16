
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class TGameVoiceRoomUI : MBase
	{
		[SerializeField] private TGameManager gameManager;
		[SerializeField] private Image[] icons;
		[SerializeField] private VoiceRoom voiceRoom;

		private void Start()
		{
			for (int i = 0; i < icons.Length; i++)
				icons[i].sprite = gameManager.GetPlayerImage(i);
		}

		private void Update()
		{
			for (int i = 0; i < icons.Length; i++)
				icons[i].gameObject.SetActive(voiceRoom.SyncedBools[i].Value);
		}
	}
}