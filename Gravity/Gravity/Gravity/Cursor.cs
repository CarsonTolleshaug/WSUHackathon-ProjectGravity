using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gravity
{
    public class Cursor
    {
        private const float WIDTH = 20f;
        private const float HEIGHT = 20f;
        private const float CURSOR_SPEED = 4.5f;

        public static Texture2D CursorNormalTexture;
        public static Texture2D CursorHighlightTexture;

        RectF _rect;
        bool _highlighting;
        public Cursor(float X, float Y)
        {
            _rect = new RectF(X,Y,WIDTH,HEIGHT);
            _highlighting = false;
        }

        public Vector2 Position
        {
            get { return _rect.Center; }
            set { _rect.Center = value; }
        }

        public bool Highlighting
        {
            get { return _highlighting; }
            set { _highlighting = value; }
        }

        public void Draw(SpriteBatch s)
        {
            if (_highlighting)
                s.Draw(CursorHighlightTexture, _rect.DestinationRect, Color.White);
            else
                s.Draw(CursorNormalTexture, _rect.DestinationRect, Color.White);
        }

        public void Up()
        {
            _rect.Y -= CURSOR_SPEED;
        }

        public void Down()
        {
            _rect.Y += CURSOR_SPEED;
        }

        public void Left()
        {
            _rect.X -= CURSOR_SPEED;
        }

        public void Right()
        {
            _rect.X += CURSOR_SPEED;
        }
    }
}
