using System;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public class Map
{
    private readonly GameController gameController;

    private readonly Space[,] map;

    private enum Terrain
    {
        empty, wall, hard_wall, edge
    }

    public Map(GameController gameController)
    {
        this.gameController = gameController;

        Terrain[,] setupMap = new Terrain[MAP_SIZE, MAP_SIZE];
        map = new Space[MAP_SIZE, MAP_SIZE];

        // create the template for the map
        for (int i = 0; i < MAP_SIZE; i++)
        {
            for (int j = 0; j < MAP_SIZE; j++)
            {
                if (UnityEngine.Random.Range(1, 101) <= 30)
                {
                    setupMap[i, j] = Terrain.wall;
                }
                else
                {
                    setupMap[i, j] = Terrain.empty;
                }
            }
        }

        // create the map, and fill it with objects
        for (int i = 0; i < MAP_SIZE; i++)
        {
            for (int j = 0; j < MAP_SIZE; j++)
            {
                var newTile = gameController.CreateTile(i, j);
                map[i, j] = new Space(newTile, new Coordinate(i, j));

                if(setupMap[i,j] == Terrain.wall)
                {
                    var newObject = gameController.CreateObstacle(i, j);
                    map[i, j].SetOccupier(new Obstacle(newObject, map[i, j]));
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
                coordinates = coordinates.GetCoordinateInDiection(currentDirection);
                currentSteps++;
                if (currentSteps == currentLength)
                {
                    currentLength++;
                    currentSteps = 0;
                    switch (currentDirection)
                    {
                        case CardinalDirection.Up:
                            currentDirection = CardinalDirection.Right;
                            break;
                        case CardinalDirection.Right:
                            currentDirection = CardinalDirection.Down;
                            break;
                        case CardinalDirection.Down:
                            currentDirection = CardinalDirection.Left;
                            break;
                        case CardinalDirection.Left:
                            currentDirection = CardinalDirection.Up;
                            break;
                    }
                }
            }
        }

        var characterModel = new Character(GetSpace(coordinates), this);
        gameController.CreateEntity(coordinates, characterModel);
        GetSpace(coordinates).SetOccupier(characterModel);

        HideAll();
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
            return map[currentCoords.x, currentCoords.y];
        }
        catch (IndexOutOfRangeException)
        {
            return null;
        }
    }

    public void HideAll()
    {
        for (int i = 0; i < MAP_SIZE; i++)
        {
            for (int j = 0; j < MAP_SIZE; j++)
            {
                map[i,j].SetRevealed(false);
            }
        }
    }

    public HashSet<Space> GetSpacesInRange(Coordinate startSpace, int range)
    {
        var spaces = new HashSet<Space>();

        for(var x = startSpace.x - range; x < startSpace.x + range; x++)
        {
            for (var y = startSpace.y - range; y < startSpace.y + range; y++)
            {
                var space = GetSpace(new Coordinate(x, y));
                if(space != null)
                {
                    spaces.Add(space);
                }
            }
        }

        return spaces;
    }
}
