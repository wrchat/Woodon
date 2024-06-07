
using Mascari4615;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

// Made by 어둠속의칼날
// Thanks to 메르보, 캡틴 설리반

// 24.06.01
// refactor by Mascari4615

public class WaktaverseNickname
{
	// 우왁굳
	// 이세계 아이돌
	// 고정 멤버 (기수 - 가나다 순)
	// 고정 멤버 아카데미 (기수 - 가나다 순)

	// 풍신님 아이디가 2개라서, 실제 멤버 수는 45.
	public const int WAKTA_NICKNAME_COUNT = 46;

	public static string[] GetNicknames()
	{
		return new string[]
		{
			"우왁굳",

			"아이네",
			"징버거",
			"릴파",
			"주르르",
			"고세구",
			"비챤",

			"곽춘식", // 1기
			"권민",
			"김치만두번영택사스가",
			"단답벌레",
			"도파민 박사",
			"뢴트게늄",
			"비밀소녀",
			"비즈니스킴",
			"왁파고",
			"캘리칼리 데이비슨",
			"풍신",
			"풍신",
			"해루석",
			"히키킹",
			"독고혜지", // 2기
			"부정형인간",
			"소피아",
			"이덕수 할아바이",
			"티파니0421",
			"카르나르 융터르", // 3기
			"프리터",
			"노스페라투 호드", // 4기

			"닌닌", // 1기
			"불곰",
			"시리안 레인",
			"아마데우스최",
			"진희",
			"캡틴 설리반",
			"사랑전도사 젠투", // 2기
			"수셈이",
			"철도왕 길버트",
			"미미짱짱세용", // 3기
			"빅토리",
			"데스해머 쵸로키", // 4기
			"아이 쓰께끼",
			"크리즈",
			"버터우스 3세", // 5기
			"성기사 샬롯",
			"메카 맹기산", // 6기
		};
	}

	public static string[] GetDisplayNames()
	{
		return new string[]
		{
			"VRwakgood",

			"INE_아이네",
			"jingburger",
			"LILPAAA",
			"jururu_twitch",
			"gosegu",
			"VIichan",

			"병장 곽춘식", // 1기
			"KwonMin",
			"김치만두번영택사스가",
			"'단답벌레'",
			"Dr Dopamine",
			"roentgenium",
			"비밀소녀",
			"비즈니스킴",
			"왁파고_",
			"Cally Carly Davidson c76f",
			"（풍신）",
			"(풍신)",
			"RUSUK",
			"Hiki King",
			"독고혜지", // 2기
			"부정형인간",
			"-소피아-",
			"이덕수할아바이",
			"TIFFANY0421",
			"카르나르 융터르 39c8", // 3기
			"Freeter",
			"Nosferatu_Hodd", // 4기

			"_닌닌_", // 1기
			"_불곰_",
			"sirian_",
			"아마데우스최",
			"__진희__",
			"캡틴 설리반",
			"사랑전도사 젠투", // 2기
			"soosemi",
			"철도왕 길버트",
			"미미짱짱세용", // 3기
			"_victory",
			"데스해머쵸로키", // 4기
			"아이 쓰께끼",
			"크리즈_",
			"버터우스 3세", // 5기
			"성기사 샬롯",
			"맹기산~" // 6기
		};
	}

	public static int GetIndex(string name)
	{
		string[] displayNames = GetDisplayNames();
		string[] nicknames = GetNicknames();

		int memberCount = displayNames.Length;
		for (int i = 0; i < memberCount; i++)
		{
			if (name == displayNames[i] ||
				name == nicknames[i])
				return i;
		}

		return MBase.NONE_INT;
	}

	public static string GetNickname(int index)
	{
		return GetNicknames()[index];
	}

	public static string GetNickname(string displayName)
	{
		int index = GetIndex(displayName);
		return index != MBase.NONE_INT ? GetNickname(index) : null;
	}

	public static string GetDisplayName(int index)
	{
		return GetDisplayNames()[index];
	}

	public static string GetDisplayName(string nickname)
	{
		int index = GetIndex(nickname);
		return index != MBase.NONE_INT ? GetDisplayName(index) : null;
	}

	public static string GetNicknameByDisplayName(string displayName)
	{
		int index = GetIndex(displayName);
		return index != MBase.NONE_INT ? GetNickname(index) : null;
	}

	public static string GetDisplayNameByNickname(string nickname)
	{
		int index = GetIndex(nickname);
		return index != MBase.NONE_INT ? GetDisplayName(index) : null;
	}
}
