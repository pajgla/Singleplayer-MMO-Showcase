using Entity;
using Entity.Abilities;
using UnityEngine;

public class PlayerControlsManager : Singleton<PlayerControlsManager>
{
    private InputMap_Gameplay m_InputMapGameplay;
    EntityBase m_BasicAttackTarget = null;

    EntityBase m_OwnChampionRef = null;

    protected override void Awake()
    {
        base.Awake();
        
        if (m_InputMapGameplay == null)
        {
            m_InputMapGameplay = new InputMap_Gameplay();
            m_InputMapGameplay.Enable();
        }
    }
    
    void Start()
    {
        InputMap_Gameplay.DefaultActions defaultActions = GetInputMapGameplay().Default;
        defaultActions.MouseRightClick.performed += context => OnMouseRightClick();
        defaultActions.CastFirstSpell.performed += context => OnCastFistSpellPressed();
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
        Vector2 mousePosition = GetInputMapGameplay().Default.MousePosition.ReadValue<Vector2>();
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

    void OnCastFistSpellPressed()
    {
        TriggerAbility(KeyCode.Q);
    }

    void TriggerBasicAttack()
    {
        TriggerAbility(KeyCode.Mouse1);
    }

    void MoveChampion(Vector3 worldPosition)
    {
        GetOwnChampionRef()?.GetMovementEntityController()?.MoveTo(worldPosition);
    }

    private EntityBase GetOwnChampionRef()
    {
        if (m_OwnChampionRef == null)
        {
            MatchManager matchManager = MatchManager.Get();
            if (matchManager == null)
            {
                Debug.LogError("MatchManager is not initialized.");
                return null;
            }
        
            EntityBase ownChampionRef = matchManager.GetOwnChampionReference();
            if (ownChampionRef == null)
            {
                Debug.LogError("Cannot find reference to own champion.");
                return null;
            }

            m_OwnChampionRef = ownChampionRef;
        }

        return m_OwnChampionRef;
    }

    private void TriggerAbility(KeyCode keyCode)
    {
        EntityAbilitySystem entityAbilitySystem = GetOwnChampionRef()?.GetComponent<EntityAbilitySystem>();
        if (entityAbilitySystem == null)
        {
            Debug.LogError("Cannot find entity ability system");
            return;
        }
        
        entityAbilitySystem.PrecastAbility(keyCode);
    }
    
    //Getters
    public EntityBase GetBasicAttackTarget()
    {
        return m_BasicAttackTarget;
    }
    
    public InputMap_Gameplay GetInputMapGameplay() { return m_InputMapGameplay; }
}
