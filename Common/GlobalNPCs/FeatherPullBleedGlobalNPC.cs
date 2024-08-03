using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace StarWarriors.Common.GlobalNPCs {
    internal class FeatherPullBleedGlobalNPC : GlobalNPC {
        public override bool InstancePerEntity => true;
        public bool FeatherBleedDebuff;

        public override void ResetEffects(NPC npc) {
            FeatherBleedDebuff = false;
        }

        public static readonly int FeatherPullBleedDamage = 30;

        public override void UpdateLifeRegen(NPC npc, ref int damage) {
            if (FeatherBleedDebuff) {
                if (npc.lifeRegen > 0) {
                    npc.lifeRegen = 0;
                }

                npc.lifeRegen -= FeatherPullBleedDamage;
                if (damage < FeatherPullBleedDamage) {
                    damage = FeatherPullBleedDamage;
                }

                npc.HitEffect(0, FeatherPullBleedDamage);
            }
        }
    }
}
