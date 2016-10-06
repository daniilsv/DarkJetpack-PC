using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack {
    class Bullet : Enemy {
        public Bullet(GameLayout game, Vector2 vel) : base(game) {
            speed = vel * 20;
            position = Layout.player.Position;
        }

        public override void onLoad() {
            texture = game.Terrain;
            rectTex = new Rectangle(246, 321, 120, 120);
            sizeDraw = new Vector2(32, 32);
        }

        public override void intersects(Enemy enemy) {
            if (enemy is LifePoint || enemy is Bullet)
                return;
            type = 0;
            enemy.type = 0;
        }

        public override void update(GameTime gameTime) {
            direction = new Vector2(1, 1);
        }
        public override void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, null, rectDraw, rectTex, null, 0, null, Color.White);
        }

    }
}
