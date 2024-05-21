# VRC_MUdons

- 왁타버스 VRChat 컨텐츠 맵에 사용하고 있는 기능들을 모았습니다.

- 성능을 일부 포기히고, 개발 편의/재사용에 중점을 뒀습니다.
  -  U#을 사용하지만, UdonGraph 같이 Event (C# 리플렉션/Unity 메시지 브로드캐스트) 기반으로 구현 
  -  Static 사용에 제한이 있기에, Util/공통적인 기능을 모아둔 클래스를 직접 상속

## Require

- [UdonSharpVideo](https://github.com/MerlinVR/USharpVideo/releases) : MUI/MFullScreen, VideoPlayerController 등
- [QvPen](https://booth.pm/ja/items/1555789) : CloverSketchbook, RusukBar 등

## 제발 내 코드에 훈수하세요

- 자유롭게 수정/사용 가능합니다, 피드백 환영합니다 !

## 기능

### ⭐ MBase
자주 사용하는 코드 블럭 모음 (직접 상속받아 사용)

### ⭐ Basic
대부분의 프로젝트에서 공통적으로 사용되는 기능 모음
- ObjectActive : 우동 이벤트를 이용하여 or `MBool`과 함께 활용
- SendEvent : 특정 이벤트(`Interact`, `OnPlayerTriggerEnter`, `KetInput` 등) 발생 시, 우동 이벤트 호출. `MEventSender`와 함께 활용
- Teleport : 단순 텔레포트

### ⭐ MBool
동기화되는 Bool 변수의 값 변화에 따른 Event 호출. `MEventSender`와 함께 활용

### ⭐ [MPlayerUdonIndex](https://cafe.naver.com/steamindiegame/14065241)
각 플레이어에게 제한된 범위 내의 고유한 Index 할당
- VRChat에서 제공하는 `PlayerID`는 플레이어가 들어올 때마다 제한없이 계속 커지기 때문에, 플레이어에게 고유한 오브젝트를 할당하는 등의 상황에서 쓰기에 어려움이 있음
 
### ⭐ MQuiz
퀴즈 관련 컨텐츠 대부분에 응용될 수 있는 기반 제공

### ⭐ MScore
동기화되는 점수판 (자잘한 기능들과 함께)

### ⭐ MSound
- SFXManager : 이벤트 혹은 제공되는 UI로 SFX/BGM을 재생해요
- Voice : `VoiceManager`를 중심으로, 일정 간격마다 플레이어들의 보이스 상태 갱신
  - VoiceSetter : 플레이어를 특정하여 보이스 상태 갱신 (주로 `MTarget`을 이용한 증폭)
  - VoiceTagger : 플레이어를 위치(`VoiceArea`) 혹은 논리적(`VoiceRoom`)으로 구분시켜 `PlayerTag`를 달고, 이를 기반으로 보이스 상태 갱신

### ⭐ [MTarget](https://cafe.naver.com/steamindiegame/8864741)
특정 플레이어의 `PlayerID`를 UI를 통해 특정하여 동기화

### ⭐ MUI
UI 관련
- [DummyCanvas](https://cafe.naver.com/steamindiegame/4641015) : 오버레이 UI 조작을 위해 필요한 기능
- [KoreanKeyboard](https://cafe.naver.com/steamindiegame/12922263) : VR 플레이어를 위한 한글 키보드
- [LoadingPanel](https://karmotrine.booth.pm/items/4330479) : 월드 입장 시 잠깐 이미지 띄우기
- Prefabs : UI 제작을 위한 단순 Helper 프리팹
  - Flexible ~ : 자식 요소 크기에 따라 배경 크기도 동적으로 변경됨
  - WorldSpaceCanvas : RenderMode WorldSpace, 스케일 .002 로 설정된 캔버스

### ⭐ PosFixer
- FollowBone : 특정 플레이어 본 위치에 오브젝트 붙이기

### ⭐ Project
위 기능들을 활용하여, 특정 프로젝트를 위해 만들어진 @기능/프리팹 모음

기능은 꾸준히 디벨롭 했지만,  
오래된 기능/프리팹들은 대부분 유지보수를 하지 않아서 Missing이 뜨거나 제대로 작동하지 않는 경우가 많음 (하하)

### ⭐ 그 외 잡다한 것들..

