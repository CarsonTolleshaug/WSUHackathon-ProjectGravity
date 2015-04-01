using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gravity
{
    public class FloorSegment : Body
    {
        private const float RESTITUTION = 0.2f;
        private const float INV_MASS = 0f;
        private const float KINETIC_F = 0.1f;

        public static Texture2D FloorTexture;

        public FloorSegment(RectF rect) : base(rect, RESTITUTION, INV_MASS, KINETIC_F)
        {
        }

        public override void Draw(SpriteBatch s)
        {
            s.Draw(FloorTexture, Shape.DestinationRect, Color.White);
        }
    }
}
