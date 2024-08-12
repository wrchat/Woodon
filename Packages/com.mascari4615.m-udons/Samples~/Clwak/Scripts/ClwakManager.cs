using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class ClwakManager : UdonSharpBehaviour
	{
		[SerializeField] private TextMeshProUGUI eventText;
		[SerializeField] private TextMeshProUGUI curHourText;
		[SerializeField] private TextMeshProUGUI curMinuteText;
		[SerializeField] private Image[] image;
		[SerializeField] private GameObject decoration;
		[SerializeField] private MeshRenderer[] colorChangeMeshrenderers;
		[SerializeField] private int updateDelay = 60;
		private ClwakData[] clwakDatas;

		private void Start()
		{
			clwakDatas = transform.GetComponentsInChildren<ClwakData>();

			if (updateDelay == 60)
			{
				UpdateClwak();
				SendCustomEventDelayedSeconds(nameof(CustomUpdate), 60 - DateTime.Now.Second + .5f);
			}
			else
			{
				CustomUpdate();
			}
		}

		public void CustomUpdate()
		{
			UpdateClwak();
			SendCustomEventDelayedSeconds(nameof(CustomUpdate), updateDelay);
		}

		public void UpdateClwak()
		{
			curHourText.text = DateTime.Now.Hour.ToString("D2");
			curMinuteText.text = DateTime.Now.Minute.ToString("D2");

			foreach (ClwakData clwakData in clwakDatas)
				if (clwakData.Hour == DateTime.Now.Hour % 12 &&
					clwakData.Minute == DateTime.Now.Minute)
				{
					eventText.text = clwakData.data;

					if (clwakData.sprite.Length > 1)
					{
						image[0].sprite = clwakData.sprite[0];
						image[0].gameObject.SetActive(true);
						image[0].rectTransform.anchoredPosition =
							new Vector3(-75, image[0].rectTransform.anchoredPosition.y, 0);
						image[1].sprite = clwakData.sprite[1];
						image[1].gameObject.SetActive(true);
						image[1].rectTransform.anchoredPosition =
							new Vector3(75, image[1].rectTransform.anchoredPosition.y, 0);
					}
					else
					{
						image[0].sprite = clwakData.sprite[0];
						image[0].gameObject.SetActive(true);
						image[0].rectTransform.anchoredPosition =
							new Vector3(0, image[0].rectTransform.anchoredPosition.y, 0);
						image[1].gameObject.SetActive(false);
					}

					foreach (MeshRenderer meshRenderer in colorChangeMeshrenderers)
						meshRenderer.material.SetColor("_Color", clwakData.color);
					decoration.gameObject.SetActive(true);
					return;
				}

			image[0].gameObject.SetActive(false);
			image[1].gameObject.SetActive(false);
			decoration.SetActive(false);

			eventText.text = "";
		}
	}
}