using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615
{
	public interface MKeyValueUpdateListener
	{
		void BeforeUpdate();
		void OnUpdate();
	}

	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MKeyValueSync : MBase
	{
		public const char DATA_PAIR_SEPARATER = '|';
		public const char DATA_SEPARATER = '#';
		public const char VALUE_SEPARATER = '&';

		public string localData;

		[SerializeField] private TextMeshProUGUI nofi;

		// [SerializeField] private MKeyValueUpdateListener[] mKeyValueUpdateListeners;
		[SerializeField] private UdonSharpBehaviour[] mKeyValueUpdateListeners;

		[SerializeField] private TextMeshProUGUI[] debugTexts;

		[UdonSynced]
		[FieldChangeCallback(nameof(GlobalData))]
		private string globalData = "";

		private string LocalData
		{
			get => localData;
			set
			{
				Debug.Log($"Data Changed : {localData} => {value}");

				string globalData = GlobalData;
				SetOwner();
				string totalResult;
				if (!globalData.Contains(Networking.LocalPlayer.displayName))
				{
					if (globalData.Length != 0)
						globalData += DATA_PAIR_SEPARATER;
					globalData += string.Concat(Networking.LocalPlayer.displayName, DATA_SEPARATER, value);
					totalResult = globalData;
				}
				else
				{
					string result = string.Empty;
					string[] dataSets = globalData.Split(DATA_PAIR_SEPARATER);
					if (dataSets.Length == 1)
						result = string.Concat(
							Networking.LocalPlayer.displayName,
							DATA_SEPARATER,
							value);
					else
						foreach (string dataSet in dataSets)
						{
							string[] data = dataSet.Split(DATA_SEPARATER);
							result += string.Concat(
								data[0],
								DATA_SEPARATER,
								data[0] == Networking.LocalPlayer.displayName ? value : data[1],
								DATA_PAIR_SEPARATER);
						}

					totalResult = result;
				}

				GlobalData = totalResult.TrimEnd(DATA_PAIR_SEPARATER, ' ');
				RequestSerialization();
				Debug.Log("LocalData Changed End");
			}
		}

		public string GlobalData
		{
			get => globalData;
			set
			{
				nofi.text = "동기화 완료";

				foreach (UdonSharpBehaviour mKeyValueUpdateListener in mKeyValueUpdateListeners)
					// mKeyValueUpdateListener.BeforeUpdate();
					mKeyValueUpdateListener.SendCustomEvent("BeforeUpdate");

				Debug.Log($"GlobalData Changed : {globalData} => {value}");
				globalData = value;

				foreach (TextMeshProUGUI item in debugTexts)
					item.text = "";
				int textIndex = 0;

				string[] dataPairs = globalData.Split(DATA_PAIR_SEPARATER);
				foreach (string dataPair in dataPairs)
				{
					string _key;
					string _value;

					{
						string[] temp = dataPair.Split(DATA_SEPARATER);
						_key = temp[0];
						_value = temp[1];
					}

					Networking.LocalPlayer.SetPlayerTag(_key, _value);

					string debug = string.Concat(_key, '\n', _value);

					if (_key == Networking.LocalPlayer.displayName)
						localData = _value;

					debugTexts[textIndex++].text = debug;
				}

				Debug.Log("GlobalData Changed End");

				foreach (UdonSharpBehaviour mKeyValueUpdateListener in mKeyValueUpdateListeners)
					// mKeyValueUpdateListener.OnUpdate();
					mKeyValueUpdateListener.SendCustomEvent("OnUpdate");
			}
		}

		private void Start()
		{
			nofi.text = Networking.IsMaster ? "동기화 완료" : "동기화 중";
			if (Networking.IsMaster)
				SetLocalData("Init");
		}

		public void SetLocalData(string _value)
		{
			LocalData = _value;
		}

		public void SetData(string key, string _value)
		{
			if (key == Networking.LocalPlayer.displayName)
			{
				LocalData = _value;
			}
			else
			{
				SetOwner();

				string globalMoneyData = GlobalData;
				string result = string.Empty;
				string[] dataPairs = globalMoneyData.Split(DATA_PAIR_SEPARATER);
				bool contains = false;
				foreach (string dataPair in dataPairs)
				{
					string[] data = dataPair.Split(DATA_SEPARATER);
					if (data[0] == key)
					{
						contains = true;
						data[1] = _value;
					}

					result += string.Concat(data[0], DATA_SEPARATER, data[1], DATA_PAIR_SEPARATER);
				}

				if (contains == false) result += string.Concat(key, DATA_SEPARATER, _value, DATA_PAIR_SEPARATER);

				GlobalData = result.TrimEnd(DATA_PAIR_SEPARATER, ' ');
				RequestSerialization();
			}
		}

		public void AddData(string key, string _value)
		{
			SetOwner();

			if (GlobalData == null || GlobalData == string.Empty)
				return;

			string globalMoneyData = GlobalData;
			string result = string.Empty;
			string[] dataPairs = globalMoneyData.Split(DATA_PAIR_SEPARATER);
			bool contains = false;
			foreach (string dataPair in dataPairs)
			{
				string[] data = dataPair.Split(DATA_SEPARATER);
				if (data[0] == key)
				{
					contains = true;
					data[1] = data[1] + VALUE_SEPARATER + _value;
				}

				result += string.Concat(data[0], DATA_SEPARATER, data[1], DATA_PAIR_SEPARATER);
			}

			if (contains == false) result += string.Concat(key, DATA_SEPARATER, _value, DATA_PAIR_SEPARATER);

			GlobalData = result.TrimEnd(DATA_PAIR_SEPARATER, ' ');
			RequestSerialization();
		}

		// public void SubData(string key, string _value)
		public void SubData(string _value)
		{
			SetOwner();

			if (GlobalData == null || GlobalData == string.Empty)
				return;

			string globalMoneyData = GlobalData;
			string result = string.Empty;
			string[] dataPairs = globalMoneyData.Split(DATA_PAIR_SEPARATER);
			foreach (string dataPair in dataPairs)
			{
				string[] data = dataPair.Split(DATA_SEPARATER);
				// if (data[0] == key)
				{
					string[] values =
						data[1].Contains(VALUE_SEPARATER.ToString())
							? data[1].Split(VALUE_SEPARATER)
							: new[] { data[1] };

					data[1] = "";
					for (int i = 0; i < values.Length; i++)
					{
						if (values[i] == _value)
							continue;

						if (data[1] != "")
							data[1] += VALUE_SEPARATER;

						data[1] += values[i];
					}
				}

				if (data[1] != "")
					result += string.Concat(data[0], DATA_SEPARATER, data[1], DATA_PAIR_SEPARATER);
			}

			GlobalData = result.TrimEnd(DATA_PAIR_SEPARATER, ' ');
			RequestSerialization();
		}
	}
}