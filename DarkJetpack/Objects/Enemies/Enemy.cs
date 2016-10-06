using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace DarkJetpack {
    public class Enemy {
        protected static Random r = new Random();
        protected GameLayout game;
        protected Texture2D texture;
        public int type = 1;
        protected Vector2 position = Vector2.Zero;
        protected Vector2 speed = Vector2.Zero;
        protected Vector2 direction = Vector2.Zero;
        protected Vector2 positionDraw = Vector2.Zero;
        protected Vector2 sizeDraw = Vector2.Zero;
        protected Rectangle rectTex = Rectangle.Empty;
        public Rectangle rectCollide;
        public Rectangle rectDraw { get { return new Rectangle((int)(positionDraw.X - sizeDraw.X / 2), (int)(positionDraw.Y - sizeDraw.Y / 2), (int)sizeDraw.X / 2, (int)sizeDraw.Y / 2); } }
        public Enemy(GameLayout _game) {
            game = _game;
            onLoad();
        }
        public virtual void onLoad() { }
        public virtual void onUnLoad() { }

        public virtual void intersects(Player player) { }
        public virtual void intersects(Enemy player) { }

        public virtual void update(GameTime gameTime) { }
        public void onUpdate(GameTime gameTime) {
            rectCollide = new Rectangle(rectDraw.X + rectDraw.Width / 6, rectDraw.Y + rectDraw.Height / 6, 2 * rectDraw.Width / 3, 2 * rectDraw.Height / 3);
            update(gameTime);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += direction * speed * elapsed;
            direction = Vector2.Zero;
            positionDraw = new Vector2(game.windowBounds.X / 2 - 60 * (Layout.player.Position.X - position.X),
                game.windowBounds.Y / 2 + 60 * (Layout.player.Position.Y - position.Y));
        }

        public virtual void draw(SpriteBatch spriteBatch) { }
        public void Draw(SpriteBatch spriteBatch) {
            draw(spriteBatch);
            if (DarkJetpack.isDebug)
                spriteBatch.Draw(DarkJetpack.baseTexture, rectCollide, new Color(Color.DarkKhaki, 0.2f));
        }
    }
}
