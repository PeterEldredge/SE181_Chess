using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightPiece : GamePiece
{
    private List<Vector2Int> _coordsToCheck = new List<Vector2Int>()
    {
        new Vector2Int(1, 2),
        new Vector2Int(1, -2),
        new Vector2Int(-1, 2),
        new Vector2Int(-1, -2),
        new Vector2Int(2, 1),
        new Vector2Int(2, -1),
        new Vector2Int(-2, 1),
        new Vector2Int(-2, -1),
    };

    protected override List<Move> GetStandardMoves()
    {
        List<Move> standardMoves = new List<Move>();

        foreach (Vector2Int check in _coordsToCheck)
        {
            if (_gameBoard.GetTile(new Vector2Int(Coordinates.x + check.x, Coordinates.y + check.y), out Tile tile))
            {
                if (!tile.CurrentPiece) //Later change to ensure CurrentPiece.Color != tile.CurrentPiece.Color
                {
                    standardMoves.Add(new Move(
                        Coordinates,
                        new Vector2Int(Coordinates.x + check.x, Coordinates.y + check.y)));
                }
            }
        }

        return standardMoves;
    }
}
