using System;

public class Enemy : AbstractEntity, IAmAnEntity
{
    public Enemy(Map map) : base(map)
    {

    }

    public override bool BlocksLOS()
    {
        return true;
    }
}
