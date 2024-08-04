using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using StarWarriors.Common.Classes;
using StarWarriors.Common.Buffs;

namespace StarWarriors.Content.Projectiles
{
    internal class FeatherDaggerProjectile : ModProjectile
    {
        public bool IsStickingToTarget
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }

        public int TargetWhoAmI
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public int GravityDelayTimer
        {
            get => (int)Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

        public bool IsPulling
        {
            get => Projectile.ai[0] == 2f;
        }

        public float PullingTimer
        {
            get => Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

        public float StickTimer
        {
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }

        public float PullingScaleBias
        {
            get => Projectile.ai[1];
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DontAttachHideToAlpha[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = ModContent.GetInstance<AirborneDamageClass>();
            Projectile.penetrate = 2;
            Projectile.timeLeft = 600; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.alpha = 0;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.hide = false;
        }

        private const int GravityDelay = 45;

        public override void AI()
        {
            if (IsPulling)
            {
                PullingAI();
            }
            else if (IsStickingToTarget)
            {
                StickyAI();
            }
            else
            {
                NormalAI();
            }
        }

        private const float TextureRotation = 255f;

        private void NormalAI()
        {
            GravityDelayTimer++; // doesn't make sense.

            // For a little while, the javelin will travel with the same speed, but after this, the javelin drops velocity very quickly.
            if (GravityDelayTimer >= GravityDelay)
            {
                GravityDelayTimer = GravityDelay;

                // wind resistance
                Projectile.velocity.X *= 0.98f;
                // gravity
                Projectile.velocity.Y += 0.35f;
            }

            // Offset the rotation by 90 degrees because the sprite is oriented vertically.
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(TextureRotation);

            // Spawn some random dusts as the javelin travels
            //if (Main.rand.NextBool(3)) {
            //    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, ModContent.DustType<Sparkle>(), Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 200, Scale: 1.2f);
            //    dust.velocity += Projectile.velocity * 0.3f;
            //    dust.velocity *= 0.2f;
            //}
            //if (Main.rand.NextBool(4)) {
            //    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, ModContent.DustType<Sparkle>(),
            //        0, 0, 254, Scale: 0.3f);
            //    dust.velocity += Projectile.velocity * 0.5f;
            //    dust.velocity *= 0.5f;
            //}
        }

        private const int StickTime = 60 * 15; // 15 seconds
        private void StickyAI()
        {
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            StickTimer += 1f;

            // Every 30 ticks, the javelin will perform a hit effect
            bool hitEffect = StickTimer % 30f == 0f;
            int npcTarget = TargetWhoAmI;
            if (StickTimer >= StickTime || npcTarget < 0 || npcTarget >= 200)
            { // If the index is past its limits, kill it
                Projectile.Kill();
            }
            else if (Main.npc[npcTarget].active && !Main.npc[npcTarget].dontTakeDamage)
            {
                //Projectile.Center = Main.npc[npcTarget].Center - Projectile.velocity.RotatedBy(Main.npc[npcTarget].rotation - RotationOnHit, Main.npc[npcTarget].Center);
                var npc = Main.npc[npcTarget];

                Projectile.Center = npc.Center - Projectile.velocity * 1.25f;
                Projectile.gfxOffY = Main.npc[npcTarget].gfxOffY;
                //if (hitEffect) {
                // Perform a hit effect here, causing the npc to react as if hit.
                // Note that this does NOT damage the NPC, the damage is done through the debuff.
                //Main.npc[npcTarget].HitEffect(0, 1.0);
                //}
            }
            else
            { // Otherwise, kill the projectile
                Projectile.Kill();
            }
        }

        public static float PullingTimerReset = 60f;
        public static float PullingStrength = 0.25f;

        private void PullingAI()
        {
            //Func<float, float> Gradient = (x) => (float) Math.Pow(-Math.E, -6.9f * x) + 1f;
            Func<float, float> Gradient = (x) => 0.04f / (float)(x - -0.038f) + -0.038f;


            if (PullingTimer == PullingTimerReset)
            {
                SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            }

            if (PullingTimerReset <= PullingTimer + 5)
            {
                // On pulling begin make ripping blood effect
                Vector2 usePos = Projectile.position;

                Vector2 rotationVector = (Projectile.rotation - MathHelper.ToRadians(TextureRotation)).ToRotationVector2();
                usePos += rotationVector * 16f;

                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(usePos, Projectile.width, Projectile.height, DustID.Blood);
                    dust.position = (dust.position + Projectile.Center) / 2f;
                    dust.velocity += rotationVector * 2f;
                    dust.velocity *= 0.5f;
                    dust.noGravity = true;
                    usePos -= rotationVector * 8f;
                }
            }

            PullingTimer -= 1f;

            if (PullingTimer <= 0)
            {
                Projectile.Kill();
                return;
            }

            Vector2 vel = new Vector2(0, PullingStrength * PullingScaleBias).RotatedBy(Projectile.rotation);

            // Projectile.velocity *= (float) Math.Log(1f + (PullingTimer / PullingTimerReset));
            Projectile.position -= vel * Gradient(1f - PullingTimer / PullingTimerReset);

            Projectile.alpha = (int)((1f - PullingTimer / PullingTimerReset) * 255);
        }

        public override void OnKill(int timeLeft)
        {


            // Make sure to only spawn items if you are the projectile owner.
            // This is an important check as Kill() is called on clients, and you only want the item to drop once
            if (Projectile.owner == Main.myPlayer)
            {
                // Drop a javelin item, 1 in 18 chance (~5.5% chance)
                int item = 0;
                if (Main.rand.NextBool(18))
                {
                    //item = Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.getRect(), ModContent.ItemType<ExampleJavelin>());
                }

                // Sync the drop for multiplayer
                // Note the usage of Terraria.ID.MessageID, please use this!
                if (Main.netMode == NetmodeID.MultiplayerClient && item >= 0)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f);
                }
            }
        }

        private const int MaxStickingFeathers = 5;
        private readonly Point[] stickingFeathers = new Point[MaxStickingFeathers + 1];

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            IsStickingToTarget = true;
            TargetWhoAmI = target.whoAmI;

            Projectile.rotation += (Main.rand.NextFloat() - 0.5f) / 2f;
            Projectile.velocity = target.Center - Projectile.Center;
            Projectile.netUpdate = true;
            Projectile.damage = 0;

            // Dumb use case, but seems valid I think
            // This method mutates Point array to find all instances of specified projectiles
            // and only if there is not enough space a projectile is evicted.
            // This means this function could be used just for searching given enough array space
            Projectile.KillOldestJavelin(-1, Type, target.whoAmI, stickingFeathers);

            int stuckFeathers = 0;

            foreach (Point feather in stickingFeathers)
            {
                if (feather.Y != 0)
                    stuckFeathers++;
            }

            // Main.NewText(stuckFeathers);

            if (stuckFeathers >= MaxStickingFeathers)
            {
                Vector2 EnemySize = Main.npc[TargetWhoAmI].Size;
                var Scale = EnemySize.X + EnemySize.Y;

                target.AddBuff(ModContent.BuffType<FeatherPullBleedDebuff>(), 5 * 60);

                foreach (Point feather in stickingFeathers)
                {
                    //Main.projectile[feather.X].Kill();
                    Main.projectile[feather.X].ai[0] = 2f;
                    Main.projectile[feather.X].ai[1] = Scale;
                    Main.projectile[feather.X].ai[2] = PullingTimerReset;

                    Main.projectile[feather.X].velocity.X = 0f;
                    Main.projectile[feather.X].velocity.Y = 0f;

                    Main.projectile[feather.X].netUpdate = true;
                }
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            // For going through platforms and such, javelins use a tad smaller size
            width = height = 10;
            return true;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // By shrinking target hitboxes by a small amount, this projectile only hits if it more directly hits the target.
            // This helps the javelin stick in a visually appealing place within the target sprite.
            if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
            {
                targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
            }
            // Return if the hitboxes intersects, which means the javelin collides or not
            return projHitbox.Intersects(targetHitbox);
        }
    }
}
