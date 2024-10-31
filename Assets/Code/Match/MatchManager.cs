using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : Singleton<MatchManager>
{
    //#TODO Use champion class type once available
    [SerializeField] private Entity.EntityBase m_SelectedChampionPrefab;
    [SerializeField] private Transform m_SpawnLocation;

    private Entity.EntityBase m_SelectedChampionReference;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (m_SelectedChampionPrefab == null)
        {
            Debug.LogError("Selected Champion is null!");
            return;
        }

        m_SelectedChampionReference = Instantiate(m_SelectedChampionPrefab, m_SpawnLocation.position, m_SpawnLocation.rotation);
    }

    public Entity.EntityBase GetOwnChampionReference()
    {
        return m_SelectedChampionReference;
    }
}
