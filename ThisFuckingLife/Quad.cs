using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThisFuckingLife
{
    class Quad
    {
        public List<Quad4> bank = new List<Quad4>();
        public HashSet<Quad4> bankH = new HashSet<Quad4>();
    }

    class Quad8 : Quad
    {
        const int size = 8;
        private bool[,] quad8;
        public Quad6 Quad6 { get; private set; }
        public Quad8(bool[,] array)
        {
            quad8 = array;
            EvaluateQuad6();
        }
        public bool this[int x, int y]
        {
            get
            {
                return quad8[x, y];
            }
        }

        private void EvaluateQuad6()
        {
            List<Quad2> listQuads2 = new List<Quad2>();
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    bool[,] quad4 = new bool[4, 4];
                    int _xShift = x * 2,
                        _yShift = y * 2;
                    for (int row = 0; row < 4; row++)
                        for (int col = 0; col < 4; col++)
                            quad4[row, col] = quad8[row + _xShift, col + _yShift];
                    if (bankH.Contains(new Quad4(quad4)))
                        listQuads2.Add(bankH.Where((z) => z == new Quad4(quad4)).Select((z) => z.Quad2).ToArray().First());
                    else listQuads2.Add(new Quad4(quad4).Quad2);
                }
            }
            bool[,] quad6 = new bool[6, 6];
            int xShift = 0,
                yShift = 0;
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    if (x < 2) xShift = 0;
                    else if (x > 3) xShift = 2;
                    else xShift = 1;

                    if (y < 2) yShift = 0;
                    else if (y > 3) xShift = 6;
                    else xShift = 3;

                    int index = xShift + yShift;

                    Quad2 temp = listQuads2.ElementAt(index);
                    quad6[x, y] = temp[x % 2 == 0 ? 0 : 1, y % 2 == 0 ? 0 : 1];
                }
            }
            Quad6 = new Quad6(quad6);
         }
    }

    class Quad6 : Quad
    {
        const int size = 6;
        private bool[,] quad6;
        public Quad6(bool[,] array)
        {
            quad6 = array;
        }
        public bool this[int x, int y]
        {
            get
            {
                return quad6[x, y];
            }
        }
    }

    class Quad2 : Quad
    {
        const int size = 2;
        private bool[,] quad2;
        public Quad2(bool[,] array)
        {
            quad2 = array;
        }
        public bool this[int x, int y]
        {
            get
            {
                return quad2[x, y];
            }
        }
    }

    class Quad4 : Quad
    {
        const int size = 4;
        private bool[,] quad4;
        public Quad2 Quad2 { get; private set; }
        public Quad4(bool[,] array)
        {
            quad4 = array;
            EvaluateQuad2();
            bankH.Add(this);
        }
        public bool this[int x, int y]
        {
            get
            {
                return quad4[x, y];
            }
        }

        private void EvaluateQuad2()
        {
            bool[,] quad2 = new bool[2, 2];
            for (int row = 0; row < 2; row++)
                for (int col = 0; col < 2; col++)
                    quad2[row, col] = UseRule(row + 1, col + 1);
            Quad2 = new Quad2(quad2);
        }

        private bool UseRule(int x, int y)
        {
            int count = Convert.ToInt32(quad4[x - 1, y - 1])
                + Convert.ToInt32(quad4[x, y - 1])
                + Convert.ToInt32(quad4[x + 1, y - 1])
                + Convert.ToInt32(quad4[x + 1, y])
                + Convert.ToInt32(quad4[x + 1, y + 1])
                + Convert.ToInt32(quad4[x, y + 1])
                + Convert.ToInt32(quad4[x - 1, y + 1])
                + Convert.ToInt32(quad4[x - 1, y]);
            return (quad4[x, y] == false && count == 3) || (quad4[x, y] == true && (count == 3 || count == 2)) ? true : false;
        }
    }
}
