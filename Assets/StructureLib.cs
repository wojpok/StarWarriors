using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;

namespace StarWarriors.Assets {
    public class Structure {
        public string Name { get; private set; }
        private string Path;
        public readonly Point16 Size;

        public Structure(string name) {
            Name = name;
            Path = $"Assets/Structures/{name}";

            Size = new Point16();

            StructureHelper.Generator.GetDimensions(
                Path, StarWarriors.Instance, ref Size
            );
        }

        public void SpawnAt(int x, int y) {
            Spawn((short)x, (short)y);
        }

        public void SpawnAt(float x, float y) {
            Spawn((short)(x / 16f), (short)(y / 16f));
        }

        public void SpawnAt(Vector2 position) {
            SpawnAt(position.X, position.Y);
        }

        private void Spawn(short x, short y) {
            StructureHelper.Generator.GenerateStructure(
                Path, new Point16(x, y), StarWarriors.Instance
            );
        }
    }

    internal class StructureLib {
        public static readonly Structure GemsparkHouse = new("GemsparkHouse");
    }
}
