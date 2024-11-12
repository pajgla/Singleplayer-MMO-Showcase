using Entity;
using Entity.Abilities;
using UnityEngine;

public class PlayerControllerManager : Singleton<PlayerControllerManager>
{
    EntityBase m_BasicAttackTarget = null;

    void Start()
    {
        ControlsManager controlsManager = ControlsManager.Get();
        if (controlsManager == null)
        {
            Debug.LogError("Cannot find ControlsManager");
            return;
        }

        controlsManager.GetInputMapGameplay().Default.MouseRightClick.performed += context => OnMouseRightClick();
    }

    void Update()
    {
        if (m_BasicAttackTarget != null)
        {
            TriggerBasicAttack();
        }
    }

    void OnMouseRightClick()
    {
        ControlsManager controlsManager = ControlsManager.Get();
        if (controlsManager == null)
        {
            Debug.LogError("ControlsManager is not initialized.");
            return;
        }
        
        Vector2 mousePosition = controlsManager.GetInputMapGameplay().Default.MousePosition.ReadValue<Vector2>();
        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out raycastHit))
        {
            //If we hit an entity, attack it
            EntityBase target = raycastHit.transform.GetComponent<EntityBase>();
            if (target != null)
            {
                m_BasicAttackTarget = target;
            }
            else
            {
                //We can clear the target
                m_BasicAttackTarget = null;
                
                //We hit the terrain, move our champion to the position
                MoveChampion(raycastHit.point);
            }
        }
    }

    void TriggerBasicAttack()
    {
        MatchManager matchManager = MatchManager.Get();
        if (matchManager == null)
        {
            Debug.LogError("Cannot find MatchManager");
            return;
        }

        EntityBase ownChampRef = matchManager.GetOwnChampionReference();
        if (ownChampRef == null)
        {
            Debug.LogError("Cannot find own champion reference");
            return;
        }
        
        EntityAbilitySystem entityAbilitySystem = ownChampRef.GetComponent<EntityAbilitySystem>();
        if (entityAbilitySystem == null)
        {
            Debug.LogError("Cannot find entity ability system");
            return;
        }
        
        entityAbilitySystem.PrecastAbility(KeyCode.Mouse1);
    }

    void MoveChampion(Vector3 worldPosition)
    {
        MatchManager matchManager = MatchManager.Get();
        if (matchManager == null)
        {
            Debug.LogError("MatchManager is not initialized.");
            return;
        }
        
        EntityBase ownChampionRef = matchManager.GetOwnChampionReference();
        if (ownChampionRef == null)
        {
            Debug.LogError("Cannot find reference to own champion.");
            return;
        }
        
        ownChampionRef.GoToPosition(worldPosition);
    }
    
    //Getters
    public EntityBase GetBasicAttackTarget()
    {
        return m_BasicAttackTarget;
    }
}
