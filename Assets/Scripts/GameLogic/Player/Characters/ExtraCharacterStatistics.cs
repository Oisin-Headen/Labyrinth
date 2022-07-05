using System;
public class ExtraCharacterStatistics
{
    public int AttackValue { get; set; }
    public int AttackRange { get; set; }

    public int MoveRange { get; set; }
    public int RemainingMovement { get; set; }
    public int RemainingAttacks { get; set; }

    public ExtraCharacterStatistics(StatBlock stats)
    {
        AttackValue = stats.Strength;
        AttackRange = 1;

        MoveRange = stats.Dexerity;

        RemainingAttacks = 1;
        RemainingMovement = MoveRange;
    }
}
