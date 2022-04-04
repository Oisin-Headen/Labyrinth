using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using static Utilities;

public class Character : AbstractEntity, IOccupy, IAmAnEntity, IViewSpaces
{
    private ISet<Space> spacesInView = new HashSet<Space>();

    // TODO Characters should be expanded, with a Stat class and a 'Class' class.
    // Stat variable;

    private int viewRange = 5;


    public Character(Space space, Map map) : base (map)
    {
        currentSpace = space;
    }

    public override void MoveToSpace(Space space)
    {
        base.MoveToSpace(space);
        var coords = currentSpace.coordinates;


        foreach(var oldSpaceInView in spacesInView)
        {
            oldSpaceInView.RemoveViewer(this);
        }
        spacesInView = FieldOfView.GetAllSpacesInSightRange(map, coords, viewRange);
        spacesInView.IntersectWith(Dijkstras.GetSpacesInRange(map, coords, viewRange, true).Keys);
        spacesInView.Add(currentSpace);

        foreach (var newSpaceInView in spacesInView)
        {
            newSpaceInView.AddViewer(this);
        }
    }

    

    public override bool BlocksLOS()
    {
        return false;
    }

    
}
