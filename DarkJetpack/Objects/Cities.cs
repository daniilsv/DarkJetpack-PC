using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DarkJetpack {

    public class Cities {
        private Texture2D Texture;      //The image to use
        public Vector2 Offset;         //Offset to start drawing our image
        public Vector2 Speed;           //Speed of movement of our parallax effect
        public float Zoom;              //Zoom level of our image

        private Viewport Viewport;      //Our game viewport

        //Calculate Rectangle dimensions, based on offset/viewport/zoom values
        private Rectangle Rectangle {
            get { return new Rectangle((int)(Offset.X), 0, (int)(Viewport.Width / Zoom), 321); }
        }

        public Cities(Texture2D texture) {
            Texture = texture;
            Offset = new Vector2(Texture.Width / 2, 0);
            Speed = new Vector2(150, 0);
            Zoom = 1.2f;
        }

        public void Update(GameTime gametime, Vector2 direction, Viewport viewport) {
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;

            //Store the viewport
            Viewport = viewport;

            //Calculate the distance to move our image, based on speed
            Vector2 distance = direction * Speed * elapsed;

            //Update our offset
            Offset.X += distance.X;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, new Vector2(Viewport.X, Viewport.Height - 321 * Zoom + Offset.Y), Rectangle, Color.White, 0, Vector2.Zero, Zoom, SpriteEffects.None, 1);

        }
    }
}
