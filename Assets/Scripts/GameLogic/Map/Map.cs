using System;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public class Map
{
    private readonly GameController gameController;

    private readonly Space[,] map;

    public enum Terrain
    {
        NOT_SETUP, empty, wall, hard_wall, edge, dark_floor
    }

    public Map(GameController gameController)
    {
        this.gameController = gameController;

        Terrain[,] setupMap =  MapGenerator.CreateMap();
        map = new Space[MAP_SIZE, MAP_SIZE];

        // create the map, and fill it with objects
        for (int y = 0; y < MAP_SIZE; y++)
        {
            for (int x = 0; x < MAP_SIZE; x++)
            {
                var newTile = gameController.CreateTile(x, y);
                map[y, x] = new Space(newTile, new Coordinate(x, y));

                if(setupMap[y,x] == Terrain.wall)
                {
                    var newObject = gameController.CreateObstacle(x, y);
                    map[y, x].Occupier = new Obstacle(newObject, map[y, x]);
                }
                else if(setupMap[y,x] == Terrain.dark_floor)
                {
                    map[y, x].GetView().GetComponent<SpriteRenderer>().sprite = gameController.GetDarkFloor();
                }
            }
        }

        // create the player in the middle

        Coordinate coordinates = new Coordinate() { x = MAP_SIZE / 2, y = MAP_SIZE / 2 };
        var currentDirection = CardinalDirection.Up;
        int currentLength = 1, currentSteps = 0;

        var done = false;

        // Spiral outwards to find a empty spot close to the centre of the map.
        while (!done)
        {
            if (GetSpace(coordinates).IsEmpty())
            {
                done = true;
            }
            else
            {
                coordinates = coordinates.GetCoordinateInDirection(currentDirection);
                currentSteps++;
                if (currentSteps == currentLength)
                {
                    currentLength++;
                    currentSteps = 0;
                    currentDirection = currentDirection.SpiralDirectionClockwise();
                }
            }
        }

        var characterModel = new Character(GetSpace(coordinates), this);
        gameController.CreateEntity(coordinates, characterModel);
        GetSpace(coordinates).Occupier = characterModel;

        characterModel.MoveToSpace(GetSpace(coordinates));
    }

    public bool Contains(Coordinate coords)
    {
        return GetSpace(coords) != null;
    }

    public Space GetSpace(Coordinate currentCoords)
    {
        try
        {
            return map[currentCoords.y, currentCoords.x];
        }
        catch (IndexOutOfRangeException)
        {
            return null;
        }
    }

    // TODO: this was a test method, it doesn't work now. I'm leaving it in case I need it again.
    //public void HideAll()
    //{
    //    for (int i = 0; i < MAP_SIZE; i++)
    //    {
    //        for (int j = 0; j < MAP_SIZE; j++)
    //        {
    //            map[i,j].SetRevealed(false);
    //        }
    //    }
    //}

    public HashSet<Space> GetSpacesInRange(Coordinate startSpace, int range)
    {
        var spaces = new HashSet<Space>();

        for(var y = startSpace.y - range; y < startSpace.y + range; y++)
        {
            for (var x = startSpace.x - range; x < startSpace.x + range; x++)
            {
                var space = GetSpace(new Coordinate(y, x));
                if(space != null)
                {
                    spaces.Add(space);
                }
            }
        }

        return spaces;
    }
}
