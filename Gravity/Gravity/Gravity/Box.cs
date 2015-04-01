using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gravity
{
    public class Box : Body
    {
        private const float RESTITUTION = 0.3f;
        private const float INV_MASS = 0.5f;
        private const float KINETIC_F = 0.3f;

        public static Texture2D BoxTexture;

        public Box(RectF rect) : base(rect, RESTITUTION, INV_MASS, KINETIC_F)
        {
            IsPullable = true;
        }

        public override void Draw(SpriteBatch s)
        {
            s.Draw(BoxTexture, Shape.DestinationRect, Color.White);
        }
    }
}
