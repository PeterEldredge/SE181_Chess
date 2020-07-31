using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Move
{
    public Vector2Int StartingPos { get; private set; }
    public Vector2Int EndingPos { get; private set; }

    public Move(Vector2Int startingPos, Vector2Int endingPos)
    {
        StartingPos = startingPos;
        EndingPos = endingPos;
    }
}

public enum PieceColor
{
    White,
    Black,
}

public abstract class GamePiece : MonoBehaviour
{
    [SerializeField] public PieceColor Color;

    public Tile CurrentTile { get; set; }

    public Vector2Int Coordinates => CurrentTile.Coordinates;

    protected GameBoard _gameBoard;

    protected void Awake()
    {
        _gameBoard = GetComponentInParent<GameBoard>();
    }

    public List<Move> GetValidMoves()
    {
        List<Move> validMoves = new List<Move>();

        validMoves.AddRange(GetStandardMoves());
        validMoves.AddRange(GetSpecialCaseMoves());

        return validMoves;
    }

    public void OnMove(Move move)
    {
        CurrentTile = _gameBoard.GetTile(move.EndingPos);

        transform.position = new Vector3(
                        GameBoard.STARTING_POSITION.x + GameBoard.TILE_SPACING * move.EndingPos.x,
                        GameBoard.STARTING_POSITION.y + GameBoard.TILE_SPACING * move.EndingPos.y,
                        0f) +
                        GameBoard.PIECE_ADJUSTMENT;
    }

    //Abstract
    protected abstract List<Move> GetStandardMoves();

    //Virtual
    protected virtual List<Move> GetSpecialCaseMoves() { return new List<Move>(); }
}
