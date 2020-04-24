using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int PlayerNumber { get; set; } = 1;
    public bool IsAI { get; set; } = false;


    [Header("Movement Settings")]
    public float m_Acceleration = 300f;
    public float m_Deceleration = 500f;
    public float m_MaxSpeed = 12f;
    public float m_IdlingTurnSpeed = 90f;
    public float m_DrivingTurnSpeed = 180f;


    [Header("Audio Settings")]
    public float m_EngineIdlingPitch = 0.5f;
    public float m_EngineDrivingPitch = 0.8f;
    public float m_PitchOffsetThreshold = 0.1f;

    
    private string m_MovementAxisName;
    private string m_TurnAxisName;
    private Rigidbody m_Rigidbody;
    private float m_Velocity;
    private float m_MovementInputValue = 0f;
    private float m_TurnInputValue = 0f;
    private float m_OriginalPitch;
    private float m_RandomPitchOffset;
    private AudioSource m_EngineSound;
    

    private EntityAudioManager m_TankAudioManager;


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_TankAudioManager = GetComponent<EntityAudioManager>();
    }


    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        SetupInputAxis();
        SetupEngineAudio();
    }


    private void Update()
    {
        UpdateInputAxis();
        AdjustEngineAudioPitch();
    }


    private void FixedUpdate()
    {
        Move(m_MovementInputValue);
        Turn(m_TurnInputValue, m_MovementInputValue);
    }


    private void SetupInputAxis()
    {
        if (IsAI) return;

        m_MovementAxisName = "Vertical" + PlayerNumber;
        m_TurnAxisName = "Horizontal" + PlayerNumber;
    }


    private void SetupEngineAudio()
    {
        m_EngineSound = m_TankAudioManager.GetAudioSource("Engine");
        m_OriginalPitch = m_EngineSound.pitch;
        m_RandomPitchOffset = Random.Range(0f, m_PitchOffsetThreshold);
        AdjustEngineAudioPitch();
        m_EngineSound.Play();
    }


    private void UpdateInputAxis()
    {
        if (IsAI) return;

        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
    }


    private void AdjustEngineAudioPitch()
    {
        //if (Mathf.Sign(m_MovementInputValue) == Mathf.Sign(m_Velocity))
        //    m_EngineSound.pitch = LerpBasedOnVelocity(m_EngineIdlingPitch, m_EngineDrivingPitch) + m_RandomPitchOffset;
        //else
            m_EngineSound.pitch = LerpBasedOnInput(m_EngineIdlingPitch, m_EngineDrivingPitch) + m_RandomPitchOffset;
    }


    private float LerpBasedOnVelocity(float a, float b)
    {
        return Mathf.Lerp(a, b, Mathf.InverseLerp(0f, m_MaxSpeed, Mathf.Abs(m_Velocity)));
    }


    private float LerpBasedOnInput(float a, float b)
    {
        return Mathf.Lerp(a, b, Mathf.InverseLerp(-1.5f, 1.5f, Mathf.Clamp(Mathf.Abs(m_MovementInputValue) + Mathf.Abs(m_TurnInputValue), -1.5f, 1.5f)));
    }


    public void Move(float moveInputValue)
    {
        // if has input
        if (moveInputValue != 0)
        {
            // accelerating motion
            m_Velocity += m_Acceleration * Time.deltaTime * Time.deltaTime * moveInputValue;

            // if inertia is opposite of input
            if (Mathf.Sign(moveInputValue) != Mathf.Sign(m_Velocity))
            {
                // add deceleration to make vehicle change direction faster
                m_Velocity += m_Deceleration * Time.deltaTime * Time.deltaTime * moveInputValue;
            }

            m_Velocity = moveInputValue > 0f ? Mathf.Min(m_Velocity, m_MaxSpeed) : Mathf.Max(m_Velocity, -m_MaxSpeed);
        }
        // if has no input
        else
        {
            // if velocity is positive
            if (m_Velocity > 0)
            {
                // subtract delceleration from velocity and clamp to 0
                m_Velocity -= m_Deceleration * Time.deltaTime * Time.deltaTime;
                m_Velocity = Mathf.Max(0f, m_Velocity);
            }
            // if velocity is negative
            else if (m_Velocity < 0)
            {
                // add deceleration to velocity and clamp to 0
                m_Velocity += m_Deceleration * Time.deltaTime * Time.deltaTime;
                m_Velocity = Mathf.Min(0f, m_Velocity);
            }
        }

        Vector3 movement = transform.forward * m_Velocity * Time.deltaTime;

        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }


    public void Turn(float turnInputValue, float moveInputValue)
    {
        // lerp from idling turn speed to driving turn speed or backward based on current velocity
        float turn = LerpBasedOnVelocity(m_IdlingTurnSpeed, m_DrivingTurnSpeed) * Time.deltaTime;

        // turn to same direction as input
        // eg. if forward and left is pressed, go foward to the left
        // and if backward and left is pressed, go backward to the left
        turn *= turnInputValue * Mathf.Sign(moveInputValue);

        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }
}