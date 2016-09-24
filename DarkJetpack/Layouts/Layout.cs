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

        public Layout(DarkJetpack _game) {
            game = _game;
            buttons = new List<Button>();
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
                spriteBatch.Draw(t, b, tb, is_active ? Color.Red : Color.White);
            }
        }
    }
}
