using Microsoft.Build.Evaluation;
using StarWarriors.Common.Classes;
using StarWarriors.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace StarWarriors.Content.Items.Weapons
{
    public class FeatherDagger : ModItem
    {
        public override void SetDefaults()
        {

            // Common Properties
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(silver: 5);
            Item.maxStack = 1;

            // Use Properties
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.consumable = false;

            // Weapon Properties			
            Item.damage = 20;
            Item.knockBack = 5f;
            Item.noUseGraphic = true; // The item should not be visible when used
            Item.noMelee = true; // The projectile will do the damage and not the item
            Item.DamageType = ModContent.GetInstance<AirborneDamageClass>();

            // Projectile Properties
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<FeatherDaggerProjectile>(); // The projectile that will be thrown
            //Item.shoot = ProjectileID.WoodenArrowFriendly;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient(ItemID.DirtBlock, 20)
                .Register();
        }
    }
}