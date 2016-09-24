using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack {
    class MainMenuLayout : Layout {
        Texture2D myTexture;
        private ParticleMashine pm;
        public MainMenuLayout(DarkJetpack _game) : base(_game) {
            pm = new ParticleMashine(50, 2, DarkJetpack.baseTexture);
            myTexture = new Texture2D(game.GraphicsDevice, 200, 200);
        }

        public override void onLoad() {
            addButton(new Rectangle(100, 100, 200, 50), DarkJetpack.baseTexture, null, (() => game.changeLayoutTo(new GameLayout(game))));

        }
        public override void onUnLoad() {
        }
        public override void update(GameTime gameTime) {
            MouseState msState = Mouse.GetState();

            #region Particle Mashine
            Random r = new Random();
            for (int i = 0; i < pm.count; i++) {
                ParticleMashine.Particle p = pm.particles[i];
                if (p.l <= 0.1) {
                    p.p.X = msState.X;
                    p.p.Y = msState.Y;

                    p.l = 8;
                }

                p.p.X += p.v.X;
                p.p.Y += p.v.Y;
                p.v.X = -3 + 6.0f * (float)r.NextDouble();
                p.v.Y = 2 + 2.0f * (float)r.NextDouble();
                p.l -= 0.8f * (float)r.NextDouble();
                pm.particles[i] = p;
            }
            #endregion



            KeyboardState kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(Keys.Enter) && !oldKbState.IsKeyDown(Keys.Enter)) {
                game.changeLayoutTo(new GameLayout(game));
            } else if (oldKbState.IsKeyDown(Keys.Enter)) {

            }
        }
        public override void draw(SpriteBatch spriteBatch, GameTime gameTime) {
            foreach (ParticleMashine.Particle p in pm.particles) {
                spriteBatch.Draw(DarkJetpack.baseTexture, new Rectangle((int)p.p.X, (int)p.p.Y, 3, 3), new Color(p.l, (float)Math.Sin(p.l), (float)Math.Cos(p.l)));
            }
            spriteBatch.Draw(myTexture, new Rectangle(200, 200, 200, 200), Color.White);
        }
    }
}
