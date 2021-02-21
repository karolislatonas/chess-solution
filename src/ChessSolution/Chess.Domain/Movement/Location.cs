﻿using System;
using System.Diagnostics;

namespace Chess.Domain.Movement
{
    [DebuggerDisplay("X-{X}, Y-{Y}")]
    public class Location : IEquatable<Location>
    {
        public Location(int column, int row)
        {
            Column = column;
            Row = row;
        }

        public int Column { get; }

        public int Row { get; }

        public bool Equals(Location other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Location);
        }

        public override int GetHashCode()
        {
            return 17 ^ Column.GetHashCode() ^ Row.GetHashCode(); 
        }

        public static bool operator ==(Location location, Location otherLocation)
        {
            if (ReferenceEquals(location, null) && ReferenceEquals(otherLocation, null))
                return true;

            if (ReferenceEquals(location, null) || ReferenceEquals(otherLocation, null))
                return false;

            return location.Column == otherLocation.Column &&
                   location.Row == otherLocation.Row; 
        }

        public static bool operator !=(Location location, Location otherLocation)
        {
            return !(location == otherLocation);
        }

        public static Location LocationAt(int x, int y) => new Location(x, y);
    }
}
