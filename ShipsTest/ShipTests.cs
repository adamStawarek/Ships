using NUnit.Framework;
using SimpleGame;
using SimpleGame.GameObjects.ShipObjects;
using SimpleGame.Helpers;

namespace ShipsTest
{
    [TestFixture]
    public class ShipTests
    {
        [TestCase(1,ExpectedResult = 1)]
        [TestCase(9, ExpectedResult = 2)]
        [TestCase(17, ExpectedResult = 3)]
        [TestCase(57, ExpectedResult = 8)]
        public int When_Passing_Ship_Part_Grid_Position_Number_Returns_Corresponding_Grid_Row(int position)
        {
            return Ship.GetRow(position, 8);
        }

        [TestCase(1, ExpectedResult = 1)]
        [TestCase(6, ExpectedResult = 6)]
        [TestCase(64, ExpectedResult = 8)]
        public int When_Passing_Ship_Part_Grid_Position_Number_Returns_Corresponding_Grid_Column(int position)
        {
            return Ship.GetColumn(position, 8);
        }
    }
}