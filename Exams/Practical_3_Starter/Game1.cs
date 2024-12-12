using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Practical_3_Starter
{
    // *************************************************************************
    // DO NOT MODIFY THIS FILE EXCEPT WHERE MARKED WITH "TODO"
    // There is no need for any new fields, properties, or methods!
    // *************************************************************************

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont debugFont;
        private string meanValueTests = "";

        // The maze itself
        Maze maze;



        /// <summary>
        /// A recursive function to find the mean of array ary.
        /// 
        /// </summary>
        /// <param name="ary">The int array to find the mean of.</param>
        /// <param name="num">The length of the integer array.</param>
        /// <returns>The mean value of the array</returns>
        private float ComputeMean(int[] ary, int num)
        {
            return 0.0f; // TODO Remove and Replace with your code!
        }



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            System.Diagnostics.Debug.WriteLine("Part1 Success: " + ExamUnitTest());
            maze = new Maze(GraphicsDevice);
            this.IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            debugFont = Content.Load<SpriteFont>("DebugFont");

            maze.SetMaze(
                Content.Load<Texture2D>("maze"),
                GraphicsDevice.Viewport.Width / 2, 0, GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(debugFont, meanValueTests, new Vector2(10, 100), Color.Black);
            maze.Draw(_spriteBatch);
            _spriteBatch.End();


            base.Draw(gameTime);
        }


        /// <summary>
        /// This is a helper function so Dr. Dunham can see if you suceeded at Part 1.
        /// DO NOT TOUCH!
        /// </summary>
        /// <returns>True if you succeed!</returns>
        private bool ExamUnitTest()
        {

            int[] tst1 = { };
            int[] tst2 = { 1 };
            int[] tst3 = { 1, 2, 3, 4, 5 };
            int[] tst4 = { 7, 7, 9, 10 };
            int[] tst5 = { 1, 1 };
            int[][] tests = { tst1, tst2, tst3, tst4, tst5 };

            meanValueTests = "";
            foreach (int[] tst in tests)
            {
                meanValueTests += "ComputeMean({" + String.Join(",", tst) + "}) = " + ComputeMean(tst, tst.Length) + "\n";

            }

            return ComputeMean(tst1, tst1.Length) == 0.0 &&
            ComputeMean(tst2, tst2.Length) == 1.0f &&
            ComputeMean(tst3, tst3.Length) == 3.0f &&
            ComputeMean(tst4, tst4.Length) == 8.25f &&
            ComputeMean(tst5, tst5.Length) == 1.0f;

        }



    }
}
