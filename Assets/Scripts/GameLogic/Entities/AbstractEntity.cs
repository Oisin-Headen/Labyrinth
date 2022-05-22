﻿using System;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public abstract class AbstractEntity : IEntity
{
    private readonly Queue<Space> movePath = new Queue<Space>();

    public EntityController Controller { get; set; }

    public abstract bool BlocksLOS { get; }
    public abstract int Armour { get; }

    protected Space currentSpace;
    protected readonly Map map;

    public AbstractEntity(Map map, Space spawnSpace)
    {
        this.map = map;
        this.currentSpace = spawnSpace;
    }


    public abstract DamageEffectiveness GetDamageEffectiveness(DamageType damageType);
    public abstract bool TakeDamage(int value);


    public void QueueMove(Space space)
    {
        movePath.Enqueue(space);
    }

    public void MoveReady()
    {
        if (movePath.Count == 0)
        {
            return;
        }

        var newSpace = movePath.Dequeue();

        if (newSpace == null || !newSpace.IsEmpty)
        {
            return;
        }

        MoveToSpace(newSpace);
        Controller.MoveTo(newSpace.Controller.gameObject);
    }

    public virtual void MoveToSpace(Space space)
    {
        currentSpace.Occupier = null;
        currentSpace = space;
        currentSpace.Occupier = this;
    }

    public Space GetCurrentSpace()
    {
        return currentSpace;
    }

    public void SetRevealed(bool seen)
    {
        Controller.GetComponent<SpriteRenderer>().enabled = seen;
    }
}
