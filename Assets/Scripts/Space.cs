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

    public void SetHidden(bool hide)
    {
        if(occupier != null)
        {
            occupier.SetHidden(hide);
        }
        view.GetComponent<SpriteRenderer>().enabled = hide;
    }

    internal bool BlocksLOS()
    {
        return !(IsEmpty() || !occupier.BlocksLOS()); ;
    }
}
