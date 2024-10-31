using Entity;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControlsManager : Singleton<PlayerControlsManager>
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
        
        m_InputMapGameplay.Default.MouseRightClick.performed += context => OnMouseRightClick();
    }
    
    void OnMouseRightClick()
    {
        MatchManager matchManager = MatchManager.Get();
        if (matchManager == null)
        {
            Debug.LogError("MatchManager is not initialized.");
            return;
        }
        
        PlayerControlsManager playerControlsManager = PlayerControlsManager.Get();
        if (playerControlsManager == null)
        {
            Debug.LogError("PlayerControlsManager is not initialized.");
            return;
        }
        
        EntityBase ownChampionRef = matchManager.GetOwnChampionReference();
        if (ownChampionRef == null)
        {
            Debug.LogError("Cannot find reference to own champion.");
            return;
        }
        
        Vector2 mousePosition = playerControlsManager.GetInputMapGameplay().Default.MousePosition.ReadValue<Vector2>();
        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out raycastHit))
        {
            //If we hit an entity, attack it
            EntityBase target = raycastHit.transform.GetComponent<EntityBase>();
            if (target != null)
            {
                ownChampionRef.SetTarget(target);
            }
            else
            {
                //We hit the terrain, move our champion to the position
                ownChampionRef.GoToPosition(raycastHit.point);
            }
        }
    }
    
    //Getters
    public InputMap_Gameplay GetInputMapGameplay() { return m_InputMapGameplay; }
}
