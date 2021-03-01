using Chess.Domain.Movement;
using System.Collections.Generic;

namespace Chess.Domain
{
    public class BoardMetadata
    {
        private readonly IReadOnlyDictionary<int, string> ColumnNames = new Dictionary<int, string>()
        {
            [1] = "a",
            [2] = "b",
            [3] = "c",
            [4] = "d",
            [5] = "e",
            [6] = "f",
            [7] = "g",
            [8] = "h",
        };

        private readonly IReadOnlyDictionary<int, string> RowNames = new Dictionary<int, string>()
        {
            [1] = "1",
            [2] = "2",
            [3] = "3",
            [4] = "4",
            [5] = "5",
            [6] = "6",
            [7] = "7",
            [8] = "8",
        };

        public string GetColumnName(int column)
        {
            return ColumnNames[column];
        }

        public string GetRowName(int row)
        {
            return RowNames[row];
        }

        public string GetCellName(Location location)
        {
            return GetCellName(location.Column, location.Row);
        }

        public string GetCellName(int column, int row)
        {
            return $"{GetColumnName(column)}{GetRowName(row)}";
        }

        public static BoardMetadata Value { get; } = new BoardMetadata();
    }
}
