﻿using System;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public class FieldOfView
{
    public Map tiles;


    //public Octant GetOctant()
    //{
    //    for (var row = 1; row < maxDistance; row++)
    //    {
    //        for (var col = 0; col <= row; col++)
    //        {
    //            var x = hero.x + col;
    //            var y = hero.y - row;

    //            paint(x, y);
    //        }
    //    }
    //}

    public FieldOfView(Map map)
    {
        tiles = map;
    }

    Coordinate TransformOctant(int row, int col, int octant)
    {
        switch (octant)
        {
            case 0: return new Coordinate(col, -row);
            case 1: return new Coordinate(row, -col);
            case 2: return new Coordinate(row, col);
            case 3: return new Coordinate(col, row);
            case 4: return new Coordinate(-col, row);
            case 5: return new Coordinate(-row, col);
            case 6: return new Coordinate(-row, -col);
            case 7: return new Coordinate(-col, -row);
            default:
                break;
        }
        return new Coordinate(0, 0);
    }

    public void RefreshVisibility(Coordinate hero)
    {
        for (var octant = 0; octant < 8; octant++)
        {
            RefreshOctant(hero, octant);
        }
    }

    void RefreshOctant(Coordinate hero, int octant)
    {
        var line = new ShadowLine();
        var fullShadow = false;

        var continueRow = true;

        for (var row = 1; continueRow; row++)
        {
            // Stop once we go out of bounds.
            var pos = hero + TransformOctant(row, 0, octant);
            if (!tiles.Contains(pos))
            {
                continueRow = false;
            }
            else
            {

                for (var col = 0; col <= row; col++)
                {
                    pos = hero + TransformOctant(row, col, octant);

                    // If we've traversed out of bounds, bail on this row.
                    if (!tiles.Contains(pos))
                    {
                        continueRow = false;
                    }
                    else
                    {

                        if (fullShadow)
                        {
                            tiles.GetSpace(pos).SetRevealed(false);
                        }
                        else
                        {
                            var projection = ProjectTile(row, col);

                            // Set the visibility of this tile.
                            var visible = !line.IsInShadow(projection);
                            tiles.GetSpace(pos).SetRevealed(visible);

                            // Add any opaque tiles to the shadow map.
                            if (visible && tiles.GetSpace(pos).BlocksLOS())
                            {
                                line.Add(projection);
                                fullShadow = line.IsFullShadow();
                            }
                        }
                    }
                }
            }
        }
    }

    // not sure what this is for
    //  for (var col = 0; col <= row; col++) {
    //var projection = _projectTile(row, col);
    //  var pos = start + transformOctant(row, col, octant);
    //  tiles[pos].isVisible = !line.isInShadow(projection);

    class ShadowLine
    {
        readonly List<Shadow> shadows = new List<Shadow>();

        internal bool IsInShadow(Shadow projection)
        {
            foreach (var shadow in shadows)
            {
                if (shadow.Contains(projection)) return true;
            }

            return false;
        }

        internal void Add(Shadow shadow)
        {
            // Figure out where to slot the new shadow in the list.
            var index = 0;
            for (; index < shadows.Count; index++)
            {
                // Stop when we hit the insertion point.
                if (shadows[index].start >= shadow.start) break;
            }

            // The new shadow is going here. See if it overlaps the
            // previous or next.
            Shadow overlappingPrevious = null;
            if (index > 0 && shadows[index - 1].end > shadow.start)
            {
                overlappingPrevious = shadows[index - 1];
            }

            Shadow overlappingNext = null;
            if (index < shadows.Count &&
                shadows[index].start < shadow.end)
            {
                overlappingNext = shadows[index];
            }

            // Insert and unify with overlapping shadows.
            if (overlappingNext != null)
            {
                if (overlappingPrevious != null)
                {
                    // Overlaps both, so unify one and delete the other.
                    overlappingPrevious.end = overlappingNext.end;
                    shadows.RemoveAt(index);
                }
                else
                {
                    // Overlaps the next one, so unify it with that.
                    overlappingNext.start = shadow.start;
                }
            }
            else
            {
                if (overlappingPrevious != null)
                {
                    // Overlaps the previous one, so unify it with that.
                    overlappingPrevious.end = shadow.end;
                }
                else
                {
                    // Does not overlap anything, so insert.
                    shadows.Insert(index, shadow);
                }
            }

        }
        internal bool IsFullShadow()
        {
            return shadows.Count == 1 &&
                Mathf.Approximately(shadows[0].start, 0) &&
                Mathf.Approximately(shadows[0].end, 1);
        }

    }
    class Shadow
    {
        internal float start;
        internal float end;

        internal Shadow(float start, float end)
        {
            this.start = start;
            this.end = end;
        }

        internal bool Contains(Shadow other)
        {
            return start <= other.start && end >= other.end;
        }
    }

    Shadow ProjectTile(float row, float col)
    {
        var topLeft = col / (row + 2);
        var bottomRight = (col + 1) / (row + 1);
        return new Shadow(topLeft, bottomRight);
    }
}

