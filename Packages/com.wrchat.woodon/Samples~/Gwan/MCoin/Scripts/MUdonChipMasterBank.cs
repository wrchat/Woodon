using TMPro;
using UdonSharp;
using UnityEngine;
using WRC.Woodon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MUdonChipMasterBank : UdonSharpBehaviour
	{
		// [SerializeField] private MTarget MTarget;
		[SerializeField] private TextMeshProUGUI targetCoinUI;
		private MUdonChip MUdonChip;

		private void Start()
		{
			MUdonChip = GameObject.Find(nameof(MUdonChip)).GetComponent<MUdonChip>();
		}

		/*public void ApplySet()
        {
            Debug.Log(nameof(ApplySet) + "_" + MTarget.CurTargetPlayerID);
            if (targetCoinUI.text.Length <= 0)
                return;

            string targetCoinString = targetCoinUI.text.TrimEnd('\n', ' ', (char)8203);
            if (targetCoinString.Length == '0')
                return;

            foreach (char item in targetCoinString)
            {
                if (!char.IsDigit(item))
                    return;
            }

            VRCPlayerApi targetPlayer = VRCPlayerApi.GetPlayerById(MTarget.CurTargetPlayerID);
            if (targetPlayer == null)
                return;

            int targetCoin = int.Parse(targetCoinString);
            MUdonChip.SetCoin(targetPlayer.displayName, targetCoin);
        }*/

		public void ApplySet()
		{
			// Debug.Log(nameof(ApplySet) + "_" + MTarget.CurTargetPlayerID);
			if (targetCoinUI.text.Length <= 0)
				return;

			string targetCoinString = targetCoinUI.text.TrimEnd('\n', ' ', (char)8203);
			if (targetCoinString.Length == '0')
				return;

			foreach (char item in targetCoinString)
				if (!char.IsDigit(item))
					return;

			/*VRCPlayerApi targetPlayer = VRCPlayerApi.GetPlayerById(MTarget.CurTargetPlayerID);
            if (targetPlayer == null)
                return;*/

			int targetCoin = int.Parse(targetCoinString);
			// MUdonChip.SetCoin(targetPlayer.displayName, targetCoin);
			MUdonChip.SetCoin(targetCoin);
		}
	}
}