using System;
using System.Collections.Generic;

public class ResistenceVulnerabiltySuite
{
    private readonly Dictionary<DamageType, DamageEffectiveness> suite;

    // By default, all damage types are treated normally, with no adjustment.
    public ResistenceVulnerabiltySuite(
        DamageEffectiveness slashing =    DamageEffectiveness.Normal,
        DamageEffectiveness bludgeoning = DamageEffectiveness.Normal,
        DamageEffectiveness piercing =    DamageEffectiveness.Normal,
        DamageEffectiveness fire =        DamageEffectiveness.Normal,
        DamageEffectiveness cold =        DamageEffectiveness.Normal,
        DamageEffectiveness lightning =   DamageEffectiveness.Normal,
        DamageEffectiveness acid =        DamageEffectiveness.Normal,
        DamageEffectiveness sonic =       DamageEffectiveness.Normal,
        DamageEffectiveness seismic =     DamageEffectiveness.Normal,
        DamageEffectiveness poison =      DamageEffectiveness.Normal,
        DamageEffectiveness holy =        DamageEffectiveness.Normal,
        DamageEffectiveness infernal =    DamageEffectiveness.Normal,
        DamageEffectiveness mental =      DamageEffectiveness.Normal,
        DamageEffectiveness force =       DamageEffectiveness.Normal,
        DamageEffectiveness dark =        DamageEffectiveness.Normal,
        DamageEffectiveness death =       DamageEffectiveness.Normal)
    {
        suite = new Dictionary<DamageType, DamageEffectiveness>() {
            { DamageType.Slashing,      slashing },
            { DamageType.Bludgeoning,   bludgeoning },
            { DamageType.Piercing,      piercing},
            { DamageType.Fire,          fire },
            { DamageType.Cold,          cold },
            { DamageType.Lightning,     lightning },
            { DamageType.Acid,          acid },
            { DamageType.Sonic,         sonic },
            { DamageType.Seismic,       seismic },
            { DamageType.Poison,        poison },
            { DamageType.Holy,          holy },
            { DamageType.Infernal,      infernal },
            { DamageType.Mental,        mental },
            { DamageType.Force,         force },
            { DamageType.Dark,          dark },
            { DamageType.Death,         death }
        };
    }

    public DamageEffectiveness this[DamageType type]
    {
        get => suite[type];
    }
}
