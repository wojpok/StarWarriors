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

namespace StarWarriors.Items {
    class CustomDrawAnimation : DrawAnimationVertical {
        public CustomDrawAnimation(int frameCount)
            : base(1, frameCount, false) { }

        // disable auto animation
        public override void Update() { }
    
        public void Next() {
            Frame = (Frame + 1) % FrameCount;
        }
    }

    internal class Gun : ModItem {

        private static CustomDrawAnimation Animation = new(2);
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();

            // Main.itemFrame[Item.type] = 2;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 2));
        }

        public override void SetDefaults() {
            base.SetDefaults();
            
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
        }

        private bool AltMode = false;


        public override bool AltFunctionUse(Player player) {
            Main.NewText("AltCanUse");
            AltMode = !AltMode;
            Animation.Next();

            if (AltMode) {
                Item.useTime = 40;
            }
            else {
                Item.useTime = 1;
            }

            return true;
        }

        public override bool CanUseItem(Player player) {
            
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {

            player.channel = true;

            //Spawn the rod projectile, set the first counter to 30 updates or 1/2 a second
            Projectile.NewProjectile(source, position, velocity, type, 0, knockback, player.whoAmI, 30f, 0f);

            //Spawn the vortex
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<GunVisuals>(), damage, knockback, player.whoAmI, 0f, 0f);

            //Makes sure original projectile doesn't spawn
            return false;
        }
    }

	public class GunVisuals : ModProjectile {
		//Dusts to spawn
		private int[] dusts = { 60, 6, 64, 61, 59, 62 };

        public override string Texture => "StarWarriors/Items/Gun";

        public override void SetDefaults() {
			Projectile.width = 74;
			Projectile.height = 78;
			Projectile.scale = 1f;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 999999;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Rainbow Devastation Rod");
		}

		public override void AI() {
			//Settings for updating on net
			Vector2 vector22 = Main.player[Projectile.owner].RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter, true);
			if (Main.myPlayer == Projectile.owner) {
				if (Main.player[Projectile.owner].channel) {
					float num263 = Main.player[Projectile.owner].inventory[Main.player[Projectile.owner].selectedItem].shootSpeed * Projectile.scale;
					Vector2 vector23 = vector22;
					float num264 = (float)Main.mouseX + Main.screenPosition.X - vector23.X;
					float num265 = (float)Main.mouseY + Main.screenPosition.Y - vector23.Y;
					if (Main.player[Projectile.owner].gravDir == -1f) {
						num265 = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector23.Y;
					}
					float num266 = (float)Math.Sqrt((double)(num264 * num264 + num265 * num265));
					num266 = (float)Math.Sqrt((double)(num264 * num264 + num265 * num265));
					num266 = num263 / num266;
					num264 *= num266;
					num265 *= num266;
					if (num264 != Projectile.velocity.X || num265 != Projectile.velocity.Y) {
						Projectile.netUpdate = true;
					}
					Projectile.velocity.X = num264;
					Projectile.velocity.Y = num265;
				}
				else {
					Projectile.Kill();
				}
			}

			//Setting the position and direction
			if (Projectile.velocity.X > 0f) {
				Main.player[Projectile.owner].ChangeDir(1);
			}
			else {
				if (Projectile.velocity.X < 0f) {
					Main.player[Projectile.owner].ChangeDir(-1);
				}
			}
			Projectile.spriteDirection = Projectile.direction;
			Main.player[Projectile.owner].ChangeDir(Projectile.direction);
			Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
			Projectile.position.X = vector22.X - (float)(Projectile.width / 2);
			Projectile.position.Y = vector22.Y - (float)(Projectile.height / 2);
			// Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 2.355f;

			//Rotation of Projectile and Item Use
			if (Projectile.spriteDirection == -1) {
				Projectile.rotation -= 1.57f;
			}
			/*
			if (Main.player[Projectile.owner].direction == 1) {
				Main.player[Projectile.owner].itemRotation = 
					(float)Math.Atan2(
						(double)(Projectile.velocity.Y * (float)Projectile.direction), 
						(double)(Projectile.velocity.X * (float)Projectile.direction)
					);
			}
			else {
				Main.player[Projectile.owner].itemRotation = 
					(float)Math.Atan2(
						(double)(Projectile.velocity.Y * (float)Projectile.direction), 
						(double)(Projectile.velocity.X * (float)Projectile.direction)
					);
			}*/

			//Make velocity really low towards mouse
			Projectile.velocity.X = Projectile.velocity.X * (1f + (float)Main.rand.Next(-3, 4) * 0.01f);

			//Animation and firing in terms of frameCounter and first counter
			if (Projectile.frameCounter % (int)Projectile.ai[0] == 0) {
				//Animation
				if (Projectile.frame < 6) {
					Projectile.frame += 1;
				}
				else {
					Projectile.frame = 0;
				}

				//Firing towards the mouse
				Vector2 vector = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
				float MouseX = (float)Main.mouseX + Main.screenPosition.X - vector.X;
				float MouseY = (float)Main.mouseY + Main.screenPosition.Y - vector.Y;
				float MouseDistance = (float)Math.Sqrt((double)(MouseX * MouseX + MouseY * MouseY));
				float shootSpeed = 4f;
				MouseDistance = shootSpeed / MouseDistance;
				MouseX *= MouseDistance;
				MouseY *= MouseDistance;
				Projectile.NewProjectile(
					new EntitySource_ItemUse_WithAmmo(
						Main.player[Projectile.owner],
						Main.item[ModContent.ItemType<Gun>()],
						ProjectileID.Bullet
					),
					Projectile.position.X + Projectile.width / 2,
					Projectile.position.Y + Projectile.height / 2, 
					MouseX + (float)Main.rand.Next(-2, 0), 
					MouseY + (float)Main.rand.Next(-2, 2), 
					ProjectileID.Bullet, 
					(int)(180f * 1), 
					Projectile.knockBack, 
					Projectile.owner, 0f, 0f
				);
				//Main.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 33);
			}
			Projectile.frameCounter++;

			//Increases rate of animation/fire rate
			if (Projectile.ai[1] > 60f) {
				if (Projectile.ai[0] > 3f) {
					//Decreasing first counter; First counter is set in the Item
					Projectile.ai[0] -= 3f;
				}
				Projectile.ai[1] = 0;
			}

			//Update secondary counter
			Projectile.ai[1] += 1f;

			//Reset how long the projectile lives
			Main.player[Projectile.owner].itemTime = 10;
			Main.player[Projectile.owner].itemAnimation = 10;
			Projectile.timeLeft = 999999;
		}
	}
}
