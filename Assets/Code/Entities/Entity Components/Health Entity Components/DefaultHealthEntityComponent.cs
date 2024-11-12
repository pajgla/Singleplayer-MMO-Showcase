using System.Collections;
using System.Collections.Generic;
using Entity.Abilities;
using UnityEngine;

namespace Entity.Components
{
    public class DefaultHealthEntityComponent : IHealthEntityComponent
    {
        public IHealthEntityComponent.AttackerInfo[] Attackers { get; set; }

        public IHealthEntityComponent.DamageOutputInfo TakeDamage(EntityBase owner, EntityBase attacker, AbilityBase usedAbility)
        {
            float potentialPhysicalDamage = 0.0f;
            float potentialMagicDamage = 0.0f;
            usedAbility.CalculateAbilityDamage(out potentialPhysicalDamage, out potentialMagicDamage);

            HealthStat ownerHealthStat = owner.GetHealthStat();
            ArmorStat ownerArmorStat = owner.GetArmorStat();
            MagicResistStat ownerMagicResistStat = owner.GetMagicResistStat();
            
            ArmorPenetrationStat attackerArmorPenetrationStat = attacker.GetArmorPenetrationStat();
            MagicResistPenetrationStat attackerMagicPenetrationStat = attacker.GetMagicResistPenetrationStat();

            float physicalDamageTaken = 0.0f;
            ownerHealthStat.TakeDamage(potentialPhysicalDamage, ownerArmorStat, attackerArmorPenetrationStat, out physicalDamageTaken);
            float magicDamageTaken = 0.0f;
            ownerHealthStat.TakeDamage(potentialMagicDamage, ownerMagicResistStat, attackerMagicPenetrationStat, out magicDamageTaken);
            
            IHealthEntityComponent.DamageOutputInfo damageOutputInfo = new IHealthEntityComponent.DamageOutputInfo();
            damageOutputInfo.m_DamagedEntity = owner;
            damageOutputInfo.m_DamageMitigated = (potentialPhysicalDamage - physicalDamageTaken) + (potentialMagicDamage - magicDamageTaken);
            damageOutputInfo.m_DamageTaken = physicalDamageTaken + magicDamageTaken;
            return damageOutputInfo;
        }
    }

}