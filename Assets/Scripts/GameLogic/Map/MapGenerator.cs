using System;
using static Utilities;
using static Map;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public static class MapGenerator
{
    public static Terrain[,] CreateMap()
    {
        var directionValues = new Dictionary<CardinalDirection, int>()
        {
            [CardinalDirection.Right] = 4,
            [CardinalDirection.Left] = 8,
            [CardinalDirection.Up] = 1,
            [CardinalDirection.Down] = 2
        };
        var DX = new Dictionary<CardinalDirection, int>()
        {
            [CardinalDirection.Right] = 1,
            [CardinalDirection.Left] = -1,
            [CardinalDirection.Up] = 0,
            [CardinalDirection.Down] = 0
        };
        var DY = new Dictionary<CardinalDirection, int>()
        {
            [CardinalDirection.Right] = 0,
            [CardinalDirection.Left] = 0,
            [CardinalDirection.Up] = -1,
            [CardinalDirection.Down] = 1
        };
        var OPPOSITE = new Dictionary<CardinalDirection, CardinalDirection>()
        {
            [CardinalDirection.Right] = CardinalDirection.Left,
            [CardinalDirection.Left] = CardinalDirection.Right,
            [CardinalDirection.Up] = CardinalDirection.Down,
            [CardinalDirection.Down] = CardinalDirection.Up
        };
        CardinalDirection[] directions = new CardinalDirection[4] { CardinalDirection.Left, CardinalDirection.Up, CardinalDirection.Down, CardinalDirection.Right };


        Terrain[,] setupMap = new Terrain[MAP_SIZE, MAP_SIZE];
        int[,] grid = new int[SETUP_MAP_SIZE, SETUP_MAP_SIZE];

        int firstX = UnityEngine.Random.Range(1, SETUP_MAP_SIZE);
        int firstY = UnityEngine.Random.Range(1, SETUP_MAP_SIZE);
        List<Coordinate> list = new List<Coordinate>() { new Coordinate(firstX, firstY) };

        Random random = new Random();


        while (list.Count > 0)
        {
            int index = ChooseIndex(list.Count);
            var chosenSpace = list[index];
            directions = directions.OrderBy(x => random.Next()).ToArray();
            foreach (var direction in directions)
            {
                int newX = chosenSpace.x + DX[direction];
                int newY = chosenSpace.y + DY[direction];
                if (newX >= 0 && newY >= 0 && newX < SETUP_MAP_SIZE && newY < SETUP_MAP_SIZE && grid[newY, newX] == 0)
                {
                    grid[chosenSpace.y, chosenSpace.x] |= directionValues[direction];
                    grid[newY, newX] |= directionValues[OPPOSITE[direction]];
                    list.Add(new Coordinate(newX, newY));
                    index = -1;
                    break;
                }
            }

            if (index != -1)
            {
                list.RemoveAt(index);
            }
        }


        for (int y = 0; y < SETUP_MAP_SIZE; ++y)
        {
            int topleftY = y * SETUP_MAP_DIVISOR;
            for (int x = 0; x < SETUP_MAP_SIZE; x++)
            {
                int topleftX = x * SETUP_MAP_DIVISOR;
                bool blockedAbove = (grid[y, x] & directionValues[CardinalDirection.Up]) != directionValues[CardinalDirection.Up];
                bool blockedLeft = (grid[y, x] & directionValues[CardinalDirection.Left]) != directionValues[CardinalDirection.Left];

                bool blockAbove = y - 1 > 0 && (grid[y - 1, x] & directionValues[CardinalDirection.Left]) != directionValues[CardinalDirection.Left];
                bool blockLeft = x - 1 > 0 && (grid[y, x - 1] & directionValues[CardinalDirection.Up]) != directionValues[CardinalDirection.Up];


                setupMap[topleftY, topleftX] = blockedAbove || blockedLeft || blockAbove || blockLeft ? Terrain.wall : Terrain.empty;

                setupMap[topleftY, topleftX + 1] = blockedAbove ? Terrain.wall : Terrain.empty;
                setupMap[topleftY, topleftX + 2] = blockedAbove ? Terrain.wall : Terrain.empty;
                setupMap[topleftY + 1, topleftX] = blockedLeft ? Terrain.wall : Terrain.empty;
                setupMap[topleftY + 2, topleftX] = blockedLeft ? Terrain.wall : Terrain.empty;

                setupMap[topleftY + 1, topleftX + 1] = Terrain.empty;
                setupMap[topleftY + 1, topleftX + 2] = Terrain.empty;
                setupMap[topleftY + 2, topleftX + 1] = Terrain.empty;
                setupMap[topleftY + 2, topleftX + 2] = Terrain.empty;   
            }
        }

        for (int y = 0; y < MAP_SIZE; ++y)
        {
            for (int x = 0; x < MAP_SIZE; x++)
            {
                if (setupMap[y,x] == Terrain.wall)
                {
                    if(UnityEngine.Random.Range(0,10) == 0)
                    {
                        setupMap[y, x] = Terrain.gold;
                    }
                }
            }
        }

        //TODO empty map for testing
        //setupMap = CreateEmptyMap();

        return setupMap;
    }

    private static int ChooseIndex(int max)
    {
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            return max - 1;
        }
        else
        {
            return UnityEngine.Random.Range(0, max);
        }
    }

    private static Terrain[,] CreateEmptyMap()
    {
        var map = new Terrain[MAP_SIZE, MAP_SIZE];
        for (int y = 0; y < MAP_SIZE; y++)
        {
            for (int x = 0; x < MAP_SIZE; x++)
            {
                map[y, x] = Terrain.empty;
            }           
        }
        return map;
    }
}

