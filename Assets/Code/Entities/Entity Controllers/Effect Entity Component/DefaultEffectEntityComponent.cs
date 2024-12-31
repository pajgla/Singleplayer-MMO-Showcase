using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Abilities;
using UnityEngine;

namespace Entity.Controllers
{
    public class DefaultEffectEntityComponent : IEffectEntityComponent
    {
        public List<EffectBase> m_ActiveEffects { get; set; }

        public void ApplyNewEffect<T>() where T : EffectBase, new()
        {
            EffectBase newEffect = new T();
            m_ActiveEffects.Add(newEffect);
        }

        public void Update()
        {
            
        }
    }

}