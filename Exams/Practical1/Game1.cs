using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace Practical1
{

    enum State
    {
        Title,
        Edit, 
        Done
    }
    public class Game1 : Game
    {
        private bool regular = true;
        const float SPRITE_SCALE = .5f;
        const double PAW_DRAW_TIME = .4;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D pawSprite;
        private Texture2D catSprite;
        private SpriteFont font;

        private Vector2 catStartPosition;

        private Rectangle catRect;
        private Rectangle bounds;

        private Vector2 catVelocity = new Vector2(-1,1);
        private int catSpeed = 2;

        private List<Vector2> pawPositions;
        private int leadPaw = 0;
        private int maxPaws = 5;

        private State state = State.Title;
  
        private KeyboardState prevKB;
        private double timer = 0.0;
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            base.Initialize();
            prevKB = Keyboard.GetState();
            pawPositions = new List<Vector2>();
            leadPaw = -1;

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Assets
            pawSprite = Content.Load<Texture2D>("pawprint");
            catSprite = Content.Load<Texture2D>("cat");
            font = Content.Load<SpriteFont>("File");

            //Compute Cat Rectangle.
            catRect = new Rectangle(0,0, (int) (catSprite.Width * SPRITE_SCALE), (int)(catSprite.Height * SPRITE_SCALE));
            catRect.X = _graphics.PreferredBackBufferWidth - catRect.Width;

            catStartPosition.X  = catRect.X;
            catStartPosition.Y = 0;

            bounds = _graphics.GraphicsDevice.Viewport.Bounds;
            bounds.Width = bounds.Width - catRect.Width +1;
            bounds.Height = bounds.Height - catRect.Height +1;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState kbSate = Keyboard.GetState();

            // Get wether or not the space bar was pressed.
            bool spacePressed = kbSate.IsKeyUp(Keys.Space) && prevKB.IsKeyDown(Keys.Space);


            switch (state)
            {
                case State.Title:
                    if (spacePressed)
                        state = State.Edit;
                    break;
                case State.Edit:
                    if (spacePressed)
                    {
                        state = State.Done;
                        break;
                    }

                    // React to the mouse being pressed.
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        // EXTRA
                        if (catRect.X < 0 || catRect.X > bounds.Width)
                            catVelocity.X *= -1;

                        if (catRect.Y < 0 || catRect.Y > bounds.Height)
                            catVelocity.Y *= -1;

                        catRect.X += catSpeed * (int)catVelocity.X;
                        catRect.Y += catSpeed * (int)catVelocity.Y;

                    }

                    // Special input for demo!
                    if (kbSate.IsKeyUp(Keys.Tab) && prevKB.IsKeyDown(Keys.Tab))
                    {
                        regular = !regular;
                        // Convert the regular paw pattern to the trailing one.
                        if (regular)
                        {
                            pawPositions.Add(pawPositions[leadPaw]);
                            pawPositions.RemoveAt(leadPaw);

                        }
                        else
                        {
                            List<Vector2> tempPaws = pawPositions;
                            pawPositions = new List<Vector2>(); 
                            for (int i = MathHelper.Max(tempPaws.Count - maxPaws, 0); i < tempPaws.Count; ++i)
                            {
                                pawPositions.Add(tempPaws[i]);
                            }
                            leadPaw = pawPositions.Count - 1;
                        }
                    }
                    
                    timer += gameTime.ElapsedGameTime.TotalSeconds;

                    // Update the paw print positions as needed.
                    if (regular) // Normal Mode.
                    {
                        if (timer > PAW_DRAW_TIME)
                        {
                            pawPositions.Add(new Vector2(catRect.X, catRect.Y));

                            if (pawPositions.Count > maxPaws)
                            {
                                pawPositions.RemoveAt(0);
                            }


                            timer = 0.0; // Reset the timer.
                        }
                    }
                    else // Extra Credit Mode.
                    {
                        if (timer > PAW_DRAW_TIME && 
                            (pawPositions.Count == 0 || // Always draw the first paw
                            catRect.X != (int)pawPositions[leadPaw].X)) // Only draw a new paw if we've moved.
                        {
                            // If we're less than the maximum number of paws.
                            if (pawPositions.Count < maxPaws)
                            {
                                pawPositions.Add(new Vector2(catRect.X + catRect.Width, catRect.Y));
                                leadPaw++;
                            }
                            else // Otherwise, shift the lead paw.
                            {
                                leadPaw = ((leadPaw + 1) % maxPaws);
                                pawPositions[leadPaw] = new Vector2(catRect.X + catRect.Width, catRect.Y);
                            }

                            timer = 0.0; // Reset the timer.
                        }
                    }

                    break;
                case State.Done:
                    // Reset the state of the cat and paw prints.
                    if (spacePressed)
                    {
                        state = State.Title;
                        pawPositions.Clear();
                        catRect.X = (int)catStartPosition.X;
                        catRect.Y = (int)catStartPosition.Y;

                        if (!regular) // Extra Credit
                        {
                            leadPaw = -1;
                        }
                    }

                    break;

            }

            prevKB = kbSate;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Draw the cat.
            _spriteBatch.Draw(catSprite, catRect, Color.White);

            switch (state)
            {
                case State.Title:
                    _spriteBatch.DrawString(font, "John Dunham; Section-01", Vector2.Zero, Color.White);
                    break;
                case State.Edit:
                    // Print Number of paws and draw them.
                    _spriteBatch.DrawString(font, "Number of Paws: " + pawPositions.Count, Vector2.Zero, Color.White);


                    if (pawPositions.Count > 0)
                    {
                        if (regular) // Normal Mode.
                        {
                            for (int i = 0; i < pawPositions.Count - 1; ++i)
                            {
                                _spriteBatch.Draw(pawSprite, pawPositions[i], Color.Gray);
                            }
                            _spriteBatch.Draw(pawSprite, pawPositions[pawPositions.Count - 1], Color.Black);
                        }
                        else // Extra Credit mode.
                        {

                            for (int i = 1; i < pawPositions.Count; ++i)
                            {
                                _spriteBatch.Draw(pawSprite, pawPositions[(leadPaw + i) % pawPositions.Count], Color.Gray);
                            }
                            _spriteBatch.Draw(pawSprite, pawPositions[leadPaw], Color.Black);
                        }
                    }

                    break;
                case State.Done:
                    // Print the final count, and draw black.
                    _spriteBatch.DrawString(font, "Final Paw Count: " + pawPositions.Count, Vector2.Zero, Color.White);
                    for (int i = 0; i < pawPositions.Count; ++i)
                    {
                        _spriteBatch.Draw(pawSprite, pawPositions[i], Color.Black);
                    }
                    break;

            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
