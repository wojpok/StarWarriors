using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using Terraria.Audio;

namespace ModName {
    public class ModName : Mod {
        public override void Load() {
            base.Load();
        }
    }

    class ModSound {
        public static SoundStyle Pipe = new("ModName/Audio/pipe");
    }
}