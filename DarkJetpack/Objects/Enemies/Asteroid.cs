using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack {
    class Asteroid : Enemy {
        double lastTime = 0;
        int num = 0;
        double rotateSpeed;
        public Asteroid(GameLayout game, Vector2 pos) : base(game) {
            speed = new Vector2(3 * (float)r.NextDouble(), 1.2f * (float)r.NextDouble());
            position = pos;
            num = (int)(r.NextDouble() * 10);
            rotateSpeed = 150 + r.NextDouble() * 150;
        }

        public override void onLoad() {
            texture = game.Terrain;
            sizeDraw = new Vector2(110, 110);
        }

        public override void intersects(Player player) {
            player.life--;
            type = 0;
        }
        public override void intersects(Enemy enemy) {
            if (enemy is Bullet) {
                enemy.type = 0;
            }
            type = 0;
        }

        public override void update(GameTime gameTime) {
            if (gameTime.TotalGameTime.TotalMilliseconds - lastTime > rotateSpeed) {
                num++;
                rectTex = new Rectangle(626 + num % 9 * 52, 322, 52, 51);
                lastTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            direction = new Vector2(-0.5f + (float)r.NextDouble(), -1);
        }
        public override void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, null, rectDraw, rectTex, null, 0, null, Color.White);
        }

    }
}
