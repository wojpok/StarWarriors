using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Biomes.Desert;

namespace ModName.Items
{
	public class BasicSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Virtaul Insanity");
			Tooltip.SetDefault("Future's made of virtual insanity");
		}

		public override void SetDefaults()
		{
			Item.damage = 2137;
			Item.DamageType = StarDefenderClass.StarDamage;
			Item.width = 200;
			Item.height = 200;
			Item.useTime = 4;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.Ale;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) {
			target.AddBuff(BuffID.OnFire, 100);
			target.AddBuff(BuffID.AbigailMinion, 100);

			/*player.AddBuff(BuffID.OnFire, 1000);
			player.Center.MoveTowards(
				player.Center + new Vector2(1000f, 1000f), 1000f
			);*/

			Main.NewText("ABC", 150, 250, 150);
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			// Main.NewText("Shoot", 150, 250, 150);

			player.velocity = new Vector2(0f, -10f);
			// player.gravity = 0f;

			
			return false;
        }

		private int counter = 60;

        /*public override void UpdateInventory(Player player) {
			Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);

			player.Center = player.Center.MoveTowards(target , 10f);
			player.gravity = 0;
		}*/
    }
}