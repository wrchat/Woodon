using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class WPlayerFollowerSetter : WBase
	{
		[Header("_" + nameof(WPlayerFollowerSetter))]
		[SerializeField] private WPlayerFollower[] wPlayerFollowers;
		[SerializeField] private WPlayer[] wPlayers;
		[SerializeField] private WInt wPlayerIndex;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			wPlayerIndex.SetMinMaxValue(0, wPlayers.Length - 1);
			wPlayerIndex.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void UpdateUI()
		{
			WDebugLog($"{nameof(UpdateUI)} - {wPlayerIndex.Value}");

			int index = wPlayerIndex.Value;
			WPlayer wPlayer = wPlayers[index];

			foreach (WPlayerFollower wPlayerFollower in wPlayerFollowers)
				wPlayerFollower.SetWPlayer(wPlayer);
		}
	}
}