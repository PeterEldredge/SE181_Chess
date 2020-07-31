using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameBoard : MonoBehaviour
{
    //Static
    public const float TILE_SPACING = 1.4f;

    public static Vector3 PIECE_ADJUSTMENT { get; private set; } = new Vector3(0f, -.5f, 0f);
    public static Vector3 STARTING_POSITION { get; private set; } = new Vector3(-4f, -4.5f, 0f);

    #region Inspector

    [Header("Tiles")]

    [SerializeField] private Sprite _whiteTile;
    [SerializeField] private Sprite _whiteSelectableTile;

    [SerializeField] private Sprite _blackTile;
    [SerializeField] private Sprite _blackSelectableTile;

    [Header("Game Pieces")]

    [SerializeField] private GameObject _whiteKing;
    [SerializeField] private GameObject _whiteQueen;
    [SerializeField] private GameObject _whiteRook;
    [SerializeField] private GameObject _whiteBishop;
    [SerializeField] private GameObject _whiteKnight;
    [SerializeField] private GameObject _whitePawn;

    [SerializeField] private GameObject _blackKing;
    [SerializeField] private GameObject _blackQueen;
    [SerializeField] private GameObject _blackRook;
    [SerializeField] private GameObject _blackBishop;
    [SerializeField] private GameObject _blackKnight;
    [SerializeField] private GameObject _blackPawn;

    #endregion

    //Properties
    public List<List<Tile>> Board { get; private set; }

    //Private
    private GameObject _pieceHolder;

    private void Awake()
    {
        _pieceHolder = new GameObject("PieceHolder");
        _pieceHolder.transform.parent = transform;

        SetupBoard();
        SetupPieces();
    }

    #region Setup

    private void SetupBoard()
    {
        Transform rowParent;

        Board = new List<List<Tile>>();

        for(int i = 0; i < 8; i++)
        {
            Board.Add(new List<Tile>());

            rowParent = new GameObject($"{i}").transform;
            rowParent.parent = transform;

            for (int j = 0; j < 8; j++)
            {
                Board[i].Add(new GameObject($"{i}x{j}").AddComponent<Tile>());

                Board[i][j].Initialize(
                    new Vector2Int(i, j),
                    new Vector3(
                        STARTING_POSITION.x + TILE_SPACING * i,
                        STARTING_POSITION.y + TILE_SPACING * j,
                        0f),
                    (i + j) % 2 == 0 ? _blackTile : _whiteTile,
                    (i + j) % 2 == 0 ? _blackSelectableTile : _whiteSelectableTile); //Not good right now but short term, may need to make another object to hold sprite data

                Board[i][j].transform.parent = rowParent;
            }
        }
    }

    private void SetupPieces()
    {
        SetPiece(_whiteRook, new Vector2Int(0, 0));
        SetPiece(_whitePawn, new Vector2Int(0, 1));
        SetPiece(_blackPawn, new Vector2Int(0, 6));
        SetPiece(_blackRook, new Vector2Int(0, 7));

        SetPiece(_whiteKnight, new Vector2Int(1, 0));
        SetPiece(_whitePawn, new Vector2Int(1, 1));                                        
        SetPiece(_blackPawn, new Vector2Int(1, 6));
        SetPiece(_blackKnight, new Vector2Int(1, 7));

        SetPiece(_whiteBishop, new Vector2Int(2, 0));
        SetPiece(_whitePawn, new Vector2Int(2, 1));
        SetPiece(_blackPawn, new Vector2Int(2, 6));
        SetPiece(_blackBishop, new Vector2Int(2, 7));

        SetPiece(_whiteQueen, new Vector2Int(3, 0));
        SetPiece(_whitePawn, new Vector2Int(3, 1));
        SetPiece(_blackPawn, new Vector2Int(3, 6));
        SetPiece(_blackQueen, new Vector2Int(3, 7));
        
        SetPiece(_whiteKing, new Vector2Int(4, 0));
        SetPiece(_whitePawn, new Vector2Int(4, 1));
        SetPiece(_blackPawn, new Vector2Int(4, 6));
        SetPiece(_blackKing, new Vector2Int(4, 7));

        SetPiece(_whiteBishop, new Vector2Int(5, 0));
        SetPiece(_whitePawn, new Vector2Int(5, 1));
        SetPiece(_blackPawn, new Vector2Int(5, 6));
        SetPiece(_blackBishop, new Vector2Int(5, 7));

        SetPiece(_whiteKnight, new Vector2Int(6, 0));
        SetPiece(_whitePawn, new Vector2Int(6, 1));
        SetPiece(_blackPawn, new Vector2Int(6, 6));
        SetPiece(_blackKnight, new Vector2Int(6, 7));

        SetPiece(_whiteRook, new Vector2Int(7, 0));
        SetPiece(_whitePawn, new Vector2Int(7, 1));
        SetPiece(_blackPawn, new Vector2Int(7, 6));
        SetPiece(_blackRook, new Vector2Int(7, 7));

    }

    private void SetPiece(GameObject prefab, Vector2Int coordinates)
    {
        GameObject pieceObject = Instantiate(prefab, _pieceHolder.transform);
        Tile tile = GetTile(coordinates);

        //Sets the piece's transform position
        pieceObject.transform.position = tile.transform.position + PIECE_ADJUSTMENT;

        //Sets the piece's reference to the current board tile
        tile.CurrentPiece = pieceObject.GetComponent<GamePiece>();

        //Sets the tile reference to the current piece
        //A little janky but it only needs to be set like this at the start of the game
        tile.CurrentPiece.CurrentTile = tile;
    }

    #endregion

    //This logic should probably be somewhere else, but its not too bad
    private void Update()
    {
        CheckIfMoveMade();
    }

    private void CheckIfMoveMade()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

            if (hit) hit.transform.GetComponent<Tile>()?.TryMove();
            
            ClearBoard();   
        }
    }

    public void HighlightValidMoves(List<Move> moves)
    {
        ClearBoard();

        foreach (Move move in moves)
        {
            GetTile(move.EndingPos).Selectable(move);
        }
    }

    public void Move(Move move)
    {
        Tile startingTile = GetTile(move.StartingPos);
        Tile endingTile = GetTile(move.EndingPos);

        startingTile.CurrentPiece.OnMove(move);

        endingTile.CurrentPiece = startingTile.CurrentPiece;
        startingTile.CurrentPiece = null;
    }

    #region Helpers

    public Tile GetTile(Vector2Int coordinates)
    {
        return Board[coordinates.x][coordinates.y];
    }

    public void ApplyToEachTile(Action<Tile> action)
    {
        for (int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                action.Invoke(Board[i][j]);
            }
        }
    }

    public void ClearBoard() => ApplyToEachTile((t) => t.Unselectable());

    #endregion

}
