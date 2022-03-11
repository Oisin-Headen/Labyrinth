using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using static Utilities;

public class Character : IOccupy, IEntityModel
{
    private EntityController view;
    private readonly Map map;
    private readonly FieldOfView fov;

    private Space currentSpace;

    private Queue<CardinalDirection> movePath = new Queue<CardinalDirection>();

    public Character(Space space, Map map)
    {
        this.map = map;
        currentSpace = space;
        fov = new FieldOfView(map);
    }

    public void SetView(EntityController view)
    {
        this.view = view;
    }

    public EntityController GetView()
    {
        return view;
    }

    public void MoveToSpace(Space space)
    {
        currentSpace.SetOccupier(null);
        currentSpace = space;
        currentSpace.SetOccupier(this);
        fov.RefreshVisibility(currentSpace.GetCoordinates());
    }

    public Space GetCurrentSpace()
    {
        return currentSpace;
    }

    public void QueueMove(CardinalDirection direction)
    {
        movePath.Enqueue(direction);
    }

    public void MoveReady()
    {
        if(movePath.Count == 0)
        {
            return;
        }

        var newCoords = currentSpace.GetCoordinates().GetCoordinateInDiection(movePath.Dequeue());
        Space newSpace = map.GetSpace(newCoords);

        if(newSpace == null || !newSpace.IsEmpty())
        {
            return;
        }

        MoveToSpace(newSpace);
        view.MoveTo(newSpace.GetView());
    }

    public bool BlocksLOS()
    {
        return false;
    }

    public void SetHidden(bool hide)
    {
        // Do nothing, should never be hidden
        // view.GetComponent<SpriteRenderer>().enabled = hide;
    }
}
