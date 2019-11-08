using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameLibrary { 
  public class Boss : Enemy {
        private string name = "FINAL BOSS";
        private const float MULTIPLYER = 10;
        public Bitmap Img { get; private set; }

        public Boss(Bitmap img) {
            Img = img;
            name = name;

            Health *= MULTIPLYER;
            Mana *= MULTIPLYER;
            Str *= MULTIPLYER;
            Def *= MULTIPLYER;

            XpDropped = 100000;
            }
        }
    }