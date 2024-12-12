using System;


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// DO NOT EDIT ANYTHING IN THIS FILE!
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
namespace Practical_3_Starter
{
    /// <summary>
    /// Data container to store basic information for a vertex in the graph.
    /// </summary>
    class Vertex
    {
        // Fields
        private int x;
        private int y;
        private MazeTile data;
        private bool visited;
        private Vertex neighborPath;

        // Properties
        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public MazeTile Data { get { return data; } set { data = value; } }
        public bool Visited { get { return visited; } set { visited = value; } }
        public Vertex? PathNeighbor { get; set; }

        // Constructor
        public Vertex(int x, int y, MazeTile data)
        {
            this.x = x;
            this.y = y;
            this.data = data;
            this.visited = false;
            this.neighborPath = null;
        }
    }
}
