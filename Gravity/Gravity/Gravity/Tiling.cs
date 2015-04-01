using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravity
{
    public class Tiling
    {
        private const float TILE_SIZE = 35f;

        public static float ToPixels(float tiles)
        {
            return (int)(tiles * TILE_SIZE);
        }

        public static float ToTiles(float pixels)
        {
            return pixels / TILE_SIZE;
        }
    }
}
