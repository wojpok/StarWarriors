using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Terraria.Graphics.CameraModifiers;
using Terraria.Audio;

namespace ModName.Items {
	public class Comet : ModProjectile {
		public override void SetDefaults() {
			Projectile.width = 1;
			Projectile.height = 1;
			Projectile.scale = 2;
			
			Projectile.friendly = true;
			Projectile.DamageType = StarDefenderClass.StarDamage;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 6000;

			DrawOriginOffsetX = 0; // -200;
			DrawOriginOffsetY = -130; // -200;
		}

        public override void SetStaticDefaults() {
            base.SetStaticDefaults();

			Main.projFrames[Projectile.type] = 4;
        }

        public override void AI() {

			//This will cycle through all of the frames in the sprite sheet
			const int frameSpeed = 4; //How fast you want it to animate
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= frameSpeed) {
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame >= Main.projFrames[Projectile.type]) {
					Projectile.frame = 0;
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			Projectile.ai[0] = Projectile.position.X;
			Projectile.ai[1] = Projectile.position.Y;

			Projectile.alpha = 255;
			Projectile.knockBack = 200f;

			Projectile.timeLeft = 3;

			return false;

			/*
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0) {
				Projectile.Kill();
			}
			else {
				Projectile.ai[0] += 0.1f;
				if (Projectile.velocity.X != oldVelocity.X) {
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y) {
					Projectile.velocity.Y = -oldVelocity.Y;
				}
				Projectile.velocity *= 0.75f;
				// SoundEngine.PlaySound(SoundID.Item10, projectile.position);
			}
			return false;
			*/
		}

		public override void Kill(int timeLeft) {
			// Main.NewText("Died");
			// return;

			/*for (int k = 0; k < 5; k++) {
				Dust.NewDust(
					Projectile.position + Projectile.velocity,
					Projectile.width,
					Projectile.height,
					DustID.MinecartSpark,
					Projectile.oldVelocity.X * 0.5f,
					Projectile.oldVelocity.Y * 0.5f
				);
			}*/
			//SoundEngine.PlaySound(SoundID.Item25, projectile.position);

			Projectile.Resize(10, 10);

			const float blastRadius = 15f;

			var inBounds = (int x, int y) => {
				return x >= 0 && y >= 0 && x < Main.maxTilesX && y < Main.maxTilesY;
			};

			int minTileX = (int)(Projectile.position.X / 16f - (float)blastRadius);
			int maxTileX = (int)(Projectile.position.X / 16f + (float)blastRadius);
			int minTileY = (int)(Projectile.position.Y / 16f - (float)blastRadius);
			int maxTileY = (int)(Projectile.position.Y / 16f + (float)blastRadius);
			if (minTileX < 0) {
				minTileX = 0;
			}
			if (maxTileX > Main.maxTilesX) {
				maxTileX = Main.maxTilesX;
			}
			if (minTileY < 0) {
				minTileY = 0;
			}
			if (maxTileY > Main.maxTilesY) {
				maxTileY = Main.maxTilesY;
			}

			/*for (int i = minTileX; i <= maxTileX; i++) {
				for (int j = minTileY; j <= maxTileY; j++) {
					float diffX = Math.Abs((float)i - Projectile.position.X / 16f);
					float diffY = Math.Abs((float)j - Projectile.position.Y / 16f);

					double distanceToTile = Math.Sqrt((double)(diffX * diffX + diffY * diffY));
					if (distanceToTile < (double)blastRadius) {
						if(Main.tile[i, j] != null) {
							WorldGen.KillTile(i, j, false, false, false);
							if (Main.netMode != NetmodeID.SinglePlayer) {
								NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
							}
						}

						
					}
				}
			}*/

			// Projectile.ExplodeTiles(V.V2(Projectile.ai[0], Projectile.ai[1]), (int) blastRadius, minTileX, maxTileX, minTileY, maxTileY, true);

			// Main.NewText($"{V.V2(Projectile.ai[0], Projectile.ai[1])}");
			//Main.NewText($"{ModSound.Pipe.Volume}");
			//ModSound.Pipe.Volume = 10f;
			//Main.NewText($"{ModSound.Pipe.Volume}");

			SoundEngine.PlaySound(ModSound.Pipe, V.V2(Projectile.ai[0], Projectile.ai[1]));
			

			Vector2 origin = V.V2(Projectile.ai[0], Projectile.ai[1]);
			foreach (var npc in Main.npc) {
				if (npc == null) {
					continue;
                }

				ApplyVelocity(npc, origin);
				npc.life -= 100;
				npc.HitEffect();
				npc.checkDead();
			}

			foreach(var player in Main.player) {
				if (player == null)
					continue;

				ApplyVelocity(player, origin, 20f);

				//player.is
            }

			//PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 20f, 6f, 20, 1000f, FullName);
			//Main.instance.CameraModifiers.Add(modifier);
		}

		private void ApplyVelocity(Entity ent, Vector2 origin, float power = 30f) {
			var diff = (origin - ent.position);
			float dist = diff.Length();

			if (dist < 300f) {
				//float power = -20f;

				diff.Normalize();

				ent.velocity += diff * power;
			}
			
        }
		/*
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
			Main.NewText("NPC hit");
			//ApplyVelocity(target);
		}

        public override void OnHitPlayer(Player target, int damage, bool crit) {
			Main.NewText("Player hit");
			//ApplyVelocity(target);
		}*/
    }
}