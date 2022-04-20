﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using static Utilities;

public class Character : AbstractEntity, IOccupy, IEntity, IViewSpaces
{
    private ISet<Space> spacesInView = new HashSet<Space>();

    public readonly StatBlock stats;

    public CharacterLook Look { get; private set; }

    public override bool BlocksLOS { get { return false; } }
    public override int Armour { get { return 0; } }
    public DamageType WeaponDamageType { get { return DamageType.Blunt; } }

    // TODO Increase by weapon damage or something.
    public int AttackValue { get { return stats.Strength + 2; } }
    public int MoveRange { get { return stats.Dexerity; } }


    // TODO Characters should be expanded, with a 'Class' class.

    // TODO temp variables, should be put elsewhere. 'Race' and 'Weapon' classes
    private int viewRange = 5;
    public readonly int AttackRange = 1;

    public Character(Space space, Map map, CharacterLook look) : base (map)
    {
        currentSpace = space;
        Look = look;

        stats = new StatBlock(3,3,3,3,3);
    }

    public override void MoveToSpace(Space space)
    {
        base.MoveToSpace(space);
        var coords = currentSpace.Coordinates;


        foreach(var oldSpaceInView in spacesInView)
        {
            oldSpaceInView.RemoveViewer(this);
        }
        spacesInView = FieldOfView.GetAllSpacesInSightRange(map, coords, viewRange);
        spacesInView.IntersectWith(Dijkstras.GetSpacesInRange(map, coords, viewRange, true).Keys);
        spacesInView.Add(currentSpace);

        foreach (var newSpaceInView in spacesInView)
        {
            newSpaceInView.AddViewer(this);
        }
    }

    public override DamageEffectiveness GetDamageEffectiveness(DamageType damageType)
    {
        return DamageEffectiveness.Normal;
    }

    public override bool TakeDamage(int value)
    {
        throw new NotImplementedException();
    }
}
