using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public class Sight
{
    public static bool LineOfSight(Map map, Coordinate startSpace, Coordinate endSpace)
    {
        float vx, vy, ox, oy, l;
        int i;
        vx = endSpace.x - startSpace.x;
        vy = endSpace.y - startSpace.y;
        l = Mathf.Sqrt((vx * vx) + (vy * vy));
        vx /= l;
        vy /= l;

        foreach (var xOffset in new float[] { 0.5f, -0.5f })
        {
            foreach (var yOffset in new float[] { 0.5f, -0.5f })
            {
                ox = endSpace.x + xOffset;
                oy = endSpace.y + yOffset;
                for (i = 0; i < (int)l; i++)
                {
                    var tempSpace = map.GetSpace(new Coordinate((int)Mathf.Round(ox), (int)Mathf.Round(oy)));
                    if (tempSpace != null && tempSpace.BlocksLOS())
                    {
                        return false;
                    }
                    ox += vx;
                    oy += vy;
                }
            }
        }
        return true;
    }
}
