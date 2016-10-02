using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack {
    class LifePoint : Enemy {
        public LifePoint(GameLayout game, Vector2 pos) : base(game) {
            speed = new Vector2(0, 0);
            position = pos;
        }

        public override void onLoad() {
            texture = game.Terrain;
            rectTex = new Rectangle(625, 376, 52, 46);
            sizeDraw = new Vector2(52, 52);
        }

        public override void intersects(Player player) {
            player.life++;
            type = 0;
        }
        public override void intersects(Enemy enemy) {
            type = 0;
            enemy.type = 0;
        }

        public override void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, positionDraw, rectTex, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
        }

    }
}
