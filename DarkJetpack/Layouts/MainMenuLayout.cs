using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack {
    class MainMenuLayout : Layout {
        Texture2D interferenceTexture;
        double lastTime = 0;
        private ParticleMashine particleMashine;
        Window testw;
        public MainMenuLayout(DarkJetpack _game) : base(_game) {
            particleMashine = new ParticleMashine(50, DarkJetpack.baseTexture, null);
            interferenceTexture = new Texture2D(game.GraphicsDevice, 200, 150);
        }

        public override void onLoad() {
            testw = new Window(new Rectangle(100, 100, windowBounds.X - 200, windowBounds.Y - 200), game.Terrain);
            addButton(new Rectangle(windowBounds.X / 2 - 100, windowBounds.Y - 200, 200, 50), game.Terrain, new Rectangle(429, 321, 123, 41), (() => game.changeLayoutTo(new GameLayout(game))));

        }
        public override void onUnLoad() {
        }
        public override void update(GameTime gameTime) {
            MouseState msState = Mouse.GetState();

            #region Particle Mashine
            Random r = new Random();
            for (int i = 0; i < particleMashine.count; i++) {
                ParticleMashine.Particle p = particleMashine.particles[i];
                if (p.l <= 0.1) {
                    p.p.X = msState.X;
                    p.p.Y = msState.Y;
                    p.s = 3 + (int)(3 * r.NextDouble());
                    p.l = 8;
                }
                p.p.X += p.v.X; p.p.Y += p.v.Y;
                p.v.X = -3 + 6.0f * (float)r.NextDouble();
                p.v.Y = 2 + 2.0f * (float)r.NextDouble();
                p.l -= 0.8f * (float)r.NextDouble();
                p.c = new Color(p.l, (float)Math.Sin(p.l), (float)Math.Cos(p.l));
                particleMashine.particles[i] = p;
            }
            #endregion

            #region Interference
            if (gameTime.TotalGameTime.TotalMilliseconds - lastTime > 100) {
                Color[] arr = new Color[200 * 200];
                for (int i = 0; i < arr.Length; i++) {
                    arr[i] = new Color(Color.Gray, 50);
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
                game.changeLayoutTo(new GameLayout(game));
            } else if (oldKbState.IsKeyDown(Keys.Enter)) {

            }
        }
        public override void draw(SpriteBatch spriteBatch, GameTime gameTime) {
            testw.onDraw(spriteBatch, gameTime);
            foreach (Button b in buttons)
                b.onDraw(spriteBatch, gameTime);
            particleMashine.draw(spriteBatch);
            //spriteBatch.Draw(interferenceTexture, new Rectangle(200, 200, 200, 150), Color.White);
        }
    }
}
