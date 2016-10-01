using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack {
    class MainMenuLayout : Layout {
        Texture2D interferenceTexture;
        double lastTime = 0;
        private ParticleMashine particleMashine1, particleMashine2, particleMashine3, particleMashine4, particleMashine5, particleMashine6, particleMashine7;
        Window testw;
        Texture2D playerTexture;
        int playerSkinNum = 0;
        private SpriteFont scoreFont;
        public MainMenuLayout(DarkJetpack game) : base(game) {
            particleMashine1 = new ParticleMashine(150, DarkJetpack.baseTexture, null);
            particleMashine2 = new ParticleMashine(150, DarkJetpack.baseTexture, null);
            particleMashine3 = new ParticleMashine(150, DarkJetpack.baseTexture, null);
            particleMashine4 = new ParticleMashine(150, DarkJetpack.baseTexture, null);
            particleMashine5 = new ParticleMashine(150, DarkJetpack.baseTexture, null);
            particleMashine6 = new ParticleMashine(150, DarkJetpack.baseTexture, null);
            particleMashine7 = new ParticleMashine(150, DarkJetpack.baseTexture, null);
            interferenceTexture = new Texture2D(game.GraphicsDevice, 200, 150);
        }
        private bool prevPlayerSkin() {
            playerSkinNum--;
            if (playerSkinNum == -1)
                playerSkinNum = 7;
            return true;
        }
        private bool nextPlayerSkin() {
            playerSkinNum++;
            if (playerSkinNum == 8)
                playerSkinNum = 0;
            return true;
        }
        public override void onLoad() {
            scoreFont = game.Content.Load<SpriteFont>(@"ScoreFont");
            playerTexture = game.Content.Load<Texture2D>(@"player");
            testw = new Window(new Rectangle(100, 100, windowBounds.X - 200, windowBounds.Y - 200), game.Terrain);
            addButton(new Rectangle(windowBounds.X / 2 - 100, windowBounds.Y - 200, 200, 50), game.Terrain, new Rectangle(429, 321, 123, 41), (() => game.changeLayoutTo(new GameLayout(game, playerSkinNum))));

            addButton(new Rectangle(windowBounds.X / 2 - 150, windowBounds.Y - 200, 35, 50), game.Terrain, new Rectangle(594, 365, 31, 44), (() => prevPlayerSkin()));
            addButton(new Rectangle(windowBounds.X / 2 + 115, windowBounds.Y - 200, 35, 50), game.Terrain, new Rectangle(594, 321, 31, 44), (() => nextPlayerSkin()));

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
                ParticleMashine.Particle p1 = particleMashine1.particles[i];
                ParticleMashine.Particle p2 = particleMashine2.particles[i];
                ParticleMashine.Particle p3 = particleMashine3.particles[i];
                ParticleMashine.Particle p4 = particleMashine4.particles[i];
                ParticleMashine.Particle p5 = particleMashine5.particles[i];
                ParticleMashine.Particle p6 = particleMashine6.particles[i];
                ParticleMashine.Particle p7 = particleMashine7.particles[i];
                if (p1.l <= 0.1) {
                    p1.p = new Vector2(viewport.Width / 2, viewport.Height / 2 - 75); p1.s = 3 + (int)(3 * r.NextDouble()); p1.l = 13;
                    p1.c = Color.Red;
                }
                if (p2.l <= 0.1) {
                    p2.p = new Vector2(viewport.Width / 2, viewport.Height / 2 - 52); p2.s = 3 + (int)(3 * r.NextDouble()); p2.l = 13;
                    p2.c = Color.Orange;
                }
                if (p3.l <= 0.1) {
                    p3.p = new Vector2(viewport.Width / 2, viewport.Height / 2 - 25); p3.s = 3 + (int)(3 * r.NextDouble()); p3.l = 13;
                    p3.c = Color.Yellow;
                }

                if (p4.l <= 0.1) {
                    p4.p = new Vector2(viewport.Width / 2, viewport.Height / 2); p4.s = 3 + (int)(3 * r.NextDouble()); p4.l = 13;
                    p4.c = Color.Green;
                }

                if (p5.l <= 0.1) {
                    p5.p = new Vector2(viewport.Width / 2, viewport.Height / 2 + 25); p5.s = 3 + (int)(3 * r.NextDouble()); p5.l = 13;
                    p5.c = Color.DeepSkyBlue;
                }
                if (p6.l <= 0.1) {
                    p6.p = new Vector2(viewport.Width / 2, viewport.Height / 2 + 50); p6.s = 3 + (int)(3 * r.NextDouble()); p6.l = 13;
                    p6.c = Color.Blue;
                }
                if (p7.l <= 0.1) {
                    p7.p = new Vector2(viewport.Width / 2, viewport.Height / 2 + 75); p7.s = 3 + (int)(3 * r.NextDouble()); p7.l = 13;
                    p7.c = Color.DarkViolet;
                }

                p1.p.X += p1.v.X; p1.p.Y += p1.v.Y + dy;
                p1.v.X = -6 - 2.0f * (float)r.NextDouble();
                p1.v.Y = -2f + 4.0f * (float)r.NextDouble();
                p1.l -= 0.02f + 0.9f * (float)r.NextDouble();

                p2.p.X += p2.v.X; p2.p.Y += p2.v.Y + dy;
                p2.v.X = -6 - 2.0f * (float)r.NextDouble();
                p2.v.Y = -2f + 4.0f * (float)r.NextDouble();
                p2.l -= 0.02f + 0.9f * (float)r.NextDouble();

                p3.p.X += p3.v.X; p3.p.Y += p3.v.Y + dy;
                p3.v.X = -6 - 2.0f * (float)r.NextDouble();
                p3.v.Y = -2f + 4.0f * (float)r.NextDouble();
                p3.l -= 0.02f + 0.9f * (float)r.NextDouble();

                p4.p.X += p4.v.X; p4.p.Y += p4.v.Y + dy;
                p4.v.X = -6 - 2.0f * (float)r.NextDouble();
                p4.v.Y = -2f + 4.0f * (float)r.NextDouble();
                p4.l -= 0.02f + 0.9f * (float)r.NextDouble();

                p5.p.X += p5.v.X; p5.p.Y += p5.v.Y + dy;
                p5.v.X = -6 - 2.0f * (float)r.NextDouble();
                p5.v.Y = -2f + 4.0f * (float)r.NextDouble();
                p5.l -= 0.02f + 0.9f * (float)r.NextDouble();

                p6.p.X += p6.v.X; p6.p.Y += p6.v.Y + dy;
                p6.v.X = -6 - 2.0f * (float)r.NextDouble();
                p6.v.Y = -2f + 4.0f * (float)r.NextDouble();
                p6.l -= 0.02f + 0.9f * (float)r.NextDouble();

                p7.p.X += p7.v.X; p7.p.Y += p7.v.Y + dy;
                p7.v.X = -6 - 2.0f * (float)r.NextDouble();
                p7.v.Y = -2f + 4.0f * (float)r.NextDouble();
                p7.l -= 0.02f + 0.9f * (float)r.NextDouble();

                particleMashine1.particles[i] = p1;
                particleMashine2.particles[i] = p2;
                particleMashine3.particles[i] = p3;
                particleMashine4.particles[i] = p4;
                particleMashine5.particles[i] = p5;
                particleMashine6.particles[i] = p6;
                particleMashine7.particles[i] = p7;
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

            KeyboardState kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(Keys.Enter) && !oldKbState.IsKeyDown(Keys.Enter)) {
                game.changeLayoutTo(new GameLayout(game, playerSkinNum));
            } else if (oldKbState.IsKeyDown(Keys.Enter)) {

            }
        }

        void drawCharacterWindow(SpriteBatch spriteBatch, GameTime gameTime) {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            Texture2D t = game.Terrain;
            Rectangle b = testw.b;
            //Top
            spriteBatch.Draw(t, new Vector2(b.Left + b.Width / 8, b.Top + b.Height / 8), null, new Rectangle(582, 321, 11, 11), null, 0, null, Color.White);
            spriteBatch.Draw(t, new Rectangle(b.Left + b.Width / 8 + 11, b.Top + b.Height / 8, b.Width - 2 * b.Width / 8 - 11, 11), new Rectangle(592, 321, 1, 11), Color.White);
            spriteBatch.Draw(t, new Vector2(b.Left + b.Width - b.Width / 8 + 11, b.Top + b.Height / 8), null, new Rectangle(582, 321, 11, 11), null, MathHelper.PiOver2, null, Color.White);

            //Center
            spriteBatch.Draw(t, null, new Rectangle(b.Left + b.Width / 8 + 11, b.Top + b.Height / 8 + 11, b.Height - 3 * b.Height / 8 - 2 * 11 + 1, 11), new Rectangle(592, 321, 1, 11), new Vector2(1, 11), -MathHelper.PiOver2, null, Color.White);
            spriteBatch.Draw(t, null, new Rectangle(b.Left + b.Width - b.Width / 8 + 11, b.Top + b.Height / 8 + 11, b.Height - 3 * b.Height / 8 - 2 * 11 + 1, 11), new Rectangle(592, 321, 1, 11), new Vector2(0, 0), MathHelper.PiOver2, null, Color.White);
            //Bottom

            spriteBatch.Draw(t, new Vector2(b.Left + b.Width / 8, b.Top + b.Height - 2 * b.Height / 8), null, new Rectangle(582, 321, 11, 11), Vector2.Zero, -MathHelper.PiOver2, null, Color.White);
            spriteBatch.Draw(t, null, new Rectangle(b.Left + b.Width / 8 + 11, b.Top + b.Height - 2 * b.Height / 8, b.Width - 2 * b.Width / 8 - 11, 11), new Rectangle(592, 321, 1, 11), new Vector2(1, 0), MathHelper.Pi, null, Color.White);
            spriteBatch.Draw(t, new Vector2(b.Left + b.Width - b.Width / 8 + 11, b.Top + b.Height - 2 * b.Height / 8), null, new Rectangle(582, 321, 11, 11), null, MathHelper.Pi, null, Color.White);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);

            spriteBatch.Draw(t, null, new Rectangle(b.Left + b.Width / 8 + 7, b.Top + b.Height / 8 + 7, b.Width - 2 * b.Width / 8 - 2, b.Height - 3 * b.Height / 8 - 14), new Rectangle((int)(gameTime.TotalGameTime.TotalMilliseconds / 4), 0, b.Width - 206, 321), Vector2.Zero, 0, null, Color.White);

            int dy = (int)(10 * Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 200));

            particleMashine1.draw(spriteBatch);
            particleMashine2.draw(spriteBatch);
            particleMashine3.draw(spriteBatch);
            particleMashine4.draw(spriteBatch);
            particleMashine5.draw(spriteBatch);
            particleMashine6.draw(spriteBatch);
            particleMashine7.draw(spriteBatch);
            spriteBatch.Draw(playerTexture, null, new Rectangle(viewport.Width / 2 - viewport.Width / 24 + 5, viewport.Height / 2 - viewport.Height / 8 + dy, viewport.Width / 12, viewport.Height / 4),
                new Rectangle(playerSkinNum * 400, 0, 400, 800), null, 0, null, Color.White);
            spriteBatch.Draw(interferenceTexture, new Rectangle(b.Left + b.Width / 8 + 7, b.Top + b.Height / 8 + 7, b.Width - 2 * b.Width / 8 - 2, b.Height - 3 * b.Height / 8 - 14), Color.White * 0.2f);
        }

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime) {
            testw.onDraw(spriteBatch, gameTime);
            drawCharacterWindow(spriteBatch, gameTime);
            foreach (Button b in buttons)
                b.onDraw(spriteBatch, gameTime);
            spriteBatch.DrawString(scoreFont, "Start Game", new Vector2(windowBounds.X / 2 - 79, windowBounds.Y - 182), Color.Black, 0, Vector2.Zero, 1.4f, SpriteEffects.None, 1);
            spriteBatch.DrawString(scoreFont, "Start Game", new Vector2(windowBounds.X / 2 - 81, windowBounds.Y - 184), Color.Black, 0, Vector2.Zero, 1.4f, SpriteEffects.None, 1);
            spriteBatch.DrawString(scoreFont, "Start Game", new Vector2(windowBounds.X / 2 - 80, windowBounds.Y - 183), new Color(255,134,26), 0, Vector2.Zero, 1.4f, SpriteEffects.None, 1);
        }
    }
}
