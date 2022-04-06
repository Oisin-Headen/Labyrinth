using System;
using UnityEngine;

public class Enemy : AbstractEntity, IAmAnEntity
{
    public EnemyType Type { get; private set; }

    public Enemy(Map map, EnemyType type) : base(map)
    {
        Type = type;
    }

    public override bool BlocksLOS()
    {
        return Type.blocksLOS;
    }
}
