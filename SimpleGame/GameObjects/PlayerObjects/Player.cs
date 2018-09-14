using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using SimpleGame.Annotations;
using SimpleGame.GameObjects.ShipObjects;

namespace SimpleGame.GameObjects.PlayerObjects
{
    public abstract class Player
    {
        public static bool IsGameOver;
        public Players PlayerType { get; set; }
        public ObservableCollection<Ship> Ships { get; }
        public ObservableCollection<int> AlreadyAttackedFields { get; }
        public event EventHandler Win;
        public ImageSource Flag { get; set; }

        protected Player(Players playerType)
        {
            PlayerType = playerType;
            AlreadyAttackedFields = new ObservableCollection<int>();
            Ships = new ObservableCollection<Ship>();
        }

        public void AddShips(IEnumerable<Ship> ships)
        {
            ships.ToList().ForEach(s => Ships.Add(s));
        }

        public void CheckIfGameOver(Player player)
        {            
            if (!player.Ships.All(s => s.IsDestroyed)) return;
            MessageBox.Show($"{player.PlayerType} has lost!!!");
            IsGameOver = true;
            OnWin();
        }

        public void OnWin()
        {
            var handler = Win;
            handler?.Invoke(this, EventArgs.Empty);
        }

        [CanBeNull]
        public Ship GetShipByField(int i)
        {
            foreach (var ship in Ships)
            {
                if (ship.Fields.Contains(i))
                    return ship;
            }

            return null;
        }

        public int? Attack(Player enemy, int field)
        {
            var ship = enemy.GetShipByField(field);
            ship?.UnderAttack(field);
            AlreadyAttackedFields.Add(field);
            CheckIfGameOver(enemy);
            return ship?.ShipLength;
        }
    }
}
