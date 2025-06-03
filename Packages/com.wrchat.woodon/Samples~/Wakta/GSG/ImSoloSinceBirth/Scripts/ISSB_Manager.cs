using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.ISD.GSG.ImSoloSinceBirth
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class ISSB_Manager : WBase
	{
		private ISSB_Seat[] seats;
		[SerializeField] private TextMeshProUGUI[] countTexts;

		// 으어
		[SerializeField] private WInt manIndex;
		[SerializeField] private SpriteList spriteList;
		[SerializeField] private WString manName;
		[SerializeField] private string[] manNames;
		public void UpdateMan()
		{
			if ((manIndex.Value < 0) || (manIndex.Value >= manNames.Length))
			{
				if (Networking.IsMaster)
				{
					manIndex.SetValue(0);
					return;
				}
			}

			manName.SetValue(manNames[manIndex.Value]);
		}

		// 으어으어
		[SerializeField] private WBool[] hereBools;
		public void ResetHereBools()
		{
			foreach (WBool hereBool in hereBools)
				hereBool.SetValue(false);
		}

		private void Start()
		{
			Init();
			UpdateUI();
		}

		public void Init()
		{
			seats = GetComponentsInChildren<ISSB_Seat>();

			foreach (ISSB_Seat seat in seats)
				seat.HeartBool.RegisterListener(this, nameof(UpdateUI));

			// 으어어어어어
			manIndex.RegisterListener(this, nameof(UpdateMan));
			UpdateMan();
		}


		// 으어어어아아어엉
		[SerializeField] private WSFXManager sfxManager;
		private int lastHeartCount = 0;
		public void UpdateUI()
		{
			int heartCount = 0;
			for (int i = 0; i < seats.Length; i++)
			{
				if (seats[i].HeartBool.Value)
					heartCount++;
			}

			foreach (TextMeshProUGUI countText in countTexts)
				countText.text = heartCount.ToString();

			if (heartCount > lastHeartCount)
				sfxManager.PlaySFX_L(3);

			lastHeartCount = heartCount;
		}

		public void ResetHearts()
		{
			foreach (ISSB_Seat seat in seats)
				seat.HeartBool.SetValue(false);
		}
	}
}
