using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MData : MString
	{
		public DataDictionary DataDictionary { get; protected set; } = new DataDictionary();

		public void SetData(string key, int value)
		{
			MDebugLog($"{nameof(SetData)}({key}, {value})");
			DataDictionary.SetValue(key, value);
		}

		public void SetData(string key, string value)
		{
			MDebugLog($"{nameof(SetData)}({key}, {value})");
			DataDictionary.SetValue(key, value);
		}

		public void SetData(string key, bool value)
		{
			MDebugLog($"{nameof(SetData)}({key}, {value})");
			DataDictionary.SetValue(key, value);
		}

		[ContextMenu(nameof(SerializeData))]
		public void SerializeData()
		{
			MDebugLog($"{nameof(SerializeData)} (Try) {DataDictionary}");

			// DataDictionary.Clear();
			// jsonData.Clear();

			// AddData(jsonData);

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
				DataDictionary = result.DataDictionary;
				MDebugLog($"{nameof(Deserialization)} (Success) {result}");

				// ParseData(Data);
				// UpdateStuff();

				SendEvents(MDataEvent.OnDeserialization);
			}
			else
			{
				MDebugLog(result.ToString(), LogType.Error);
			}
		}

		protected override void Init()
		{
			RegisterListener(this, nameof(Deserialization));
			base.Init();
		}
	}
}