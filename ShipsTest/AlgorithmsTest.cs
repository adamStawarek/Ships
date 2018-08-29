using NUnit.Framework;
using SimpleGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SimpleGame.Helpers;
using SimpleGame.ViewModels;
using Assert = NUnit.Framework.Assert;

namespace ShipsTest
{
    [TestFixture]
    public class AlgorithmsTest
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

        [Test]
        public void GetRandomSetOfShips_Returns_Different_Positions_For_Each_Ship()
        {
            var ships = ShipHelpers.GetRandomSetOfShips();
            var allFields=new List<int>();
            ships.ToList().ForEach(s=>allFields.AddRange(s.Fields));
            Assert.IsTrue(allFields.Count==allFields.Distinct().Count());
        }

       
    }
}
