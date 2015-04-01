using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gravity
{
    public class RectF
    {
        Vector2 _pos, _size, _hSize;
        Rectangle _destRect;

        public RectF(Vector2 position, Vector2 size)
        {
            _pos = position;
            _size = size;

            //Calculate half size at intansiation
            _hSize = new Vector2(size.X * 0.5f, size.Y * 0.5f);
            _destRect = new Rectangle();
        }
        public RectF(float X, float Y, float W, float H) : this(new Vector2(X, Y), new Vector2(W, H)) { }


        public float X
        {
            get { return _pos.X; }
            set { _pos.X = value; }
        }

        public float Y
        {
            get { return _pos.Y; }
            set { _pos.Y = value; }
        }

        public Vector2 Position
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public Vector2 Center
        {
            get { return _pos + _hSize; }
            set { _pos = value - _hSize; }
        }

        public float HalfWidth
        {
            get { return _hSize.X; }
        }

        public float HalfHeight
        {
            get { return _hSize.Y; }
        }

        public Vector2 HalfSize
        {
            get { return _hSize; }
        }

        public Vector2 Size
        {
            get { return _size; }
            set
            {
                _size = value;
                _hSize.X = _size.X * 0.5f;
                _hSize.Y = _size.Y * 0.5f;
            }
        }

        public float Height
        {
            get { return _size.Y; }
            set
            {
                _size.Y = value;
                _hSize.Y = value * 0.5f;
            }
        }

        public float Width
        {
            get { return _size.X; }
            set
            {
                _size.X = value;
                _hSize.X = value * 0.5f;
            }
        }

        public Rectangle DestinationRect
        {
            get
            {
                _destRect.X = (int)_pos.X;
                _destRect.Y = (int)_pos.Y;
                _destRect.Width = (int)_size.X;
                _destRect.Height = (int)_size.Y;
                return _destRect;
            }
        }

        public bool ContainsPoint(Vector2 p)
        {
            if (p.X < X || p.X > X + _size.X)
                return false;
            if (p.Y < Y || p.Y > Y + _size.Y)
                return false;
            return true;
        }
    }

    // Basic class for storing collision data
    public class Manifold
    {
        Body _a, _b;


        public Manifold(Body a, Body b)
        {
            A = a;
            B = b;
        }

        public Body A
        {
            get { return _a; }
            private set { _a = value; }
        }

        public Body B
        {
            get { return _b; }
            private set { _b = value; }
        }

        public Vector2 Normal
        {
            get;
            set;
        }

        public float PenatrationDepth
        {
            get;
            set;
        }
    }
}
