// Single source of truth for mapping between world position, tilemap cells, and board indices.
using UnityEngine;

public class BoardLayout : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Vector3Int origin;
    
    // World position -> board index (0..8). Returns false if outside the 3x3 board.
    public bool TryWorldToIndex(Vector3 worldPosition, out int index)
    {
        index = -1;
        var cell = grid.WorldToCell(worldPosition);
        var col = cell.x - origin.x;
        var row = cell.y - origin.y;
        if (col < 0 || col >= BoardModel.Size || row < 0 || row >= BoardModel.Size)
            return false;
        index = BoardModel.ToIndex(col, row);
        return true;
    }

    // Board index -> tilemap cell coordinate.
    public Vector3Int IndexToCell(int index)
        => new(origin.x + BoardModel.ColOf(index),
            origin.y + BoardModel.RowOf(index),
            origin.z);
}