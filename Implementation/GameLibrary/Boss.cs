using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameLibrary { 
  public class Boss : Enemy {
        private static string name = "FINAL BOSS";
        private const float MULTIPLYER = 10;
        public Boss(int level, Bitmap img) : base(level, img, name) {
  
            Health = level * MULTIPLYER;
            Mana = level * MULTIPLYER;
            Str = level * MULTIPLYER;
            Def = level * MULTIPLYER;
            
            }
        }
    }