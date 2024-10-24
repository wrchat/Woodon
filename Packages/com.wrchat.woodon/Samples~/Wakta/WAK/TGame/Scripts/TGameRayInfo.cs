using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.WAK.TGame
{
	public class TGameRayInfo : RayInfo
	{
		[SerializeField] private TGameManager tGameManager;

		protected override void Update()
		{
			if (tGameManager.IsGaming == false)
				return;
			base.Update();
		}

		protected override string GetString(GameObject obj)
		{
			int order = int.Parse(obj.name.Split('_')[1]);
			if (tGameManager.Data.CurRound == 5 && tGameManager.CurRoundData.isRoundEnd)
			{
				return tGameManager.GetPlayerNameByEndingSeatIndex(order);
			}
			else
			{
				return tGameManager.GetPlayerName(tGameManager.CurRoundData.NumberByOrder[order], DEBUG);
			}
		}
	}
}