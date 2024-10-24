using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using WRC.Woodon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MUdonChip : UdonSharpBehaviour
	{
		/*private const char DATA_SET_SEPARATER = '#';
        private const char DATA_SEPARATER = '|';

        // [UdonSynced, FieldChangeCallback(nameof(Money))]
        public int _money;
        private int Money
        {
            get => _money;
            set
            {
                Debug.Log($"Money Changed : {_money} => {value}");
                // mTextManager.SetText(_money, value);
                // _money = value;

                UpdateCoinText();

                string globalMoneyData = GlobalMoneyData;
                TryTakeOwnership(gameObject);
                string totalResult;
                if (!globalMoneyData.Contains(Networking.LocalPlayer.displayName))
                {
                    if (globalMoneyData.Length != 0)
                        globalMoneyData += DATA_SET_SEPARATER;
                    globalMoneyData += string.Concat(Networking.LocalPlayer.displayName, DATA_SEPARATER, value);
                    totalResult = globalMoneyData;
                }
                else
                {
                    string result = string.Empty;
                    string[] dataSets = globalMoneyData.Split(DATA_SET_SEPARATER);
                    if (dataSets.Length == 1)
                        result = string.Concat(
                            Networking.LocalPlayer.displayName,
                            DATA_SEPARATER,
                            value.ToString());
                    else
                    {
                        foreach (string dataSet in dataSets)
                        {
                            string[] data = dataSet.Split(DATA_SEPARATER);
                            result += string.Concat(
                                data[0],
                                DATA_SEPARATER,
                                (data[0] == Networking.LocalPlayer.displayName) ? value.ToString() : data[1],
                                DATA_SET_SEPARATER);
                        }
                    }
                    totalResult = result;
                }
                GlobalMoneyData = totalResult.TrimEnd(DATA_SET_SEPARATER, ' ');
                RequestSerialization();
                Debug.Log($"Money Changed End");
            }
        }

        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private TextMeshProUGUI debugText;
        [SerializeField] private string format = "$ {0:F0}";

        [UdonSynced, FieldChangeCallback(nameof(GlobalMoneyData))] private string _globalMoneyData = "";
        public string GlobalMoneyData
        {
            get => _globalMoneyData;
            set
            {
                Debug.Log($"GlobalMoneyData Changed : {_globalMoneyData} => {value}");
                _globalMoneyData = value;
                string[] dataSets = _globalMoneyData.Split(DATA_SET_SEPARATER);
                string debug = string.Empty;
                foreach (string dataSet in dataSets)
                {
                    string[] data = dataSet.Split(DATA_SEPARATER);
                    // Networking.LocalPlayer.SetPlayerTag(data[0], data[1]);
                    debug += string.Concat(data[0], DATA_SEPARATER, data[1], '\n');
                    if (data[0] == Networking.LocalPlayer.displayName)
                    {
                        if (_money != int.Parse(data[1]))
                            mTextManager.SetText(_money, int.Parse(data[1]));
                        _money = int.Parse(data[1]);
                    }
                }
                UpdateCoinText();
                UpdateDebugText(debug);
                Debug.Log($"GlobalMoneyData Changed End");
            }
        }

        [SerializeField] MTextManager mTextManager;

        private void Start()
        {
            UpdateCoinText();

            if (Networking.LocalPlayer.isMaster)
                Money = 1;
        }

        public void AddCoin(int amount)
        {
            Money += amount;
        }

        public bool SendCoin(string receiverName, int amount)
        {
            if (Money < amount)
                return false;

            _money -= amount;
            UpdateCoinText();

            TryTakeOwnership(gameObject);
            string globalMoneyData = GlobalMoneyData;
            string result = string.Empty;
            string[] dataSets = globalMoneyData.Split(DATA_SET_SEPARATER);
            foreach (string dataSet in dataSets)
            {
                string[] data = dataSet.Split(DATA_SEPARATER);
                if (data[0] == Networking.LocalPlayer.displayName)
                    data[1] = _money.ToString();
                else if (data[0] == receiverName)
                    data[1] = (int.Parse(data[1]) + amount).ToString();
                result += string.Concat(data[0], DATA_SEPARATER, data[1], DATA_SET_SEPARATER);
            }
            GlobalMoneyData = result.TrimEnd(DATA_SET_SEPARATER, ' ');

            RequestSerialization();
            return true;
        }

        public void SetCoin(string targetName, int amount)
        {
            if (targetName == Networking.LocalPlayer.displayName)
            {
                Money = amount;
            }
            else
            {
                TryTakeOwnership(gameObject);

                string globalMoneyData = GlobalMoneyData;
                string result = string.Empty;
                string[] dataSets = globalMoneyData.Split(DATA_SET_SEPARATER);
                foreach (string dataSet in dataSets)
                {
                    string[] data = dataSet.Split(DATA_SEPARATER);
                    if (data[0] == targetName)
                        data[1] = amount.ToString();
                    result += string.Concat(data[0], DATA_SEPARATER, data[1], DATA_SET_SEPARATER);
                }
                GlobalMoneyData = result.TrimEnd(DATA_SET_SEPARATER, ' ');
                RequestSerialization();
            }
        }

        public void GetCoin100()
        {
            AddCoin(100);
        }

        public void UseCoin100()
        {
            AddCoin(-100);
        }

        public void UpdateCoinText()
        {
            if (coinText != null)
                coinText.text = string.Format(Money.ToString(), format);
        }

        public void UpdateDebugText(string text)
        {
            if (debugText != null)
                debugText.text = text;
        }

        private void TryTakeOwnership(GameObject targetObject)
        {
            if (!IsOwner(targetObject))
                Networking.SetOwner(Networking.LocalPlayer, targetObject);
        }
        private bool IsOwner(GameObject targetObject) => Networking.LocalPlayer.IsOwner(targetObject);*/

		//

		private const char DATA_SET_SEPARATER = '#';
		private const char DATA_SEPARATER = '|';

		[UdonSynced]
		[FieldChangeCallback(nameof(Money))]
		public int _money;

		[SerializeField] private TextMeshProUGUI coinText;
		[SerializeField] private TextMeshProUGUI debugText;
		[SerializeField] private string format = "$ {0:F0}";
		[SerializeField] private MTextManager mTextManager;

		private int Money
		{
			get => _money;
			set
			{
				Debug.Log($"Money Changed : {_money} => {value}");
				mTextManager.SetText(_money, value);
				_money = value;
				UpdateCoinText();

				if (debugText != null)
					debugText.text = $"Wakgood Money : {_money}";

				Debug.Log("Money Changed End");
			}
		}

		private void Start()
		{
			UpdateCoinText();

			if (Networking.LocalPlayer.isMaster)
				Money = 1;
		}

		public void AddCoin(int amount)
		{
			TryTakeOwnership();
			Money += amount;
			RequestSerialization();
		}

		public void SetCoin(int amount)
		{
			TryTakeOwnership();
			Money = amount;
			RequestSerialization();
		}

		public void GetCoin100()
		{
			AddCoin(100);
		}

		public void UseCoin100()
		{
			AddCoin(-100);
		}

		public void UpdateCoinText()
		{
			if (coinText != null)
				coinText.text = string.Format(Money.ToString(), format);
		}

		private void TryTakeOwnership()
		{
			if (!Networking.LocalPlayer.IsOwner(gameObject))
				Networking.SetOwner(Networking.LocalPlayer, gameObject);
		}
	}
}