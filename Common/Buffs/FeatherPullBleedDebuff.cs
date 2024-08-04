using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using StarWarriors.Common.GlobalNPCs;

namespace StarWarriors.Common.Buffs {
    internal class FeatherPullBleedDebuff : ModBuff {
        public override void SetStaticDefaults() {
            // BuffID.Sets.GrantImmunityWith[Type].Add(BuffID.BoneJavelin);
        }

        public override void Update(NPC npc, ref int buffIndex) {
            npc.GetGlobalNPC<FeatherPullBleedGlobalNPC>().FeatherBleedDebuff = true;
        }
    }
}
