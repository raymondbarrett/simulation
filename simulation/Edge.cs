using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace simulation
{
    class Edge
    {
        Vector2 EdgeVector;
        int Index;
        Vector2 NormalVector;
        float Distance;
        Vector2 originToEdgeVector;
        public Edge(Vector2 edgevector, int index, Vector2 normal, float distance)
        {
            EdgeVector = edgevector;
            Index = index;
            NormalVector = normal;
            Distance = distance;
        }
        public Edge(Vector2 edgevector, int index, Vector2 normal, float distance, Vector2 originToEdge)
        {
            EdgeVector = edgevector;
            Index = index;
            NormalVector = normal;
            Distance = distance;
            originToEdgeVector = originToEdge;
        }
        public Edge()
        {
            EdgeVector = new Vector2();
            Index = 0;
        }
        public Vector2 toOrigin {
            get { return originToEdgeVector; }
            set { originToEdgeVector = value; }
        }
        public Vector2 vector {
            get { return EdgeVector; }
            set { EdgeVector = value; }
        }
        public Vector2 normal {
            get { return NormalVector;  }
            set { NormalVector = value; }
        }
        public float dist {
            get { return Distance; }
            set { Distance = value; }
        }
        public int index {
            get { return Index; }
            set { Index = value; }
        }
    }
}
