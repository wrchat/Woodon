using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;

namespace WRC.Woodon
{
	[DefaultExecutionOrder(-100000)]
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class WJson : WString
	{
		public DataDictionary DataDictionary { get; protected set; } = new DataDictionary();
		public DataDictionary ChangedData { get; protected set; } = new DataDictionary();

		[SerializeField] private LogType logTypeWhenFailed = LogType.Error;

		[ContextMenu(nameof(SerializeData))]
		public void SerializeData()
		{
			WDebugLog($"{nameof(SerializeData)} (Try) {DataDictionary}");

			SendEvents(WJsonEvent.OnSerialization);

			if (VRCJson.TrySerializeToJson(DataDictionary, JsonExportType.Beautify, out DataToken result))
			{
				WDebugLog($"{nameof(SerializeData)} (Success) {result}");

				if (Value == result.String)
				{
					WDebugLog($"{nameof(SerializeData)} (Skip Same Value) {Value}");
					return;
				}

				SetValue(result.String);
			}
			else
			{
				WDebugLog(result.ToString(), LogType.Error);
			}
		}

		[ContextMenu(nameof(Deserialization))]
		public void Deserialization()
		{
			WDebugLog($"{nameof(Deserialization)} (Try) {Value}");

			if (Value == string.Empty)
				return;

			if (VRCJson.TryDeserializeFromJson(Value, out DataToken result))
			{
				WDebugLog($"{nameof(Deserialization)} (Success) {result}");

				DataDictionary = result.DataDictionary;
				SendEvents(WJsonEvent.OnDeserialization);
			}
			else
			{
				WDebugLog($"Value:\n{Value} \n\n Result:\n{result}", logTypeWhenFailed);
			}
		}

		public void SetData(string key, DataToken value)
		{
			DataToken keyToken = new DataToken(key);
			DataDictionary.SetValue(keyToken, value);
		}
		public void SetData(string key, int value) => SetData(key, new DataToken(value));
		public void SetData(DataToken key, int value) => SetData(key.String, new DataToken(value));
		public void SetData(string key, string value) => SetData(key, new DataToken(value));
		public void SetData(DataToken key, string value) => SetData(key.String, new DataToken(value));
		public void SetData(string key, bool value) => SetData(key, new DataToken(value));
		public void SetData(DataToken key, bool value) => SetData(key.String, new DataToken(value));
		public void SetData(string key, DataList value) => SetData(key, new DataToken(value));
		public void SetData(DataToken key, DataList value) => SetData(key.String, new DataToken(value));
		public void SetData(string key, DataDictionary value) => SetData(key, new DataToken(value));
		public void SetData(DataToken key, DataDictionary value) => SetData(key.String, new DataToken(value));

		public bool TryGetData(string key, out DataToken value) => DataDictionary.TryGetValue(key, out value);

		private DataToken GetDataToken(string key, DataToken defaultValue)
		{
			DataToken keyToken = new DataToken(key);
			if (DataDictionary.TryGetValue(keyToken, out DataToken value))
			{
				return value;
			}
			else
			{
				DataDictionary.SetValue(key, defaultValue);
				return DataDictionary[key];
			}
		}
		public int GetData(string key, int defaultValue) => GetDataToken(key, new DataToken(defaultValue)).Int();
		public int GetData(DataToken key, int defaultValue) => GetDataToken(key.String, new DataToken(defaultValue)).Int();
		public string GetData(string key, string defaultValue) => GetDataToken(key, new DataToken(defaultValue)).String;
		public string GetData(DataToken key, string defaultValue) => GetDataToken(key.String, new DataToken(defaultValue)).String;
		public bool GetData(string key, bool defaultValue) => GetDataToken(key, new DataToken(defaultValue)).Boolean;
		public bool GetData(DataToken key, bool defaultValue) => GetDataToken(key.String, new DataToken(defaultValue)).Boolean;
		public DataList GetData(string key, DataList defaultValue) => GetDataToken(key, new DataToken(defaultValue)).DataList;
		public DataList GetData(DataToken key, DataList defaultValue) => GetDataToken(key.String, new DataToken(defaultValue)).DataList;
		public DataDictionary GetData(string key, DataDictionary defaultValue) => GetDataToken(key, new DataToken(defaultValue)).DataDictionary;
		public DataDictionary GetData(DataToken key, DataDictionary defaultValue) => GetDataToken(key.String, new DataToken(defaultValue)).DataDictionary;

		protected override void OnValueChange(string origin, string cur)
		{
			base.OnValueChange(origin, cur);

			if (VRCJson.TryDeserializeFromJson(cur, out DataToken result))
				if (VRCJson.TryDeserializeFromJson(origin, out DataToken originResult))
					ChangedData = GetDifference(originResult.DataDictionary, result.DataDictionary);

			Deserialization();
		}

		private DataDictionary GetDifference(DataDictionary originData, DataDictionary newData)
		{
			DataDictionary diff = new DataDictionary();

			DataList keys = newData.GetKeys();
			for (int i = 0; i < keys.Count; i++)
			{
				DataToken key = keys[i];

				if (originData.TryGetValue(key, out DataToken originToken))
				{
					DataToken newToken = newData[key];

					bool isChanged = false;

					isChanged |= originToken.CompareTo(newToken) != 0;
					isChanged |= (originToken.TokenType == TokenType.Boolean) && (originToken.Boolean != newToken.Boolean);

					if (isChanged)
					{
						DataDictionary diffBlock = new DataDictionary();
						diffBlock.SetValue("origin", originToken);
						diffBlock.SetValue("cur", newToken);

						WDebugLog($"{key} {originToken} -> {newToken}");

						diff.SetValue(key, diffBlock);
					}
				}
			}

			return diff;
		}

		private bool HasDataChanged(DataToken key, out DataToken origin, out DataToken cur)
		{
			if (ChangedData.TryGetValue(key, out DataToken diff))
			{
				DataDictionary diffBlock = diff.DataDictionary;
				origin = diffBlock["origin"];
				cur = diffBlock["cur"];
				return true;
			}
			else
			{
				origin = new DataToken(DataError.None);
				cur = new DataToken(DataError.None);
				return false;
			}
		}

		public bool HasDataChanged(DataToken key, out int origin, out int cur)
		{
			if (HasDataChanged(key, out DataToken originToken, out DataToken curToken))
			{
				origin = originToken.Int();
				cur = curToken.Int();
				return true;
			}
			else
			{
				origin = 0;
				cur = 0;
				return false;
			}
		}

		public bool HasDataChanged(DataToken key, out string origin, out string cur)
		{
			if (HasDataChanged(key, out DataToken originToken, out DataToken curToken))
			{
				origin = originToken.String;
				cur = curToken.String;
				return true;
			}
			else
			{
				origin = string.Empty;
				cur = string.Empty;
				return false;
			}
		}

		public bool HasDataChanged(DataToken key, out bool origin, out bool cur)
		{
			if (HasDataChanged(key, out DataToken originToken, out DataToken curToken))
			{
				origin = originToken.Boolean;
				cur = curToken.Boolean;
				return true;
			}
			else
			{
				origin = false;
				cur = false;
				return false;
			}
		}

		public bool HasDataChanged(DataToken key, out DataList origin, out DataList cur)
		{
			if (HasDataChanged(key, out DataToken originToken, out DataToken curToken))
			{
				origin = originToken.DataList;
				cur = curToken.DataList;
				return true;
			}
			else
			{
				origin = new DataList();
				cur = new DataList();
				return false;
			}
		}

		public bool HasDataChanged(DataToken key, out DataDictionary origin, out DataDictionary cur)
		{
			if (HasDataChanged(key, out DataToken originToken, out DataToken curToken))
			{
				origin = originToken.DataDictionary;
				cur = curToken.DataDictionary;
				return true;
			}
			else
			{
				origin = new DataDictionary();
				cur = new DataDictionary();
				return false;
			}
		}
	}
}