using System;
using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int PlayerNumber { get; set; } = 1;
    public bool IsAI { get; set; } = false;

    
    public Transform m_FireTransform;
    public Slider m_AimSlider;
    public float m_MinLaunchForce = 15f;
    public float m_MaxLaunchForce = 30f;
    public float m_MaxChargeTime = 0.75f;


    private Rigidbody m_ShellRigidbody;
    private AudioSource m_ShootingSound;
    private AudioSource m_ChargingSound;


    private static ObjectPooler m_ObjectPooler;


    private string m_FireButton;
    private float m_CurrentLaunchForce;
    private float m_ChargeSpeed;
    private bool m_Fired;
    private EntityAudioManager m_TankAudioManager;


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        AssignObjectPooler();
        SetupInput();
        SetupAimSlider();
        SetupAudio();

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }


    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
        if (!IsAI)
            CheckInput();
    }


    private static void AssignObjectPooler()
    {
        if (!m_ObjectPooler)
            m_ObjectPooler = ObjectPooler.Instance;
    }


    private void SetupAimSlider()
    {
        m_AimSlider.minValue = m_MinLaunchForce;
        m_AimSlider.maxValue = m_MaxLaunchForce;
        ResetAimSlider();
    }


    private void ResetAimSlider()
    {
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void SetupInput()
    {
        if (IsAI) return;

        m_FireButton = "Fire" + PlayerNumber;
    }


    private void SetupAudio()
    {
        m_TankAudioManager = GetComponent<EntityAudioManager>();
        m_ShootingSound = m_TankAudioManager.GetAudioSource("ShotFiring");
        m_ChargingSound = m_TankAudioManager.GetAudioSource("ShotCharging");
    }


    private void CheckInput()
    {
        if (Input.GetButtonDown(m_FireButton))
        {
            m_Fired = false;
            ResetAimSlider();
            m_ChargingSound.Play();
        }

        if (m_Fired) return;

        if (Input.GetButtonUp(m_FireButton) || m_AimSlider.value == m_MaxLaunchForce)
        {
            Fire();
            ResetAimSlider();
        }

        if (Input.GetButton(m_FireButton) && !m_Fired)
        {
            UpdateAimSliderValue(m_ChargeSpeed * Time.deltaTime);
        }
    }

    private void UpdateAimSliderValue(float amount)
    {
        m_AimSlider.value += amount;
        m_AimSlider.value = Mathf.Min(m_AimSlider.value, m_MaxLaunchForce);
    }

    private void Fire()
    {
        m_Fired = true;

        m_ShootingSound.Stop();
        m_ShellRigidbody = m_ObjectPooler.SpawnFromPool("Shell", m_FireTransform.position, m_FireTransform.rotation).GetComponent<Rigidbody>();
        m_ShellRigidbody.velocity = m_AimSlider.value * m_FireTransform.forward;
        m_ShootingSound.Play();
    }
}