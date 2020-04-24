using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellRotation : MonoBehaviour
{
    private Rigidbody m_ShellRigidbody;


    private void Start()
    {
        m_ShellRigidbody = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        m_ShellRigidbody.rotation = Quaternion.Slerp(m_ShellRigidbody.rotation, Quaternion.FromToRotation(m_ShellRigidbody.transform.forward, m_ShellRigidbody.velocity), 50f * Time.deltaTime) * m_ShellRigidbody.rotation;
    }
}
