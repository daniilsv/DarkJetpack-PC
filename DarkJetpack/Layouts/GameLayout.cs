using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack {
    class GameLayout : Layout {

        List<Background> Backgrounds;
        Player player;

        public GameLayout(DarkJetpack _game) : base(_game) {
        }

        public override void onLoad() {
            oldKbState = Keyboard.GetState();

            Backgrounds = new List<Background>();
            Backgrounds.Add(new Background(game.Content.Load<Texture2D>(@"Clouds1"), new Vector2(300, 300), 0.6f));
            Backgrounds.Add(new Background(game.Content.Load<Texture2D>(@"Clouds2"), new Vector2(500, 500), 0.8f));
            Backgrounds.Add(new Background(game.Content.Load<Texture2D>(@"Clouds3"), new Vector2(700, 700), 1.1f));
            player = new Player(game);
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
            else if (kbState.IsKeyDown(Keys.Down))
                direction = new Vector2(0, 1);
            if (kbState.IsKeyDown(Keys.Left))
                direction += new Vector2(-1, 0);
            else if (kbState.IsKeyDown(Keys.Right))
                direction += new Vector2(1, 0);

            if (kbState.IsKeyDown(Keys.F11)) {
                game.graphics.IsFullScreen = !game.graphics.IsFullScreen;
                game.graphics.ApplyChanges();
            }

            foreach (Background bg in Backgrounds)
                bg.Update(gameTime, direction, viewport);

            player.Update(gameTime, direction, viewport);
        }
        public override void draw(SpriteBatch spriteBatch, GameTime gameTime) {
            foreach (Background bg in Backgrounds)
                bg.Draw(spriteBatch);

            DarkJetpack.DrawLine(spriteBatch, Color.BlueViolet, new Vector2(0, viewport.Height / 2), new Vector2(viewport.Width, viewport.Height / 2));
            DarkJetpack.DrawLine(spriteBatch, Color.YellowGreen, new Vector2(viewport.Width / 2, 0), new Vector2(viewport.Width / 2, viewport.Height));

            player.Draw(spriteBatch);
        }
    }
}
