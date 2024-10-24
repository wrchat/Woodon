using UdonSharp;
using UnityEngine;
using WRC.Woodon;

namespace Mascari4615.Project.VTuber.HYANG.YejolGyosil
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MBurger : MBase
	{
		[Header("_" + nameof(MBurger))]
		[SerializeField] private MBurgerIngredient[] ingredients;
		[SerializeField] private GameObject[] ingredientActive;
		[SerializeField] private MSFXManager sfxManager;

		[SerializeField] private MValue mValue;
		[SerializeField] private int activeValue = 1;
		private bool IsActiveValue => (mValue == null) || mValue.Value == activeValue;

		[UdonSynced, FieldChangeCallback(nameof(Data))] private string data = string.Empty;
		public string Data
		{
			get => data;
			private set
			{
				if (data.Length < value.Length && IsActiveValue)
					sfxManager.PlaySFX_L(0);

				data = value;
				OnDataChange();
			}
		}

		[SerializeField] private GameObject[] showingResultOffObjects;
		[SerializeField] private MPickup correctResultPickup;
		[SerializeField] private MPickup wrongRecultPickup;

		[SerializeField] private string answer = "0123";

		[UdonSynced, FieldChangeCallback(nameof(IsShowingResult))] private bool _isShowingResult = false;
		public bool IsShowingResult
		{
			get => _isShowingResult;
			private set
			{
				_isShowingResult = value;
				OnShowingResultChanged();
			}
		}

		private void OnShowingResultChanged()
		{
			MDebugLog($"{nameof(OnShowingResultChanged)}, IsShowingResult = {IsShowingResult}");

			foreach (MBurgerIngredient ingredient in ingredients)
				ingredient.SetEnabled(IsShowingResult == false && Data.Contains(ingredient.Index.ToString()) == false);

			bool isDataCorrect = IsDataCorrect();
			correctResultPickup.SetEnabled(IsShowingResult && IsActiveValue && isDataCorrect);
			wrongRecultPickup.SetEnabled(IsShowingResult && IsActiveValue && (isDataCorrect == false));

			if (IsShowingResult && IsActiveValue)
			{
				sfxManager.PlaySFX_L(isDataCorrect ? 1 : 2);
				sfxManager.PlaySFX_L(3);
			}

			foreach (GameObject showingResultOffObject in showingResultOffObjects)
				showingResultOffObject.SetActive(IsShowingResult == false);
		}

		public void ToggleShow()
		{
			SetOwner();
			IsShowingResult = !IsShowingResult;
			RequestSerialization();

			if (IsShowingResult == false)
			{
				correctResultPickup.DropAndRespawn_G();
				wrongRecultPickup.DropAndRespawn_G();
			}
		}

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			for (int i = 0; i < ingredients.Length; i++)
				ingredients[i].Init(i);

			OnDataChange();
			OnShowingResultChanged();
		}

		private void OnDataChange()
		{
			MDebugLog(data.ToString());

			foreach (MBurgerIngredient ingredient in ingredients)
				ingredient.SetEnabled(IsShowingResult == false && IsActiveValue && Data.Contains(ingredient.Index.ToString()) == false);

			foreach (GameObject go in ingredientActive)
				go.SetActive(false);

			string[] ss = data.Split(DATA_SEPARATOR);
			foreach (string s in ss)
			{
				// int index = int.Parse(s);

				if (int.TryParse(s, out int index) == false)
					continue;

				ingredientActive[index].SetActive(true);
				ingredientActive[index].transform.SetAsFirstSibling();
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (IsShowingResult)
				return;

			MDebugLog($"{nameof(OnTriggerEnter)}, other.name = {other.name}");

			for (int i = 0; i < ingredients.Length; i++)
			{
				if (ingredients[i] == null)
					continue;

				if (ingredients[i].gameObject != other.gameObject)
					continue;

				if (IsOwner(ingredients[i].gameObject) == false)
					return;

				MDebugLog($"{nameof(OnTriggerEnter)}, ingredients[i].name = {ingredients[i].name}");
				StackData(i);
				ingredients[i].DropAndRespawn();
				break;
			}
		}

		public void StackData(int index)
		{
			if (Data.Contains(index.ToString()))
				return;

			SetOwner();
			if (Data == string.Empty)
				Data += index.ToString();
			else
				Data += $"{DATA_SEPARATOR}{index}";
			RequestSerialization();
		}

		public void Reset()
		{
			SetOwner();
			Data = string.Empty;
			IsShowingResult = false;
			RequestSerialization();

			foreach (MBurgerIngredient ingredient in ingredients)
				ingredient.DropAndRespawn_G();

			wrongRecultPickup.DropAndRespawn_G();
			correctResultPickup.DropAndRespawn_G();
		}

		public bool IsDataCorrect()
		{
			// Answer의 각 글자가 Data에 포함되어 있는지 확인
			if (data.Length == 0)
				return false;

			// 0@1@2@3
			if (data.Length != 7)
				return false;

			foreach (char c in answer)
			{
				if (Data.Contains(c.ToString()) == false)
					return false;
			}

			return true;
		}
	}
}