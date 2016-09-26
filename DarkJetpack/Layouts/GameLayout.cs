using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack {
    class GameLayout : Layout {
        List<Background> Backgrounds;
        Player player;
        int score;
        private SpriteFont scoreFont;
        Cities cities;
        List<Color> colors = new List<Color> { new Color(76, 220, 241), Color.DarkBlue, Color.Red, Color.Green, Color.Yellow };
        public GameLayout(DarkJetpack _game) : base(_game) {
        }
        public override void onLoad() {
            oldKbState = Keyboard.GetState();

            Backgrounds = new List<Background>();
            Backgrounds.Add(new Background(game.Content.Load<Texture2D>(@"Clouds1"), new Vector2(300, 300), 0.6f));
            Backgrounds.Add(new Background(game.Content.Load<Texture2D>(@"Clouds2"), new Vector2(500, 500), 0.8f));
            Backgrounds.Add(new Background(game.Content.Load<Texture2D>(@"Clouds3"), new Vector2(700, 700), 1.1f));
            scoreFont = game.Content.Load<SpriteFont>(@"ScoreFont");
            player = new Player(game, game.Terrain);
            cities = new Cities(game.Terrain);
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
            score = Math.Max(score, (int)player.Position.Y);

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
            player.Draw(spriteBatch);
        }

    }
}
