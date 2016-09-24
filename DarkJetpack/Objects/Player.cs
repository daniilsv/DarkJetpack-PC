using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DarkJetpack {
    class Player {

        private Texture2D Texture;      //The image to use
        public Vector2 Position;         //Offset to start drawing our image
        private Vector2 Speed;           //Speed of movement of our parallax effect
        private float Rotation;
        private Viewport Viewport;      //Our game viewport
        private float scale;
        private ParticleMashine pm;
        DarkJetpack game;
        private Rectangle Rectangle {
            get {
                return new Rectangle((Viewport.Width / 2 - Viewport.Width / 16),
              (Viewport.Height / 2 - Viewport.Height / 8),
              (Viewport.Width / 8),
              (Viewport.Height / 4));
            }
        }
        
        public Player(DarkJetpack _game) {
            game = _game;
            Texture = game.Content.Load<Texture2D>(@"player");
            Position = Vector2.Zero;
            Rotation = 0;
            Speed = new Vector2(1, 1);
            pm = new ParticleMashine(20, 64, game.Content.Load<Texture2D>(@"Smoke"));
        }

        public void Update(GameTime gametime, Vector2 direction, Viewport viewport) {
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;

            //Store the viewport
            Viewport = viewport;

            scale = ((float)Viewport.Height / 2 / Texture.Height);

            //Calculate the distance to move our image, based on speed
            Vector2 distance = direction * Speed * elapsed;

            Position += distance;

            /*
            if (direction.X == 1 && Rotation <= 0.38) {
                Rotation += 0.03f;
            } else
            if (direction.X == -1 && Rotation >= -0.38) {
                Rotation -= 0.03f;
            } else if (Math.Abs(Rotation) >= 0.03f && direction.X == 0) {
                Rotation -= Math.Sign(Rotation) * 0.025f;
            }
            */
            
            #region Particle Mashine
            Random r = new Random();
            for (int i = 0; i < pm.count; i++) {
                ParticleMashine.Particle p = pm.particles[i];
                if (p.l <= 0.1) {
                    p.p.X = 40 + Viewport.Width / 2 - Texture.Width * scale / 3f + (float)r.NextDouble() * Texture.Width * scale / 3f;
                    p.p.Y = Viewport.Height / 2 + 100;

                    p.l = 20;
                }

                p.p.X += p.v.X;
                p.p.Y += p.v.Y;
                p.v.X = -3f + 6 * (float)r.NextDouble() - direction.X * 3 * (float)r.NextDouble();
                p.v.Y = 2 - direction.Y * (float)r.NextDouble();
                p.l -= (float)r.NextDouble();
                pm.particles[i] = p;
            }
            #endregion

        }
        public void Draw(SpriteBatch spriteBatch) {
            pm.draw(spriteBatch);
            spriteBatch.Draw(Texture, new Vector2(Viewport.Width / 2, Viewport.Height / 2 + Texture.Height * scale / 2),
                new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, Rotation, new Vector2(Texture.Width / 2, Texture.Height),
                scale, SpriteEffects.None, 1);
        }
    }
}
