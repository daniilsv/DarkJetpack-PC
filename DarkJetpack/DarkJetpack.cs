﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
namespace DarkJetpack {
    public class DarkJetpack : Game {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Texture2D baseTexture;
        Layout curLayout;

        Stack<Layout> layoutBackStack;

        public DarkJetpack() : base() {
            IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            layoutBackStack = new Stack<Layout>();
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
        }

        protected override void Initialize() {
            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            baseTexture = new Texture2D(GraphicsDevice, 1, 1);
            baseTexture.SetData(new Color[] { Color.White });

            curLayout = new MainMenuLayout(this);
        }

        protected override void UnloadContent() {
            curLayout.onUnLoad();
        }

        protected override void Update(GameTime gameTime) {
            KeyboardState kbState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kbState.IsKeyDown(Keys.Escape))
                Exit();

            curLayout.onUpdate(gameTime, GraphicsDevice.Viewport);
            base.Update(gameTime);
        }

        public static void DrawLine(SpriteBatch sb, Color color, Vector2 start, Vector2 end) {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);
            sb.Draw(baseTexture, new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), 1), null, color, angle, new Vector2(0, 0), SpriteEffects.None, 0);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);

            curLayout.onDraw(spriteBatch, gameTime);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public bool changeLayoutTo(Layout layoutToChange) {
            layoutBackStack.Push(curLayout);
            curLayout = layoutToChange;
            return true;
        }
        public Layout changeLayoutBack() {
            Layout prevLayout = curLayout;
            curLayout = layoutBackStack.Pop();
            return prevLayout;
        }
    }
}
