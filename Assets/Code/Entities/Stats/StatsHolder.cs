using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


namespace Entity
{
    [Serializable]
    public class StatsHolder
    {
        Dictionary<System.Type, EntityStatBase> m_Stats = new Dictionary<System.Type, EntityStatBase>();
        
        public T GetStat<T>() where T : EntityStatBase
        {
            return m_Stats[typeof(T)] as T;
        }

        public void InitializeStatsFromDataset(EntityDataset dataset)
        {
            if (dataset == null)
            {
                Debug.LogError("Null dataset provided");
                return;
            }

            foreach (EntityStatBase stat in dataset.m_Stats)
            {
                //Create a new instance of type 'stat' instead of creating EntityStatBase
                EntityStatBase statCopy = (EntityStatBase)Activator.CreateInstance(stat.GetType(), stat);
                m_Stats.Add(statCopy.GetType(), statCopy);
            }
        }
    }
}
