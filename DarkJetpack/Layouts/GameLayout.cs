using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack
{
    class GameLayout : Layout
    {
        List<Background> Backgrounds;
        Player player;
        int score;
        private SpriteFont scoreFont;
        Cities cities;
        Color startColor;
        public GameLayout(DarkJetpack _game) : base(_game)
        {
        }
        bool isChangingColor = false;
        public override void onLoad()
        {
            oldKbState = Keyboard.GetState();

            Backgrounds = new List<Background>();
            Backgrounds.Add(new Background(game.Content.Load<Texture2D>(@"Clouds1"), new Vector2(300, 300), 0.6f));
            Backgrounds.Add(new Background(game.Content.Load<Texture2D>(@"Clouds2"), new Vector2(500, 500), 0.8f));
            Backgrounds.Add(new Background(game.Content.Load<Texture2D>(@"Clouds3"), new Vector2(700, 700), 1.1f));
            scoreFont = game.Content.Load<SpriteFont>(@"ScoreFont");
            player = new Player(game, game.Terrain);
            cities = new Cities(game.Terrain);
        }

        public override void onUnLoad()
        {

        }
        public override void update(GameTime gameTime)
        {

            KeyboardState kbState = Keyboard.GetState();

            if (kbState.IsKeyDown(Keys.Space) && !oldKbState.IsKeyDown(Keys.Space))
            {
                onBackPressed();
            }
            else if (oldKbState.IsKeyDown(Keys.Space))
            {

            }
            Vector2 direction = Vector2.Zero;
            if (kbState.IsKeyDown(Keys.Up))
                direction = new Vector2(0, -1);
            else if (kbState.IsKeyDown(Keys.Down) && player.Position.Y >= 0)
            {
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

            if (player.Position.Y <= 15)
            {
                ChangeColor(game.backColor, Color.DarkBlue);
            }

            #region Background
            foreach (Background bg in Backgrounds)
                bg.Update(gameTime, direction, viewport, (1 - player.Position.Y / 5));

            cities.Update(gameTime, direction, viewport);
            cities.Offset.Y = player.Position.Y * 50;

            #endregion

        }

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (Background bg in Backgrounds)
                bg.Draw(spriteBatch);
            cities.Draw(spriteBatch);
            spriteBatch.DrawString(scoreFont, score + "", new Vector2(50, 50), Color.Red);
            player.Draw(spriteBatch);

        }

        public void ChangeColor(Color _startColor, Color endColor)
        {
            if (_startColor != startColor && !isChangingColor)
            {
                startColor = _startColor;
            }
            isChangingColor = true;
            game.backColor = Color.Lerp(startColor, endColor, player.Position.Y / 15);
        }
    }
}
