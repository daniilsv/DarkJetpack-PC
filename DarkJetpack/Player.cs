using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace DarkJetpack
{
    class Player
    {

        private Texture2D Texture;      //The image to use
        public Vector2 Position;         //Offset to start drawing our image
        private Vector2 Speed;           //Speed of movement of our parallax effect
        private float Rotation;
        private Viewport Viewport;      //Our game viewport
        private float scale;
        //Calculate Rectangle dimensions, based on offset/viewport/zoom values
        private Rectangle Rectangle
        {
            get
            {
                return new Rectangle((Viewport.Width / 2 - Viewport.Width / 16),
              (Viewport.Height / 2 - Viewport.Height / 8),
              (Viewport.Width / 8),
              (Viewport.Height / 4));
            }
        }

        public Player(Texture2D texture, Vector2 speed)
        {
            Texture = texture;
            Position = Vector2.Zero;
            Rotation = 0;
            Speed = speed;
        }

        public void Update(GameTime gametime, Vector2 direction, Viewport viewport)
        {
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;

            //Store the viewport
            Viewport = viewport;

            //Calculate the distance to move our image, based on speed
            Vector2 distance = direction * Speed * elapsed;

            //Update our offset
            Position += distance;
            if (direction.X == 1 && Rotation <= 0.38)
            {
                Rotation += 0.03f;
            }
            else
            if (direction.X == -1 && Rotation >= -0.38)
            {
                Rotation -= 0.03f;
            }
            else if (System.Math.Abs(Rotation) >= 0.03f && direction.X == 0)
            {
                Rotation -= System.Math.Sign(Rotation) * 0.025f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            scale = ((float)Viewport.Height / 2 / Texture.Height);
            spriteBatch.Draw(Texture, new Vector2(Viewport.Width / 2, Viewport.Height / 2 + Texture.Height * scale / 2),
                new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, Rotation, new Vector2(Texture.Width / 2, Texture.Height),
                scale, SpriteEffects.None, 1);
        }
    }
}
