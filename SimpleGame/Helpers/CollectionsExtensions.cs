using System;
using System.Collections.Generic;
using System.Linq;
using SimpleGame.GameObjects.ShipsObjects;

namespace SimpleGame.Helpers
{
    public static class CollectionsExtensions
    {
        public static List<Ship> BuildShips<T>(this List<Ship> ships,int amount) where T : Ship,new()
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException("Negative parameters are not supported");
            }
                
            while (amount-->0)
            {
                ships.Add(new T());
            }

            return ships;
        }

        public static bool ContainsAny<T>(this IEnumerable<T> A, IEnumerable<T> B)
        {
            return B.Any(element => A.Contains(element));
        }
    }
}