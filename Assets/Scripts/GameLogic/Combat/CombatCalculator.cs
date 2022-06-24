using UnityEngine;
public static class CombatCalculator
{
    public static int CalculateDamage(int attack, int armour, DamageEffectiveness effectiveness)
    {
        int armourReduction = 100 - (5 * armour);
        int damageRange = UnityEngine.Random.Range(75, 126);

        var damageValue = attack * effectiveness.GetNumericalValue() * armourReduction * damageRange / 100 / 100 / 100;

        // Currently logs debug, will display in game eventually. TODO
        Debug.Log(string.Format("Attacked for: {0} damage. Attack: {1}. Armour Reduction: {2}. Damage Swing: {3}", damageValue, attack, armourReduction, damageRange));


        return damageValue;
    }

    public static int GetNumericalValue(this DamageEffectiveness effectiveness)
    {
        switch (effectiveness)
        {
            case DamageEffectiveness.Normal:
                return 100;
            case DamageEffectiveness.Immune:
                return 0;
            case DamageEffectiveness.Resistent:
                return 75;
            case DamageEffectiveness.Vulnerable:
                return 125;
            default:
                return 100;
        }
    }
}
