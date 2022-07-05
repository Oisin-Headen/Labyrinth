using System;
public class BasicWeapon: IObserveCharacters
{
    private string name;
    private int attackIncrease, rangeIncrease;

    public BasicWeapon(string name, int attackIncrease, int rangeIncrease = 0)
    {
        this.name = name;
        this.attackIncrease = attackIncrease;
        this.rangeIncrease = rangeIncrease;
    }


    public CalculateFromStatsReturn WhenCalculatingFromStats()
    {
        return new CalculateFromStatsReturn(attackAddition: attackIncrease, attackRangeAddition: rangeIncrease);
    }
}
