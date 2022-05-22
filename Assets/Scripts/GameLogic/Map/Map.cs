using System;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public class Map
{
    private readonly Space[,] map;

    public enum Terrain
    {
        NOT_SETUP, empty, wall, gold, hardWall, edge
    }

    public Map(GameManager gameController, List<Enemy> enemies)
    {
        Terrain[,] setupMap =  MapGenerator.CreateMap();
        map = new Space[MAP_SIZE, MAP_SIZE];

        // create the map, and fill it with objects
        for (int y = 0; y < MAP_SIZE; y++)
        {
            for (int x = 0; x < MAP_SIZE; x++)
            {
                var coords = new Coordinate(x, y);
                var space = new Space(coords, gameController);
                map[y, x] = space;
                gameController.CreateSpaceForModel(coords, space);

                if(setupMap[y,x] == Terrain.wall)
                {
                    var newObject = gameController.CreateObstacleView(x, y);
                    map[y, x].Occupier = new Obstacle(newObject, map[y, x], ObstacleType.Wall);
                }
                if (setupMap[y, x] == Terrain.gold)
                {
                    var newObject = gameController.CreateObstacleView(x, y);
                    map[y, x].Occupier = new Obstacle(newObject, map[y, x], ObstacleType.Gold);
                }
                else if(setupMap[y,x] == Terrain.empty)
                {
                    if(UnityEngine.Random.Range(1, 100) == 1)
                    {
                        Enemy newEnemy = new Enemy(this, EnemyType.FlameSentinal, space);
                        gameController.CreateEntity(map[y, x], newEnemy);
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
