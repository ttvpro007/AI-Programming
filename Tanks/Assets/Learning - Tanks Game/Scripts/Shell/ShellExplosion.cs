using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    public float m_MaxDamage = 100f;
    public float m_ExplosionForce = 1000f;
    public float m_ExplosionRadius = 5f;


    private GameObject m_ExplosionFX;
    private ParticleSystem m_ExplosionParticleSystem;
    private EntityAudioManager m_ExplosionAudioManager;
    private AudioSource m_ExplosionSound;
    private Rigidbody m_Rigidbody;


    private Rigidbody tankRigidbody;
    private TankHealth tankHealth;


    private static ObjectPooler m_ObjectPooler;


    private void Start()
    {
        AssignObjectPooler();

        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private static void AssignObjectPooler()
    {
        if (!m_ObjectPooler)
            m_ObjectPooler = ObjectPooler.Instance;
    }


    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

        foreach (Collider c in colliders)
        {
            DamageTarget(c);
        }

        Explode();
    }


    private void DamageTarget(Collider targetCollider)
    {
        tankRigidbody = targetCollider.GetComponent<Rigidbody>();
        if (!tankRigidbody) return;
        tankRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

        tankHealth = tankRigidbody.GetComponent<TankHealth>();
        if (!tankHealth) return;
        tankHealth.TakeDamage(CalculateDamage(tankRigidbody.position));
    }


    private void Explode()
    {
        m_ExplosionFX = m_ObjectPooler.SpawnFromPool("ShellExplosion", transform.position, Quaternion.Euler(-90f, 0f, 0f));

        if (m_ExplosionFX)
        {
            m_ExplosionParticleSystem = m_ExplosionFX.GetComponent<ParticleSystem>();
            m_ExplosionAudioManager = m_ExplosionFX.GetComponent<EntityAudioManager>();

            if (m_ExplosionParticleSystem)
                m_ExplosionParticleSystem.Play();

            if (m_ExplosionAudioManager)
            {
                m_ExplosionSound = m_ExplosionAudioManager.GetAudioSource("ShellExplosion");
                m_ExplosionSound.Play();
            }
        }

        Reset();
    }


    private ParticleSystem GetExplosionParticle(string objectName)
    {
        return m_ObjectPooler.GetGameObject(objectName).GetComponent<ParticleSystem>();
    }


    private EntityAudioManager GetAudioManager(string objectName)
    {
        return m_ObjectPooler.GetGameObject(objectName).GetComponent<EntityAudioManager>();
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        return Mathf.Lerp(0f, m_MaxDamage, (m_ExplosionRadius - Vector3.Distance(transform.position, targetPosition)) / m_ExplosionRadius);
    }


    private void Reset()
    {
        m_Rigidbody.velocity = Vector3.zero;

        if (transform.parent)
        {
            transform.position = transform.parent.position;
            transform.rotation = transform.parent.rotation;
        }

        gameObject.SetActive(false);
    }
}