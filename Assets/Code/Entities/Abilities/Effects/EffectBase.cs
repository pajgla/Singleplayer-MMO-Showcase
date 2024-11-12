
namespace Entity.Abilities
{
    public enum EEffectType
    {
        Buff,
        Debuff
    }

    public abstract class EffectBase
    {
        private EffectDataset m_EffectDataset;


        private EntityBase m_Instigator;
        private EntityBase m_Target;
        public abstract void ApplyEffect(EntityBase target, EntityBase instigator);
    }
}
