﻿using System;
public interface IOccupy
{
    public int Armour { get; }
    public bool BlocksLOS { get; }
    void SetRevealed(bool hide);
    DamageEffectiveness GetDamageEffectiveness(DamageType damageType);

    // returns true if the target was destroyed
    bool TakeDamage(int value);
}
