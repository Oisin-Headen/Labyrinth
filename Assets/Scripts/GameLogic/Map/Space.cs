using System;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public class Space
{

    public SelectionType CurrentSelectionType { get; private set; }
    public Coordinate Coordinates { get; private set; }
    public SpaceController Controller { get; private set; }

    public IOccupy Occupier { get; set; }

    private readonly GameController gameController;

    private readonly ISet<IViewSpaces> viewers = new HashSet<IViewSpaces>();


    public Space(Coordinate coordinates, GameController gameController)
    {
        this.gameController = gameController;
        Coordinates = coordinates;
    }

    public void SetController(SpaceController controller)
    {
        Controller = controller;
    }

    public bool IsEmpty()
    {
        return Occupier == null;
    }

    public bool BlocksLOS()
    {
        return !(IsEmpty() || !Occupier.BlocksLOS()); ;
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
        if (CurrentSelectionType != SelectionType.none)
        {
            gameController.Player.SpaceClicked(this);
        }
    }
    
}
