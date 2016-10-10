using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack {
    class SpeedBoost : Enemy {
        public SpeedBoost(GameLayout game, Vector2 pos) : base(game) {
            speed = new Vector2(0, 0);
            position = pos;
        }

        public override void onLoad() {
            texture = game.Terrain;
            rectTex = new Rectangle(1405, 321, 52, 52);
            sizeDraw = new Vector2(110, 110);
        }

        public override void intersects(Player player) {
            player.nitro[player.skinNum] = 520+(52*player.skinNum);
            type = 0;
        }
        public override void intersects(Enemy enemy) {
        }

        public override void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, null, rectDraw, rectTex, null, 0, null, Color.White);
        }

    }
}