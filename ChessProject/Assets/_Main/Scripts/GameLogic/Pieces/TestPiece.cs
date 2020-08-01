using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPiece : GamePiece
{
    protected override List<Move> GetStandardMoves()
    {
        List<Move> standardMoves = new List<Move>();

        if (!_gameBoard.Board[Coordinates.x][Coordinates.y + 1].CurrentPiece)
        {
            standardMoves.Add(new Move(
                Coordinates,
                new Vector2Int(Coordinates.x, Coordinates.y + 1)));
        }

        return standardMoves;
    }
}
