using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkJetpack {
    class BigBoss : Enemy {
        int life = 7;
        double lastTime = 0;
        public BigBoss(GameLayout game, Vector2 pos) : base(game) {
            speed = new Vector2(10, 0);
            position = pos;
            rectTex = new Rectangle(0, 443, 300, 170);
        }

        public override void onLoad() {
            texture = game.Terrain;
            sizeDraw = new Vector2(600, 350);
        }

        public override void intersects(Player player) {
            player.life--;
            life--;
        }
        public override void intersects(Enemy enemy) {
            if (!(enemy is MonsterMini))
                enemy.type = 0;
            if (enemy is Bullet) {
                life--;
            }
            if (life == 0)
                type = 0;
        }
        public override void update(GameTime gameTime) {
            position.X = Layout.player.Position.X + 2 * (float)Math.Cos(gameTime.TotalGameTime.TotalMilliseconds / 700) + 3;
            position.Y += 0.01f * (float)Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 200);

            if (gameTime.TotalGameTime.TotalMilliseconds - lastTime > 4500) {
                game.postEnemiesAdd.Add(new MonsterMini(game,
                    new Vector2(position.X - 2 * (float)r.NextDouble(), position.Y + 2 * (float)r.NextDouble())));
                lastTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }

        public override void draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(DarkJetpack.baseFont, life + "", new Vector2(rectDraw.Center.X, rectDraw.Bottom + 50),
                Color.Red, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
            spriteBatch.Draw(texture, null, rectDraw, rectTex, null, 0, null, Color.White);
        }

    }
}
