using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace DarkJetpack {
    public class DarkJetpack : Game {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Texture2D baseTexture;
        Layout curLayout;
        public Point mousePos;
        public Color backColor;
        Stack<Layout> layoutBackStack;
        protected Song[] song = new Song[15];
        private int nextSong = 0;
        public Texture2D Terrain;
        public Texture2D Explosion;
        public Texture2D Explosion1;
        public static bool isDebug = false;
        public static SpriteFont baseFont;
        public DarkJetpack() : base() {
            IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            layoutBackStack = new Stack<Layout>();
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Window.Position = new Point(0, 0);
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }
        protected override void LoadContent() {
            if (!(File.Exists("Save.djp")))
                SaveGameStorage.SaveData(0);
            spriteBatch = new SpriteBatch(GraphicsDevice);


            baseTexture = new Texture2D(GraphicsDevice, 1, 1);
            baseTexture.SetData(new Color[] { Color.White });
            Terrain = Content.Load<Texture2D>(@"Terrain");
            Explosion = Content.Load<Texture2D>(@"Explosion1");
            Explosion1 = Content.Load<Texture2D>(@"Explosion2");
            #region LoadMusic
            song[0] = Content.Load<Song>(@"music/01. Epsilon");
            song[1] = Content.Load<Song>(@"music/02. Game Not Over");
            song[2] = Content.Load<Song>(@"music/03. Dead Space");
            song[3] = Content.Load<Song>(@"music/04. Mounting a Nightmare");
            song[4] = Content.Load<Song>(@"music/05. Reach the Galaxy");
            song[5] = Content.Load<Song>(@"music/06. Sky Pony");
            song[6] = Content.Load<Song>(@"music/07. Hard Drive");
            song[7] = Content.Load<Song>(@"music/08. Jinx");
            song[8] = Content.Load<Song>(@"music/09. Between Death And Stars");
            song[9] = Content.Load<Song>(@"music/10. Nobody Will Escape ");
            song[10] = Content.Load<Song>(@"music/11. Pen In My Hand");
            song[11] = Content.Load<Song>(@"music/12. Road To Heaven (feat. Matty M.)");
            song[12] = Content.Load<Song>(@"music/13. Optimistic (Instrumental)");
            song[13] = Content.Load<Song>(@"music/14. Rain Of That Day (Elzevir Cover)");
            song[14] = Content.Load<Song>(@"music/15. Dark Energy (Instrumental)");
            #endregion
            changeLayoutTo(new MainMenuLayout(this));

            backColor = new Color(76, 220, 241);
            baseFont = Content.Load<SpriteFont>(@"ScoreFont");
        }
        protected override void UnloadContent() {
            curLayout.onUnLoad();
            Content.Unload();
        }

        protected override void Update(GameTime gameTime) {
            curLayout.onUpdate(gameTime, GraphicsDevice.Viewport);
            #region Music
            if (MediaPlayer.State != MediaState.Playing) {
                if (nextSong == 0) {
                    Random rnd = new Random();
                    MediaPlayer.Play(song[rnd.Next(14)]);
                } else {
                    MediaPlayer.Play(song[nextSong]);
                    nextSong = 0;
                }
            }
            #endregion
            base.Update(gameTime);
        }

        public static void DrawLine(SpriteBatch sb, Color color, Vector2 start, Vector2 end) {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);
            sb.Draw(baseTexture, new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), 1), null, color, angle, new Vector2(0, 0), SpriteEffects.None, 0);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(backColor);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);

            curLayout.onDraw(spriteBatch, gameTime);

            if (isDebug) {
                DrawLine(spriteBatch, Color.BlueViolet, new Vector2(0, GraphicsDevice.Viewport.Height / 2), new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height / 2));
                DrawLine(spriteBatch, Color.YellowGreen, new Vector2(GraphicsDevice.Viewport.Width / 2, 0), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height));
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void changeSong(int number, bool stopCurrent = false, bool withFade = false) {
            if (stopCurrent) {
                if (withFade) {
                    while (MediaPlayer.Volume > .1f) {
                        MediaPlayer.Volume -= .0001f;
                    }
                    MediaPlayer.Stop();
                    MediaPlayer.Volume = 1f;
                    MediaPlayer.Play(song[number - 1]);
                }
                MediaPlayer.Stop();
                MediaPlayer.Play(song[number - 1]);
            } else {
                nextSong = number;
            }
        }

        public bool exitGame() {
            SaveGameStorage.SaveData(Layout.player.highscore);
            this.Exit();
            return true;
        }

        public bool changeLayoutTo(Layout layoutToChange) {
            if (layoutToChange == null)
                return false;
            if (curLayout != null) {
                layoutBackStack.Push(curLayout);
                curLayout.onPause();
            }
            curLayout = layoutToChange;
            if (curLayout != null) {
                curLayout.onLoad();
                curLayout.onResume();
            }
            return true;
        }
        public bool changeLayoutBack() {
            if (curLayout != null) {
                curLayout.onPause();
                curLayout.onUnLoad();
            }
            curLayout = layoutBackStack.Pop();
            if (curLayout != null)
                curLayout.onResume();
            return true;
        }
    }
}