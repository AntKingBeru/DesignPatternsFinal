// Renders the board model onto a Tilemap. Reacts to model events only — contains no game logic. (View)
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardView : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private BoardLayout layout;
    [SerializeField] private TileBase xTile;
    [SerializeField] private TileBase oTile;
    
    private BoardModel _board;

    private void Start()
    {
        _board = GameManager.Instance.Board;
        _board.CellChanged += OnCellChanged;
        _board.BoardReset += OnBoardReset;
        OnBoardReset();
    }

    private void OnDestroy()
    {
        if (_board == null)
            return;
        _board.CellChanged -= OnCellChanged;
        _board.BoardReset -= OnBoardReset;
    }
    
    private void OnCellChanged(int index, Mark mark) =>
        tilemap.SetTile(layout.IndexToCell(index), MarkToTile(mark));

    private void OnBoardReset()
    {
        for (var i = 0; i < BoardModel.CellCount; i++)
            tilemap.SetTile(layout.IndexToCell(i), null);
    }

    private TileBase MarkToTile(Mark mark)
    {
        return mark switch
        {
            Mark.X => xTile,
            Mark.O => oTile,
            _ => null
        };
    }
}