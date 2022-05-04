using UnityEngine;
using static Utilities;

public class Obstacle : IOccupy
{
    private readonly GameObject view;
    private readonly ObstacleType type;

    public int Armour { get { return type.armour; } }
    public int Health { get; private set; }

    private Space space;

    public Obstacle(GameObject view, Space space, ObstacleType type)
    {
        this.view = view;
        this.space = space;
        this.type = type;
        Health = type.maxHealth;
        view.GetComponent<SpriteRenderer>().sprite = GameSprites.GetSpriteFor(type);
    }

    public bool BlocksLOS { get { return type.blocksLOS; } }

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

    public DamageEffectiveness GetDamageEffectiveness(DamageType damageType)
    {
        return type.suite.suite[damageType];
    }

    public bool TakeDamage(int value)
    {
        Health -= value;
        if (Health <= 0)
        {
            Object.Destroy(view);
            return true;
        }
        return false;
    }
}