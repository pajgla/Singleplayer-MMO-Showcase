using Entity.Abilities;

namespace Entity.Controllers
{
    public interface IHealthEntityController : IEntityController
    {
        public struct AttackerInfo
        {
            public EntityBase m_AttackerEntity;
            public float m_LastAttackTime;
            public AbilityBase m_UsedAbility;
        }

        public struct DamageOutputInfo
        {
            public EntityBase m_DamagedEntity;
            public float m_DamageTaken;
            public float m_DamageMitigated;
        }
        
        public AttackerInfo[] Attackers { get; set; }
        public DamageOutputInfo TakeDamage(EntityBase owner, EntityBase attacker, AbilityBase usedAbility);
    }
}
