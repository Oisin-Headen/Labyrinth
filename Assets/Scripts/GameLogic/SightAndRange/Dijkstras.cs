using System;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public static class Dijkstras
{
    public static readonly int ONE_SPACE = 10, DIAGONAL_SPACE = 14, IMPASSIBLE = -1;

    public static IDictionary<Space, IList<Space>> GetSpacesInRange(Map map, Coordinate startSpace, int maxCost, bool ignoreImpassible)
    {
        var nodes = new Dictionary<Space, DijkstrasNode>();
        var currentSpace = map.GetSpace(startSpace);

        nodes.Add(currentSpace, new DijkstrasNode(currentSpace));

        bool done = false;
        while(!done)
        {
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
            
        }

        return null;
    }

    private class DijkstrasNode
    {
        public readonly Space space;
        public DijkstrasNode Previous { get; set; }
        public int Cost { get; set; }
        public bool Visited { get; set; }

        public DijkstrasNode(Space space, DijkstrasNode previous = null)
        {
            this.space = space;
            Previous = previous;
        }
    }
}
