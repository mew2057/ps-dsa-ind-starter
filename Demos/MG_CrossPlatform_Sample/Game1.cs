using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MG_CrossPlatform_Sample
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Constants
        private const int WindowHeight = 600;
        private const int WindowWidth = 800;

        // Variables to help when drawing
        private Rectangle scaledDuckyLoc;

        // Assets
        private SpriteFont defaultFont;
        private Texture2D duckyTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            defaultFont = Content.Load<SpriteFont>("defaultFont");
            duckyTexture = Content.Load<Texture2D>("ducky");
            scaledDuckyLoc = new Rectangle(WindowWidth/2, WindowHeight/2, duckyTexture.Width/2, duckyTexture.Height/2);

            _graphics.PreferredBackBufferWidth = WindowWidth;  // set this value to the desired width
            _graphics.PreferredBackBufferHeight = WindowHeight;   // set this value to the desired height
            _graphics.ApplyChanges();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.PaleGoldenrod);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.DrawString(defaultFont, "Hello GDAPS2!", new Vector2(50, 50), Color.Black);
            _spriteBatch.Draw(duckyTexture, scaledDuckyLoc, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}