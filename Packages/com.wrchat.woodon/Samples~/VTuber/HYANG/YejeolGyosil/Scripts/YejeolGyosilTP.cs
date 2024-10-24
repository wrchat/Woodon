using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using WRC.Woodon;
using static WRC.Woodon.MUtil;

namespace Mascari4615.Project.VTuber.HYANG.YejolGyosil
{
	public class YejeolGyosilTP : MBase
	{
		[field: Header("_" + nameof(YejeolGyosilTP))]

		[Header("_ MJJ Scene")]
		[SerializeField] private string kingName;
		[SerializeField] private string kingBehindSayakDrinkName;
		[SerializeField] private string kingBehindName;
		[SerializeField] private string[] eightMugwanNames;
		[SerializeField] private string viiName;
		[SerializeField] private string hyangName;
		[SerializeField] private string bujungName;
		[SerializeField] private string cameraManName;

		[SerializeField] private Transform kingPos;
		[SerializeField] private Transform kingBehindSayakDrinkPos;
		[SerializeField] private Transform kingBehindPos;
		[SerializeField] private Transform[] eightMugwanPos;
		[SerializeField] private Transform viiPos;
		[SerializeField] private Transform hyangPos;
		[SerializeField] private Transform bujungPos;
		[SerializeField] private Transform cameraManPos;

		[Header("_ Ending Scene")]
		[SerializeField] private Transform endingPos;

		public void TP_MJJ()
		{
			MDebugLog($"{nameof(TP_MJJ)}");

			if (Networking.LocalPlayer.displayName == kingName)
				TP(kingPos);
			else if (Networking.LocalPlayer.displayName == kingBehindSayakDrinkName)
				TP(kingBehindSayakDrinkPos);
			else if (Networking.LocalPlayer.displayName == kingBehindName)
				TP(kingBehindPos);
			else if (Networking.LocalPlayer.displayName == viiName)
				TP(viiPos);
			else if (Networking.LocalPlayer.displayName == hyangName)
				TP(hyangPos);
			else if (Networking.LocalPlayer.displayName == bujungName)
				TP(bujungPos);
			else if (Networking.LocalPlayer.displayName == cameraManName)
				TP(cameraManPos);
			else
			{
				for (int i = 0; i < eightMugwanNames.Length; i++)
				{
					if (Networking.LocalPlayer.displayName == eightMugwanNames[i])
					{
						TP(eightMugwanPos[i]);
						break;
					}
				}
			}
		}

		public void TP_MJJ_G()
		{
			MDebugLog($"{nameof(TP_MJJ_G)}");

			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(TP_MJJ));
		}

		public void TP_Ending()
		{
			MDebugLog($"{nameof(TP_Ending)}");

			TP(endingPos);
		}

		public void TP_Ending_G()
		{
			MDebugLog($"{nameof(TP_Ending_G)}");

			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(TP_Ending));
		}
	}
}