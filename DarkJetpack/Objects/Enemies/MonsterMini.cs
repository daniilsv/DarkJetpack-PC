using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack {
    class MonsterMini : Enemy {
        double lastTime = 0;
        int num = 0, num2 = 0;
        public MonsterMini(GameLayout game, Vector2 pos) : base(game) {
            speed = new Vector2(1f + 1.5f * (float)r.NextDouble(), 1f + 1.5f * (float)r.NextDouble());
            position = pos;
            num = (int)(r.NextDouble() * 3);
            num2 = (int)(r.NextDouble() * 2);
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
            if (enemy is MonsterMini || enemy is MonsterBig || enemy is BigBoss) return;
            type = 0;
            enemy.type = 0;
        }

        public override void update(GameTime gameTime) {
            if (gameTime.TotalGameTime.TotalMilliseconds - lastTime > 1250) {
                num2++;
                rectTex = new Rectangle(1093 + num * 104 + num2 % 2 * 52, 321, 52, 52);
                lastTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            direction = Layout.player.Position - position;
            direction.Normalize();
        }
        public override void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, null, rectDraw, rectTex, null, 0, null, Color.White);
        }

    }
}
