using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace DarkJetpack
{
    public class DarkJetpack : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Background> Backgrounds;
        Player player;
        static Texture2D baseTexture;

        public DarkJetpack() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            baseTexture = new Texture2D(GraphicsDevice, 1, 1);
            baseTexture.SetData(new Color[] { Color.White });

            //Load the background images
            Backgrounds = new List<Background>();
            Backgrounds.Add(new Background(Content.Load<Texture2D>(@"Clouds1"), new Vector2(300, 300), 0.6f));
            Backgrounds.Add(new Background(Content.Load<Texture2D>(@"Clouds2"), new Vector2(500, 500), 0.8f));
            Backgrounds.Add(new Background(Content.Load<Texture2D>(@"Clouds3"), new Vector2(700, 700), 1.1f));
            player = new Player(Content.Load<Texture2D>(@"player"), new Vector2(1, 1));
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kbState.IsKeyDown(Keys.Escape))
                Exit();

            //Get directional vector based on keyboard input
            Vector2 direction = Vector2.Zero;
            if (kbState.IsKeyDown(Keys.Up))
                direction = new Vector2(0, -1);
            else if (kbState.IsKeyDown(Keys.Down))
                direction = new Vector2(0, 1);
            if (kbState.IsKeyDown(Keys.Left))
                direction += new Vector2(-1, 0);
            else if (kbState.IsKeyDown(Keys.Right))
                direction += new Vector2(1, 0);

            if (kbState.IsKeyDown(Keys.F11))
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
            }

            if (kbState.IsKeyDown(Keys.Space))
                direction = new Vector2(0, -1);

            //Update backgrounds
            foreach (Background bg in Backgrounds)
                bg.Update(gameTime, direction, GraphicsDevice.Viewport);

            player.Update(gameTime, direction, GraphicsDevice.Viewport);
            base.Update(gameTime);
        }

        public static void DrawLine(SpriteBatch sb, Color color, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;
            float angle = (float)System.Math.Atan2(edge.Y, edge.X);
            sb.Draw(baseTexture, new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), 1), null, color, angle, new Vector2(0, 0), SpriteEffects.None, 0);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Draw our parallax backgrounds, using a Linear Wrap
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            foreach (Background bg in Backgrounds)
                bg.Draw(spriteBatch);

            DrawLine(spriteBatch, Color.BlueViolet, new Vector2(0, GraphicsDevice.Viewport.Height / 2), new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height / 2));
            DrawLine(spriteBatch, Color.YellowGreen, new Vector2(GraphicsDevice.Viewport.Width / 2, 0), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height));

            player.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
