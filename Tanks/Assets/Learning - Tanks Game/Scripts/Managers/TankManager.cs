﻿using System;
using UnityEngine;

[System.Serializable]
public class TankManager
{
    public string m_Side;
    public Color m_PlayerColor;            
    public bool m_IsAI;
    [HideInInspector] public Transform m_SpawnPoint;         
    [HideInInspector] public int m_PlayerNumber;             
    [HideInInspector] public string m_ColoredPlayerText;
    [HideInInspector] public GameObject m_Instance;          
    [HideInInspector] public int m_Wins;


    private TankMovement m_Movement;       
    private TankShooting m_Shooting;
    private GameObject m_CanvasGameObject;


    public void Setup()
    {
        if (!m_IsAI)
        {
            SetupInputControl();
        }

        SetupColor();
    }


    public void SetupTransform(Transform transform)
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;
    }


    private void SetupColor()
    {
        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_PlayerColor;
        }
    }


    private void SetupInputControl()
    {
        m_Movement = m_Instance.GetComponent<TankMovement>();
        m_Shooting = m_Instance.GetComponent<TankShooting>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        m_Movement.PlayerNumber = m_PlayerNumber;
        m_Shooting.PlayerNumber = m_PlayerNumber;
    }


    public void DisableInputControl()
    {
        if (!m_IsAI)
        {
            m_Movement.enabled = false;
            m_Shooting.enabled = false;
        }

        m_CanvasGameObject.SetActive(false);
    }


    public void EnableInputControl()
    {
        if (!m_IsAI)
        {
            m_Movement.enabled = true;
            m_Shooting.enabled = true;
        }

        m_CanvasGameObject.SetActive(true);
    }


    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
