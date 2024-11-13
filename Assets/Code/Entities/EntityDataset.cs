using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entity
{
    [CreateAssetMenu(fileName = "Entity Dataset", menuName = "Entity/New Entity Dataset", order = 1)]
    public class EntityDataset : ScriptableObject
    {
        //#TODO Add data check method to check for null values and duplicates
        [FormerlySerializedAs("stats")] [SerializeReference]
        public List<EntityStatBase> m_Stats = new List<EntityStatBase>();
        
        //Basic champion information
        public string m_EntityName;
        public string m_EntityDescription;
        public string m_EntityAvatarPath;
        
        //Other info
        public BasicAttackProjectile m_ProjectilePrefab;
        public float m_ProjectileSpeed;
    }
}