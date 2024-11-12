using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.Components
{
    public interface IEffectEntityComponent
    {
        public List<Abilities.EffectBase> m_ActiveEffects { get; set; }
        public void ApplyNewEffect<T>() where T : Abilities.EffectBase, new();
        public void Update();
    }
}
