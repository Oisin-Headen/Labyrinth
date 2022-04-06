using System;

public class EnemyType
{
    public readonly bool blocksLOS;

    private EnemyType(bool blocksLOS)
    {
        this.blocksLOS = blocksLOS;
    }

    public static EnemyType FlameSentinal = new EnemyType(false);
}
