using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gravity
{
    public class WinArea
    {
        public static Texture2D WinTexture;

        private Rectangle _rect;
        public WinArea(Rectangle area)
        {
            _rect = area;
        }

        public void CheckForWin(Player p)
        {
            if (p != null)
            {
                if (_rect.Contains(p.Shape.DestinationRect))
                {
                    p.Win();
                }
            }
        }

        public void Draw(SpriteBatch s)
        {
            s.Draw(WinTexture, _rect, Color.White);
        }
    }
}
