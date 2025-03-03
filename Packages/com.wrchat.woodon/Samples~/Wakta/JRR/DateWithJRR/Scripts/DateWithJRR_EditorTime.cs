
using System.Collections.Generic;
using System.Linq;
using UdonSharp;
using UnityEditor;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

public class DateWithJRR_EditorTime : MonoBehaviour
{
#if UNITY_EDITOR
	[SerializeField] private string urlPrefix = "http://";
	// 확장자
	[SerializeField] private string extension = ".mp4";

	[ContextMenu("Update Data")]
	public void UpdateData()
	{
		List<VideoData> videos = transform.GetComponentsInChildren<VideoData>(true).ToList();

		foreach (VideoData video in videos)
		{
			string name = video.gameObject.name;
			// ASD [1-1]
			// ASD [1-2]
			// ASD [1-3]
			// 에서 뒤에 숫자 부분 추출
			string part = name.Substring(name.IndexOf('[') + 1, name.IndexOf(']') - name.IndexOf('[') - 1);
			string[] parts = part.Split('-');

			video.Name = part;
			video.Name = part;

			if (parts.Length != 2)
			{
				Debug.LogWarning($"Invalid name: {name}");
				EditorUtility.SetDirty(video);
				continue;
			}

			int mainNumber = int.Parse(parts[0]);
			int subNumber = int.Parse(parts[1]);

			string mainNumberString = mainNumber.ToString("D2");
			string subNumberString = subNumber.ToString("D2");

			// VIi: string newURL = $"{urlPrefix}{mainNumberString}/{mainNumberString}_{subNumberString}{extension}";
			string newURL = $"{urlPrefix}/{mainNumberString}_{subNumberString}{extension}";
			VRCUrl newVRCUrl = new VRCUrl(newURL);

			video.SetVRCUrl(newVRCUrl);

			// SetDirty
			EditorUtility.SetDirty(video);
		}

		// Save
		AssetDatabase.SaveAssets();
	}
#endif
}
