using System;
public struct StatBlock
{
    public int Strength;
    public int Dexerity;
    public int Constitution;
    public int Intelligence;
    public int Willpower;
    public int Presence;

    public StatBlock(int strength, int dexerity, int constitution, int intelligence, int willpower, int presence)
    {
        Strength = strength;
        Dexerity = dexerity;
        Constitution = constitution;
        Intelligence = intelligence;
        Willpower = willpower;
        Presence = presence;
    }
}
