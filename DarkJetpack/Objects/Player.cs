using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DarkJetpack {
    public class Player {
        private Texture2D texture;      //The image to use
        public Color[] textureData;
        public Vector2 Position;         //Offset to start drawing our image
        private Vector2 Speed;           //Speed of movement of our parallax effect
        private float Rotation;
        private Viewport Viewport;      //Our game viewport
        private float scale;
        public int highscore;
        public bool unlocked = false;
        public int score;
        private ParticleMashine pmS, pmF;
        public int life = 3;
        public Rectangle RectangleCollide;
        DarkJetpack game;
        public Rectangle Rectangle {
            get {
                return new Rectangle(Viewport.Width / 2 - Viewport.Width / 18, Viewport.Height / 2 - Viewport.Height / 6, Viewport.Width / 9, Viewport.Height / 3);
            }
        }
        public int skinNum;
        Texture2D Terrain;
        public Player(DarkJetpack _game, Texture2D terrain) {
            game = _game;
            Terrain = terrain;
            Position = Vector2.Zero;
            texture = game.Content.Load<Texture2D>(@"player");
            Rotation = 0;
            Speed = new Vector2(2, 2);
            pmS = new ParticleMashine(50, Terrain, new Rectangle(365, 321, 64, 64));
            pmF = new ParticleMashine(20, Terrain, new Rectangle(365, 381, 64, 64));
        }

        public void setSkin(int skinN) {
            skinNum = skinN;
            SavedData d = SaveGameStorage.LoadData(skinN);
            unlocked = game.unlocked[skinNum] = d.unlocked;
            highscore = d.highscore;
        }
        public void start() {
            Position = Vector2.Zero;
            life = 3;
            score = 0;

        }
        public void Update(GameTime gametime, Vector2 direction, Viewport viewport) {
            RectangleCollide = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width - Rectangle.Width / 3, Rectangle.Height);

            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;
            direction.Y = -direction.Y;

            Viewport = viewport;
            scale = ((float)Viewport.Height / 3 / texture.Height);

            Vector2 distance = direction * Speed * elapsed;
            Position += distance;
            score = Math.Max(score, (int)(Position.Y * 10));
            highscore = Math.Max(highscore, score);

            if (score >= skinNum * 400) {
                game.unlocked[skinNum + 1] = true;
            }

            #region Particle Mashine
            Random r = new Random();
            for (int i = 0; i < pmS.count; i++) {
                ParticleMashine.Particle ps = pmS.particles[i];
                if (ps.l <= 0.1) {
                    ps.p.X = 30 + Viewport.Width / 2 - Viewport.Width / 18 + (float)r.NextDouble() * Viewport.Width / 16;
                    ps.p.Y = Viewport.Height / 2 + 80;
                    ps.s = 16 + (int)(32 * r.NextDouble());
                    ps.c = new Color(Color.LightGray, 155 + (int)(100 * r.NextDouble()));
                    ps.l = 8;
                }

                ps.p.X += ps.v.X; ps.p.Y += ps.v.Y;
                ps.v.X = -10 + 20 * (float)r.NextDouble() - 10 * direction.X * (float)r.NextDouble();
                ps.v.Y = 5 + 2 * direction.Y * (float)r.NextDouble();
                ps.l -= (float)r.NextDouble();
                pmS.particles[i] = ps;
            }

            for (int i = 0; i < pmF.count; i++) {
                ParticleMashine.Particle pf = pmF.particles[i];
                if (pf.l <= 0.1) {
                    pf.p.X = 30 + Viewport.Width / 2 - Viewport.Width / 18 + (float)r.NextDouble() * Viewport.Width / 16;
                    pf.p.Y = Viewport.Height / 2 + 100;
                    pf.s = 10 + (int)(20 * r.NextDouble());
                    pf.c = new Color(Color.Red, 55 + (int)(200 * r.NextDouble()));
                    pf.l = 8;
                }

                pf.p.X += pf.v.X; pf.p.Y += pf.v.Y;
                pf.v.X = -5 + 10 * (float)r.NextDouble() - 5 * direction.X * (float)r.NextDouble();
                pf.v.Y = 3 - direction.Y * (float)r.NextDouble();
                pf.l -= (float)r.NextDouble();
                pmF.particles[i] = pf;
            }

            #endregion

        }
        public void Draw(SpriteBatch spriteBatch) {
            pmF.draw(spriteBatch);
            pmS.draw(spriteBatch);
            spriteBatch.Draw(texture, null, Rectangle,
                new Rectangle(400 * skinNum, 0, 400, 800), null, Rotation, null, Color.White);
            if (DarkJetpack.isDebug)
                spriteBatch.Draw(DarkJetpack.baseTexture, RectangleCollide, new Color(Color.HotPink, 0.2f));
        }
    }
}
