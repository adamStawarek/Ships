using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SimpleGame.GameObjects.ShipObjects
{
    public abstract class Ship
    {
        public static readonly ImageSource MissedShoot = new BitmapImage(new Uri(@"\Images\MissedShoot.png", UriKind.Relative));

        public static int GetRow(int fieldNumber,int gridWidth)
        {
            return (fieldNumber - 1) / gridWidth + 1;
        }

        public static int GetColumn(int fieldNumber, int gridWidth)
        {
            var val = fieldNumber % gridWidth;
            return val==0?gridWidth:val;
        }

        public static ImageSource GetDestroyedShipImage(int length)
        {
            return new BitmapImage(new Uri(@"\Images\ship_"+length+"_destroyed.png", UriKind.Relative));
        }

        public bool IsDestroyed { get; set; }
        public byte ShipLength { get; set; }
        public ObservableCollection<int> Fields { get; set; }
        public event EventHandler Destroyed;

        protected Ship(byte shipLength)
        {           
            ShipLength = shipLength;
            Fields=new ObservableCollection<int>();
            Fields.CollectionChanged += CheckIfDestroyed;
        }

        public ImageSource GetShipImage()
        {
            return new BitmapImage(new Uri(@"\Images\ship_"+ShipLength+".png", UriKind.Relative));
        }

        public void AssignFields(IEnumerable<int> fields)
        {
            if (fields == null || fields.Count() != ShipLength)
                throw new ArgumentException($"inputIV should be int[{ShipLength}]");
            foreach (var field in fields)
            {
                Fields.Add(field);
            }
        }

        public void UnderAttack(int field)
        {
            if (Fields.Contains(field))
                Fields.Remove(field);
        }

        public void OnDestroyed()
        {
            EventHandler handler = Destroyed;
            handler?.Invoke(this, EventArgs.Empty);
        }

        public override string ToString()
        {
            StringBuilder sb=new StringBuilder();
            sb.Append("{");
            Fields.ToList().ForEach(f=>sb.Append(String.Format($"{f} ")));
            sb.Append("}");
            return sb.ToString();
        }

        private void CheckIfDestroyed(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Fields.Count != 0) return;
            IsDestroyed = true;
            MessageBox.Show("Ship destroyed");
            OnDestroyed();

        }
    }
}
