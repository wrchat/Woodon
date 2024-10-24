using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static WRC.Woodon.MUtil;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class VoiceSeparateByPos : MBase
	{
		[SerializeField] private bool targetX;
		[SerializeField] private bool targetY;
		[SerializeField] private bool targetZ;

		[SerializeField] private float targetScalar;

		[SerializeField] private float far = 50;
		[SerializeField] private float near;
		[SerializeField] private float gain = 10;

		private VRCPlayerApi[] _players;

		private readonly string[] list = { "VIichan", "EomSeokDae", "시간할아버지" };

		private void Update()
		{
			if (_players != null)
			{
				var lp = Networking.LocalPlayer.GetPosition();

				// FIX : This is just for RaniBell2
				if (lp.x > targetScalar)
				{
					foreach (var player in _players)
					{
						var find = false;
						foreach (var main in list)
							if (player.displayName == main)
							{
								SetVoiceGlobal(player, true);
								break;
							}

						if (find)
							continue;

						var tp = player.GetPosition();

						if (tp.x > targetScalar)
							SetVoiceGlobal(player, true);
						else
							SetVoiceDefault(player, true);
					}

					return;
				}

				foreach (var player in _players)
				{
					var find = false;
					foreach (var main in list)
						if (player.displayName == main)
						{
							SetVoiceGlobal(player, true);
							break;
						}

					if (find)
						continue;

					var tp = player.GetPosition();

					if (targetX)
						SetVoiceGlobal(player, tp.x > targetScalar == lp.x > targetScalar);
					else if (targetY)
						SetVoiceGlobal(player, tp.y > targetScalar == lp.y > targetScalar);
					else if (targetZ)
						SetVoiceGlobal(player, tp.z > targetScalar == lp.z > targetScalar);
				}
			}
		}

		public override void OnPlayerJoined(VRCPlayerApi player)
		{
			_players = GetPlayers();
		}

		public override void OnPlayerLeft(VRCPlayerApi player)
		{
			_players = GetPlayers();
		}

		public void SetVoiceGlobal(VRCPlayerApi player, bool canHear)
		{
			player.SetVoiceDistanceFar(canHear ? far : 0);
			player.SetVoiceDistanceNear(canHear ? near : 0);
			player.SetVoiceGain(canHear ? gain : 0);
		}

		public void SetVoiceDefault(VRCPlayerApi player, bool canHear)
		{
			player.SetVoiceDistanceFar(canHear ? 25 : 0);
			player.SetVoiceDistanceNear(canHear ? 0 : 0);
			player.SetVoiceGain(canHear ? 15 : 0);
		}
	}
}