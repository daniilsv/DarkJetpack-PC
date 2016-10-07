using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkJetpack {
    class BigBoss : Enemy {
        public int life = 6;
        double lastTime = 0;
        int num = -1;
        Rectangle rectTex2;
        public BigBoss(GameLayout game, Vector2 pos) : base(game) {
            speed = new Vector2(10, 0);
            position = pos;
            rectTex = new Rectangle(0, 443, 300, 170);
        }

        public override void onLoad() {
            explosion = game.Explosion;
            texture = game.Terrain;
            sizeDraw = new Vector2(600, 350);
        }

        public override void intersects(Player player) {
            if (num == -1) {
                life--;
                player.life--;
            }
        }
        public override void intersects(Enemy enemy) {
            if ((enemy is LifePoint || enemy is SpeedBoost || enemy is Ammo && num == -1)) {
                enemy.type = 0;
            }
            if (enemy is Bullet && num == -1) {
                life--;
                enemy.type = 0;
            }
            if (life == 0)
                num = 0;
        }
        public override void update(GameTime gameTime) {
            position.X = Layout.player.Position.X + 2 * (float)Math.Cos(gameTime.TotalGameTime.TotalMilliseconds / 700) + 3;
            position.Y += 0.01f * (float)Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 200);
            if (num >= 11) {
                type = 0;
            } else if (gameTime.TotalGameTime.TotalMilliseconds - lastTime > 50 && num >= 0) {
                rectTex2 = new Rectangle(92 * num, 0, 92, 52);
                num++;
                lastTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (gameTime.TotalGameTime.TotalMilliseconds - lastTime > 4500) {
                game.postEnemiesAdd.Add(new MonsterMini(game,
                    new Vector2(position.X - 2 * (float)r.NextDouble(), position.Y + 2 * (float)r.NextDouble())));
                lastTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }

        public override void draw(SpriteBatch spriteBatch) {
            if (num == -1) {
                spriteBatch.DrawString(DarkJetpack.baseFont, life + "", new Vector2(rectDraw.Center.X, rectDraw.Bottom + 50),
                    Color.Red, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
                spriteBatch.Draw(texture, null, rectDraw, rectTex, null, 0, null, Color.White);
            } else if (num >= 0 && num < 11) {
                spriteBatch.Draw(explosion, null, rectDraw, rectTex2, null, 0, null, Color.White);
            } else {
                spriteBatch.Draw(explosion, null, rectDraw, rectTex2, null, 0, null, Color.White * 0.0f);
            }
        }

    }
}
