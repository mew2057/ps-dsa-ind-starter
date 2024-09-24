using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MonoGame.Framework.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Reflection.Metadata;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingExample
{
    internal class Level
    {
        public const string TILESET_NAME = "biomes1";
        public const bool DEBUG_TILES = true;

        private List<Tile> tileset; /// The tileset, defines textures with rectangles.


        private int[,] tiles;

        public int Width
        {
            get { return tiles.GetLength(0); }
        }

        public int Height
        {
            get { return tiles.GetLength(1); }
        }



        // Level content manager.      
        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;


        private Player player; // Only managed by the level.

        private Vector2 ScreenOffset;



        public Level(IServiceProvider serviceProvider, Stream fileStream)
        {
            content = new ContentManager(serviceProvider, "Content"); ;
            player = new Player(this);

            LoadTileset();
            LoadTiles(fileStream);
        }

        /// <summary>
        /// Loads the tileset from the tileset contained in the texture at TILESET_NAME.
        /// </summary>
        private void LoadTileset() {
            tileset = new List<Tile>();

            Texture2D tile = content.Load<Texture2D>(TILESET_NAME);

            /// This makes assumptions about the layouts of the tiles, it would be better to store this kind of information in a data file.
            tileset.Add(new Tile(null, Vector2.Zero)); // Clear Tile
            tileset.Add(new Tile(tile, new Vector2(0, 240))); // Block Tile
        }

        /// <summary>
        /// Loads the appropriate tiles from the 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <exception cref="Exception"></exception>
        private void LoadTiles(Stream fileStream)
        {

            // Load the level and ensure all of the lines are the same length.
            int width;
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                        throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                    line = reader.ReadLine();
                }
            }
            tiles = new int[width, lines.Count];


            // For each line set the tile in the tiles grid.
            for (int y = 0; y < lines.Count; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    int val = 0;
                    if (!int.TryParse(lines[y][x].ToString(), out val) || val > tileset.Count) /// **Note** this only works for digits 0-9 If you have more than 10 tiles in your tileset you'll need to do something different!
                    {
                        if (val > tileset.Count)
                        {
                            val = 0;
                        }
                        else
                        {
                            // Special behavior for special characters.
                            switch (lines[y][x])
                            {
                                case 'p':
                                    player.Spawn(new Vector2(x * Tile.Width, y * Tile.Height));
                                    break;
                            }
                        }
                    }
                    tiles[x, y] = val;
                }
            }
        }

        public void DrawTiles(SpriteBatch spriteBatch)
        {
            Vector2 screen_tiles = (spriteBatch.GraphicsDevice.Viewport.Bounds.Size.ToVector2()) / Tile.Size + Vector2.One; // Add one to make sure we always have something visible. ## TODO fix the times 2 padding.
            Vector2 half_screen_tiles = screen_tiles * .5f; // Get half then floor (helper)
            half_screen_tiles.Floor();

            // Determine the tile x and y for the center of the screen.
            Vector2 center = player.ScreenPos / Tile.Size;
            center.Floor();

            

            // Find the tile the player is currently on in the world map.
            Vector2 player_tile = player.Position / Tile.Size;
            player_tile.Floor();

            Vector2 offset = player.Position - (Tile.Size * player_tile); /// Gets the remainder from the difference of the player poisition from the tile position.
            Vector2 offset_start = -Tile.Size * .5f; // Centers the tile positions.


            // Find the start tile, clamp it to zero so we don't accidentally access something out of the range.
            Vector2 start_tile = player_tile - half_screen_tiles; /// This tracks the actual tile in the tile map.
            start_tile.X = Math.Max(start_tile.X, 0);
            start_tile.Y = Math.Max(start_tile.Y, 0);

            Vector2 screen_start_tile = Vector2.Zero; // Assume we're starting on the top left tile for the visible tile range.

            // If the tiles to the left or above in the map the are less than the number of tiles from the center of the rendered tiles, we need to adjust which tiles we're rendering.
            if (player_tile.X <= center.X) 
                screen_start_tile.X = (int)(half_screen_tiles.X - player_tile.X);

            if (player_tile.Y <= center.Y)
                screen_start_tile.Y = (int)(half_screen_tiles.Y - player_tile.Y);
            
            for (int y = (int)start_tile.Y, screen_tile_y = (int)screen_start_tile.Y; // We can declare multiple variables in the for statement
                    y < Height && screen_tile_y <= screen_tiles.Y;                    // We can perform multiple boolean checks.
                    ++y, ++screen_tile_y)                                             // Increment both of them each loop.
            {
                for (int x = (int)start_tile.X, screen_tile_x = (int)screen_start_tile.X; 
                    x < Width && screen_tile_x <= screen_tiles.X; 
                    ++x, ++screen_tile_x)
                {
                    // Retrieve the tilemap representation of the tile we're on.
                    int tile_id = tiles[x, y];
                    Tile tile = tileset[tile_id];

                    if (tile.Texture !=null)
                    {
                        Vector2 position = offset_start + new Vector2(screen_tile_x, screen_tile_y) * Tile.Size - offset;

                        Color tile_color = Color.White;

                        if (DEBUG_TILES)
                            if (screen_tile_x == 0 || screen_tile_y == 0)
                                tile_color = Color.Blue;
                            else if (screen_tile_x == screen_tiles.X || screen_tile_y == screen_tiles.Y)
                                tile_color = Color.Red;

                        spriteBatch.Draw(tile.Texture, position, tile.Rect, tile_color);
                    }

                }
            }

        }

        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawTiles(spriteBatch);
            player.Draw(gameTime, spriteBatch);
        }

    }
}
