using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThisFuckingLife
{
    class Node
    {
        public Node nw, ne, sw, se, res, nn, cc, ss, cw, ce;
        public int Level { get; private set; }
        private int size;
        public int Population { get; private set; }
        public bool IsAlive { get; private set; }

        public Node(bool alive)
        {
            Level = 0;
            IsAlive = alive;
            Population = alive ? 1 : 0;
            ne = nw = sw = se = null;
        }

        public Node(Node nw, Node ne, Node sw, Node se)
        {
            this.nw = nw;
            this.ne = ne;
            this.sw = sw;
            this.se = se;
            nn = cc = ss = cw = ce = null;
            Level = nw.Level == 0 ? 1 : 3;
            Population = nw.Population + ne.Population + sw.Population + se.Population;
            IsAlive = Population != 0;
        }

        public Node(Node[] nineNodes)
        {
            nw = nineNodes[0];
            nn = nineNodes[1];
            ne = nineNodes[2];
            cw = nineNodes[3];
            cc = nineNodes[4];
            ce = nineNodes[5];
            sw = nineNodes[6];
            ss = nineNodes[7];
            se = nineNodes[8];
            res = null;
            Level = nw.Level == 0 ? 2 : 4;
            Population = 0;
            foreach (var node in nineNodes)
                Population += node.Population;
            IsAlive = Population != 0;
        }

        private Node GetLevelOneResult()
        {
            if (Level == 3)
            {
                int ngbNW = nw.nw.Population + nw.se.Population + nw.ne.Population
                            + ne.nw.Population + ne.sw.Population + se.nw.Population
                            + sw.nw.Population + sw.ne.Population,
                    ngbNE = ne.nw.Population + ne.se.Population + ne.ne.Population
                           + se.nw.Population + se.ne.Population + se.nw.Population
                           + nw.se.Population + nw.ne.Population,
                    ngbSW = sw.nw.Population + sw.sw.Population + sw.se.Population
                           + nw.se.Population + nw.sw.Population + ne.sw.Population
                           + se.nw.Population + se.sw.Population,
                    ngbSE = se.sw.Population + se.se.Population + se.ne.Population
                           + ne.sw.Population + ne.se.Population + sw.ne.Population
                           + sw.ne.Population + nw.se.Population;
                return (new Node(
                            new Node((nw.se.IsAlive == false && ngbNW == 3) || (nw.se.IsAlive == true && (ngbNW == 3 || ngbNW == 2)) ? true : false),
                            new Node((ne.nw.IsAlive == false && ngbNE == 3) || (nw.se.IsAlive == true && (ngbNE == 3 || ngbNE == 2)) ? true : false),
                            new Node((sw.ne.IsAlive == false && ngbSW == 3) || (nw.se.IsAlive == true && (ngbSW == 3 || ngbSW == 2)) ? true : false),
                            new Node((se.nw.IsAlive == false && ngbSE == 3) || (nw.se.IsAlive == true && (ngbSE == 3 || ngbSE == 2)) ? true : false)
                    ));
            }
            else return null;
                    
        }
            

        private Node FindResultNode()
        {
            if(Level == 3)
            {
                return GetLevelOneResult();
            }
            else
            {
                Node[] resNode = new Node[9];
                resNode[0] = nw.FindResultNode();
                resNode[1] = new Node(nw.ne, ne.nw, nw.se, ne.sw).FindResultNode();
                resNode[2] = ne.FindResultNode();
                resNode[3] = new Node(nw.sw, nw.se, sw.nw, sw.ne).FindResultNode();
                resNode[4] = new Node(nw.ne, ne.nw, nw.se, ne.sw).FindResultNode();
                resNode[5] = new Node(ne.sw, ne.se, se.nw, se.ne).FindResultNode();
                resNode[6] = sw.FindResultNode();
                resNode[7] = new Node(sw.ne, se.nw, sw.se, se.sw).FindResultNode();
                resNode[8] = se.FindResultNode();
                return res = new Node(resNode);
            }
        }
    }
}
