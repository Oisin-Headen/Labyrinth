using System;
public class CalculateFromStatsReturn
{
    public readonly int moveAddition;
    public readonly int attackAddition;
    public readonly int attackRangeAddition;

    public CalculateFromStatsReturn(int moveAddition = 0, int attackAddition = 0, int attackRangeAddition = 0)
    {
        this.moveAddition = moveAddition;
        this.attackAddition = attackAddition;
        this.attackRangeAddition = attackRangeAddition;
    }
}
