using System.Collections;
using System.Collections.Generic;
using Entity.Abilities;
using UnityEngine;

namespace Entity.Controllers
{
    public class DefaultHealthEntityController : IHealthEntityController
    {
        public EntityBase p_Owner { get; set; }
        
        public void Initialize(EntityBase ownerEntity)
        {
            p_Owner = ownerEntity;
        }

        public void Update()
        {
            
        }

        public IHealthEntityController.AttackerInfo[] Attackers { get; set; }

        public IHealthEntityController.DamageOutputInfo TakeDamage(EntityBase owner, EntityBase attacker, AbilityBase usedAbility)
        {
            usedAbility.CalculateAbilityDamage(out float potentialPhysicalDamage, out float potentialMagicDamage);

            HealthStat ownerHealthStat = owner.GetEntityStat<HealthStat>();
            ArmorStat ownerArmorStat = owner.GetEntityStat<ArmorStat>();
            MagicResistStat ownerMagicResistStat = owner.GetEntityStat<MagicResistStat>();
            
            ArmorPenetrationStat attackerArmorPenetrationStat = attacker.GetEntityStat<ArmorPenetrationStat>();
            MagicResistPenetrationStat attackerMagicPenetrationStat = attacker.GetEntityStat<MagicResistPenetrationStat>();

            ownerHealthStat.TakeDamage(potentialPhysicalDamage, ownerArmorStat, attackerArmorPenetrationStat, out float physicalDamageTaken);
            ownerHealthStat.TakeDamage(potentialMagicDamage, ownerMagicResistStat, attackerMagicPenetrationStat, out float magicDamageTaken);
            
            //#TODO Notify if health drops at or below 0
            
            IHealthEntityController.DamageOutputInfo damageOutputInfo = new IHealthEntityController.DamageOutputInfo();
            damageOutputInfo.m_DamagedEntity = owner;
            damageOutputInfo.m_DamageMitigated = (potentialPhysicalDamage - physicalDamageTaken) + (potentialMagicDamage - magicDamageTaken);
            damageOutputInfo.m_DamageTaken = physicalDamageTaken + magicDamageTaken;
            return damageOutputInfo;
        }
    }

}