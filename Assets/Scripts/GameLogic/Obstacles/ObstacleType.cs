using System;
public class ObstacleType
{
    public readonly bool blocksLOS;
    public readonly ResistenceVulnerabiltySuite suite;
    public readonly int maxHealth;
    public readonly int armour;


    private ObstacleType(bool blocksLOS, ResistenceVulnerabiltySuite suite, int maxHealth, int armour)
    {
        this.blocksLOS = blocksLOS;
        this.suite = suite;
        this.maxHealth = maxHealth;
        this.armour = armour;
    }

    private static readonly ResistenceVulnerabiltySuite wallResistences = new ResistenceVulnerabiltySuite(
        slashing:    DamageEffectiveness.Resistent,
        bludgeoning: DamageEffectiveness.Vulnerable,
        piercing:    DamageEffectiveness.Vulnerable,
        seismic:     DamageEffectiveness.Vulnerable,
        acid:        DamageEffectiveness.Vulnerable,
        poison:      DamageEffectiveness.Immune,
        mental:      DamageEffectiveness.Immune);

    public static ObstacleType Wall = new ObstacleType(true, wallResistences, 30, 5);
    public static ObstacleType Gold = new ObstacleType(true, wallResistences, 20, 2);
}
