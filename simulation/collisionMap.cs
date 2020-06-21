using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simulation
{
    class collisionMap
    {
        int[,] collisionArray;
        int squareCollision = 1;
        int _45degSlopeNW = 2;
        int _45degSlopeNE = 3;
        int _45degSlopeSW;
        int _45degSlopeSE;
        int _60degSlopeNW1 = 4;
        int _60degSlopeNW2 = 5;
        int _60degSlopeNE1 = 6;
        int _60degSlopeNE2 = 7;

        public collisionMap(int[,] collisionarray) {
            collisionArray = collisionarray;
        }
        public float sweptAABB() { }
    }
}
