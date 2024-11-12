using Entity;
using UnityEngine;
using UnityEngine.AI;

public class ControlsManager : Singleton<ControlsManager>
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
