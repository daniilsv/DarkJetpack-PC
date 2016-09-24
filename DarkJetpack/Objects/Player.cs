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
        private ParticleMashine pmS, pmF;
        DarkJetpack game;
        private Rectangle Rectangle {
            get {
                return new Rectangle((Viewport.Width / 2 - Viewport.Width / 16),
              (Viewport.Height / 2 - Viewport.Height / 8),
              (Viewport.Width / 8),
              (Viewport.Height / 4));
            }
        }
        Texture2D Terrain;
        public Player(DarkJetpack _game, Texture2D terrain) {
            game = _game;
            Terrain = terrain;
            Texture = game.Content.Load<Texture2D>(@"player");
            Position = Vector2.Zero;
            Rotation = 0;
            Speed = new Vector2(1, 1);
            pmS = new ParticleMashine(20, Terrain, new Rectangle(1887, 642, 64, 64));
            pmF = new ParticleMashine(20, Terrain, new Rectangle(1887, 702, 64, 64));
        }

        public void Update(GameTime gametime, Vector2 direction, Viewport viewport) {
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;

            //Store the viewport
            Viewport = viewport;

            scale = ((float)Viewport.Height / 3 / Texture.Height);

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
            for (int i = 0; i < pmF.count; i++) {
                ParticleMashine.Particle ps = pmS.particles[i];
                ParticleMashine.Particle pf = pmF.particles[i];
                if (ps.l <= 0.1) {
                    ps.p.X = 50 + Viewport.Width / 2 - Texture.Width * scale / 3f + (float)r.NextDouble() * Texture.Width * scale / 3f;
                    ps.p.Y = Viewport.Height / 2 + 80;
                    ps.s = 16 + (int)(32 * r.NextDouble());
                    ps.c = new Color(Color.White, 100 + (int)(155 * r.NextDouble()));
                    ps.l = 16;

                    pf.p.X = 40 + Viewport.Width / 2 - Texture.Width * scale / 3f + (float)r.NextDouble() * Texture.Width * scale / 4f;
                    pf.p.Y = Viewport.Height / 2 + 100;
                    pf.s = 10 + (int)(20 * r.NextDouble());
                    pf.c = new Color(Color.Orange, 25 + (int)(55 * r.NextDouble()));
                    pf.l = 8;
                }

                ps.p.X += ps.v.X; ps.p.Y += ps.v.Y;
                ps.v.X = -3f + 6 * (float)r.NextDouble() - direction.X * 3 * (float)r.NextDouble();
                ps.v.Y = 2 - direction.Y * (float)r.NextDouble();
                ps.l -= (float)r.NextDouble();
                pmS.particles[i] = ps;

                pf.p.X += pf.v.X; pf.p.Y += pf.v.Y;
                pf.v.X = -3f + 6 * (float)r.NextDouble() - direction.X * 3 * (float)r.NextDouble();
                pf.v.Y = 2 - direction.Y * (float)r.NextDouble();
                pf.l -= (float)r.NextDouble();
                pmF.particles[i] = pf;
            }

            #endregion

        }
        public void Draw(SpriteBatch spriteBatch) {
            pmF.draw(spriteBatch);
            pmS.draw(spriteBatch);
            spriteBatch.Draw(Texture, null, new Rectangle(Viewport.Width / 2 - Viewport.Width / 18, Viewport.Height / 2 - Viewport.Height / 6, Viewport.Width / 9, Viewport.Height / 3),
                new Rectangle(0, 0, Texture.Width, Texture.Height), null, Rotation, null, Color.White);
        }
    }
}
