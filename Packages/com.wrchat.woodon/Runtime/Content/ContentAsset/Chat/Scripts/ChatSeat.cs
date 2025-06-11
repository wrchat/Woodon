﻿using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class ChatSeat : WBase
	{
		[SerializeField] private InputField chatInputField;
		[SerializeField] private ChatManager chatManager;
		
		[SerializeField] private WInt chatRoomIndex;
		public TeamType ChatRoom => (TeamType)chatRoomIndex.Value;

		[SerializeField] private WString additionalData;
		[SerializeField] private float jumpImpulse = 4;

		public void SendChatMessage()
		{
			WDebugLog($"{nameof(SendChatMessage)}");

			if (string.IsNullOrEmpty(chatInputField.text))
				return;

			string additionalData = this.additionalData == null ? string.Empty : this.additionalData.Value;
			chatManager.SendChatMessage(chatInputField.text, ChatRoom, additionalData);
			chatInputField.text = string.Empty;
		}

		private bool isFocused;

		private void Start()
		{
			Init();
		}

		private void Update()
		{
			if (chatInputField.isFocused)
			{
				if (isFocused == false)
				{
					isFocused = true;
					OnEnter();
				}

				if (Input.GetKeyDown(KeyCode.Return))
				{
					string additionalData = this.additionalData == null ? string.Empty : this.additionalData.Value;
					chatManager.SendChatMessage(chatInputField.text.TrimEnd(), ChatRoom, additionalData);
					chatInputField.text = string.Empty;
				}
			}
			else
			{
				if (isFocused)
				{
					isFocused = false;
					OnExit();
				}
			}
		}

		private void Init()
		{
			WDebugLog($"{nameof(Init)}");
			// jumpImpulse = Networking.LocalPlayer.GetJumpImpulse();
		}

		public void OnEnter()
		{
			WDebugLog($"{nameof(OnEnter)}");
			Networking.LocalPlayer.SetVelocity(Vector3.zero);
			Networking.LocalPlayer.SetJumpImpulse(0);
			Networking.LocalPlayer.Immobilize(true);
		}

		public void OnExit()
		{
			WDebugLog($"{nameof(OnExit)}");
			Networking.LocalPlayer.SetJumpImpulse(jumpImpulse);
			Networking.LocalPlayer.Immobilize(false);
		}

		public void ClearChatInputField()
		{
			chatInputField.text = string.Empty;
		}
	}
}