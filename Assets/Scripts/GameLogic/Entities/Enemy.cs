using UnityEngine;

public class Enemy : AbstractEntity, IEntity
{
    public EnemyType Type { get; private set; }
    public int Health { get; private set; }

    public override bool BlocksLOS { get { return Type.blocksLOS; } }
    public override int Armour { get { return Type.armour; } }

    public Enemy(Map map, EnemyType type, Space currentSpace) : base(map, currentSpace)
    {
        Type = type;
        Health = type.maxHealth;
    }

    public void Act()
    {
        //TODO change how they act depending on something
        foreach(var space in FieldOfView.GetAllSpacesInSightRange(map, currentSpace.Coordinates, 1))
        {
            if(!space.IsEmpty && space.Occupier.GetType() == typeof(Character))
            {
                int damage = CombatCalculator.CalculateDamage(
                    Type.attackValue,
                    space.Occupier.Armour,
                    space.Occupier.GetDamageEffectiveness(Type.damageType));
                space.Occupier.TakeDamage(damage);
            }
        }
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
