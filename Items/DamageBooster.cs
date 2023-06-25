using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace StarWarriors.Items {
	[AutoloadEquip(EquipType.Shoes)]
	public class DamageBooster : ModItem {
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Example Hermes Boots");
			Tooltip.SetDefault("The wearer can run super fast");
		}

		public override void SetDefaults() {
			Item.width = 28;
			Item.height = 24;
			Item.accessory = true; // Makes this item an accessory.
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(gold: 1); // Sets the item sell price to one gold coin.
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.accRunSpeed = 60f; // The player's maximum run speed with accessories
			player.moveSpeed += 10f; // The acceleration multiplier of the player's movement speed
			player.GetDamage(StarDefenderClass.StarDamage) += 2f;
		}
	}
}