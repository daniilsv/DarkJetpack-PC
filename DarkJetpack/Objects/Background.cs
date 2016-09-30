using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DarkJetpack {

    public class Background {
        private Texture2D Texture;      //The image to use
        private Vector2 Offset;         //Offset to start drawing our image
        public Vector2 Speed;           //Speed of movement of our parallax effect
        public float Zoom;              //Zoom level of our image

        private Viewport Viewport;      //Our game viewport

        float alpha;

        //Calculate Rectangle dimensions, based on offset/viewport/zoom values
        private Rectangle Rectangle {
            get { return new Rectangle((int)(Offset.X), (int)(Offset.Y), (int)(Viewport.Width / Zoom), (int)(Viewport.Height / Zoom)); }
        }

        public Background(Vector2 speed, float zoom) {
            Texture = null;
            Offset = Vector2.Zero;
            Speed = speed;
            Zoom = zoom;
        }
        public void Update(GameTime gametime, Vector2 direction, Viewport viewport, float _alpha) {
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;
            Viewport = viewport;
            Vector2 distance = direction * Speed * elapsed;
            Offset += distance;
            alpha = _alpha;
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (Texture == null)
                return;
            spriteBatch.Draw(Texture, new Vector2(Viewport.X, Viewport.Y), Rectangle, Color.White * alpha, 0, Vector2.Zero, Zoom, SpriteEffects.None, 1);
        }
        public void setTexture(Texture2D tex) {
            Texture = tex;
        }
    }
}
