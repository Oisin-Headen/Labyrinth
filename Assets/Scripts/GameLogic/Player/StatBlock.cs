using System;
public struct StatBlock
{
    public int Strength;
    public int Dexerity;
    public int Constitution;
    public int Intelligence;
    public int Presence;

    public StatBlock(int strength, int dexerity, int constitution, int intelligence, int presence)
    {
        Strength = strength;
        Dexerity = dexerity;
        Constitution = constitution;
        Intelligence = intelligence;
        Presence = presence;
    }
}
