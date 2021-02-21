using System;
using System.Diagnostics;

namespace Chess.Domain.Movement
{
    [DebuggerDisplay("X-{X}, Y-{Y}")]
    public class Location : IEquatable<Location>
    {
        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }

        public int Y { get; }

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
            return 17 * X.GetHashCode() ^ Y.GetHashCode(); 
        }

        public static bool operator ==(Location location, Location otherLocation)
        {
            if (ReferenceEquals(location, null) && ReferenceEquals(otherLocation, null))
                return true;

            if (ReferenceEquals(location, null) || ReferenceEquals(otherLocation, null))
                return false;

            return location.X == otherLocation.X &&
                   location.Y == otherLocation.Y; 
        }

        public static bool operator !=(Location location, Location otherLocation)
        {
            return !(location == otherLocation);
        }

        public static Location LocationAt(int x, int y) => new Location(x, y);
    }
}
