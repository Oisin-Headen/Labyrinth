using System;
using System.Collections.Generic;
using static Utilities;

public class Map
{
    private readonly Space[,] map;

    public enum Terrain
    {
        NOT_SETUP, empty, wall, gold, hardWall, edge
    }

    public Map(GameManager gameManager, List<Enemy> enemies)
    {
        Terrain[,] setupMap =  MapGenerator.CreateMap();
        map = new Space[MAP_SIZE, MAP_SIZE];

        // create the map, and fill it with objects
        for (int y = 0; y < MAP_SIZE; y++)
        {
            for (int x = 0; x < MAP_SIZE; x++)
            {
                var coords = new Coordinate(x, y);
                var space = new Space(coords, gameManager);
                map[y, x] = space;
                gameManager.CreateSpaceForModel(space);

                if(setupMap[y,x] == Terrain.wall)
                {
                    gameManager.CreateObstacle(space, ObstacleType.Wall);
                }
                if (setupMap[y, x] == Terrain.gold)
                {
                    gameManager.CreateObstacle(space, ObstacleType.Gold);
                }
                else if(setupMap[y,x] == Terrain.empty)
                {
                    if(UnityEngine.Random.Range(1, 100) == 1)
                    {
                        var newEnemy = gameManager.CreateEnemy(map[y, x], EnemyType.FlameSentinal);
                        enemies.Add(newEnemy);
                    }
                }
            }
        }
    }

    public Space GetSpawnLocation()
    {
        Coordinate coordinates = new Coordinate() { x = MAP_SIZE / 2, y = MAP_SIZE / 2 };
        var currentDirection = CardinalDirection.Up;
        int currentLength = 1, currentSteps = 0;

        var done = false;

        // Spiral outwards to find a empty spot close to the centre of the map.
        while (!done)
        {
            if (GetSpace(coordinates).IsEmpty)
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
        return GetSpace(coordinates);
    }

    public bool Contains(Coordinate coords)
    {
        return GetSpace(coords) != null;
    }

    public Space GetSpace(Coordinate coords)
    {
        try
        {
            return map[coords.y, coords.x];
        }
        catch (IndexOutOfRangeException)
        {
            return null;
        }
    }

    public HashSet<Space> GetSpacesInRange(Space startSpace, int range)
    {
        var spaces = new HashSet<Space>();

        for(var y = startSpace.Coordinates.y - range; y < startSpace.Coordinates.y + range; y++)
        {
            for (var x = startSpace.Coordinates.x - range; x < startSpace.Coordinates.x + range; x++)
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
