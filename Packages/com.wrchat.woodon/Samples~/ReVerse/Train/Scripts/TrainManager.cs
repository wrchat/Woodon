using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using WRC.Woodon;

namespace Mascari4615.Project.ReVerse
{
	public class TrainManager : UdonSharpBehaviour
	{
		[SerializeField] private GameObject trainHead;
		[SerializeField] private TrainCart[] trainCarts;
		[SerializeField] private float trainCartDistance = 1;

		[SerializeField] private Image[] killTrainCartButtonImages;

		[UdonSynced] [FieldChangeCallback(nameof(TrainData))]
		private int trainData = int.MaxValue;

		private int TrainData
		{
			get => trainData;
			set
			{
				int oldData = trainData;
				int newData = value;

				trainData = value;
				UpdateKillButtonsColor();

				if (trainData == int.MaxValue)
				{
					for (int i = 0; i < trainCarts.Length; i++)
						trainCarts[i].Revival();
					SortTrain();
					return;
				}

				for (int i = 0; i < trainCarts.Length; i++)
					if ((newData & (1 << i)) != (oldData & (1 << i)))
						if ((newData & (1 << i)) == 0)
						{
							KillTrainCart_Network(i);
							return;
						}
			}
		}

		private void Start()
		{
			SortTrain();
			UpdateKillButtonsColor();
		}

		public void KillTrainCart0()
		{
			KillTrainCart(0);
		}

		public void KillTrainCart1()
		{
			KillTrainCart(1);
		}

		public void KillTrainCart2()
		{
			KillTrainCart(2);
		}

		public void KillTrainCart3()
		{
			KillTrainCart(3);
		}

		public void KillTrainCart4()
		{
			KillTrainCart(4);
		}

		public void KillTrainCart5()
		{
			KillTrainCart(5);
		}

		public void KillTrainCart6()
		{
			KillTrainCart(6);
		}

		public void KillTrainCart7()
		{
			KillTrainCart(7);
		}

		public void KillTrainCart8()
		{
			KillTrainCart(8);
		}

		public void KillTrainCart9()
		{
			KillTrainCart(9);
		}

		public void KillTrainCart10()
		{
			KillTrainCart(10);
		}

		public void KillTrainCart11()
		{
			KillTrainCart(11);
		}

		public void KillTrainCart12()
		{
			KillTrainCart(12);
		}

		private void KillTrainCart(int targetCartIndex)
		{
			TakeOwner();
			TrainData = TrainData & ~(1 << targetCartIndex);
			RequestSerialization();
		}


		private void KillTrainCart_Network(int targetCartIndex)
		{
			trainCarts[targetCartIndex].Die();
		}

		private void UpdateKillButtonsColor()
		{
			for (int i = 0; i < killTrainCartButtonImages.Length; i++)
				killTrainCartButtonImages[i].color = (trainData & (1 << i)) != 0 ? Color.green : Color.red;
		}

		public void SortTrain_Global()
		{
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(SortTrain));
		}

		public void SortTrain()
		{
			trainHead.transform.localPosition = Vector3.zero;

			int remainTrainCount = 1;
			for (int i = 0; i < trainCarts.Length; i++)
				if ((trainData & (1 << i)) != 0)
				{
					// trainCarts[i].transform.localPosition = Vector3.back * trainCartDistance * remainTrainCount;
					trainCarts[i].MoveTo(Vector3.back * trainCartDistance * remainTrainCount, .5f);
					remainTrainCount++;
				}
		}

		public void ResetTrain()
		{
			TakeOwner();
			TrainData = int.MaxValue;
			RequestSerialization();
		}

		private void TakeOwner()
		{
			if (!Networking.IsOwner(gameObject))
				Networking.SetOwner(Networking.LocalPlayer, gameObject);
		}
	}
}