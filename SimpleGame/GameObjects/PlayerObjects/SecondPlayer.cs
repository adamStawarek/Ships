﻿using System;
using System.Windows.Media.Imaging;

namespace SimpleGame.GameObjects.PlayerObjects
{
    public class SecondPlayer : Player
    {
        public SecondPlayer(Players playerType) : base(playerType)
        {
            Flag = new BitmapImage(new Uri(@"\Images\russia.png",UriKind.Relative));
        }
    }
}