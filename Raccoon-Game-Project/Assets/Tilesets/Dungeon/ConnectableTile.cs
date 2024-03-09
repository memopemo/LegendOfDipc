using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class ConnectableRuleTile : RuleTile<ConnectableRuleTile.Neighbor>
{
    public List<TileBase> sibings = new List<TileBase>();
    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int Sibing = 3;
        public const int NotSiblingsNorThis = 4;
    }
    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        switch (neighbor)
        {
            case Neighbor.Sibing: return sibings.Contains(tile);
            case Neighbor.NotSiblingsNorThis: return !(sibings.Contains(tile) || tile == this);
        }
        return base.RuleMatch(neighbor, tile);
    }
}