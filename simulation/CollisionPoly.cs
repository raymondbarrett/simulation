using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace simulation
{
    class CollisionPoly
    {
        Vector2[] Vertices;

        public CollisionPoly(Vector2[] vertices)
        {
            Vertices = vertices;
        }
        
        public Vector2[] vert
        {
            get { return Vertices; }
            set { value = Vertices; }
        }

        public Vector2 supportFunction(Vector2 direction)
        {
            float currentMaxDistance = Vector2.Dot(Vertices[0], direction);
            Vector2 currentMaxDistanceVert = Vertices[0];
            for(int i=1; i < Vertices.Length; i++)
            {
                if (Vector2.Dot(Vertices[i], direction) > currentMaxDistance)
                {
                    currentMaxDistance = Vector2.Dot(Vertices[i], direction);
                    currentMaxDistanceVert = Vertices[i];
                }
            }
            return currentMaxDistanceVert;
        }
        public Vector2 center()
        {
            Vector2 centerpoint = new Vector2();
            foreach (Vector2 vertex in Vertices)
            {
                centerpoint.X += vertex.X;
                centerpoint.Y += vertex.Y;
            }
            centerpoint = Vector2.Divide(centerpoint, Vertices.Length);
            return centerpoint;
        }
        //public static Boolean isCollision(CollisionPoly obj1, CollisionPoly obj2)
        //{

        //}
    }
    
}
