using System;
using UnityEngine;
using VRC.SDKBase;
using VRC.SDK3.Data;

namespace WRC.Woodon
{
	// Util & Helper
	public static class WExtension
	{
		public static void Resize<T>(this T[] originArr, int size)
		{
			T[] newArr = new T[size];

			// 기존 요소 복사
			int copyLength = Math.Min(originArr.Length, size);
			Array.Copy(originArr, 0, newArr, 0, copyLength);

			// 나머지 요소 초기화
			for (int i = originArr.Length; i < size; i++)
				newArr[i] = default;

			originArr = newArr;
		}

		public static void Add<T>(this T[] originArr, T element)
		{
			originArr.Resize(originArr.Length + 1);
			originArr[originArr.Length - 1] = element;
		}

		public static void AddRange<T>(this T[] originArr, T[] elements)
		{
			originArr.Resize(originArr.Length + elements.Length);
			Array.Copy(elements, 0, originArr, originArr.Length - elements.Length, elements.Length);
		}

		public static void Remove<T>(this T[] originArr, T element)
		{
			int index = Array.IndexOf(originArr, element);
			originArr.RemoveAt(index);
		}

		public static void RemoveAt<T>(this T[] originArr, int index)
		{
			if (index < 0 || index >= originArr.Length)
			{
				Debug.LogError($"{nameof(WUtil)}.{nameof(RemoveAt)} : Index out of range");
				return;
			}

			T[] newArr = new T[originArr.Length - 1];
			if (index > 0)
				Array.Copy(originArr, 0, newArr, 0, index);

			if (index < originArr.Length - 1)
				Array.Copy(originArr, index + 1, newArr, index, originArr.Length - index - 1);

			originArr = newArr;
		}

		public static bool ContainsInt(this DataList dataList, int intValue)
		{
			for (int i = 0; i < dataList.Count; i++)
			{
				if ((int)dataList[i].Double == intValue)
					return true;
			}
			return false;
		}

		public static int Int(this DataToken dataToken) => (int)dataToken.Double;
	}
}