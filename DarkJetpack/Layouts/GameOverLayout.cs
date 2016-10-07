using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack {
    class GameOverLayout : Layout {
        Texture2D interferenceTexture;
        double lastTime = 0;
        private ParticleMashine particleMashine;
        Window testw;
        Texture2D playerTexture;
        private SpriteFont scoreFont;
        int score;
        public GameOverLayout(DarkJetpack game) : base(game) {
            score = player.score;
            particleMashine = new ParticleMashine(150, DarkJetpack.baseTexture, null);
            interferenceTexture = new Texture2D(game.GraphicsDevice, 200, 150);
            SaveGameStorage.SaveData(player.highscore);
        }
        public override void onLoad() {
            scoreFont = game.Content.Load<SpriteFont>(@"ScoreFont");
            playerTexture = game.Content.Load<Texture2D>(@"player");
            testw = new Window(new Rectangle(100, 100, windowBounds.X - 200, windowBounds.Y - 200), game.Terrain);
            addButton(new Rectangle(windowBounds.X / 2 - 100, windowBounds.Y - 200, 200, 50), game.Terrain, new Rectangle(429, 321, 123, 41), (() => game.changeLayoutBack()));
        }

        public override void onUnLoad() {
        }

        public override void update(GameTime gameTime) {
            MouseState msState = Mouse.GetState();
            Point mousePosition = new Point(msState.X, msState.Y);

            #region Particle Mashine
            int dy = (int)(2.5f * Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 200));
            Random r = new Random();
            for (int i = 0; i < 150; i++) {
                ParticleMashine.Particle p1 = particleMashine.particles[i];
                if (p1.l <= 0.1) {
                    p1.p = new Vector2(viewport.Width / 2, viewport.Height / 2 - 75); p1.s = 3 + (int)(3 * r.NextDouble()); p1.l = 13;
                    p1.c = Color.Red;
                }
                p1.p.X = viewport.Width / 2 + (float)Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 700) * 200 + (float)r.NextDouble() * 20 * p1.v.X;
                p1.p.Y = viewport.Height / 2 + (float)Math.Cos(gameTime.TotalGameTime.TotalMilliseconds / 700) * 200 + (float)r.NextDouble() * 20 * p1.v.Y + dy;
                p1.v.X = -3 + 6.0f * (float)r.NextDouble();
                p1.v.Y = -2f + 4.0f * (float)r.NextDouble();
                p1.l -= 0.02f + 0.9f * (float)r.NextDouble();
                p1.c = new Color((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());

                particleMashine.particles[i] = p1;
            }
            #endregion

            #region Interference
            if (gameTime.TotalGameTime.TotalMilliseconds - lastTime > 100) {
                Color[] arr = new Color[200 * 200];
                for (int i = 0; i < arr.Length; i++) {
                    arr[i] = Color.Black;
                }
                Random a = new Random();
                for (int t = 0; t < 10; t++) {
                    int y = (int)((interferenceTexture.Height - 3) * a.NextDouble());
                    for (int i = 0; i < interferenceTexture.Width; i++)
                        for (int j = y; j < y + 2; j++)
                            arr[j * interferenceTexture.Width + i] = new Color(50, 50, 50, (int)(250 * a.NextDouble()));
                }
                interferenceTexture.SetData(arr);
                lastTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            #endregion

        }

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime) {
            testw.onDraw(spriteBatch, gameTime);
            foreach (Button b in buttons)
                b.onDraw(spriteBatch, gameTime);

            spriteBatch.DrawString(scoreFont, score + "", new Vector2(windowBounds.X / 2 - 70, 300), Color.MonoGameOrange, 0, Vector2.Zero, 1.75f, SpriteEffects.None, 1);
            spriteBatch.DrawString(DarkJetpack.baseFont, "Retry", new Vector2(windowBounds.X / 2 - 39, windowBounds.Y - 182), Color.Black, 0, Vector2.Zero, 1.4f, SpriteEffects.None, 1);
            spriteBatch.DrawString(DarkJetpack.baseFont, "Retry", new Vector2(windowBounds.X / 2 - 41, windowBounds.Y - 184), Color.Black, 0, Vector2.Zero, 1.4f, SpriteEffects.None, 1);
            spriteBatch.DrawString(DarkJetpack.baseFont, "Retry", new Vector2(windowBounds.X / 2 - 40, windowBounds.Y - 183), new Color(255, 134, 26), 0, Vector2.Zero, 1.4f, SpriteEffects.None, 1);

        }
    }
}
