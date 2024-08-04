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
using System.Reflection;
using System.Text.RegularExpressions;
using rail;
using StarWarriors.Content.Projectiles;

namespace StarWarriors.Common.Commands
{
    interface IFieldModifier {
        public void TryApply(string arg);
        public string Elaborate();
        public bool IsModified { get; }
    }

    class FieldModifier<T> : IFieldModifier where T : IComparable<T> {
        private FieldInfo Field;
        private String ClassName;
        public T InitialValue { private set; get; }
        public T CurrentValue { private set; get; }

        public bool IsModified { get => (InitialValue.CompareTo(CurrentValue)) != 0; }

        public FieldModifier(Type ClassType, String Name) {
            Field = typeof(FeatherDaggerProjectile).GetField(Name,
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            ClassName = ClassType.Name;
            InitialValue = CurrentValue = (T) Field.GetValue(null);
        }
    
        public string Elaborate() { 
            return $"{ClassName}:{Field.Name} I:{InitialValue} C:{CurrentValue}"; 
        }

        public virtual object? TryParse(string str) { return null; }

        public void TryApply(String arg) {
            try {
                var v = TryParse(arg);

                if(v != null) {
                    Field.SetValue(null, v);
                    CurrentValue = (T) v;

                    Main.NewText("Updated");
                }
            }
            catch {
                Main.NewText("Error");
            }
        }
    }

    class IntFieldModifier : FieldModifier<int> {
        public IntFieldModifier(Type ClassType, string Name) : base(ClassType, Name) {}

        public override object TryParse(string str) {
            return int.Parse(str);
        }
    }

    class FloatFieldModifier : FieldModifier<float> {
        public FloatFieldModifier(Type ClassType, string Name) : base(ClassType, Name) {}

        public override object TryParse(string str) {
            return float.Parse(str);
        }
    }

    internal class ModifyCommand : ModCommand {
        private readonly Dictionary<String, IFieldModifier> Fields = new () {
            {"k1", new FloatFieldModifier(typeof(FeatherDaggerProjectile), "PullingTimerReset")},
            {"k2", new FloatFieldModifier(typeof(FeatherDaggerProjectile), "PullingStrength")}
        };

        public override string Command => "m";

        public override CommandType Type => CommandType.Chat;

        public override void Action(CommandCaller caller, string input, string[] args) {
            try {
                switch (args[0]) {

                    case "ls":
                        foreach (var kv in Fields) {
                            Main.NewText($"{kv.Key} => {kv.Value.Elaborate()}");
                        }
                        return;

                    case "s":
                        Fields[args[1]].TryApply(args[2]);
                        return;

                    case "ch":
                        foreach (var kv in Fields) {
                            if (kv.Value.IsModified)
                                Main.NewText($"{kv.Key} => {kv.Value.Elaborate()}");
                        }
                        return;

                }
            }
            catch (Exception e) {
                Main.NewText(e);
            }
        }

        public override string Usage
           => "";

        // A short description of this command
        public override string Description
            => "Modifies internal exposed constants";
    }
}
