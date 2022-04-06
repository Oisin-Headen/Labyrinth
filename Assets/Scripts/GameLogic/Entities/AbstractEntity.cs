using System;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public abstract class AbstractEntity : IAmAnEntity
{
    private readonly Queue<CardinalDirection> movePath = new Queue<CardinalDirection>();

    public EntityController View { get; set; }

    protected Space currentSpace;
    protected readonly Map map;

    public AbstractEntity(Map map)
    {
        this.map = map;
    }


    public void QueueMove(CardinalDirection direction)
    {
        movePath.Enqueue(direction);
    }

    public void MoveReady()
    {
        if (movePath.Count == 0)
        {
            return;
        }

        var newCoords = currentSpace.coordinates.GetCoordinateInDirection(movePath.Dequeue());
        Space newSpace = map.GetSpace(newCoords);

        if (newSpace == null || !newSpace.IsEmpty())
        {
            return;
        }

        MoveToSpace(newSpace);
        View.MoveTo(newSpace.GetView());
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


    public abstract bool BlocksLOS();

    public void SetRevealed(bool seen)
    {
        View.GetComponent<SpriteRenderer>().enabled = seen;
    }
}
