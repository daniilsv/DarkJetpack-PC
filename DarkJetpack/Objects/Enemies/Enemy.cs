using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace DarkJetpack {
    public class Enemy {
        protected DarkJetpack game;
        public Enemy(DarkJetpack _game) {
            game = _game;
            onLoad();
        }
        public virtual void onLoad() { }
        public virtual void onUnLoad() { }
        public virtual void update(GameTime gameTime) { }
        public void onUpdate(GameTime gameTime) {
            update(gameTime);
        }

        public virtual void draw(SpriteBatch spriteBatch, GameTime gameTime) { }
        public void onDraw(SpriteBatch spriteBatch, GameTime gameTime) {
            draw(spriteBatch, gameTime);
        }
    }
}
