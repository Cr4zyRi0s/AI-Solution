using System;
using System.Linq;
using QuikGraph;
using System.Collections.Generic;
using UnityEngine;

public partial class NavigationZone : MonoBehaviour{
    public class Sample
    {
        public int id { get; private set; }
        public Vector3 position { get; private set; }

        public Sample(int id)
        {
            this.id = id;
        }

        public Sample SetPosition(Vector3 position)
        {
            this.position = position;
            return this;
        }

        public override bool Equals(object obj)
        {
            return obj is Sample node &&
                   id == node.id;
        }

        public override int GetHashCode()
        {
            return 1877310944 + id.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("(id:{0} / p.x:{1}, p.y:{2}, p.z:{3})", this.id, this.position.x, this.position.y, this.position.z);
        }
    }

    public struct AreaOverlap 
    {
        public Vector3 start;
        public Vector3 end;
    }

    public class AreaConnection : IEdge<WalkableArea>
    {
        public WalkableArea Source { get; private set; }
        public WalkableArea Target { get; private set; }
        public AreaOverlap Overlap { get; private set; }

        public AreaConnection(WalkableArea source, WalkableArea target)
        {
            this.Source = source;
            this.Target = target;
            this.Overlap = CalculateOverlap(source, target);
        }

        private AreaOverlap CalculateOverlap(WalkableArea source, WalkableArea target) {
            var overlap = new AreaOverlap();
            var start = Vector3.zero;
            var end = Vector3.zero;
            
            if (source.GridArea.IsAbove(target.GridArea))
            {
                if (source.GridArea.Size.y > target.GridArea.Size.y) 
                {
                    start = target.WorldBounds.min;
                    end = target.WorldBounds.max;
                    end.x = target.WorldBounds.min.x;
                }
                else
                {
                    start = source.WorldBounds.min;
                    start.x = source.WorldBounds.max.x;
                    end = source.WorldBounds.max;                    
                }
            }
            else if (source.GridArea.IsBelow(target.GridArea))
            {
                if (source.GridArea.Size.y > target.GridArea.Size.y)
                {
                    start = target.WorldBounds.min;
                    start.x = target.WorldBounds.max.x;
                    end = target.WorldBounds.max;
                }
                else
                {
                    start = source.WorldBounds.min;
                    end = source.WorldBounds.max;
                    end.x = source.WorldBounds.min.x;
                }
            }
            else if (source.GridArea.IsLeftOf(target.GridArea))
            {
                if (source.GridArea.Size.x > target.GridArea.Size.x)
                {
                    start = target.WorldBounds.min;
                    end = target.WorldBounds.max;
                    end.z = target.WorldBounds.min.z;
                }
                else
                {
                    start = source.WorldBounds.min;
                    start.z = source.WorldBounds.max.z;
                    end = source.WorldBounds.max;
                }
            }
            else if (source.GridArea.IsRightOf(target.GridArea))
            {
                if (source.GridArea.Size.x > target.GridArea.Size.x)
                {
                    start = target.WorldBounds.min;
                    start.z = target.WorldBounds.max.z;
                    end = target.WorldBounds.max;
                }
                else
                {
                    start = source.WorldBounds.min;
                    end = source.WorldBounds.max;
                    end.z = source.WorldBounds.min.z;
                }
            }
            else 
            {
                throw new InvalidOperationException("Source and Target areas are not adjacent!");
            }

            overlap.start = start;
            overlap.end = end;
            return overlap;
        }

        public override bool Equals(object obj)
        {
            return obj is AreaConnection edge &&
                   EqualityComparer<WalkableArea>.Default.Equals(Source, edge.Source) &&
                   EqualityComparer<WalkableArea>.Default.Equals(Target, edge.Target);
        }

        public override int GetHashCode()
        {
            int hashCode = -1592377951;
            hashCode = hashCode * -1521134295 + EqualityComparer<WalkableArea>.Default.GetHashCode(Source);
            hashCode = hashCode * -1521134295 + EqualityComparer<WalkableArea>.Default.GetHashCode(Target);
            return hashCode;
        }
    }

    public class GridArea
    {
        public Vector2Int Anchor { get; private set; }
        public Vector2Int Size { get; private set; }
        public Vector2Int MinCorner { get { return Anchor; } }
        public Vector2Int MaxCorner { get { return new Vector2Int(Anchor.x + Size.x - 1, Anchor.y + Size.y - 1); } }

        public GridArea()
        {
        }

        public GridArea(Vector2Int anchor, Vector2Int size)
        {
            Anchor = anchor;
            Size = size;
        }

        public static List<List<GridArea>> GroupAreasBySizeAlignmentY(IEnumerable<GridArea> areas)
        {
            Dictionary<int, List<GridArea>> isoSizebuckets = new Dictionary<int, List<GridArea>>();
            foreach (var a in areas)
            {
                int id = a.Size.y * 19 + a.Anchor.y * 23;
                if (!isoSizebuckets.ContainsKey(id))
                {
                    isoSizebuckets[id] = new List<GridArea>();
                }
                isoSizebuckets[id].Add(a);
            }
            return isoSizebuckets.Values.ToList();
        }
        public static List<List<GridArea>> GroupAreasBySizeAlignmentX(IEnumerable<GridArea> areas)
        {
            Dictionary<int, List<GridArea>> isoSizebuckets = new Dictionary<int, List<GridArea>>();
            foreach (var a in areas)
            {
                int id = a.Size.x * 19 + a.Anchor.x * 23;
                if (!isoSizebuckets.ContainsKey(id))
                {
                    isoSizebuckets[id] = new List<GridArea>();
                }
                isoSizebuckets[id].Add(a);
            }
            return isoSizebuckets.Values.ToList();
        }

        public bool IsAbove(GridArea other) { return this.MaxCorner.x == other.MinCorner.x - 1
                && this.MinCorner.y >= other.MinCorner.y 
                && this.MinCorner.y <= other.MaxCorner.y; }
        public bool IsBelow(GridArea other) { return this.MinCorner.x - 1 == other.MaxCorner.x
                && this.MinCorner.y >= other.MinCorner.y
                && this.MinCorner.y <= other.MaxCorner.y; }
        public bool IsLeftOf(GridArea other) { return this.MaxCorner.y == other.MinCorner.y - 1
                && this.MinCorner.x >= other.MinCorner.x
                && this.MinCorner.x <= other.MaxCorner.x; }
        public bool IsRightOf(GridArea other) { return this.MinCorner.y - 1 == other.MaxCorner.y
                && this.MinCorner.x >= other.MinCorner.x
                && this.MinCorner.x <= other.MaxCorner.x; }

        public bool IsAdjacentHorizontal(GridArea other) { return this.IsLeftOf(other) || this.IsRightOf(other); }
        public bool IsAdjacentVertical(GridArea other) { return this.IsAbove(other) || this.IsBelow(other); }
        public bool IsAdjacent(GridArea other) { return this.IsAdjacentHorizontal(other) || this.IsAdjacentVertical(other); }

        public static GridArea MergeOrNull(GridArea a, GridArea b)
        {
            bool sameXSize = a.Size.x == b.Size.x;
            bool sameYSize = a.Size.y == b.Size.y;
            if (!sameXSize && !sameYSize) return null;
            bool toRight = a.IsRightOf(b);
            bool toLeft = a.IsLeftOf(b);
            bool above = a.IsAbove(b);
            bool below = a.IsBelow(b);
            bool adjacentY = toLeft || toRight;
            bool adjacentX = above || below;
            if (!adjacentX && !adjacentY) return null;
            Vector2Int size;
            Vector2Int anchor;
            if (adjacentY && sameXSize)
            {
                if (toLeft)
                {
                    size = b.MaxCorner - a.MinCorner;
                    anchor = a.Anchor;
                }
                else
                {
                    size = a.MaxCorner - b.MinCorner;
                    anchor = b.Anchor;
                }
            }
            else if (adjacentX && sameYSize)
            {
                if (above)
                {
                    size = b.MaxCorner - a.MinCorner;
                    anchor = a.MinCorner;
                }
                else
                {
                    size = a.MaxCorner - b.MinCorner;
                    anchor = b.Anchor;
                }
            }
            else
                return null;
            size += Vector2Int.one;
            return new GridArea(anchor, size);
        }

        public WalkableArea ToWalkableArea(Sample[,] sampleGrid, float VoxelSize)
        {
            var bounds = new Bounds();
            bounds.size = new Vector3(this.Size.x, 0.1f, this.Size.y) * VoxelSize;
            bounds.center = Vector3.Lerp(sampleGrid[this.MinCorner.x, this.MinCorner.y].position,
                            sampleGrid[this.MaxCorner.x, this.MaxCorner.y].position, .5f);

            var walkArea = new WalkableArea(this,bounds);
            return walkArea;
        }
    }
    public class WalkableArea
    {
        public GridArea GridArea { get; private set; }
        public Bounds WorldBounds { get; private set; }

        public WalkableArea()
        {
        }

        public WalkableArea(GridArea gridArea, Bounds worldBounds)
        {
            GridArea = gridArea;
            WorldBounds = worldBounds;
        }
    }
}
