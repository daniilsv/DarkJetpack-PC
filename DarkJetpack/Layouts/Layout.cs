using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace DarkJetpack {
    public class Layout {
        protected DarkJetpack game;
        protected Viewport viewport;
        protected MouseState oldMsState;
        protected KeyboardState oldKbState;
        List<Button> buttons;
        public Point windowBounds;
        public Layout(DarkJetpack _game) {
            game = _game;
            buttons = new List<Button>();
            windowBounds = new Point(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            onLoad();
        }
        public virtual void onLoad() { }
        public virtual void onUnLoad() { }
        public virtual void update(GameTime gameTime) { }
        public void onUpdate(GameTime gameTime, Viewport _viewport) {
            viewport = _viewport;
            update(gameTime);

            MouseState msState = Mouse.GetState();
            #region Buttons
            foreach (Button b in buttons) {
                if (b.b.Contains(msState.X, msState.Y)) {
                    b.is_active = true;
                    if (oldMsState.LeftButton == ButtonState.Released && msState.LeftButton == ButtonState.Pressed)
                        b.onClick();
                    break;
                } else
                    b.is_active = false;
                b.onUpdate();
            }
            #endregion
            oldMsState = msState;

            KeyboardState kbState = Keyboard.GetState();
            oldKbState = kbState;
        }

        public virtual void draw(SpriteBatch spriteBatch, GameTime gameTime) { }
        public void onDraw(SpriteBatch spriteBatch, GameTime gameTime) {
            draw(spriteBatch, gameTime);
            foreach (Button b in buttons)
                b.onDraw(spriteBatch, gameTime);
        }


        public virtual void onBackPressed() { game.changeLayoutBack(); }

        public void addButton(Rectangle bounds, Texture2D texture, Rectangle? texBounds, Func<bool> callback) { buttons.Add(new Button(bounds, texture, texBounds, callback)); }

        public class Button {
            public Rectangle b;
            Texture2D t;
            Rectangle? tb;
            Func<bool> cb;
            public bool is_active = false;

            public Button(Rectangle bounds, Texture2D texture, Rectangle? texBounds, Func<bool> callback) {
                b = bounds;
                t = texture;
                tb = texBounds;
                cb = callback;
            }
            public virtual void onClick() {
                cb();
            }
            public virtual void onUpdate() {

            }
            public virtual void onDraw(SpriteBatch spriteBatch, GameTime gameTime) {
                spriteBatch.Draw(t, b, tb, is_active ? Color.LightGray : Color.White);
            }
        }

        public class Window {

            public Rectangle b;
            Texture2D t;

            public Window(Rectangle bounds, Texture2D texture) {
                b = bounds;
                t = texture;
            }
            public virtual void onDraw(SpriteBatch spriteBatch, GameTime gameTime) {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                //Top
                spriteBatch.Draw(t, new Vector2(b.Left, b.Top), null, new Rectangle(552, 321, 29, 29), null, 0, null, Color.White);
                spriteBatch.Draw(t, new Rectangle(b.Left + 29, b.Top, b.Width - 3 * 29, 29), new Rectangle(581, 321, 1, 29), Color.White);
                spriteBatch.Draw(t, new Vector2(b.Left + b.Width - 29, b.Top), null, new Rectangle(552, 321, 29, 29), null, MathHelper.PiOver2, null, Color.White);

                //Center
                spriteBatch.Draw(t, null, new Rectangle(b.Left + 29, b.Top + 29, b.Height - 3 * 29, 29), new Rectangle(581, 321, 1, 29), new Vector2(1, 29), -MathHelper.PiOver2, null, Color.White);
                spriteBatch.Draw(DarkJetpack.baseTexture, new Rectangle(b.Left + 29, b.Top + 29, b.Width - 3 * 29, b.Height - 3 * 29), new Color(149, 163, 85));
                spriteBatch.Draw(t, null, new Rectangle(b.Left + b.Width - 29, b.Top + 29, b.Height - 3 * 29, 29), new Rectangle(581, 321, 1, 29), new Vector2(0, 0), MathHelper.PiOver2, null, Color.White);

                //Bottom
                spriteBatch.Draw(t, new Vector2(b.Left, b.Top + b.Height - 29), null, new Rectangle(552, 321, 29, 29), Vector2.Zero, -MathHelper.PiOver2, null, Color.White);
                spriteBatch.Draw(t, null, new Rectangle(b.Left + 29, b.Top + b.Height - 2 * 29, b.Width - 3 * 29, 29), new Rectangle(581, 321, 1, 29), new Vector2(1, 29), MathHelper.Pi, null, Color.White);
                spriteBatch.Draw(t, new Vector2(b.Left + b.Width - 29, b.Top + b.Height - 29), null, new Rectangle(552, 321, 29, 29), null, MathHelper.Pi, null, Color.White);

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, null, null);
            }
        }

    }
}
