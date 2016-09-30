using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack {
    public class GameLayout : Layout {
        double lastTime = 0;
        List<Background> Backgrounds;
        public Player player;
        public Texture2D Terrain;
        int score;
        private SpriteFont scoreFont;
        Cities cities;
        List<Color> colors = new List<Color> { new Color(76, 220, 241), Color.DarkBlue, Color.Gainsboro, Color.CadetBlue, Color.Black };

        List<List<Texture2D>> backTexs;

        List<Asteroid> asteroids;
        int playerSkinNum;
        public GameLayout(DarkJetpack game, int playerSkinN) : base(game) {
            playerSkinNum = playerSkinN;
            Terrain = game.Terrain;
        }
        public override void onLoad() {
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
            asteroids = new List<Asteroid>();
        }

        public override void onUnLoad() {

        }

        public override void update(GameTime gameTime) {

            KeyboardState kbState = Keyboard.GetState();

            if (kbState.IsKeyDown(Keys.Space) && !oldKbState.IsKeyDown(Keys.Space)) {
                onBackPressed();
            } else if (oldKbState.IsKeyDown(Keys.Space)) {

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
            if (player.Position.Y <= 6)
                ChangeColor(1, player.Position.Y / 6);
            else if (player.Position.Y <= 12)
                ChangeColor(2, player.Position.Y / 6 - 1);
            else if (player.Position.Y <= 18)
                ChangeColor(3, player.Position.Y / 6 - 2);
            else if (player.Position.Y <= 24)
                ChangeColor(4, player.Position.Y / 6 - 3);

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

            if (player.Position.Y < 15) {
                cities.Update(gameTime, direction, viewport);
                cities.Offset.Y = player.Position.Y * 50;
            }
            #endregion

            #region Asteroids
            foreach (Asteroid ast in asteroids)
                ast.onUpdate(gameTime);

            for (int i = 0; i < asteroids.Count - 1; i++) {
                Asteroid ast1 = asteroids[i];
                if (ast1.rectDraw.Top > viewport.Height * 2) {
                    asteroids.RemoveAt(i);
                    i--; continue;
                }
                if (ast1.rectDraw.Intersects(player.Rectangle)) {
                    player.life--;
                    asteroids.RemoveAt(i);
                    i--; continue;
                }
                if (ast1 == null) continue;
                for (int j = i + 1; j < asteroids.Count; j++) {
                    Asteroid ast2 = asteroids[j];
                    if (ast2 == null) continue;
                    if (ast1.rectDraw.Intersects(ast2.rectDraw)) {
                        asteroids.RemoveAt(j);
                        j--;
                    }
                }
            }

            Random r = new Random();
            if (gameTime.TotalGameTime.TotalMilliseconds - lastTime > 1500) {
                for (int i = 0; i < 2; i++)
                    asteroids.Add(new Asteroid(this, new Vector2(player.Position.X - windowBounds.X / 120 + (float)(windowBounds.X / 60 * r.NextDouble()), player.Position.Y + windowBounds.Y / 100)));
                lastTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
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


            player.Draw(spriteBatch);
            foreach (Asteroid ast in asteroids)
                ast.Draw(spriteBatch);

            spriteBatch.DrawString(scoreFont, player.Position + "", new Vector2(10, 10), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            for (int i = 0; i < player.life; i++)
                spriteBatch.Draw(Terrain, new Vector2(50 + 30 * i, 100), null, new Rectangle(625, 376, 52, 46), Vector2.Zero, 0, new Vector2(0.5f), Color.Orange);

            spriteBatch.DrawString(scoreFont, score + "", new Vector2(50, 50), Color.Red, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
        }
    }
}
