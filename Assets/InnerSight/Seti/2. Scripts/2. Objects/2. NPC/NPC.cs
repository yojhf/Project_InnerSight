using UnityEngine;

namespace InnerSight_Seti
{
    public abstract class NPC : MonoBehaviour
    {
        public abstract void Interaction();
        protected abstract void AIBehaviour(NPC_Behaviour npcBehaviour);
    }
}

/*
1. 상호작용 기능 (Interaction)
대화 (Dialogue): 플레이어와의 대화 기능이 필요하지. 대화를 시작하거나 대화 옵션을 제공하는 기능.
상호작용 가능 여부 (CanInteract): NPC가 상호작용 가능한 상태인지 체크하는 기능. 예를 들어 특정 조건(플레이어 레벨, 특정 아이템 보유 등)이 충족되어야 상호작용할 수 있게 하는 것.

2. AI 행동 패턴 (AI Behavior)
일반적인 행동 루틴 (Behavior Routine): 예를 들면 특정 경로를 순찰하거나, 정해진 장소에서 기다리는 행동을 반복하는 기능.
상태 변경 (State Change): AI 상태를 바꾸는 기능. 예를 들어 대화 중, 경계 중, 또는 공격 모드와 같이 상태 변환을 관리하는 기능.
반응 시스템 (Response System): 플레이어나 주변 상황에 따라 반응하는 기능. 예를 들어, 특정 이벤트가 발생하면 대화 내용이나 행동이 변하는 것.

3. 위치와 이동 (Movement and Position)
경로 탐색 (Pathfinding): 플레이어나 지정된 위치로 이동하는 기능. Unity에서라면 NavMesh를 이용해 경로를 계산할 수 있어.
스폰 위치 설정 (Spawn Location): NPC의 초기 생성 위치를 설정하고 관리하는 기능.

4. 상호작용 관련 데이터 (Interaction Data)
퀘스트 제공 및 관리 (Quest Management): 플레이어에게 퀘스트를 제공하고, 퀘스트와 관련된 데이터를 추적하는 기능. 퀘스트가 완료되었을 때 이에 반응하는 것도 포함돼.
아이템 거래 (Item Trade): 플레이어와 아이템을 교환하거나 판매하는 기능. 상점 NPC나 아이템을 제공하는 NPC라면 꼭 필요해.

5. 애니메이션 및 시각적 피드백 (Animation and Visual Feedback)
애니메이션 트리거 (Animation Trigger): 특정 상황에서 실행되는 애니메이션 트리거 기능. 예를 들어 플레이어와 대화할 때 NPC가 손을 흔드는 등의 제스처.
상호작용 표시 (Interaction Indicator): 플레이어가 상호작용 가능한 NPC에 가까워졌을 때 머리 위에 아이콘이 뜨는 등의 시각적 피드백을 주는 기능.

6. 대화 관련 정보 (Dialogue Data)
대화 스크립트 (Dialogue Script): NPC가 사용할 대화 내용이나 대화 옵션을 제공하는 기능.
대화 컨텍스트에 따른 변화 (Context-based Dialogue): 대화가 진행됨에 따라 달라지는 대화 내용, 예를 들어 플레이어의 행동에 따라 대화 선택지가 달라지는 기능.

7. 플레이어와의 관계 (Player Relationship)
호감도 시스템 (Affinity/Favor System): NPC가 플레이어와의 관계에 따라 다른 반응을 보이도록 하는 기능. 예를 들어, 호감도가 높아지면 상호작용이 달라지거나 새로운 대화가 열리는 것.
 */