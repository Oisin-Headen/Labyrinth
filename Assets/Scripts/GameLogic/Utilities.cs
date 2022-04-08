using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public const int MAP_SIZE = 102;
    // Must be divided equally
    public const int SETUP_MAP_DIVISOR = 3;
    public const int SETUP_MAP_SIZE = MAP_SIZE / SETUP_MAP_DIVISOR;


    public const float TILE_SIZE = 1.6f;

    public static Color SPRITE_DARKEN = new Color(0.7f, 0.7f, 0.7f);
    public static Color SPRITE_LIGHT = new Color(1, 1, 1);
    public static Color SPRITE_MOVE = new Color(0.3f, 0.3f, 1);
    public static Color SPRITE_MOVE_SELECTED = new Color(0.7f, 0.7f, 1);




    public const float MAX_CAMERA_X = MAP_SIZE * TILE_SIZE;
    public const float MAX_CAMERA_Y = MAP_SIZE * TILE_SIZE;

    public const float MIN_CAMERA_X = 0;
    public const float MIN_CAMERA_Y = 0;

    public const int CAMERA_SPEED = 10;

    public const float MAX_CAMERA_SIZE = 11.0f;
    public const float MIN_CAMERA_SIZE = 2.0f;

    public const int CAMERA_ZOOM_SPEED = 15;
    public const int CAMERA_SNAP_SPEED = 80;

    public const float MOVEMENT_SPEED = 5f;

    public enum CardinalDirection
    {
        Up, Right, Down, Left
    }

    public struct Coordinate
    {
        public int x, y;

        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Coordinate operator +(Coordinate a, Coordinate b)
        {
            return new Coordinate(a.x + b.x, a.y + b.y);
        }
    }
    

    public static Coordinate GetCoordinateInDirection(this Coordinate coordinate, CardinalDirection direction)
    {
        switch (direction)
        {
            case CardinalDirection.Up:
                coordinate.y++;
                break;
            case CardinalDirection.Left:
                coordinate.x--;
                break;
            case CardinalDirection.Down:
                coordinate.y--;
                break;
            case CardinalDirection.Right:
                coordinate.x++;
                break;
        }
        return coordinate;
    }

    public static CardinalDirection SpiralDirectionClockwise(this CardinalDirection direction)
    {
        return direction switch
        {
            CardinalDirection.Up => CardinalDirection.Right,
            CardinalDirection.Right => CardinalDirection.Down,
            CardinalDirection.Down => CardinalDirection.Left,
            CardinalDirection.Left => CardinalDirection.Up,
            _ => direction,
        };
    }
}
