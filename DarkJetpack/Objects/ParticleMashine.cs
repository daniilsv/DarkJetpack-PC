using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DarkJetpack {
    public class ParticleMashine {

        public struct Particle {
            public Vector2 p;
            public Vector2 v;
            public Color c;
            public int s;
            public float l;
            public Particle(Vector2 pos, Vector2 vel, int siz, float lif, Color col) { p = pos; v = vel; s = siz; l = lif; c = col; }
        };
        public List<Particle> particles;
        Texture2D texture;
        Rectangle? sourceRect;
        public int count;

        public ParticleMashine(int _count, Texture2D _texture, Rectangle? sourceRectangle) {
            count = _count;
            particles = new List<Particle>();
            for (int i = 0; i < count; i++) {
                particles.Add(new Particle(Vector2.Zero, Vector2.Zero, 0, 0, Color.White));
            }
            texture = _texture;
            sourceRect = sourceRectangle;
        }
        public virtual void update() {

        }
        public void draw(SpriteBatch sB) {
            foreach (Particle p in particles) {
                sB.Draw(texture, new Rectangle((int)p.p.X - p.s, (int)p.p.Y - p.s, p.s, p.s), sourceRect, p.c);
            }
        }
    }
}
