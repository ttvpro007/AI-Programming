using System;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float m_StartingHealth = 100f;
    

    [Header("UI Properties")]
    public Slider m_Slider;
    public Image m_FillImage;
    public Color m_FullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;


    [Header("FX Properties")]
    public GameObject m_ExplosionPrefab;


    private EntityAudioManager m_ExplosionParticlesAudioManager;
    private AudioSource m_ExplosionSound;
    private ParticleSystem m_ExplosionParticles;
    private float m_CurrentHealth;
    private bool m_Dead;


    private void Awake()
    {
        SetupParticleSystem();
        SetupExplosionSoundFX();
    }


    private void SetupExplosionSoundFX()
    {
        m_ExplosionParticlesAudioManager = m_ExplosionParticles.GetComponent<EntityAudioManager>();
        m_ExplosionSound = m_ExplosionParticlesAudioManager.GetAudioSource("TankExplosion");
    }

    // instantiate particle system and set inactive
    private void SetupParticleSystem()
    {
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionParticles.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI();
    }


    public void TakeDamage(float amount)
    {
        m_CurrentHealth -= amount;
        m_CurrentHealth = Mathf.Max(0f, m_CurrentHealth);

        SetHealthUI();

        if (m_CurrentHealth == 0f && !m_Dead)
            OnDeath();
    }


    private void SetHealthUI()
    {
        m_Slider.value = m_CurrentHealth / m_StartingHealth;

        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_Slider.value);
    }


    private void OnDeath()
    {
        m_Dead = true;

        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);
        m_ExplosionParticles.Play();

        m_ExplosionSound.Play();

        gameObject.SetActive(false);
    }
}