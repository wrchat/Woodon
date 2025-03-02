using UnityEngine;

namespace WRC.Woodon
{
	public class WColorUtil
	{
		public static Color GetColor(WColorPreset wColorPreset)
		{
			switch (wColorPreset)
			{
				case WColorPreset.White:
					return Color.white;
				case WColorPreset.WhiteGray:
					return new Color(184f / 255f, 181f / 255f, 185f / 255f);
				case WColorPreset.Gray:
					return new Color(69f / 255f, 68f / 255f, 79f / 255f);
				case WColorPreset.Black:
					return new Color(38f / 255f, 43f / 255f, 68f / 255f);
				case WColorPreset.Red:
					return new Color(177f / 255f, 82f / 255f, 84f / 255f);
				case WColorPreset.Green:
					return new Color(195f / 255f, 210f / 255f, 113f / 255f);
				case WColorPreset.Blue:
					return new Color(76f / 255f, 130f / 255f, 199f / 255f);
				case WColorPreset.Wakgood:
					return new Color(24f / 255f, 69f / 255f, 51f / 255f); // 24 69 51
				case WColorPreset.Ine:
					return new Color(137f / 255f, 55f / 255f, 221f / 255f); // 137 55 221
				case WColorPreset.Jingburger:
					return new Color(239f / 255f, 168f / 255f, 95f / 255f); // 239 168 95
				case WColorPreset.Lilpa:
					return new Color(67f / 255f, 58f / 255f, 99f / 255f); // 67 58 99
				case WColorPreset.Jururu:
					return new Color(253f / 255f, 8f / 255f, 138f / 255f); // 253 8 138
				case WColorPreset.Gosegu:
					return new Color(71f / 255f, 128f / 255f, 195f / 255f); // 71 128 195
				case WColorPreset.Viichan:
					return new Color(150f / 255f, 191f / 255f, 45f / 255f); // 150 191 45
				default:
					return default;
			}
		}

		public static Color GetGreenOrRed(bool boolVar) => GetColorByBool(boolVar, WColorPreset.Green, WColorPreset.Red);
		public static Color GetWhiteOrGray(bool boolVar) => GetColorByBool(boolVar, WColorPreset.White, WColorPreset.Gray);
		public static Color GetBlackOrGray(bool boolVar) => GetColorByBool(boolVar, WColorPreset.Black, WColorPreset.Gray);
		public static Color GetWhiteOrBlack(bool boolVar) => GetColorByBool(boolVar, WColorPreset.White, WColorPreset.Black);

		public static Color GetColorByBool(bool boolVar, WColorPreset trueWColorPreset, WColorPreset falseWColorPreset) => boolVar ? GetColor(trueWColorPreset) : GetColor(falseWColorPreset);
		public static Color GetColorByBool(bool boolVar, Color trueColor, WColorPreset falseWColorPreset) => boolVar ? trueColor : GetColor(falseWColorPreset);
		public static Color GetColorByBool(bool boolVar, WColorPreset trueWColor, Color falseColor) => boolVar ? GetColor(trueWColor) : falseColor;
		public static Color GetColorByBool(bool boolVar, Color trueColor, Color falseColor) => boolVar ? trueColor : falseColor;
	}
}