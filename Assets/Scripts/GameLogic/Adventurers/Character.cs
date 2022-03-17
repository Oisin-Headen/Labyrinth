using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using static Utilities;

public class Character : IOccupy, IAmAnEntity, IViewSpaces
{
    private EntityController view;
    private readonly Map map;
    private ISet<Space> spacesInView = new HashSet<Space>();

    // TODO Characters should be expanded, with a Stat class and a 'Class' class.
    // Stat variable;
    private int viewRange = 5;

    private Space currentSpace;

    private readonly Queue<CardinalDirection> movePath = new Queue<CardinalDirection>();

    public Character(Space space, Map map)
    {
        this.map = map;
        currentSpace = space;
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
        currentSpace.Occupier = null;
        currentSpace = space;
        currentSpace.Occupier = this;
        var coords = currentSpace.coordinates;


        foreach(var oldSpaceInView in spacesInView)
        {
            oldSpaceInView.RemoveViewer(this);
        }
        spacesInView = FieldOfView.GetAllSpacesInSightRange(map, coords, viewRange);
        spacesInView.Add(currentSpace);

        foreach (var newSpaceInView in spacesInView)
        {
            newSpaceInView.AddViewer(this);
        }
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

        var newCoords = currentSpace.coordinates.GetCoordinateInDirection(movePath.Dequeue());
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

    public void SetRevealed(bool hide)
    {
        // Do nothing, should never be hidden
        // view.GetComponent<SpriteRenderer>().enabled = hide;
    }
}
