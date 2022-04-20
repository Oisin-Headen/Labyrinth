using UnityEngine;

public class Enemy : AbstractEntity, IEntity
{
    public EnemyType Type { get; private set; }
    public int Health { get; private set; }

    public override bool BlocksLOS { get { return Type.blocksLOS; } }
    public override int Armour { get { return Type.armour; } }

    public Enemy(Map map, EnemyType type) : base(map)
    {
        Type = type;
        Health = type.maxHealth;
    }

    public override DamageEffectiveness GetDamageEffectiveness(DamageType damageType)
    {
        return Type.suite.suite[damageType];
    }

    public override bool TakeDamage(int value)
    {
        Health -= value;
        if (Health <= 0)
        {
            Object.Destroy(Controller.gameObject);
            return true;
        }
        return false;
    }
}
