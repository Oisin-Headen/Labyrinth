using System;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public class Space : MonoBehaviour
{
    private SpriteRenderer selector;

    private SelectionType currentSelectionType;

    private Player player;

    public Coordinate Coordinates { get; private set; }

    public IOccupy Occupier { get; set; }

    private readonly ISet<IViewSpaces> viewers = new HashSet<IViewSpaces>();

    public void Setup(Coordinate coordinates, Player player)
    {
        this.player = player;
        Coordinates = coordinates;
        selector = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public GameObject GetView()
    {
        return gameObject;
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
        if (seen)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<SpriteRenderer>().color = SPRITE_LIGHT;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = SPRITE_DARKEN;
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

    public void SetSelected(SelectionType move)
    {
        currentSelectionType = move;
        switch (move)
        {
            case SelectionType.none:
                selector.enabled = false;
                break;
            case SelectionType.move:
                selector.color = SPRITE_MOVE;
                selector.enabled = true;
                break;
            case SelectionType.attack:
                selector.color = SPRITE_ATTACK;
                selector.enabled = true;
                break;
        }
    }

    public void OnMouseOver()
    {
        if(currentSelectionType == SelectionType.move)
        {
            selector.color = SPRITE_MOVE_SELECTED;
        }
        else if (currentSelectionType == SelectionType.attack)
        {
            selector.color = SPRITE_ATTACK_SELECTED;
        }
    }

    public void OnMouseExit()
    {
        if (currentSelectionType == SelectionType.move)
        {
            selector.color = SPRITE_MOVE;
        }
        else if (currentSelectionType == SelectionType.attack)
        {
            selector.color = SPRITE_ATTACK;
        }
    }

    public void OnMouseDown()
    {
        if (currentSelectionType != SelectionType.none)
        {
            player.SpaceClicked(this);
        }
    }
}
