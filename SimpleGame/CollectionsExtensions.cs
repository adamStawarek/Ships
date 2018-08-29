using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleGame
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

        public static bool ContainsCollection<T>(this IEnumerable<T> A, IEnumerable<T> B)
        {
            var tmp=new List<T>(A);
            foreach (var element in B)
            {
                if (tmp.Contains(element))
                    tmp.Remove(element);
                else
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ContainsAny<T>(this IEnumerable<T> A, IEnumerable<T> B)
        {
            return B.Any(element => A.Contains(element));
        }
    }
}