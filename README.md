![Banner](/Website/banner.png)

# Woodon

- VRChat 컨텐츠 맵에 사용하고 있는 우동/프리팹들을 모은 유니티 패키지입니다.

## How to Use

### 설치

- <vcc://vpm/addRepo?url=https://wrchat.github.io/Woodon/index.json>
- 위 주소에 접속하여 간편하게 `Woodon`을 `VCC (VRChat Creator Conpanion)`에 등록하세요!
- `VCC`에서, `Woodon`를 사용하고자 하는 프로젝트의 `Manage Project` 페이지를 열고, 해당 프로젝트에 `Woodon` 패키지를 추가하세요.

### 사용

- 유니티 에디터의 `Project` 창에서, `Package/Woodon` 폴더를 찾고, `Runtime` 폴더에서 우동/프리팹을 찾아보세요.
- 유니티 에디터의 `Package Manager` 창에서, `Woodon` 패키지를 찾고, `Samples` 페이지에서 샘플들을 찾아보세요.

## Require

아래 에셋은 일부 샘플에서 사용하지만, 반드시 추가할 필요는 없습니다.

- [UdonSharpVideo (Github)](https://github.com/MerlinVR/USharpVideo/releases)

## 제발 내 코드에 훈수하세요

- 자유롭게 수정/사용 가능합니다, 피드백 환영합니다 !

## 기능

### ⭐ _Base

본 라이브러리에서 사용하는 최상위 루트 클래스, Util 클래스 모음

- `MBase` : 최상위 루트 클래스 (직접 상속받아 사용)
- `MEventSender` : 이벤트 발생 시, 다른 오브젝트에게 이벤트 전달

### ⭐ Basic

대부분의 프로젝트에서 공통적으로 사용되는 기능 모음

- `ObjectActive` : 우동 이벤트를 이용하여 or `MBool`과 함께 활용
- `SendEvent` : 특정 이벤트 발생 시, 우동 이벤트 호출.
  - `MEventSender`와 함께 활용
  - (`Interact`, `OnPlayerTriggerEnter`, `KetInput` 등)
- `Teleport` : 단순 텔레포트 기능
- `Waktaverse` : 왁타버스 관련 기능 (왁타버스 멤버 추적/닉네임 불러오기 등)
  - ['어둠속의 칼날'](https://cafe.naver.com/steamindiegame/11576279)님의 `WaktaverseNameChanger`스크립트를 일부 참고했습니다.

### ⭐ MBool

동기화되는 Bool 변수의 값 변화에 따른 Event 호출.  
`_Base/MEventSender`와 함께 활용

### ⭐ MCamera

### ⭐ [MPlayerUdonIndex](https://cafe.naver.com/steamindiegame/14065241)

각 플레이어에게 제한된 범위 내의 고유한 Index 할당

- VRChat에서 제공하는 `PlayerID`는 플레이어가 들어올 때마다 제한없이 계속 커지기 때문에, 플레이어에게 고유한 오브젝트를 할당하는 등의 상황에서 쓰기에 어려움이 있음

### ⭐ MSeat

`다수의 플레이어`, `턴`이 존재하는 시스템 대부분에 응용될 수 있는 기반 제공  
i.e. 경매, 조추첨, 투표, 퀴즈 컨텐츠  

### ⭐ MValue

단순히, 동기화되는 숫자 값 (자잘한 기능들과 함께)  

### ⭐ MSound

- `SFXManager` : 이벤트 혹은 제공되는 UI로 SFX/BGM을 재생해요
- Voice : `VoiceManager`를 중심으로, 일정 간격마다 플레이어들의 보이스 상태 갱신
  - `VoiceSetter` : 플레이어를 특정하여 보이스 상태 갱신 (주로 `MTarget`을 이용한 증폭)
  - `VoiceTagger` : 플레이어를 위치(`VoiceArea`) 혹은 논리적(`VoiceRoom`)으로 구분시켜 `PlayerTag`를 달고, 이를 기반으로 보이스 상태 갱신

### ⭐ [MTarget](https://cafe.naver.com/steamindiegame/8864741)

특정 플레이어의 `PlayerID`를 UI를 통해 특정하여 동기화

### ⭐ MUI

UI 관련

- [`DummyCanvas`](https://cafe.naver.com/steamindiegame/4641015) : 오버레이 UI 조작을 위해 필요한 기능
- [`KoreanKeyboard`](https://cafe.naver.com/steamindiegame/12922263) : VR 플레이어를 위한 한글 키보드
- [`LoadingPanel`](https://karmotrine.booth.pm/items/4330479) : 월드 입장 시 잠깐 이미지 띄우기
- Prefabs : 빠른 UI 제작을 위한 단순 Helper 프리팹
  - `AutoSize ~` : 자식 요소 크기에 따라 배경 크기도 동적으로 변경됨
  - `[Canvas] WorldSpace` : RenderMode WorldSpace, 스케일 .002 로 설정된 캔버스

### ⭐ PosFixer

- `FollowBone` : 특정 플레이어 본 위치에 오브젝트 붙이기

### ⭐ Shooting

### ⭐ 그 외 잡다한 기능들
