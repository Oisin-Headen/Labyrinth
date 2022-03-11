using System;
using UnityEngine;
using static Utilities;

public class Space
{
    private readonly GameObject view;
    private readonly Coordinate coordinates;

    private IOccupy occupier;

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

    public void SetRevealed(bool seen)
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

    internal bool BlocksLOS()
    {
        return !(IsEmpty() || !occupier.BlocksLOS()); ;
    }
}
