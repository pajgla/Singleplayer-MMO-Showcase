using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.Abilities
{
    [CreateAssetMenu(fileName = "New Effect Dataset", menuName = "Entity/Abilities/New Effect Dataset")]
    public class EffectDataset : ScriptableObject
    {
        public string m_EffectName;
        public string m_EffectDescription;
        EEffectType m_EffectType;
    }
}
