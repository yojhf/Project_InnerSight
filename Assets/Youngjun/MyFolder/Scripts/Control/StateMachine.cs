using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MyPet.AI
{
    // <T>의 State를 관리하는 클래스
    [Serializable]
    public abstract class State<T>
    {
        // 현 State가 등록되어 있는 Machine
        protected StateMachine<T> stateMachine;
        // stateMachine을 가지고 있는 주체
        protected T context;

        // 생성자
        public State() { }

        // 생성후 1회 실행 (초기화)
        public virtual void Init() { }
        // 상태 전환시 상태로 돌아올때 1회 실행
        public virtual void OnEnter() { }
        // 상태 실행 중
        public abstract void Update(float deltaTime);
        // 상태 전환시 상태로 나갈 때 1회 실행
        public virtual void OnExit() { }

        public void SetMachineAndContext(StateMachine<T> _stateMachine, T _context)
        {
            stateMachine = _stateMachine;
            context = _context;

            Init();
        }
    }

    // <T>의 State들을 관리하는 클래스
    public class StateMachine<T>
    {
        // StateMachine을 가지고 있는 주체
        private T context;
        // 현재 상태
        private State<T> curState;

        public State<T> CurState => curState;

        //이전 상태
        private State<T> prevState;

        public State<T> PrevState => prevState;

        // 현재 상태 지속시간
        private float elapsedTimeInState = 0f;
        public  float ElapsedTimeInState => elapsedTimeInState;

        // 등록된 상태를 상태의 키값으로 저장
        private Dictionary<Type, State<T>> states = new Dictionary<Type, State<T>>();

        public StateMachine(T _context, State<T> initState)
        {
            context = _context;
            AddState(initState);
            curState = initState;
            curState.OnEnter();
        }

        // StateMachine에 State 등록
        public void AddState(State<T> state)
        {
            state.SetMachineAndContext(this, context);
            states[state.GetType()] = state;
        }

        // StateMachine에서 State의 업데이트 실행
        public void Update(float deltaTime)
        {
            elapsedTimeInState += deltaTime;

            curState.Update(deltaTime);
        }

        // currentState의 상태 바꾸기
        public R ChangeState<R>() where R : State<T>
        { 
            // 현재 상태와 새로운 상태 비교
            var newType = typeof(R);

            if (curState.GetType() == newType)
                return curState as R;

            // 상태 변경이전
            if (curState != null)
            {
                curState.OnExit();
            }

            prevState = curState;

            curState = states[newType];

            curState.OnEnter();

            elapsedTimeInState = 0f;

            return curState as R;
        }
    }

}
