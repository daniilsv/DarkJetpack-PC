using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack {
    class MonsterBig : Enemy {
        double lastTime = 0;
        int num = -1;
        Rectangle rectTex2;
        public MonsterBig(GameLayout game, Vector2 pos) : base(game) {
            speed = new Vector2(1f + 1.5f * (float)r.NextDouble(), 1f + 1.5f * (float)r.NextDouble());
            position = pos;
            rectTex = new Rectangle(781 + (int)(r.NextDouble() * 6) % 2 * 104, 376, 104, 67);
        }

        public override void onLoad() {
            explosion = game.Explosion;
            texture = game.Terrain;
            sizeDraw = new Vector2(250, 125);
        }

        public override void intersects(Player player) {
            if (num == -1) {
                num = 0;
                player.life--;
            }
        }
        public override void intersects(Enemy enemy) {
            if (enemy is MonsterMini || enemy is MonsterBig || enemy is BigBoss) return;
            if (enemy is Bullet) {
                enemy.type = 0;
            }
            if (num == -1) {
                num = 0;
            }
        }

        public override void update(GameTime gameTime) {
            direction = Layout.player.Position - position;
            if (num >= 10) {
                type = 0;
            } else if (gameTime.TotalGameTime.TotalMilliseconds - lastTime > 50 && num >= 0) {
                rectTex2 = new Rectangle(92 * num, 0, 92, 52);
                num++;
                lastTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            direction.Normalize();
        }
        public override void draw(SpriteBatch spriteBatch) {
            if (num == -1) {
                spriteBatch.Draw(texture, null, rectDraw, rectTex, null, 0, null, Color.White);
            } else if (num >= 0 && num < 10) {
                spriteBatch.Draw(explosion, null, rectDraw, rectTex2, null, 0, null, Color.White);
            } else {
                spriteBatch.Draw(explosion, null, rectDraw, rectTex2, null, 0, null, Color.White * 0.0f);
            }
        }

    }
}
