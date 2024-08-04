using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace StarWarriors.Common.Classes {
    internal class AirborneDamageClass : DamageClass {
        //public static readonly DamageClass AirborneDamage = new AirborneDamageClass();
        // public new string Name => "Airborne";
        // public new string DisplayName => "Airborne";
        //protected override string DisplayNameInternal => "star damage";


        public override bool GetEffectInheritance(DamageClass damageClass) {
            return damageClass == Ranged;
        }

        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass) {
            if (damageClass == Generic || damageClass == Ranged)
                return StatInheritanceData.Full;

            return StatInheritanceData.None;
        }
    }
}
