using System;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public class Space
{
    private readonly GameObject view;
    public readonly Coordinate coordinates;

    public IOccupy Occupier { get; set; }

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

    public bool IsEmpty()
    {
        return Occupier == null;
    }

    private void SetRevealed(bool seen)
    {
        if(Occupier != null)
        {
            Occupier.SetRevealed(seen);
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
        return !(IsEmpty() || !Occupier.BlocksLOS()); ;
    }
}
