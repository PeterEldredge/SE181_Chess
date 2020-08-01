using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnPiece : GamePiece
{
    private bool _hasMoved = false;
    private int _topDownMultiplier = 1;

    protected override void Awake()
    {
        base.Awake();

        if (Color == PieceColor.Black) _topDownMultiplier = -1;
    }

    public override void OnMove(Move move)
    {
        base.OnMove(move);

        _hasMoved = true;
    }

    protected override List<Move> GetStandardMoves()
    {
        List<Move> standardMoves = new List<Move>();

        if(_gameBoard.GetTile(new Vector2Int(Coordinates.x, Coordinates.y + 1 * _topDownMultiplier), out Tile tile))
        {
            if (!tile.CurrentPiece) //Later change to ensure CurrentPiece.Color != tile.CurrentPiece.Color
            {
                standardMoves.Add(new Move(
                    Coordinates,
                    new Vector2Int(Coordinates.x, Coordinates.y + 1 * _topDownMultiplier)));
            }
        }

        return standardMoves;
    }

    protected override List<Move> GetSpecialCaseMoves()
    {
        List<Move> standardMoves = new List<Move>();

        if (_hasMoved) return standardMoves;

        if (_gameBoard.GetTile(new Vector2Int(Coordinates.x, Coordinates.y + 2 * _topDownMultiplier), out Tile tile))
        {
            if (!tile.CurrentPiece) //Later change to ensure CurrentPiece.Color != tile.CurrentPiece.Color
            {
                standardMoves.Add(new Move(
                    Coordinates,
                    new Vector2Int(Coordinates.x, Coordinates.y + 2 * _topDownMultiplier)));
            }
        }

        return standardMoves;
    }
}
