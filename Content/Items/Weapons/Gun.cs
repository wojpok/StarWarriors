using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace StarWarriors.Content.Items.Weapons
{
    class CustomDrawAnimation : DrawAnimationVertical
    {
        public CustomDrawAnimation(int frameCount)
            : base(1, frameCount, false) { }

        // disable auto animation
        public override void Update() { }

        public void Next()
        {
            Frame = (Frame + 1) % FrameCount;
        }
    }

    internal class Gun : ModItem
    {

        private static CustomDrawAnimation Animation = new(2);
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            // Main.itemFrame[Item.type] = 2;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 2));
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.channel = true;

            Item.useTime = 1;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.value = 10000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            //Item.shoot = ProjectileID.Ale;

            Item.scale = 2f;

            Item.shoot = ProjectileID.Bullet; // For some reason, all the guns in the vanilla source have this.
            Item.shootSpeed = 10f; // The speed of the projectile (measured in pixels per frame.)
            Item.useAmmo = AmmoID.None; // The "

            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(platinum: 999, gold: 99, silver: 99, copper: 99);
        }

        private bool AltMode = false;


        public override bool AltFunctionUse(Player player)
        {
            Main.NewText("AltCanUse");
            AltMode = !AltMode;
            Animation.Next();

            if (AltMode)
            {
                Item.useTime = 40;
            }
            else
            {
                Item.useTime = 1;
            }

            return true;
        }

        public override bool CanUseItem(Player player)
        {

            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            // player.channel = true;

            //Spawn the rod projectile, set the first counter to 30 updates or 1/2 a second
            Projectile.NewProjectile(
                source,
                position,
                velocity,
                type,
                0,
                knockback,
                player.whoAmI,
                30f,
                0
            );

            //Spawn the vortex
            Projectile.NewProjectile(
                source,
                position,
                velocity,
                ModContent.ProjectileType<GunVisuals>(),
                damage,
                knockback,
                player.whoAmI,
                AltMode ? 1 : 0,
                0f
            );

            //Makes sure original projectile doesn't spawn
            return false;
        }
    }

    public class GunVisuals : ModProjectile
    {
        //Dusts to spawn
        private int[] dusts = { 60, 6, 64, 61, 59, 62 };

        public override string Texture => "StarWarriors/Content/Items/Weapons/Gun";

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.scale = 1f;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Rainbow Devastation Rod");
        }

        public override void AI()
        {

            Player player = Main.player[Projectile.owner];
            Vector2 mountedCenter = player.RotatedRelativePoint(player.MountedCenter);

            Projectile.frame = (int)Projectile.ai[0];

            //Using Timers for implementing shoot cooldowns
            bool cooldown = true;
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] >= 10)
            {
                Projectile.ai[1] = 0;
                cooldown = false;
            }

            //If cooldown is over and we inside current player(for multiplayer)
            if (!cooldown && Main.myPlayer == Projectile.owner)
            {

                // We can shoot if the button is pressed and there is still ammunition, and the player has items.
                bool canShoot = player.channel;

                if (canShoot)
                {
                    Projectile.timeLeft = 20;
                }
                else
                {
                    Projectile.Kill();
                }
            }
            // Updating projectile parameters
            Projectile.position = player.RotatedRelativePoint(player.MountedCenter, false, false) - Projectile.Size / 2f;
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == -1 ? (float)Math.PI : 0f);
            Projectile.spriteDirection = Projectile.direction;

            player.ChangeDir(Projectile.direction); // Update player direction
            player.heldProj = Projectile.whoAmI; // The sprite is drawn in hand (If disabled, the sprite will be behind the player)
            Projectile.timeLeft = 2; // As long as the weapon can shoot, we update the life time
            player.SetDummyItemTime(2); // Always setting Item.useTime, Item.useAnimation to 2 frames so the Shoot method will never be called once again
                                        // It is necessary for the player to understand the position of the item. (for example, how to rotate hand)
            player.itemRotation = MathHelper.WrapAngle((float)Math.Atan2(Projectile.velocity.Y * Projectile.direction, Projectile.velocity.X * Projectile.direction));
        }

        public override void OnKill(int timeLeft)
        {
            Main.NewText("Died");
        }
    }
}
