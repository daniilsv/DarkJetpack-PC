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
            speed = new Vector2(1, 1);
            sizeDraw = new Vector2(52, 52);
            position = pos;
            num = (int)(r.NextDouble() * 10);
            rotateSpeed = 150 + r.NextDouble() * 150;
        }

        public override void onLoad() {
            texture = game.Terrain;
        }

        public override void onUnLoad() {

        }
        public override void update(GameTime gameTime) {
            if (gameTime.TotalGameTime.TotalMilliseconds - lastTime > rotateSpeed) {
                num++;
                rectTex = new Rectangle(626 + num % 9 * 52, 322, 52, 51);
                lastTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            direction = new Vector2(0, -1);
        }
        public override void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, positionDraw, rectTex, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
        }

    }
}
