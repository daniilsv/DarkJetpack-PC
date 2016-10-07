using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack {
    class MonsterMini : Enemy {
        double lastTime = 0, lastTime1 = 0;
        int num = 0, num2 = 0, num3 = -1;
        Rectangle rectTex2;
        public MonsterMini(GameLayout game, Vector2 pos) : base(game) {
            speed = new Vector2(1f + 1.5f * (float)r.NextDouble(), 1f + 1.5f * (float)r.NextDouble());
            position = pos;
            num = (int)(r.NextDouble() * 3);
            num2 = (int)(r.NextDouble() * 2);
        }

        public override void onLoad() {
            explosion = game.Explosion1;
            texture = game.Terrain;
            sizeDraw = new Vector2(110, 110);
        }

        public override void intersects(Player player) {
            if (num3 == -1) {
                num3 = 0;
                player.life--;
            }
        }
        public override void intersects(Enemy enemy) {
            if (enemy is MonsterMini || enemy is MonsterBig || enemy is BigBoss) return;
            if (enemy is Bullet) {
                enemy.type = 0;
            }
            if (num3 == -1) {
                num3 = 0;
            }
        }

        public override void update(GameTime gameTime) {
            if (num3 == -1) {
                if (gameTime.TotalGameTime.TotalMilliseconds - lastTime > 1250) {
                    num2++;
                    rectTex = new Rectangle(1093 + num * 104 + num2 % 2 * 52, 321, 52, 52);
                    lastTime = gameTime.TotalGameTime.TotalMilliseconds;
                }
            } else if (num3 >= 9) {
                type = 0;
            } else {
                if (gameTime.TotalGameTime.TotalMilliseconds - lastTime1 > 50) {
                    rectTex2 = new Rectangle(92 * num3, 0, 92, 51);
                    num3++;
                    lastTime1 = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            direction = Layout.player.Position - position;
            direction.Normalize();
        }
        public override void draw(SpriteBatch spriteBatch) {
            if (num3 == -1) {
                spriteBatch.Draw(texture, null, rectDraw, rectTex, null, 0, null, Color.White);
            } else if (num3 >= 0 && num3 < 9) {
                spriteBatch.Draw(explosion, null, rectDraw, rectTex2, null, 0, null, Color.White);
            } else {
                spriteBatch.Draw(explosion, null, rectDraw, rectTex2, null, 0, null, Color.White * 0.0f);
            }
        }

    }
}
