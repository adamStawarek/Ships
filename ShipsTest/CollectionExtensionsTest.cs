using NUnit.Framework;
using SimpleGame;
using System;
using System.Collections.Generic;
using System.Linq;
using SimpleGame.GameObjects.ShipObjects;
using Assert = NUnit.Framework.Assert;

namespace ShipsTest
{
    [TestFixture]
    public class CollectionExtensionsTest
    {
        [Test]
        public void BuildShip_Method_Returns_Proper_Number_Of_Given_types_Ships()
        {
            int numberOfTinyShips = 6;
            int numberOfMediumShips = 1;
            int numberOfSmallShips = 2;
            int numberOfLargeShips = 2;
            var ships=new List<Ship>();
            ships.BuildShips<TinyShip>(numberOfTinyShips)
                .BuildShips<LargeShip>(numberOfLargeShips)
                .BuildShips<SmallShip>(numberOfSmallShips)
                .BuildShips<MediumShip>(numberOfMediumShips);
            Assert.IsTrue(ships.Count(s => s.GetType()==typeof(TinyShip))==
                numberOfTinyShips);
            Assert.IsTrue(ships.Count(s => s.GetType() == typeof(SmallShip)) ==
                          numberOfSmallShips);
            Assert.IsTrue(ships.Count(s => s.GetType() == typeof(MediumShip)) ==
                          numberOfMediumShips);
            Assert.IsTrue(ships.Count(s => s.GetType() == typeof(LargeShip)) ==
                          numberOfLargeShips);

        }

        [TestCase(0)]
        [TestCase(-12)]       
        public void BuildShip_Method_Throws_AnException_When_Parameter_NumberOfShips_Is_Not_Positive_Integer(int numberOfShips)
        {
            var ships = new List<Ship>();
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => ships.BuildShips<TinyShip>(numberOfShips));
            Assert.That(ex.ParamName,Is.EqualTo("Negative parameters are not supported"));
           
        }           
    }
}
