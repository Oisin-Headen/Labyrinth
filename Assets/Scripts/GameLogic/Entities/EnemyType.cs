using System;

public class EnemyType
{
    public readonly bool blocksLOS;
    public int attackValue;
    public DamageType damageType;
    public readonly int armour;
    public readonly int maxHealth;
    public readonly ResistenceVulnerabiltySuite suite;

    private EnemyType(bool blocksLOS, int attackValue, DamageType damageType, int armour, int maxHealth, ResistenceVulnerabiltySuite suite)
    {
        this.blocksLOS = blocksLOS;
        this.attackValue = attackValue;
        this.damageType = damageType;
        this.armour = armour;
        this.maxHealth = maxHealth;
        this.suite = suite;
    }

    private static readonly ResistenceVulnerabiltySuite flameGolemResitences = new ResistenceVulnerabiltySuite(
        fire: DamageEffectiveness.Immune);

    public static EnemyType FlameSentinal = new EnemyType(false, 3, DamageType.Fire, 0, 10, flameGolemResitences);
}
