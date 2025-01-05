using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class WJson : MString
	{
		public DataDictionary DataDictionary { get; protected set; } = new DataDictionary();
		public DataDictionary ChangedData { get; protected set; } = new DataDictionary();

		[SerializeField] private LogType logTypeWhenFailed = LogType.Error;

		[ContextMenu(nameof(SerializeData))]
		public void SerializeData()
		{
			MDebugLog($"{nameof(SerializeData)} (Try) {DataDictionary}");

			SendEvents(WJsonEvent.OnSerialization);

			if (VRCJson.TrySerializeToJson(DataDictionary, JsonExportType.Beautify, out DataToken result))
			{
				MDebugLog($"{nameof(SerializeData)} (Success) {result}");

				if (Value == result.String)
				{
					MDebugLog($"{nameof(SerializeData)} (Skip Same Value) {Value}");
					return;
				}

				SetValue(result.String);
			}
			else
			{
				MDebugLog(result.ToString(), LogType.Error);
			}
		}

		[ContextMenu(nameof(Deserialization))]
		public void Deserialization()
		{
			MDebugLog($"{nameof(Deserialization)} (Try) {Value}");

			if (Value == string.Empty)
				return;

			if (VRCJson.TryDeserializeFromJson(Value, out DataToken result))
			{
				MDebugLog($"{nameof(Deserialization)} (Success) {result}");

				DataDictionary = result.DataDictionary;
				SendEvents(WJsonEvent.OnDeserialization);
			}
			else
			{
				MDebugLog($"Value:\n{Value} \n\n Result:\n{result}", logTypeWhenFailed);
			}
		}

		public bool TryGetData(DataToken key, out DataToken value) => DataDictionary.TryGetValue(key, out value);
		public void SetData(DataToken key, DataToken value) => DataDictionary.SetValue(key, value);

		public DataToken GetData(DataToken key) => DataDictionary[key];
		public int GetData(DataToken key, int defaultValue)
		{
			if (DataDictionary.TryGetValue(key, out DataToken value))
			{
				return (int)value.Double;
			}
			else
			{
				DataToken defaultToken = new DataToken(defaultValue);
				DataDictionary.SetValue(key, defaultToken);
				return defaultToken.Int();
			}
		}

		public string GetData(DataToken key, string defaultValue)
		{
			if (DataDictionary.TryGetValue(key, out DataToken value))
			{
				return value.String;
			}
			else
			{
				DataToken defaultToken = new DataToken(defaultValue);
				DataDictionary.SetValue(key, defaultToken);
				return defaultToken.String;
			}
		}

		public bool GetData(DataToken key, bool defaultValue)
		{
			if (DataDictionary.TryGetValue(key, out DataToken value))
			{
				return value.Boolean;
			}
			else
			{
				DataToken defaultToken = new DataToken(defaultValue);
				DataDictionary.SetValue(key, defaultToken);
				return defaultToken.Boolean;
			}
		}

		public DataList GetData(DataToken key, DataList defaultValue)
		{
			if (DataDictionary.TryGetValue(key, out DataToken value))
			{
				return value.DataList;
			}
			else
			{
				DataToken defaultToken = new DataToken(defaultValue);
				DataDictionary.SetValue(key, defaultToken);
				return defaultToken.DataList;
			}
		}

		public DataDictionary GetData(DataToken key, DataDictionary defaultValue)
		{
			if (DataDictionary.TryGetValue(key, out DataToken value))
			{
				return value.DataDictionary;
			}
			else
			{
				DataToken defaultToken = new DataToken(defaultValue);
				DataDictionary.SetValue(key, defaultToken);
				return defaultToken.DataDictionary;
			}
		}

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

						MDebugLog($"{key} {originToken} -> {newToken}");

						diff.SetValue(key, diffBlock);
					}
				}
			}

			return diff;
		}

		public bool HasDataChanged(DataToken key, out DataToken origin, out DataToken cur)
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
				origin = (int)originToken.Double;
				cur = (int)curToken.Double;
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