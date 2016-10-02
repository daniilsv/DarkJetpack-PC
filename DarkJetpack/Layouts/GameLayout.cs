using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack {
    public class GameLayout : Layout {
        double lastTime = 0, lastTime2 = 0;
        List<Background> Backgrounds;
        public Player player;
        public Texture2D Terrain;
        int score;
        private SpriteFont scoreFont;
        Cities cities;
        List<Color> colors = new List<Color> { new Color(76, 220, 241), Color.DarkBlue, Color.Gainsboro, Color.CadetBlue, Color.Black };

        List<List<Texture2D>> backTexs;

        List<Enemy> enemies;
        int playerSkinNum;
        public GameLayout(DarkJetpack game, int playerSkinN) : base(game) {
            playerSkinNum = playerSkinN;
            Terrain = game.Terrain;
        }

        public override void onLoad()
        {
            scoreFont = game.Content.Load<SpriteFont>(@"ScoreFont");
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
            player = new Player(game, game.Terrain, playerSkinNum);
            enemies = new List<Enemy>();
            game.changeSong(7,true);
        }

        public override void onUnLoad() {

        }

        public override void update(GameTime gameTime) {
            if (isButtonPressed(Keys.Space)) {
                onBackPressed();
            }

            Vector2 direction = Vector2.Zero;
            if (kbState.IsKeyDown(Keys.Up))
                direction = new Vector2(0, -1);
            else if (kbState.IsKeyDown(Keys.Down) && player.Position.Y >= 0) {
                direction = new Vector2(0, 1);
            }

            if (kbState.IsKeyDown(Keys.Left))
                direction += new Vector2(-1, 0);
            else if (kbState.IsKeyDown(Keys.Right))
                direction += new Vector2(1, 0);

            if (kbState.IsKeyDown(Keys.LeftShift))
                direction *= 3;

            player.Update(gameTime, direction, viewport);
            score = Math.Max(score, (int)(player.Position.Y * 10));

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
                if (enm1.rectDraw.Top > viewport.Height * 2) {
                    enemies.RemoveAt(i);
                    i--; continue;
                }
                if (enm1.rectDraw.Intersects(player.Rectangle)) {
                    if (enm1 is LifePoint && player.life >= 7)
                        continue;
                    enm1.intersects(player);
                    if (enm1.type == 0) {
                        enemies.RemoveAt(i);
                        i--;
                    }
                    continue;
                }
                if (enm1 == null) continue;
                for (int j = i + 1; j < enemies.Count; j++) {
                    Enemy enm2 = enemies[j];
                    if (enm2 == null) continue;
                    if (enm1.rectDraw.Intersects(enm2.rectDraw)) {
                        enm1.intersects(enm2);
                        if (enm2.type == 0) {
                            enemies.RemoveAt(j);
                            j--;
                        }
                    }
                }
            }
            #endregion

            #region Adding
            Random r = new Random();
            if (gameTime.TotalGameTime.TotalMilliseconds - lastTime > 500 + 2000 * r.NextDouble()) {
                for (int i = 0; i < 3 * r.NextDouble(); i++)
                    enemies.Add(new Asteroid(this, new Vector2(player.Position.X - windowBounds.X / 120 + (float)(windowBounds.X / 60 * r.NextDouble()), player.Position.Y + windowBounds.Y / 100)));
                lastTime = gameTime.TotalGameTime.TotalMilliseconds;
            }

            if (gameTime.TotalGameTime.TotalMilliseconds - lastTime2 > 15000) {
                if (player.life < 7)
                    enemies.Add(new LifePoint(this, new Vector2(player.Position.X - windowBounds.X / 120 + (float)(windowBounds.X / 60 * r.NextDouble()), player.Position.Y + windowBounds.Y / 100)));
                lastTime2 = gameTime.TotalGameTime.TotalMilliseconds;
            }
            #endregion

            #endregion

            if (player.life <= 0) {
                game.changeLayoutBack();
                game.changeLayoutTo(new GameOverLayout(game, score));
            }
        }

        public void ChangeColor(int endColor, float pc) {
            game.backColor = Color.Lerp(colors[endColor - 1], colors[endColor], pc);
        }

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime) {
            foreach (Background bg in Backgrounds)
                bg.Draw(spriteBatch);
            cities.Draw(spriteBatch);


            foreach (Enemy enm in enemies)
                enm.Draw(spriteBatch);

            player.Draw(spriteBatch);
            spriteBatch.DrawString(scoreFont, player.Position + "", new Vector2(10, 10), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            for (int i = 0; i < player.life; i++)
                spriteBatch.Draw(Terrain, new Vector2(50 + 50 * i, 50), null, new Rectangle(625, 376, 51, 46), Vector2.Zero, 0, new Vector2(0.8f));
            spriteBatch.Draw(Terrain, new Vector2(windowBounds.X - 102, 50), null, new Rectangle(677, 376, 52, 46), Vector2.Zero, 0);
            spriteBatch.DrawString(scoreFont, score + "", new Vector2(windowBounds.X / 2 - 20, 50), Color.Red, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
        }
    }
}
