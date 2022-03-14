using System;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public class Space
{
    private readonly GameObject view;
    private readonly Coordinate coordinates;

    private IOccupy occupier;

    private ISet<IViewSpaces> viewers = new HashSet<IViewSpaces>();

    public Space(GameObject view, Coordinate coordinates)
    {
        this.view = view;
        this.coordinates = coordinates;
    }

    public GameObject GetView()
    {
        return view;
    }

    public void SetOccupier(IOccupy occupier)
    {
        this.occupier = occupier;
    }

    public IOccupy GetOccupier()
    {
        return occupier;
    }

    public bool IsEmpty()
    {
        return occupier == null;
    }

    public Coordinate GetCoordinates()
    {
        return coordinates;
    }

    private void SetRevealed(bool seen)
    {
        if(occupier != null)
        {
            occupier.SetRevealed(seen);
        }
        if (seen)
        {
            view.GetComponent<SpriteRenderer>().enabled = true;
            view.GetComponent<SpriteRenderer>().color = SPRITE_LIGHT;
        }
        else
        {
            view.GetComponent<SpriteRenderer>().color = SPRITE_DARKEN;
        }

    }

    public void AddViewer(IViewSpaces viewer)
    {
        viewers.Add(viewer);
        SetRevealed(true);
    }
    public void RemoveViewer(IViewSpaces viewer)
    {
        viewers.Remove(viewer);
        if(viewers.Count == 0)
        {
            SetRevealed(false);
        }
    }

    public bool BlocksLOS()
    {
        return !(IsEmpty() || !occupier.BlocksLOS()); ;
    }
}
