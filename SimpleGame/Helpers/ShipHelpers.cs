using System;
using System.Collections.Generic;
using SimpleGame.GameObjects.ShipObjects;

namespace SimpleGame.Helpers
{
    public  class ShipHelpers
    {
        public static IEnumerable<Ship> GetRandomSetOfShips()
        {
            var assignedFields = new List<int>();
            var ships = new List<Ship>();
            ships.BuildShips<TinyShip>(2)
                .BuildShips<SmallShip>(2)
                .BuildShips<MediumShip>(2)
                .BuildShips<LargeShip>(1);

            foreach (var ship in ships)
            {                                                      
                bool areFieldsAssigned = false;
                while (!areFieldsAssigned)
                {
                    var randomPossiblePlaces = GetPossibleShipPositions(8, ship.ShipLength);
                    foreach (var randomPossiblePlace in randomPossiblePlaces)
                    {                        
                        if(assignedFields.ContainsAny(randomPossiblePlace))
                            continue;
                        areFieldsAssigned = true;
                        ship.AssignFields(randomPossiblePlace);
                        assignedFields.AddRange(randomPossiblePlace);
                        break;
                    }
                }                    
            }

            return ships;
        }

        private static List<int[]> GetPossibleShipPositions(int gridSideLength, int shipSize)
        {
            var gridMaxValue = gridSideLength * gridSideLength;
            Random r = new Random(DateTime.Now.Millisecond);
            int rnd = r.Next(1, gridMaxValue);
            List<int[]> lst = new List<int[]>();

            //1st case(right position)
            if (Ship.GetColumn(rnd,gridSideLength)+shipSize - 1 < gridSideLength)
            {
                int[] arr = new int[shipSize];
                for (int j = 0; j < shipSize; j++)
                {
                    arr[j] = rnd + j;
                }

                lst.Add(arr);
            }
            //2nd case(down position)
            if (Ship.GetRow(rnd,gridSideLength)+gridSideLength-1 < gridSideLength)
            {
                int[] arr = new int[shipSize];
                for (int j = 0; j < shipSize; j++)
                {
                    arr[j] = rnd + j * gridSideLength;
                }

                lst.Add(arr);
            }
            //3rd case(left position)
            if (Ship.GetColumn(rnd,gridSideLength) - shipSize >= 0)
            {
                int[] arr = new int[shipSize];
                for (int j = 0; j < shipSize; j++)
                {
                    arr[j] = rnd - j;
                }

                lst.Add(arr);
            }
            //4th case(up position)
            if (Ship.GetRow(rnd,gridSideLength)-shipSize >= 0)
            {
                int[] arr = new int[shipSize];
                for (int j = 0; j < shipSize; j++)
                {
                    arr[j] = rnd - j * gridSideLength;
                }

                lst.Add(arr);
            }
            return lst;
        }

    }
}
