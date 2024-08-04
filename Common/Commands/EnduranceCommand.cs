using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using StarWarriors.Common.Buffs;
using StarWarriors.Common.Player;

namespace StarWarriors.Common.Commands {
    internal class EnduranceCommand : ModCommand {
        public override CommandType Type
            => CommandType.Chat;

        // The desired text to trigger this command
        public override string Command
            => "e";

        // A short description of this command
        public override string Description
            => "Test command";

        public override void Action(CommandCaller caller, string input, string[] args) {
            // if (Main.netMode == NetmodeID.MultiplayerClient) {
            //     Main.NewText("Usage?");
            // }

            //Main.NewText("Usage?");

            //caller.Player.AddBuff(ModContent.BuffType<EnduranceBuff>(), 60 * 60 * 10);


            if(!Int32.TryParse(args[0] ?? "0", out int val)) {
                Main.NewText("Invalid Data");
                return;
            };
                

            Main.NewText(val / 100f);

            var pe = caller.Player.GetModPlayer<PlayerEndurance>();

            Main.NewText(caller.Player.name);

            pe.DamageReduction = val / 100f;
        }
    }
}
