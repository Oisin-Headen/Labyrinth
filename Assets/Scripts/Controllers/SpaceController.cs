using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public class SpaceController : MonoBehaviour
{
    public Space Model { get; private set; }

    public SpriteRenderer selector;
    public SpriteRenderer tile;

    public void SetModel(Space model)
    {
        Model = model;
    }

    public void SetRevealed(bool seen)
    {
        if (seen)
        {
            tile.enabled = true;
            tile.color = SPRITE_LIGHT;
        }
        else
        {
            tile.color = SPRITE_DARKEN;
        }

    }

    public void SetSelected()
    {
        switch (Model.CurrentSelectionType)
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
            case SelectionType.character:
                selector.color = SPRITE_CHARACTER;
                selector.enabled = true;
                break;
        }
    }

    public void OnMouseOver()
    {
        if (Model.CurrentSelectionType == SelectionType.move)
        {
            selector.color = SPRITE_MOVE_SELECTED;
        }
        else if (Model.CurrentSelectionType == SelectionType.attack)
        {
            selector.color = SPRITE_ATTACK_SELECTED;
        }
    }

    public void OnMouseExit()
    {
        if (Model.CurrentSelectionType == SelectionType.move)
        {
            selector.color = SPRITE_MOVE;
        }
        else if (Model.CurrentSelectionType == SelectionType.attack)
        {
            selector.color = SPRITE_ATTACK;
        }
    }

    public void OnMouseDown()
    {
        Model.Clicked();
    }
}
