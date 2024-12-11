using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MyPet.AI
{
    // <T>�� State�� �����ϴ� Ŭ����
    [Serializable]
    public abstract class State<T>
    {
        // �� State�� ��ϵǾ� �ִ� Machine
        protected StateMachine<T> stateMachine;
        // stateMachine�� ������ �ִ� ��ü
        protected T context;

        // ������
        public State() { }

        // ������ 1ȸ ���� (�ʱ�ȭ)
        public virtual void Init() { }
        // ���� ��ȯ�� ���·� ���ƿö� 1ȸ ����
        public virtual void OnEnter() { }
        // ���� ���� ��
        public abstract void Update(float deltaTime);
        // ���� ��ȯ�� ���·� ���� �� 1ȸ ����
        public virtual void OnExit() { }

        public void SetMachineAndContext(StateMachine<T> _stateMachine, T _context)
        {
            stateMachine = _stateMachine;
            context = _context;

            Init();
        }
    }

    // <T>�� State���� �����ϴ� Ŭ����
    public class StateMachine<T>
    {
        // StateMachine�� ������ �ִ� ��ü
        private T context;
        // ���� ����
        private State<T> curState;

        public State<T> CurState => curState;

        //���� ����
        private State<T> prevState;

        public State<T> PrevState => prevState;

        // ���� ���� ���ӽð�
        private float elapsedTimeInState = 0f;
        public  float ElapsedTimeInState => elapsedTimeInState;

        // ��ϵ� ���¸� ������ Ű������ ����
        private Dictionary<Type, State<T>> states = new Dictionary<Type, State<T>>();

        public StateMachine(T _context, State<T> initState)
        {
            context = _context;
            AddState(initState);
            curState = initState;
            curState.OnEnter();
        }

        // StateMachine�� State ���
        public void AddState(State<T> state)
        {
            state.SetMachineAndContext(this, context);
            states[state.GetType()] = state;
        }

        // StateMachine���� State�� ������Ʈ ����
        public void Update(float deltaTime)
        {
            elapsedTimeInState += deltaTime;

            curState.Update(deltaTime);
        }

        // currentState�� ���� �ٲٱ�
        public R ChangeState<R>() where R : State<T>
        { 
            // ���� ���¿� ���ο� ���� ��
            var newType = typeof(R);

            if (curState.GetType() == newType)
                return curState as R;

            // ���� ��������
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
