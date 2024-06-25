using System;
using UnityEngine;

namespace Mascari4615
{
	public class MDataUtil
	{
		public static void ResizeArr<T>(ref T[] originArr, int size)
		{
			Debug.Log($"Sans {nameof(MDataUtil)}.{nameof(ResizeArr)}");

			T[] newArr = new T[size];

			// 기존 요소 복사
			int copyLength = Math.Min(originArr.Length, size);
			Array.Copy(originArr, 0, newArr, 0, copyLength);

			// 나머지 요소 초기화
			for (int i = originArr.Length; i < size; i++)
				newArr[i] = default(T);

			originArr = newArr;
		}

		public static void RemoveAt<T>(ref T[] originArr, int index)
		{
			if (index < 0 || index >= originArr.Length)
			{
				Debug.LogError($"{nameof(MDataUtil)}.{nameof(RemoveAt)} : Index out of range");
				return;
			}

			T[] newArr = new T[originArr.Length - 1];
			if (index > 0)
				Array.Copy(originArr, 0, newArr, 0, index);

			if (index < originArr.Length - 1)
				Array.Copy(originArr, index + 1, newArr, index, originArr.Length - index - 1);

			originArr = newArr;
		}
	}
}