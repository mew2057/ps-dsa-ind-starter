using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace ScrollingExample
{
    internal class Player
    {
        private float speed = 5.0f;
        public Vector2 Position { get { return position; } }
        private Vector2 position; // This is the world position
        private Texture2D texture;
        private Vector2 screen_pos;

        public Rectangle ScreenRect
        {
            get { return screenRect; }
        }

        private Rectangle screenRect;

        public Vector2 ScreenPos
        {
            get 
            {
                return screen_pos;
            }
        }

        public Level Level
        {
            get { return level; }
        }
        private Level level;

        public Player(Level lvl)
        {
            level = lvl;
            position = new Vector2(100, 100);
            LoadContent();
        }

        public void LoadContent()
        {
            texture = Level.Content.Load<Texture2D>("player");

        }

        public void Spawn(Vector2 spawn_pos)
        {
            position = spawn_pos;
        }


        public void Update(GameTime gameTime)
        {
            // Get the keyboard inputs.
            KeyboardState currentState = Keyboard.GetState();
            float lr_axis = (currentState.IsKeyDown(Keys.Right) ? 1.0f : 0.0f) - (currentState.IsKeyDown(Keys.Left) ? 1.0f : 0.0f) ;
            float ud_axis = (currentState.IsKeyDown(Keys.Down) ? 1.0f : 0.0f) - (currentState.IsKeyDown(Keys.Up) ? 1.0f : 0.0f) ;

            position = new Vector2(position.X + lr_axis * speed, position.Y + ud_axis * speed);
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            /// This probably doesn't need to be  called every frame!
            screen_pos = new Vector2(sb.GraphicsDevice.Viewport.Width * 0.5f - texture.Width * 0.5f, sb.GraphicsDevice.Viewport.Height * 0.5f - texture.Height * 0.5f);

            sb.Draw(texture, screen_pos, Color.White);



        }

    }
}
