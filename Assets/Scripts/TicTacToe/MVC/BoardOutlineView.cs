// Paints an outline tile on every board cell so players can see where to click. (View)
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardOutlineView : MonoBehaviour
{
    [SerializeField] private Tilemap outlineTilemap;
    [SerializeField] private BoardLayout layout;
    [SerializeField] private TileBase outlineTile;

    // Draw the outline once at startup; it stays put as marks are placed over it.
    private void Start()
    {
        for (var i = 0; i < BoardModel.CellCount; i++)
            outlineTilemap.SetTile(layout.IndexToCell(i), outlineTile);
    }
}