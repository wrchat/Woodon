﻿using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIWSFXManager : WBase
	{
		[Header("_" + nameof(UIWSFXManager))]
		[SerializeField] private WSFXManager sfxManager;
		[SerializeField] private bool global = false;
		
		private UIWSFXManagerButton[] buttons;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			buttons = GetComponentsInChildren<UIWSFXManagerButton>(true);

			if (buttons == null)
				return;

			for (int i = 0; i < buttons.Length; i++)
			{
				bool isInvalidIndex = i >= sfxManager.AudioClips.Length;
				bool isElementNull = (isInvalidIndex == false) && sfxManager.AudioClips[i] == null;

				if (isInvalidIndex || isElementNull)
				{
					WDebugLog($"Invalid index: {i} | Element is null: {isElementNull}");
					buttons[i].gameObject.SetActive(false);
					continue;
				}

				buttons[i].gameObject.SetActive(true);
				buttons[i].Init(this, i, sfxManager.AudioClips[i].name);
			}
		}

		public void PlaySFX(int index)
		{
			if (global)
				sfxManager.PlaySFX_G(index);
			else
				sfxManager.PlaySFX_L(index);
		}

		#region HorribleEvents
		public void StopSFX_Global() => sfxManager.StopSFX_Global();
		public void StopSFX() => sfxManager.StopSFX();
		#endregion
	}
}