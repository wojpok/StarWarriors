using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using Ionic.Zlib;

namespace StarWarriors.Common.Player {
    internal class PlayerEndurance : ModPlayer {
        public float DamageReduction = 1.0f;


        public override void ResetEffects() {
            DamageReduction = 1.0f;
        }

        // public override void OnHurt(Terraria.Player.HurtInfo info) {
        //     info.Damage -= (int) (info.Damage * DamageReduction);
        //     
        //     base.OnHurt(info);
        // }


        public override void ModifyHurt(ref Terraria.Player.HurtModifiers modifiers) {
            //Main.NewText(this.Player.name);
            //
            //Main.NewText(DamageReduction);

            modifiers.FinalDamage *= 1f - DamageReduction;
        }

        // public override void OnHurt(Player.HurtInfo info) {
        //     // On Hurt is used in this example to act upon another player being hurt.
        //     // If the player who was hurt was defended, check if the local player should take the remaining damage for them
        //     Player localPlayer = Main.LocalPlayer;
        //     if (defendedByAbsorbTeamDamageEffect && Player != localPlayer && IsClosestShieldWearerInRange(localPlayer, Player.Center, Player.team)) {
        //         // The intention of AbsorbTeamDamageAccessory is to transfer 30% of damage taken by teammates to the wearer.
        //         // In ModifiedHurt, we reduce the damage by 30%. The resulting reduced damage is passed to OnHurt, where the player wearing AbsorbTeamDamageAccessory hurts themselves.
        //         // Since OnHurt is provided with the damage already reduced by 30%, we need to reverse the math to determine how much the damage was originally reduced by
        //         // Working through the math, the amount of damage that was reduced is equal to: damage * (percent / (1 - percent))
        //         float percent = AbsorbTeamDamageAccessory.DamageAbsorptionMultiplier;
        //         int damage = (int)(info.Damage * (percent / (1 - percent)));
        // 
        //         // Don't bother pinging the defending player and upsetting their immunity frames if the portion of damage we're taking rounds down to 0
        //         if (damage > 0) {
        //             localPlayer.Hurt(PlayerDeathReason.LegacyEmpty(), damage, 0);
        //         }
        //     }
        // }
    }
}
