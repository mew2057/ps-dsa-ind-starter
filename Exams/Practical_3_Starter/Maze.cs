using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// DO NOT MODIFY ANYTHING IN THIS FILE 
// EXCEPT WHERE MARKED WITH TODO COMMENTS
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
namespace Practical_3_Starter
{
    // The possible Maze Tile values ============
    enum MazeTile
    {
        Empty,
        Wall,
        Start,
        End
    }

    class Maze
    {
        #region Fields for drawing
        // Constants ============================

        // Drawing-related constants
        const int MAZE_UNIT_SIZE = 10;

        // Fields ===========================

        // Maze colors
        private Color MAZE_COLOR_EMPTY = Color.White;
        private Color MAZE_COLOR_WALL = Color.Black;
        private Color MAZE_COLOR_START = Color.Lime;
        private Color MAZE_COLOR_END = Color.Red;
        private Color MAZE_COLOR_PATH = Color.Purple;
        private Color[] MAZE_COLORS;

        // The maze sizes
        private int mazeSizeX;
        private int mazeSizeY;

        // A 1x1 white texture (basically a single pixel) for drawing
        private Texture2D pixel;

        // The maze offsets (using for placing the maze correctly in the window)
        int xOffset;
        int yOffset;
        #endregion

        // The maze Vertices
        private Vertex[,] vertices;
        private Vertex startVertex;
        private Vertex endVertex;


        #region Constructor and setup
        // Constructor ==========================
        /// <summary>
        /// Creates a new maze object
        /// </summary>
        /// <param name="device">The graphics device for the game</param>
        public Maze(GraphicsDevice device)
        {
            // Create the 1x1 white pixel texture
            pixel = new Texture2D(device, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            // Set up the color array
            MAZE_COLORS = new Color[4];
            MAZE_COLORS[(int)MazeTile.Empty] = MAZE_COLOR_EMPTY;
            MAZE_COLORS[(int)MazeTile.Wall] = MAZE_COLOR_WALL;
            MAZE_COLORS[(int)MazeTile.Start] = MAZE_COLOR_START;
            MAZE_COLORS[(int)MazeTile.End] = MAZE_COLOR_END;
        }

        // Setup Methods ========================

        /// <summary>
        /// Sets up the maze data
        /// </summary>
        /// <param name="mazeTexture">The texture to get the maze from</param>
        public void SetMaze(Texture2D mazeTexture, int originX, int originY, int screenWidth, int screenHeight)
        {
            // Get the maze sizes
            mazeSizeX = mazeTexture.Width;
            mazeSizeY = mazeTexture.Height;

            // Offsets (for centering)
            xOffset = originX + (screenWidth - MAZE_UNIT_SIZE * mazeSizeX) / 2;
            yOffset = originY + (screenHeight - MAZE_UNIT_SIZE * mazeSizeY) / 2;

            // Get the data from the maze texture
            Color[] textureData = new Color[mazeTexture.Width * mazeTexture.Height];
            mazeTexture.GetData<Color>(textureData);

            // Set up the Vertices
            vertices = new Vertex[mazeSizeX, mazeSizeY];
            for (int y = 0; y < mazeSizeY; y++)
            {
                for (int x = 0; x < mazeSizeX; x++)
                {
                    // Set up the data to represent an empty space
                    MazeTile currentData = MazeTile.Empty;

                    // Get the current color
                    Color currentColor = textureData[y * mazeSizeX + x];

                    // Check for various colors
                    if (currentColor == MAZE_COLOR_WALL) currentData = MazeTile.Wall;
                    else if (currentColor == MAZE_COLOR_START) currentData = MazeTile.Start;
                    else if (currentColor == MAZE_COLOR_END)
                        currentData = MazeTile.End;

                    // Set up this Vertex and check for start/end
                    vertices[x, y] = new Vertex(x, y, currentData);
                    if (currentColor == MAZE_COLOR_START) startVertex = vertices[x, y];
                    else if (currentColor == MAZE_COLOR_END)
                        endVertex = vertices[x, y];

                }
            }
        }
        #endregion


        #region Drawing
        // Draw =================================

        /// <summary>
        /// Draws the maze on the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the maze itself
            DrawMaze(spriteBatch);

            // Reset all Vertices and get the starting Vertex
            ResetAllVertices();

            // Solve the maze and draw the solution
            DrawSolution(spriteBatch, SolveMaze());
        }

        /// <summary>
        /// Draws the walls, start and end locations of the map
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawMaze(SpriteBatch spriteBatch)
        {
            // The current color to draw
            Color currentColor = Color.White;

            // The rectangle to use for drawing
            Rectangle rect = new Rectangle();
            rect.Width = MAZE_UNIT_SIZE;
            rect.Height = MAZE_UNIT_SIZE;

            // Loop and draw
            for (int x = 0; x < mazeSizeX; x++)
            {
                for (int y = 0; y < mazeSizeY; y++)
                {
                    // Set up the rectangle
                    rect.X = x * MAZE_UNIT_SIZE + xOffset;
                    rect.Y = y * MAZE_UNIT_SIZE + yOffset;

                    // Draw
                    spriteBatch.Draw(pixel, rect, MAZE_COLORS[(int)vertices[x, y].Data]);
                }
            }
        }

        /// <summary>
        /// Draw the solution path
        /// </summary>
        /// <param name="spriteBatch">Used for drawing</param>
        /// <param name="solution">The list that represents the solution</param>
        public void DrawSolution(SpriteBatch spriteBatch, List<Vertex> solution)
        {
            // Set up the rectangle
            Rectangle rect = new Rectangle();
            rect.Width = MAZE_UNIT_SIZE;
            rect.Height = MAZE_UNIT_SIZE;

            // Loop until we're out of Vertices
            foreach (Vertex currentVertex in solution)
            {
                // Check the data
                if (currentVertex.Data == MazeTile.Empty)
                {
                    // Set up this rectangle
                    rect.X = currentVertex.X * MAZE_UNIT_SIZE + xOffset;
                    rect.Y = currentVertex.Y * MAZE_UNIT_SIZE + yOffset;

                    // Draw
                    spriteBatch.Draw(pixel, rect, MAZE_COLOR_PATH);
                }
            }
        }
        #endregion

        /// <summary>
        /// Sets all Vertices to "not visited" and return the start Vertex
        /// This is called for you and will never need to be called again!
        /// </summary>
        public void ResetAllVertices()
        {
            for (int x = 0; x < mazeSizeX; x++)
                for (int y = 0; y < mazeSizeY; y++)
                {
                    // Reset the Vertex
                    vertices[x, y].Visited = false;
                }
        }


        /// <summary>
        /// Checks a vertex to see if it is explorable
        ///    - It determines if the vertex at [x,y] is a "valid" tile to search.
        ///    - Valid means the indices are within the graph and it is not yet visited.
        ///	   - It does NOT check adjacency - you'll need to do that yourself
        ///	      in either SolveMaze() or a helper method.
        /// </summary>
        /// <param name="x">The x value of the vertex to check</param>
        /// <param name="y">The y value of the vertex to check</param>
        /// <returns>True if the vertex is valid, false otherwise</returns>
        public Boolean IsTileValid(int x, int y)
        {
            // Valid indices?
            if (y < 0 || x < 0 ||
                y >= vertices.GetLength(0) ||
                x >= vertices.GetLength(1))
            {
                return false;
            }

            // Return true if this is an open space and not yet visited
            return (vertices[x, y].Data != MazeTile.Wall && !vertices[x, y].Visited);
        }

        // Student Method *****************************************************

        /// <summary>
        /// Complete the SolveMaze method below:
        /// - It must return a list of vertices on the path to the exit.
        /// 
        /// This is NOT Dijkstra's algorithm - Just a normal graph traversal.
        /// 
        /// Some other useful information/tips:
        /// * Feel free to write any helper methods you may need
        /// 
        /// * The vertices are stored in a 2D array: vertices[x, y]
        ///    - This is NOT an adjacency matrix.
        ///    - In fact, there is no adjacency matrix.
        ///    - But determining adjacency on a grid should be quite simple.
        ///    
        /// * Assume you can NOT move diagonally
        /// 
        /// * The maze already knows about the start and end vertices, 
        ///   which are stored in the following fields:
        ///    - startVertex
        ///    - endVertex
        ///    
        /// * The end condition is finding the "endVertex" during the search.
        ///   Once you come across it, end the search and use the vertices
        ///    currently in your data structure as the path.
        /// 
        /// * A helper method has been created for you - IsTileValid(x, y)
        ///    - It determines if the tile at [x,y] is a "valid" tile to search.
        ///    - Valid means the indices are within the graph, the tile is 
        ///       open (not a wall) and it is not yet visited.
        ///	   - It does NOT check adjacency - you'll need to do that yourself
        ///	      in either SolveMaze() or a helper method.
        /// 
        /// </summary>
        /// 
        public List<Vertex> SolveMaze()
        {
            // Use either Depth-First Search or Breadth-First Search
            //    - One of them is more appropriate for this task.
            //    - Which one are you using?  


            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // TODO: LEAVE A COMMENT with either DFS or BFS and WHY you chose it
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // TODO: Uncomment & use the MOST APPROPRIATE structure below for the search
            // List<Vertex> list = new List<Vertex>();
            // Queue<Vertex> queue = new Queue<Vertex>();
            // Stack<Vertex> stack = new Stack<Vertex>();
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // TODO: IMPLEMENT the graph search here (make sure to reference the general pseudo-code given in the write-up!)
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            // List to hold the final path
            List<Vertex> path = new List<Vertex>();

            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // TODO: Add verts found during the search to the "path" List created for you.
            // - You can use the path list's AddRange() method to make this easier,
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            // All done! This should now be returning the full "path"
            return path;
        }


        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // TODO: You can add helper methods below this point.
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    }
}
