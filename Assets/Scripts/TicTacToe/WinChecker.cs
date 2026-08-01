// Evaluates the board for a winning line. Single responsibility: win detection only.
using System.Linq;

public static class WinChecker
{
    // The 8 winning lines as cell-index triplets (3 rows, 3 cols, 2 diagonals).
    private static readonly int[][] Lines =
    {
        new[] { 0, 1, 2 },
        new[] { 3, 4, 5 },
        new[] { 6, 7, 8 },
        new[] { 0, 3, 6 },
        new[] { 1, 4, 7 },
        new[] { 2, 5, 8 },
        new[] { 0, 4, 8 },
        new[] { 2, 4, 6 }
    };
    
    // Returns the winning mark, or Mark.None if there is no winner yet.
    public static Mark GetWinner(BoardModel board)
    {
        return (from line in Lines let a = board.GetCell(line[0])
            where a != Mark.None
            where a == board.GetCell(line[1]) && a == board.GetCell(line[2])
            select a).FirstOrDefault();
    }
}