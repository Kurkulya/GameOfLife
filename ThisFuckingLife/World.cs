using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ThisFuckingLife
{
    class World
    {
        static Random rnd = new Random();
        public int Height { get; private set; }
        public int Width { get; private set; }
        bool[,] world;
        int[,] neighbours;

        public World(int height, int width)
        {
            Height = height;
            Width = width;
            world = new bool[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    world[i,j] = true;                   
                }
            }
           
            Height = width;
        }

       

        private void NumOfNeighbours()
        {           
            int x, y, count;
            neighbours = new int[Height, Width];
            for (int h = 0; h < Height; h++)
            {
                for (int w = 0; w < Width; w++)
                {
                    count = 0;
                    for (int i = h - 1; i < h + 2; i++)
                    {
                        for (int j = w - 1; j < w + 2; j++)
                        {
                            x = i < 0 ? Height + i : i >= Height ? i - Height : i;
                            y = j < 0 ? Width + j : j >= Width ? j - Width : j;
                            if ((x != h || y != w) && world[x, y] == true)
                            {
                                count++;
                            }
                        }
                    }
                    neighbours[h, w] = count;
                }
            }
        }

        public void NextGeneration()
        {
            NumOfNeighbours();
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (neighbours[i, j] == 3 && world[i,j] == false) world[i, j] = true;
                    else if ((neighbours[i, j] >= 4 || neighbours[i, j] <= 1) && world[i, j] == true) world[i, j] = false;
                }
            }
            Builder bld = new Builder(world);
        }

        public bool this[int i, int j]
        {
            get
            {
                return world[i, j];
            }
            set
            {
                world[i, j] = value;
            }
        }
    }
}
