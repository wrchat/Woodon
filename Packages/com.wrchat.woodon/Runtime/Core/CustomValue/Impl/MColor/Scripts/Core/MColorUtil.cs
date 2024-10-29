using UnityEngine;

namespace WRC.Woodon
{
	public class MColorUtil
	{
		public static Color GetColor(MColorPreset mColorPreset)
		{
			switch (mColorPreset)
			{
				case MColorPreset.White:
					return Color.white;
				case MColorPreset.WhiteGray:
					return new Color(184f / 255f, 181f / 255f, 185f / 255f);
				case MColorPreset.Gray:
					return new Color(69f / 255f, 68f / 255f, 79f / 255f);
				case MColorPreset.Black:
					return new Color(38f / 255f, 43f / 255f, 68f / 255f);
				case MColorPreset.Red:
					return new Color(177f / 255f, 82f / 255f, 84f / 255f);
				case MColorPreset.Green:
					return new Color(195f / 255f, 210f / 255f, 113f / 255f);
				case MColorPreset.Blue:
					return new Color(76f / 255f, 130f / 255f, 199f / 255f);
				case MColorPreset.Wakgood:
					return new Color(24f / 255f, 69f / 255f, 51f / 255f); // 24 69 51
				case MColorPreset.Ine:
					return new Color(137f / 255f, 55f / 255f, 221f / 255f); // 137 55 221
				case MColorPreset.Jingburger:
					return new Color(239f / 255f, 168f / 255f, 95f / 255f); // 239 168 95
				case MColorPreset.Lilpa:
					return new Color(67f / 255f, 58f / 255f, 99f / 255f); // 67 58 99
				case MColorPreset.Jururu:
					return new Color(253f / 255f, 8f / 255f, 138f / 255f); // 253 8 138
				case MColorPreset.Gosegu:
					return new Color(71f / 255f, 128f / 255f, 195f / 255f); // 71 128 195
				case MColorPreset.Viichan:
					return new Color(150f / 255f, 191f / 255f, 45f / 255f); // 150 191 45
				default:
					return default;
			}
		}

		public static Color GetGreenOrRed(bool boolVar) => GetColorByBool(boolVar, MColorPreset.Green, MColorPreset.Red);
		public static Color GetWhiteOrGray(bool boolVar) => GetColorByBool(boolVar, MColorPreset.White, MColorPreset.Gray);
		public static Color GetBlackOrGray(bool boolVar) => GetColorByBool(boolVar, MColorPreset.Black, MColorPreset.Gray);
		public static Color GetWhiteOrBlack(bool boolVar) => GetColorByBool(boolVar, MColorPreset.White, MColorPreset.Black);

		public static Color GetColorByBool(bool boolVar, MColorPreset trueMColorPreset, MColorPreset falseMColorPreset) => boolVar ? GetColor(trueMColorPreset) : GetColor(falseMColorPreset);
		public static Color GetColorByBool(bool boolVar, Color trueColor, MColorPreset falseMColorPreset) => boolVar ? trueColor : GetColor(falseMColorPreset);
		public static Color GetColorByBool(bool boolVar, MColorPreset trueMColor, Color falseColor) => boolVar ? GetColor(trueMColor) : falseColor;
		public static Color GetColorByBool(bool boolVar, Color trueColor, Color falseColor) => boolVar ? trueColor : falseColor;
	}
}