using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThisFuckingLife
{
    class Builder
    {
        List<Quad8> worldList;
        
        public Builder(bool[,] world)
        {
            worldList = new List<Quad8>();
            int size = (int)Math.Sqrt(world.Length);
            int step = size / 6;
            int newRow, newCol;
            for (int x = 0; x < step; x++)
            {
                for (int y = 0; y < step; y++)
                {
                    int xShift = x * 6 - 1,
                        yShift = y * 6 - 1;
                    bool[,] temp = new bool[8, 8];
                    for (int row = 0; row < 8; row++)
                    {
                        for (int column = 0; column < 8; column++)
                        {
                            newRow = (row + xShift == -1) ? size - 1 : (row + xShift == size) ? 0 : row;
                            newCol = (column + yShift == -1) ? size - 1 : (column + yShift == size) ? 0 : column;
                            temp[row, column] = world[newRow, newCol];
                        }
                    }
                    worldList.Add(new Quad8(temp));
                }
            }
        }
    }
}
