using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SimpleGame;
using SimpleGame.GameObjects.ShipObjects;
using SimpleGame.Helpers;

namespace ShipsTest
{
    [TestFixture]
    public class ShipHelpersTests
    {
        [Test]
        public void GetRandomSetOfShips_Returns_Different_Positions_For_Each_Ship()
        {
            var ships = ShipHelpers.GetRandomSetOfShips();
            var allFields = new List<int>();
            ships.ToList().ForEach(s => allFields.AddRange(s.Fields));
            Assert.IsTrue(allFields.Count == allFields.Distinct().Count());
        }

        [TestCaseSource(nameof(_ships))]
        public void Every_Ship_Has_Parts_Only_In_One_Column_Or_Row(Ship ship)
        {
            bool allShipPartsInOneRow = true, allShipPartsInOneColumn = true;
            var gridPositionOfFirstShipPart =
                new Tuple<int, int>(Ship.GetRow(ship.Fields[0], 8), Ship.GetColumn(ship.Fields[0], 8));
            for (int i = 1; i < ship.Fields.Count; i++)
            {
                if (Ship.GetRow(ship.Fields[i], 8) != gridPositionOfFirstShipPart.Item1)
                    allShipPartsInOneColumn = false;
                if (Ship.GetColumn(ship.Fields[i], 8) != gridPositionOfFirstShipPart.Item2)
                    allShipPartsInOneRow = false;
            }
            Assert.That(allShipPartsInOneColumn || allShipPartsInOneRow, Is.True);
        }

        private static List<Ship> _ships = new List<Ship>(ShipHelpers.GetRandomSetOfShips());
    }
}