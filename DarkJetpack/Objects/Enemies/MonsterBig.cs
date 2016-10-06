using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack {
    class MonsterBig : Enemy {
        public MonsterBig(GameLayout game, Vector2 pos) : base(game) {
            speed = new Vector2(1f + 1.5f * (float)r.NextDouble(), 1f + 1.5f * (float)r.NextDouble());
            position = pos;
            rectTex = new Rectangle(781 + (int)(r.NextDouble() * 6) % 2 * 104, 376, 104, 67);
        }

        public override void onLoad() {
            texture = game.Terrain;
            sizeDraw = new Vector2(250, 125);
        }

        public override void intersects(Player player) {
            player.life--;
            type = 0;
        }
        public override void intersects(Enemy enemy) {
            if (enemy is MonsterMini || enemy is MonsterBig || enemy is BigBoss) return;
            type = 0;
            enemy.type = 0;
        }

        public override void update(GameTime gameTime) {
            direction = Layout.player.Position - position;
            direction.Normalize();
        }
        public override void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, null, rectDraw, rectTex, null, 0, null, Color.White);
        }

    }
}
