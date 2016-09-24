using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DarkJetpack {
    public class ParticleMashine {

        public struct Particle {
            public Vector2 p;
            public Vector2 v;
            public float l;
            public Particle(Vector2 pos, Vector2 vel, float lif) { p = pos; v = vel; l = lif; }
        };
        public List<Particle> particles;
        Texture2D texture;
        public int count;
        private int size;
        public ParticleMashine(int _count, int _size, Texture2D _texture) {
            count = _count;
            particles = new List<Particle>();
            for (int i = 0; i < count; i++) {
                particles.Add(new Particle(Vector2.Zero, Vector2.Zero, 0));
            }
            texture = _texture;
            size = _size;
        }
        public void update() {

        }
        public void draw(SpriteBatch sB) {
            foreach (Particle p in particles) {
                sB.Draw(texture, new Rectangle((int)p.p.X - size, (int)p.p.Y - size, size, size), Color.White);
            }
        }
    }
}
