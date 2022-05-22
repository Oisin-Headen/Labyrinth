using System;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public class Space
{
    public SelectionType CurrentSelectionType { get; private set; }
    public Coordinate Coordinates { get; private set; }
    public SpaceController Controller { get; set; }

    public IOccupy Occupier { get; set; }

    public bool IsEmpty { get { return Occupier == null; } }
    public bool BlocksLOS { get { return !(IsEmpty || !Occupier.BlocksLOS); } }

    private readonly GameManager gameController;
    private readonly ISet<IViewSpaces> viewers = new HashSet<IViewSpaces>();

    public Space(Coordinate coordinates, GameManager gameController)
    {
        this.gameController = gameController;
        Coordinates = coordinates;
    }

    private void SetRevealed(bool seen)
    {
        if(Occupier != null)
        {
            Occupier.SetRevealed(seen);
        }
        Controller.SetRevealed(seen);
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

    public void SetSelected(SelectionType move)
    {
        CurrentSelectionType = move;
        Controller.SetSelected();
    }

    public void Clicked()
    {
        gameController.Player.SpaceClickedOn(this);
    }
    
}
