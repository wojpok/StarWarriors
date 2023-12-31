﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

namespace StarWarriors {
    internal class V {

        public static Vector2 V2(float a, float b) {
            return new Vector2(a, b);
        }

        public static Vector2 Mouse() {
            return new Vector2(Main.mouseX, Main.mouseY);
        }

        public static Vector2 MouseWorldPos() {
            return Main.screenPosition + Mouse();
        }

    }

    internal class T {
        public static string ParseTooltips(List<string> xs) {
            return xs.Aggregate("", (acc, x) => $"{x}\n{acc}");
        }
    }
}
