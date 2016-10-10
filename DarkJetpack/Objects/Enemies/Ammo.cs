using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack {
    class Ammo : Enemy {
        public Ammo(GameLayout game, Vector2 pos) : base(game) {
            speed = new Vector2(0, 0);
            position = pos;
        }

        public override void onLoad() {
            texture = game.Terrain;
            rectTex = new Rectangle(1458, 321, 69, 32);
            sizeDraw = new Vector2(110, 52);
        }

        public override void intersects(Player player) {
            player.bullets[player.skinNum] = 25 + (6 * player.skinNum);
            type = 0;
        }
        public override void intersects(Enemy enemy) {
        }

        public override void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, null, rectDraw, rectTex, null, 0, null, Color.White);
        }

    }
}