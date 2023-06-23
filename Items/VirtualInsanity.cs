using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Biomes.Desert;
using System.Collections.Generic;

namespace ModName.Items
{
	public class VirtualInsanity : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Virtaul Insanity");
            Tooltip.SetDefault(T.ParseTooltips(new() {
				"Right click to summon huge commet",
				"A gift from heaven",
			}));
		}

		bool mode = false;

		public override void SetDefaults()
		{
			if(mode) {
				Item.damage = 2137;
				Item.DamageType = StarDefenderClass.StarDamage;
			}
			else {
				Item.damage = 1337;
				Item.DamageType = StarDefenderClass.Melee;
			}
			
			Item.width = 400;
			Item.height = 400;
			Item.scale = 2f;
			Item.useTime = 40;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			//Item.shoot = ProjectileID.Ale;

			Item.shoot = ModContent.ProjectileType<Comet>(); // For some reason, all the guns in the vanilla source have this.
			Item.shootSpeed = 10f; // The speed of the projectile (measured in pixels per frame.)
			Item.useAmmo = AmmoID.None; // The "
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

			/*player.AddBuff(BuffID.OnFire, 1000);
			player.Center.MoveTowards(
				player.Center + new Vector2(1000f, 1000f), 1000f
			);

			Main.NewText("ABC", 150, 250, 150);*/
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {

			if(!mode && false) {
				return false;
            }

			// Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);

			var spos = position + V.V2(-500f, -500f);
			var nvel = V.MouseWorldPos() - spos;
			nvel.Normalize();
			nvel *= 20f;

			// Main.NewText($"{spos} {nvel} {V.Mouse()}");

			Projectile.NewProjectileDirect(
				source, 
				spos,
				nvel, 
				type, 
				1, 
				knockback, 
				player.whoAmI
			);


			return false;
		}


        private int counter = 60;

        /*public override void UpdateInventory(Player player) {
			Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);

			player.Center = player.Center.MoveTowards(target , 10f);
			player.gravity = 0;
		}*/

        class ES : IEntitySource {
            public string Context => "The fuck is context?";
        }

        public override bool AltFunctionUse(Player player) {
			mode = !mode;

			SetDefaults();

			/*var p = new Vector2(Main.mouseX, Main.mouseY);
			new Comet(p, p + new Vector2(0, -10f));

			Projectile.NewProjectile(new ES(), player.Center, player.velocity,, Item.damage, Item.knockBack);


			return true;*/

			Main.NewText($"{player.position}");

			return true;
        }
    }
}