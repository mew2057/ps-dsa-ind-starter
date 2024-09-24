using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ScrollingExample
{


    /// <summary>
    /// Stores the appearance and collision behavior of a tile.
    /// </summary>
    class Tile
    {
        public Texture2D Texture;
        public Rectangle Rect;

        public const int Width = 80;
        public const int Height = 80;

        public static readonly Vector2 Size = new Vector2(Width, Height);

        /// <summary>
        /// Constructs a new tile.
        /// </summary>
        public Tile(Texture2D texture, Vector2 offset)
        {
            Texture = texture;
            Rect = new Rectangle((int)offset.X, (int)offset.Y, Width, Height);
        }
    }
    
}
