using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameLibrary
{
    public class Goomba : Enemy
    {
        public Bitmap Img { get; private set; }

        public Goomba(int level, Bitmap img) : base(level) {
            Img = img;
        }
    }
}
