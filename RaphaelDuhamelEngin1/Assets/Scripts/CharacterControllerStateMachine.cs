using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerStateMachine : MonoBehaviour
{
    public Camera Camera { get; private set; }
    [field:SerializeField]
    public Rigidbody RB { get; private set; }
    [field:SerializeField]
    public Animator Animator { get; set; }

    [field: SerializeField]
    public float AccelerationValue { get; private set; }
    [field: SerializeField]
    public float MaxVelocity { get; private set; }
    [field: SerializeField]
    public float JumpIntensity { get; private set; } = 1000.0f;

    [field: SerializeField]
    public float MaxSideVelocity { get; private set; } = 4f;
    [field: SerializeField]
    public float MaxBackwardVelocity { get; private set; } = 3f;
    [field: SerializeField]
    public float DecelerationValue { get; private set; } = 10f;

    [field: SerializeField]
    public float AirControlSpeed { get; private set; } = 3f;

    [SerializeField]
    private CharacterFloorTrigger m_floorTrigger;
    private CharacterState m_currentState;
    private List<CharacterState> m_possibleStates;

    public bool m_isHit = false;
    public bool m_isStunned = false;
    public bool m_isGettingUp = false;
    public bool m_isStunInAir = false;
    public bool m_isGrounded = false;

    private void Awake()
    {
        m_possibleStates = new List<CharacterState>();
        m_possibleStates.Add(new FreeState());
        m_possibleStates.Add(new JumpState());
        m_possibleStates.Add(new AttackingState());
        m_possibleStates.Add(new HitState());
        m_possibleStates.Add(new FallingState());
        m_possibleStates.Add(new OnGroundState());
        m_possibleStates.Add(new GettingUpState());
        m_possibleStates.Add(new StunInAirState());
    }

    // Start is called before the first frame update
    void Start()
    {
        Camera = Camera.main;

        foreach (CharacterState state in m_possibleStates)
        {
            state.OnStart(this);
        }
        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();
    }

    private void Update()
    {
        m_currentState.OnUpdate();
        TryStateTransition();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_currentState.OnFixedUpdate();
    }

    private void TryStateTransition()
    {
        if (!m_currentState.CanExit())
        {
            return;
        }

        //Je PEUX quitter le state actuel
        foreach (var state in m_possibleStates)
        {
            if (m_currentState == state)
            {
                continue;
            }

            if (state.CanEnter())
            {
                //Quitter le state actuel
                m_currentState.OnExit();
                m_currentState = state;
                //Rentrer dans le state state
                m_currentState.OnEnter();
                return;
            }
        }
    }

    public bool IsInContactWithFloor()
    {
        return m_floorTrigger.IsOnFloor;
    }

    public void UpdateAnimatorValues(Vector2 movementVecValue)
    {
        //Aller chercher ma vitesse actuelle
        //Communiquer directement avec mon Animator

        movementVecValue = new Vector2(movementVecValue.x, movementVecValue.y / MaxVelocity);

        Animator.SetFloat("MoveX", movementVecValue.x);
        Animator.SetFloat("MoveY", movementVecValue.y);
    }

    public bool IsInState<T>() where T : CharacterState
    {
        return m_currentState is T;
    }
}
