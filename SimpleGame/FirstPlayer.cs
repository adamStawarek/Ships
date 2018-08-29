using System;
using System.Windows.Media.Imaging;

namespace SimpleGame
{
    public class FirstPlayer : Player
    {
        public FirstPlayer(Players playerType) : base(playerType)
        {
            Flag = new BitmapImage(new Uri(@"\Images\america.png", UriKind.Relative));
        }
    }
}