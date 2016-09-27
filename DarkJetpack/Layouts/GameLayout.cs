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
        List<Color> colors = new List<Color> { new Color(76, 220, 241), Color.DarkBlue, Color.BlanchedAlmond, Color.CadetBlue, Color.BlueViolet };

        List<Asteroid> asteroids;

        public GameLayout(DarkJetpack game) : base(game) {
            Terrain = game.Terrain;
        }
        public override void onLoad() {
            scoreFont = game.Content.Load<SpriteFont>(@"ScoreFont");

            Backgrounds = new List<Background>();
            Backgrounds.Add(new Background(game.Content.Load<Texture2D>(@"Clouds1"), new Vector2(300, 300), 0.6f));
            Backgrounds.Add(new Background(game.Content.Load<Texture2D>(@"Clouds2"), new Vector2(500, 500), 0.8f));
            Backgrounds.Add(new Background(game.Content.Load<Texture2D>(@"Clouds3"), new Vector2(700, 700), 1.1f));
            cities = new Cities(game.Terrain);
            player = new Player(game, game.Terrain);

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

            foreach (Background bg in Backgrounds)
                bg.Update(gameTime, direction, viewport, (1 - player.Position.Y / 15));

            cities.Update(gameTime, direction, viewport);
            cities.Offset.Y = player.Position.Y * 50;
            #endregion

            foreach (Asteroid ast in asteroids)
                ast.onUpdate(gameTime);

            for (int i = 0; i < asteroids.Count - 1; i++) {
                Asteroid ast1 = asteroids[i];
                if (ast1 == null) continue;
                for (int j = i + 1; j < asteroids.Count; j++) {
                    Asteroid ast2 = asteroids[j];
                    if (ast2 == null) continue;
                    if (ast1.rectDraw.Intersects(ast2.rectDraw)) {
                        asteroids.RemoveAt(j);
                        j--;
                    }
                }
                if (ast1.rectDraw.Intersects(player.Rectangle)) {
                    player.life--;
                    asteroids.RemoveAt(i);
                    i--;
                }
            }
            
            Random r = new Random();
            if (gameTime.TotalGameTime.TotalMilliseconds - lastTime > 1500) {
                for (int i = 0; i < 2; i++)
                    asteroids.Add(new Asteroid(this, new Vector2(player.Position.X - windowBounds.X / 120 + (float)(windowBounds.X / 60 * r.NextDouble()), player.Position.Y + windowBounds.Y / 100)));
                lastTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }

        public void ChangeColor(int endColor, float pc) {
            game.backColor = Color.Lerp(colors[endColor - 1], colors[endColor], pc);
        }

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime) {
            foreach (Background bg in Backgrounds)
                bg.Draw(spriteBatch);
            cities.Draw(spriteBatch);

            spriteBatch.DrawString(scoreFont, score + "", new Vector2(50, 50), Color.Red, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
            spriteBatch.DrawString(scoreFont, player.Position + "", new Vector2(10, 10), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            spriteBatch.DrawString(scoreFont, player.life + "", new Vector2(50, 100), Color.Red, 0, Vector2.Zero, 2, SpriteEffects.None, 1);

            player.Draw(spriteBatch);
            foreach (Asteroid ast in asteroids)
                ast.Draw(spriteBatch);
        }

    }
}
