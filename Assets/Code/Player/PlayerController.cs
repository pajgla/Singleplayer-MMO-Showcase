using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    private InputMap_Gameplay m_InputMapGameplay;

    protected override void Awake()
    {
        base.Awake();
        
        if (m_InputMapGameplay == null)
        {
            m_InputMapGameplay = new InputMap_Gameplay();
            m_InputMapGameplay.Enable();
        }
    }
    
    //Getters
    public InputMap_Gameplay GetInputMapGameplay() { return m_InputMapGameplay; }
}
