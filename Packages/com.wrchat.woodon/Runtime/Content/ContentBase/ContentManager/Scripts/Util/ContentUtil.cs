using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	public static class ContentUtil
	{
		public static int GetMaxData(ContentManager contentManager, string dataName)
		{
			int maxValue = 0;

			foreach (WSeat seat in contentManager.Seats)
				maxValue = Mathf.Max(maxValue, seat.GetData(dataName));

			return maxValue;
		}

		public static WSeat[] GetMaxDataSeats(ContentManager contentManager, string dataName)
		{
			int maxData = GetMaxData(contentManager, dataName);
			WSeat[] maxDataSeats = new WSeat[0];

			foreach (WSeat seat in contentManager.Seats)
			{
				if (seat.TurnData == maxData)
					WUtil.Add(ref maxDataSeats, seat);
			}

			return maxDataSeats;
		}
	}
}