using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack {
    class GameLayout : Layout {
        private Texture2D Terrain;
        List<Background> Backgrounds;
        Player player;
        int score;
        private SpriteFont scoreFont;
        Cities cities;
        public GameLayout(DarkJetpack _game) : base(_game) {
        }

        public override void onLoad() {
            oldKbState = Keyboard.GetState();

            Backgrounds = new List<Background>();
            Terrain = game.Content.Load<Texture2D>(@"Terrain");
            Backgrounds.Add(new Background(game.Content.Load<Texture2D>(@"Clouds1"), new Vector2(300, 300), 0.6f));
            Backgrounds.Add(new Background(game.Content.Load<Texture2D>(@"Clouds2"), new Vector2(500, 500), 0.8f));
            Backgrounds.Add(new Background(game.Content.Load<Texture2D>(@"Clouds3"), new Vector2(700, 700), 1.1f));
            scoreFont = game.Content.Load<SpriteFont>(@"ScoreFont");
            player = new Player(game, Terrain);
            cities = new Cities(Terrain);
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
            else if (kbState.IsKeyDown(Keys.Down) && player.Position.Y <= 0) {
                direction = new Vector2(0, 1);
            }

            if (kbState.IsKeyDown(Keys.Left))
                direction += new Vector2(-1, 0);
            else if (kbState.IsKeyDown(Keys.Right))
                direction += new Vector2(1, 0);

            if (kbState.IsKeyDown(Keys.F11)) {
                game.graphics.IsFullScreen = !game.graphics.IsFullScreen;
                game.graphics.ApplyChanges();
            }

            cities.Update(gameTime, direction, viewport);

            player.Update(gameTime, direction, viewport);

            foreach (Background bg in Backgrounds)
                bg.Update(gameTime, direction, viewport, (1 + player.Position.Y / 2));

            cities.Offset.Y = -player.Position.Y * 50;
            score = Math.Max(score, -(int)player.Position.Y);
            game.backColor = new Color(
                (int)(76 * (1 + player.Position.Y / 15)),
                (int)(220 * (1 + player.Position.Y / 2)),
                (int)(241 * (1 + player.Position.Y / 2)));

        }

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime) {
            foreach (Background bg in Backgrounds)
                bg.Draw(spriteBatch);
            cities.Draw(spriteBatch);
            DarkJetpack.DrawLine(spriteBatch, Color.BlueViolet, new Vector2(0, viewport.Height / 2), new Vector2(viewport.Width, viewport.Height / 2));
            DarkJetpack.DrawLine(spriteBatch, Color.YellowGreen, new Vector2(viewport.Width / 2, 0), new Vector2(viewport.Width / 2, viewport.Height));
            spriteBatch.DrawString(scoreFont, score + "", new Vector2(50, 50), Color.Red);
            player.Draw(spriteBatch);

        }
    }
}
