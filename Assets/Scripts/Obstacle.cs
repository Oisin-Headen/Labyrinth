using UnityEngine;
using static Utilities;

public class Obstacle : IOccupy
{
    private readonly GameObject view;

    private Space space;

    public Obstacle(GameObject view, Space space)
    {
        this.view = view;
        this.space = space;
    }

    public GameObject GetView()
    {
        return view;
    }

    // Shouldn't use this, I don't think. Obstacles don't move that much.
    public void SetSpace(Space space)
    {
        this.space = space;
    }

    public Space GetSpace()
    {
        return space;
    }

    public bool BlocksLOS()
    {
        return true;
    }

    public void SetRevealed(bool seen)
    {
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
}