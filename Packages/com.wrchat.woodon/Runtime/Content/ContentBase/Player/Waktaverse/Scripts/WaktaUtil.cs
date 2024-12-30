// Made by 어둠속의칼날
// Thanks to 메르보, 캡틴 설리반

// 24.06.01
// refactor by Mascari4615

using VRC.SDKBase;
using static WRC.Woodon.WUtil;

namespace WRC.Woodon
{
	public static class WaktaUtil
	{
		// 우왁굳
		// 이세계 아이돌
		// 고정 멤버 (기수 - 가나다 순)
		// 고정 멤버 아카데미 (기수 - 가나다 순)

		public const WaktaMember FIRST_MEMBER = WaktaMember.우왁굳;
		public const WaktaMember LAST_MEMBER = WaktaMember.메카_맹기산;

		public const WaktaMember FIRST_ISD_MEMBER = WaktaMember.아이네;
		public const WaktaMember LAST_ISD_MEMBER = WaktaMember.비챤;

		public const WaktaMember FIRST_GOMEM_MEMBER = WaktaMember.곽춘식;
		public const WaktaMember LAST_GOMEM_MEMBER = WaktaMember.노스페라투_호드;

		public const WaktaMember FIRST_GOMEM_ACADEMY_MEMBER = WaktaMember.닌닌;
		public const WaktaMember LAST_GOMEM_ACADEMY_MEMBER = WaktaMember.메카_맹기산;

		public static WaktaMember[] GetMembersByType(WaktaMemberType type)
		{
			int startIndex, endIndex;

			switch (type)
			{
				case WaktaMemberType.Wakgood:
					startIndex = (int)WaktaMember.우왁굳;
					endIndex = (int)WaktaMember.우왁굳;
					break;
				case WaktaMemberType.ISD:
					startIndex = (int)FIRST_ISD_MEMBER;
					endIndex = (int)LAST_ISD_MEMBER;
					break;
				case WaktaMemberType.Gomem:
					startIndex = (int)FIRST_GOMEM_MEMBER;
					endIndex = (int)LAST_GOMEM_MEMBER;
					break;
				case WaktaMemberType.GomemAcademy:
					startIndex = (int)FIRST_GOMEM_ACADEMY_MEMBER;
					endIndex = (int)LAST_GOMEM_ACADEMY_MEMBER;
					break;
				default:
					return new WaktaMember[0];
			}

			int memberCount = endIndex - startIndex + 1;
			WaktaMember[] members = new WaktaMember[memberCount];

			for (int i = 0; i < memberCount; i++)
				members[i] = (WaktaMember)(startIndex + i);

			return members;
		}

		// public static string GetNickname(WaktaMember member)
		// {
		// 	string nickname = member.ToString();

		// 	string[] nicknameParts = nickname.Split('_');
		// 	if (nicknameParts.Length > 1)
		// 	{
		// 		string actualNickname = nicknameParts[0];
		// 		for (int i = 1; i < nicknameParts.Length; i++)
		// 			actualNickname += " " + nicknameParts[i];

		// 		nickname = actualNickname;
		// 	}

		// 	return nickname;
		// }

		public static string GetNickname(WaktaMember member)
		{
			return GetNicknames()[(int)member];
		}

		// public static string[] GetNicknames()
		// {
		// 	string[] nicknames = new string[WAKTA_NICKNAME_COUNT];

		// 	for (int i = 0; i < WAKTA_NICKNAME_COUNT; i++)
		// 	{
		// 		WaktaMember member = (WaktaMember)i;
		// 		nicknames[i] = GetNickname(member);
		// 	}

		// 	return nicknames;
		// }

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

		// TODO: TryGet
		public static WaktaMember GetWaktaMember(string name)
		{
			string[] displayNames = GetDisplayNames();
			string[] nicknames = GetNicknames();

			int memberCount = displayNames.Length;
			for (int i = 0; i < memberCount; i++)
			{
				if ((name == displayNames[i]) ||
					(name == nicknames[i]))
					return (WaktaMember)i;
			}

			return WaktaMember.None;
		}

		public static string GetNickname(string displayName)
		{
			WaktaMember member = GetWaktaMember(displayName);
			return (member == WaktaMember.None) ? string.Empty : GetNickname(member);
		}

		public static string GetDisplayName(WaktaMember index)
		{
			return GetDisplayNames()[(int)index];
		}

		public static string GetDisplayName(string nickname)
		{
			WaktaMember member = GetWaktaMember(nickname);
			return (member == WaktaMember.None) ? string.Empty : GetDisplayName(member);
		}

		public static string GetNicknameByDisplayName(string displayName)
		{
			WaktaMember member = GetWaktaMember(displayName);
			return (member == WaktaMember.None) ? string.Empty : GetNickname(member);
		}

		public static string GetDisplayNameByNickname(string nickname)
		{
			WaktaMember member = GetWaktaMember(nickname);
			return (member == WaktaMember.None) ? string.Empty : GetDisplayName(member);
		}
	}
}
