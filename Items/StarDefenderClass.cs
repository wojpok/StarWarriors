using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using System;

namespace StarWarriors.Items {
    // This class handles everything for our custom damage class
    // Any class that we wish to be using our custom damage class will derive from this class, instead of ModItem
    public class StarDefenderClass : DamageClass { }

    /*
    public class PermanatingBuff : ModPlayer {
        int c = 0;

        public override void PostUpdate() {
            Player.GetDamage(StarDefenderClass.StarDamage) += (float)Math.Sin(c++);
        }
    }
    */
}