using System;
using UnityEngine;
using VRC.SDKBase;
using VRC.SDK3.Data;

namespace WRC.Woodon
{
	public static class WExtension
	{
		#region VRC.SDK3.Data
		public static int Int(this DataToken dataToken) => (int)dataToken.Double;
		
		public static bool ContainsInt(this DataList dataList, int intValue)
		{
			for (int i = 0; i < dataList.Count; i++)
			{
				if (dataList[i].Int() == intValue)
					return true;
			}
			return false;
		}

		public static bool RemoveInt(this DataList dataList, int intValue)
		{
			for (int i = 0; i < dataList.Count; i++)
			{
				if (dataList[i].Int() == intValue)
				{
					dataList.RemoveAt(i);
					return true;
				}
			}

			return false;
		}

		public static void Shuffle(this DataList dataList)
		{
			for (int i = 0; i < dataList.Count; i++)
			{
				int randomIndex = UnityEngine.Random.Range(i, dataList.Count);
				DataToken temp = dataList[i];
				dataList[i] = dataList[randomIndex];
				dataList[randomIndex] = temp;
			}
		}
		#endregion
	}
}