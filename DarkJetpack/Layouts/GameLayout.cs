using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack {
    public class GameLayout : Layout {
        double lastTime = 0, lastTime2 = 0, lastTime3 = 0, lastTime4 = 0, lastTime5 = 0, lastTime6 = 0, lastTime7 = 0;
        List<Background> Backgrounds;
        public Texture2D Terrain;
        public Texture2D Explosion;
        public Texture2D Explosion1;
        Cities cities;
        List<Color> colors = new List<Color> { new Color(76, 220, 241), Color.DarkBlue, Color.Gainsboro, Color.CadetBlue, Color.Black };

        List<List<Texture2D>> backTexs;

        List<Enemy> enemies;
        public List<Enemy> postEnemiesAdd;

        int playerSkinNum;
        int bossLevel = 1;
        Window testw;
        bool isPause = false;
        public GameLayout(DarkJetpack game, int playerSkinN) : base(game) {
            playerSkinNum = playerSkinN;
            Terrain = game.Terrain;
            Explosion = game.Explosion;
            Explosion1 = game.Explosion1;
        }

        public override void onLoad() {
            backTexs = new List<List<Texture2D>> {
                new List<Texture2D> { game.Content.Load<Texture2D>(@"Clouds1"), game.Content.Load<Texture2D>(@"Stars1") },
                new List<Texture2D> { game.Content.Load<Texture2D>(@"Clouds2"), game.Content.Load<Texture2D>(@"Stars2") },
                new List<Texture2D> { game.Content.Load<Texture2D>(@"Clouds3"), game.Content.Load<Texture2D>(@"Stars3") }
            };
            Backgrounds = new List<Background>();
            Backgrounds.Add(new Background(new Vector2(300, 300), 0.6f));
            Backgrounds.Add(new Background(new Vector2(400, 400), 0.8f));
            Backgrounds.Add(new Background(new Vector2(700, 700), 1.1f));
            cities = new Cities(game.Terrain);
            enemies = new List<Enemy>();
            postEnemiesAdd = new List<Enemy>();
            game.changeSong(8, true);
            testw = new Window(new Rectangle(windowBounds.X / 6, windowBounds.Y / 6, windowBounds.X - windowBounds.X / 3, windowBounds.Y - windowBounds.Y / 3), game.Terrain);
            player.start();
        }

        public override void onUnLoad() {

        }

        public override void update(GameTime gameTime) {
            if (isButtonPressed(Keys.Escape))
                isPause = !isPause;

            if (isPause) {

                return;
            }

            #region Player update

            Vector2 direction = Vector2.Zero;
            if (kbState.IsKeyDown(Keys.W) || kbState.IsKeyDown(Keys.Up))
                direction = new Vector2(0, -1);
            else if ((kbState.IsKeyDown(Keys.S) || kbState.IsKeyDown(Keys.Down)) && player.Position.Y >= 0) {
                direction = new Vector2(0, 1);
            }

            if (kbState.IsKeyDown(Keys.A) || kbState.IsKeyDown(Keys.Left))
                direction += new Vector2(-2, 0);
            else if (kbState.IsKeyDown(Keys.D) || kbState.IsKeyDown(Keys.Right))
                direction += new Vector2(2, 0);

            if (kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift)) {
                if (player.nitro > 0) {
                    direction *= 2;
                    player.nitro -= 1;
                }
            }

            player.Update(gameTime, direction, viewport);

            if (msState.LeftButton == ButtonState.Pressed && gameTime.TotalGameTime.TotalMilliseconds - lastTime4 > 500 && player.bullets > 0) {
                Point p = msState.Position;
                Vector2 v = new Vector2(p.X - windowBounds.X / 2, windowBounds.Y / 2 - p.Y) + 5 * direction;
                v.Normalize();
                enemies.Add(new Bullet(this, v));
                player.bullets -= 1;
                lastTime4 = gameTime.TotalGameTime.TotalMilliseconds;
            }

            if (player.life <= 0) {
                game.changeLayoutBack();
                game.changeLayoutTo(new GameOverLayout(game));
            }

            #endregion

            #region Background

            #region Color
            if (player.Position.Y <= 6)
                ChangeColor(1, player.Position.Y / 6);
            else if (player.Position.Y <= 12)
                ChangeColor(2, player.Position.Y / 6 - 1);
            else if (player.Position.Y <= 18)
                ChangeColor(3, player.Position.Y / 6 - 2);
            else if (player.Position.Y <= 24)
                ChangeColor(4, player.Position.Y / 6 - 3);
            #endregion

            #region Image
            for (int i = 0; i < Backgrounds.Count; i++) {
                Background bg = Backgrounds[i];
                if (player.Position.Y <= 15) {
                    bg.setTexture(backTexs[i][0]);
                    bg.Update(gameTime, direction, viewport, (1 - player.Position.Y / 15));
                } else {
                    bg.setTexture(backTexs[i][1]);
                    bg.Update(gameTime, direction, viewport, (player.Position.Y / 15 - 1));
                }
            }
            #endregion

            if (player.Position.Y < 15) {
                cities.Update(gameTime, direction, viewport);
                cities.Offset.Y = player.Position.Y * 50;
            }

            #endregion

            #region Enemies

            foreach (Enemy enm in enemies)
                enm.onUpdate(gameTime);
            #region Intersection
            for (int i = 0; i < enemies.Count - 1; i++) {
                Enemy enm1 = enemies[i];
                if (enm1.rectDraw.Top > viewport.Height * 5 && i >= 0) {
                    enemies.RemoveAt(i);
                    i--; continue;
                }
                if (enm1.rectCollide.Intersects(player.RectangleCollide)) {
                    if (enm1 is LifePoint && player.life >= 5)
                        continue;
                    enm1.intersects(player);
                    if (enm1.type == 0 && i >= 0) {
                        enemies.RemoveAt(i);
                        i--;
                    }
                    continue;
                }
                if (enm1 == null) continue;
                for (int j = i + 1; j < enemies.Count; j++) {
                    Enemy enm2 = enemies[j];
                    if (enm2 == null) continue;
                    if (enm1.rectCollide.Intersects(enm2.rectCollide)) {
                        enm1.intersects(enm2);
                        if (enm2.type == 0 && j >= 0) {
                            enemies.RemoveAt(j);
                            j--;
                        }
                        if (enm1.type == 0 && i >= 0) {
                            enemies.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
            #endregion

            #region Adding
            Random r = new Random();

            if (gameTime.TotalGameTime.TotalMilliseconds - lastTime2 > 50000) {
                if (player.life < 5)
                    enemies.Add(new LifePoint(this, new Vector2(player.Position.X - windowBounds.X / 120 + (float)(windowBounds.X / 60 * r.NextDouble()), player.Position.Y + windowBounds.Y / 100)));
                lastTime2 = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (gameTime.TotalGameTime.TotalMilliseconds - lastTime6 > 35000) {
                enemies.Add(new SpeedBoost(this, new Vector2(player.Position.X - windowBounds.X / 120 + (float)(windowBounds.X / 60 * r.NextDouble()), player.Position.Y + windowBounds.Y / 100)));
                lastTime6 = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (gameTime.TotalGameTime.TotalMilliseconds - lastTime7 > 10000) {
                enemies.Add(new Ammo(this, new Vector2(player.Position.X - windowBounds.X / 120 + (float)(windowBounds.X / 60 * r.NextDouble()), player.Position.Y + windowBounds.Y / 100)));
                lastTime7 = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (gameTime.TotalGameTime.TotalMilliseconds - lastTime > 750 + 2000 * r.NextDouble()) {
                for (int i = 0; i < 1 + 3 * r.NextDouble(); i++)
                    enemies.Add(new Asteroid(this, new Vector2(player.Position.X - windowBounds.X / 120 + (float)(windowBounds.X / 60 * r.NextDouble()), player.Position.Y + windowBounds.Y / 100)));
                lastTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (player.score > 400)
                if (gameTime.TotalGameTime.TotalMilliseconds - lastTime3 > 5000 - player.Position.Y * 5 + 10000 * r.NextDouble()) {
                    int num = 1 + ((int)player.Position.Y + (int)(r.NextDouble() * 200)) / 100;
                    for (int i = 0; i < num; i++)
                        enemies.Add(new MonsterMini(this, new Vector2(player.Position.X - windowBounds.X / 120 + (float)(windowBounds.X / 60 * r.NextDouble()), player.Position.Y + windowBounds.Y / 100)));
                    lastTime3 = gameTime.TotalGameTime.TotalMilliseconds;
                }
            if (player.score > 900)
                if (gameTime.TotalGameTime.TotalMilliseconds - lastTime5 > 12500 - player.Position.Y * 5 + 3500 * r.NextDouble()) {
                    int num = 1 + ((int)player.Position.Y + (int)(r.NextDouble() * 400)) / 200;
                    for (int i = 0; i < num; i++)
                        enemies.Add(new MonsterBig(this, new Vector2(player.Position.X - windowBounds.X / 120 + (float)(windowBounds.X / 60 * r.NextDouble()), player.Position.Y + windowBounds.Y / 100)));
                    lastTime5 = gameTime.TotalGameTime.TotalMilliseconds;
                }
            enemies.AddRange(postEnemiesAdd);
            postEnemiesAdd = new List<Enemy>();
            if (player.score >= bossLevel * 1100) {
                bossLevel++;
                BigBoss boss = new BigBoss(this, new Vector2(player.Position.X, player.Position.Y + windowBounds.Y / 100));
                boss.life = 6 + bossLevel;
                enemies.Add(boss);
            }
            #endregion

            #endregion

        }

        public void ChangeColor(int endColor, float pc) {
            game.backColor = Color.Lerp(colors[endColor - 1], colors[endColor], pc);
        }

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime) {
            if (player.Position.Y < 25)
                cities.Draw(spriteBatch);
            foreach (Background bg in Backgrounds)
                bg.Draw(spriteBatch);

            foreach (Enemy enm in enemies)
                enm.Draw(spriteBatch);

            player.Draw(spriteBatch);
            for (int i = 0; i < player.life; i++)
                spriteBatch.Draw(Terrain, new Vector2(50 + 50 * i, 50), null, new Rectangle(625, 376, 51, 46),
                    Vector2.Zero, 0, new Vector2(0.8f));
            spriteBatch.Draw(Terrain, new Vector2(90, 132), null, new Rectangle(1458, 321, (int)(player.bullets * 28f) / 10, 32),
                    Vector2.Zero, 0);
            spriteBatch.DrawString(DarkJetpack.baseFont, "Ammo: " + player.bullets, new Vector2(75, 192), Color.White * ((player.bullets * 0.05f)));
            spriteBatch.Draw(Terrain, new Vector2(90, 252), null, new Rectangle(1405, 321, player.nitro / 10, 52),
                    Vector2.Zero, 0);
            spriteBatch.DrawString(DarkJetpack.baseFont, "Press SHIFT", new Vector2(65, 322), Color.White * ((player.nitro / 5.2f) * 0.01f));
            #region Score
            if (player.score < player.highscore) {
                spriteBatch.DrawString(DarkJetpack.baseFont, player.score + "", new Vector2(windowBounds.X / 2 - 30, 50),
                    Color.Red, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 1);
                spriteBatch.DrawString(DarkJetpack.baseFont, "HS: " + player.highscore, new Vector2(2 * windowBounds.X / 3, 50),
                    Color.OrangeRed, 0, Vector2.Zero, 1.2f, SpriteEffects.None, 1);
            } else
                spriteBatch.DrawString(DarkJetpack.baseFont, player.score + "", new Vector2(windowBounds.X / 2 - 20, 50),
                    Color.Red, 0, Vector2.Zero, 2, SpriteEffects.None, 1);

            #endregion

            #region Pause
            if (isPause) {
                testw.onDraw(spriteBatch, gameTime);
                foreach (Button b in buttons)
                    b.onDraw(spriteBatch, gameTime);
            }
            #endregion

            #region Debug
            if (DarkJetpack.isDebug) {
                spriteBatch.DrawString(DarkJetpack.baseFont, player.Position + "", new Vector2(10, 10), Color.Red, 0,
                    Vector2.Zero, 1, SpriteEffects.None, 1);

            }
            #endregion
        }
    }
}
