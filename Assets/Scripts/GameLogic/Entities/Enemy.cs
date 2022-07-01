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
        
    }

    public override DamageEffectiveness GetDamageEffectiveness(DamageType damageType)
    {
        return Type.suite[damageType];
    }

    public override bool TakeDamage(int value)
    {
        Health -= value;
        if (Health <= 0)
        {
            // TODO, make sure this removes the controller and model as well. Might have to remove itself from lists in GameManger, once I'm keeping track of them.
            Controller.Die();
            currentSpace.Occupier = null;
            return true;
        }
        return false;
    }
}
