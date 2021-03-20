using Chess.Domain;
using Chess.Domain.Movement;
using Chess.Domain.Pieces;
using Microsoft.AspNetCore.Components;
using System;

namespace Chess.WebUI.Components
{
    public partial class PieceView
    {
        [Parameter]
        public Location Location { get; set; }

        [Parameter]
        public IPiece Piece { get; set; }

        [Parameter]
        public bool IsDragged { get; set; }

        [Parameter]
        public Action<PieceView> OnDragStarted { get; set; }

        [Parameter]
        public Action<PieceView> OnDragEnded { get; set; }

        private void HandleDragStarted()
        {
            IsDragged = true;
            OnDragStarted?.Invoke(this);
        }

        private void HandleDragEnded()
        {
            IsDragged = false;
            OnDragEnded?.Invoke(this);
        }

        public string GetImageUrl()
        {
            var color = Piece.Color == ChessColor.Black ? "b" : "w";

            return Piece switch
            {
                King _ => $"https://images.chesscomfiles.com/chess-themes/pieces/neo/150/{color}k.png",
                Pawn _ => $"https://images.chesscomfiles.com/chess-themes/pieces/neo/150/{color}p.png",
                Rook _ => $"https://images.chesscomfiles.com/chess-themes/pieces/neo/150/{color}r.png",
                Queen _ => $"https://images.chesscomfiles.com/chess-themes/pieces/neo/150/{color}q.png",
                Bishop _ => $"https://images.chesscomfiles.com/chess-themes/pieces/neo/150/{color}b.png",
                Knight _ => $"https://images.chesscomfiles.com/chess-themes/pieces/neo/150/{color}n.png",

                _ => throw new Exception("Unknown piece")
            };
        }
    }
}
