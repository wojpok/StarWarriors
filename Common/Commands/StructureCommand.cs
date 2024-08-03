using StructureHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace StarWarriors.Common.Commands {
    internal class StructureCommand : ModCommand {
        public override string Command => "spawn";

        public override CommandType Type => CommandType.World;

        public override void Action(CommandCaller caller, string input, string[] args) {
            var point = new Point16((int)(Main.MouseWorld.X / 16f), (int)(Main.MouseWorld.Y / 16f));

            Main.NewText(point);
            Main.NewText(caller.Player.position);

            bool succ = Generator.GenerateStructure(
                "Assets/Structures/GemsparkHouse",
               point,
                StarWarriors.Instance
            );

            Main.NewText(succ);
        }
    }
}
