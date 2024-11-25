using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MData : MString
	{
		public DataDictionary DataDictionary { get; protected set; } = new DataDictionary();
		public DataDictionary ChangedData { get; protected set; } = new DataDictionary();

		public void SetData(DataToken key, DataToken value)
		{
			MDebugLog($"{nameof(SetData)}({key}, {value})");
			DataDictionary.SetValue(key, value);
		}

		[ContextMenu(nameof(SerializeData))]
		public void SerializeData()
		{
			MDebugLog($"{nameof(SerializeData)} (Try) {DataDictionary}");

			SendEvents(MDataEvent.OnSerialization);

			if (VRCJson.TrySerializeToJson(DataDictionary, JsonExportType.Beautify, out DataToken result))
			{
				MDebugLog($"{nameof(SerializeData)} (Success) {result}");
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
				SendEvents(MDataEvent.OnDeserialization);
			}
			else
			{
				MDebugLog(result.ToString(), LogType.Error);
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

		private DataDictionary GetDifference(DataDictionary origin, DataDictionary cur)
		{
			DataDictionary diff = new DataDictionary();

			DataList keys = cur.GetKeys();
			for (int i = 0; i < keys.Count; i++)
			{
				DataToken key = keys[i];

				if (origin.TryGetValue(key, out DataToken originToken))
				{
					DataToken curToken = cur[key];
					if (originToken.CompareTo(curToken) != 0)
					{
						DataDictionary diffBlock = new DataDictionary();
						diffBlock.SetValue("origin", originToken);
						diffBlock.SetValue("cur", curToken);

						MDebugLog($"{key} {originToken} -> {curToken}");

						diff.SetValue(key, diffBlock);
					}
				}
			}

			return diff;
		}
	}
}