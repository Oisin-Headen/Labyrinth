using System;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public static class Dijkstras
{
    public static readonly int ONE_SPACE = 10, DIAGONAL_SPACE_INCREASE = 4, IMPASSIBLE = -1;

    public static IDictionary<Space, (IList<Space>, int)> GetSpacesInRange(Map map, Coordinate startSpace, int maxCost, bool ignoreImpassible)
    {
        var nodes = new Dictionary<Space, DijkstrasNode>();
        var currentSpace = map.GetSpace(startSpace);

        nodes.Add(currentSpace, new DijkstrasNode(currentSpace));
        nodes[currentSpace].Visited = true;

        bool done = false;
        while(!done)
        {
            // make a list of adjacent spaces
            var adjacentSpaces = new List<Space>();
            foreach (CardinalDirection direction in Enum.GetValues(typeof(CardinalDirection)))
            {
                var newSpace = map.GetSpace(currentSpace.coordinates.GetCoordinateInDirection(direction));
                if(newSpace != null && (newSpace.Occupier == null || ignoreImpassible))
                {
                    adjacentSpaces.Add(newSpace);
                    var diagonalSpace = map.GetSpace(newSpace.coordinates.GetCoordinateInDirection(direction.SpiralDirectionClockwise()));
                    if(diagonalSpace != null && (diagonalSpace.Occupier == null || ignoreImpassible))
                    {
                        adjacentSpaces.Add(diagonalSpace);
                    }
                }
            }

            // loop through and update the adjacent spaces
            foreach (var space in adjacentSpaces)
            {
                // get cost for this adjacent space
                int newNodeCost = nodes[currentSpace].Cost + ONE_SPACE;
                if (Mathf.Abs(currentSpace.coordinates.x - space.coordinates.x) == 1 &&
                    Mathf.Abs(currentSpace.coordinates.y - space.coordinates.y) == 1)
                {
                    // if it's a diagonal, increse the movement cost
                    newNodeCost += DIAGONAL_SPACE_INCREASE;
                }

                // never seen before, create node, if the current node is below the max cost
                if (!nodes.ContainsKey(space) && nodes[currentSpace].Cost < maxCost)
                {
                    nodes.Add(space, new DijkstrasNode(space, newNodeCost, nodes[currentSpace]));
                }
                // node already exists, update it
                else
                {
                    if(!nodes[space].Visited)
                    {
                        nodes[space].Update(newNodeCost, nodes[currentSpace]);
                    }
                }
            }

            // find the next node
            Space lowestSpace = null;
            foreach (KeyValuePair<Space, DijkstrasNode> pair in nodes)
            {
                if (!pair.Value.Visited)
                {
                    if (lowestSpace == null)
                    {
                        lowestSpace = pair.Key;
                    }
                    else if (pair.Value.Cost < nodes[lowestSpace].Cost)
                    {
                        lowestSpace = pair.Key;
                    }
                }
            }

            // no next node, done looping
            if (lowestSpace == null)
            {
                done = true;
            }
            else
            {
                nodes[lowestSpace].Visited = true;
                currentSpace = lowestSpace;
            }
        }

        var returnValue = new Dictionary<Space, (IList<Space>, int)>();
        foreach(KeyValuePair<Space, DijkstrasNode> pair in nodes)
        {
            returnValue.Add(pair.Key, (pair.Value.GetPath(), pair.Value.Cost));
        }

        return returnValue;
    }

    private class DijkstrasNode
    {
        public readonly Space space;
        public DijkstrasNode Previous { get; set; }
        public int Cost { get; private set; }
        public bool Visited { get; set; }

        public DijkstrasNode(Space space, int cost = 0, DijkstrasNode previous = null)
        {
            this.space = space;
            Previous = previous;
            Cost = cost;
        }

        public void Update(int newNodeCost, DijkstrasNode dijkstrasNode)
        {
            if(newNodeCost < Cost)
            {
                Cost = newNodeCost;
                Previous = dijkstrasNode;
            }
        }

        public IList<Space> GetPath()
        {
            if (Previous == null)
            {
                return new List<Space>(){space};
            }
            else
            {
                IList<Space> path = Previous.GetPath();
                path.Add(space);
                return path;
            }
        }
    }
}
