using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Gravity
{
    class cButton
    {
        Texture2D itemTexture;
        Vector2 position;
        Rectangle rect;

        Color color = new Color(255, 255, 255, 255);
        public Vector2 size;

        public cButton (Texture2D newTexture, GraphicsDevice graphics) 
        {
            itemTexture = newTexture;
            size = new Vector2(graphics.Viewport.Width / 4, graphics.Viewport.Height / 10);
        }

        bool down;
        public bool isClicked;

        public void Update(MouseState mouse)
        {
            rect = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            isClicked = false;
            if (mouseRectangle.Intersects(rect))
            {
                if (color.A == 255) down = false;
                if (color.A == 0) down = true;
                if (down) color.A += 3; else color.A -= 3;
                if (mouse.LeftButton == ButtonState.Pressed) isClicked = true;
            }
            else if (color.A < 255)
            {
                color.A += 3;
            }
        }

        public void SetPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(itemTexture, rect, color);
        }

    }
}
