namespace Chess.Domain.Extensions
{
    public static class ChessColorExtensions
    {
        public static bool IsWhite(this ChessColor chessColor)
        {
            return chessColor == ChessColor.White;
        }

        public static ChessColor GetOpposite(this ChessColor chessColor)
        {
            return chessColor == ChessColor.White ? ChessColor.Black : ChessColor.White;
        }
    }
}
