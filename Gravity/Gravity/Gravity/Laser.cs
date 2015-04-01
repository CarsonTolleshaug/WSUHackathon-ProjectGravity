using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gravity
{
    /// <summary>
    /// Describes which direction a laser will point. 
    /// Each value is associated with a character to make parsing XML easier.
    /// </summary>
    public enum LaserDirection {UP = 'u', DOWN = 'd', LEFT = 'l', RIGHT = 'r'}

    /// <summary>
    /// A Laser object will have a base at a specific location and point in a specified
    /// direction. The laser "base" refers to the square object that the laser is emitted 
    /// from and is an infinite-mass body.The laser beam will continue from the base in 
    /// the specified direction until it collides with an object (that is not another
    /// laser beam). 
    /// </summary>
    public class Laser : Body
    {
        private const float RESTITUTION = 0.2f;
        private const float INV_MASS = 0f;
        private const float KINETIC_F = 0.4f;

        private const float HEIGHT = 35f;
        private const float WIDTH = 35f;
        private const float MAX_LENGTH = 800f;
        private const float BEAM_WIDTH = 5f;
        private const float BEAM_OFFSET = 15f;

        public static Texture2D LaserBaseTexture;
        public static Texture2D LaserBeamTexture;

        private readonly LaserDirection _direct;
        private RectF _beamRect;
        private float _rotation;
        private Rectangle _beamDestRect;
        private Rectangle _baseDestRect;

        public Laser(Vector2 position, LaserDirection direction) : base(new RectF(position, new Vector2(WIDTH,HEIGHT)), RESTITUTION, INV_MASS, KINETIC_F)
        {
            //Account for centering
            Shape.Position += Shape.HalfSize;
            _baseDestRect = Shape.DestinationRect; _baseDestRect.X++; _baseDestRect.Y++;

            _direct = direction;
            _beamDestRect = new Rectangle();
            switch (direction)
            {
                case LaserDirection.RIGHT:
                    _rotation = (3 * (float)Math.PI) / 2f;
                    _beamRect = new RectF(Shape.Center.X, position.Y + BEAM_OFFSET, MAX_LENGTH, BEAM_WIDTH);
                    break;
                case LaserDirection.LEFT:
                    _rotation = (float)Math.PI / 2f;
                    _beamRect = new RectF(Shape.Center.X - MAX_LENGTH, position.Y + BEAM_OFFSET, MAX_LENGTH, BEAM_WIDTH);
                    break;
                case LaserDirection.UP:
                    _rotation = (float)Math.PI;
                    _beamRect = new RectF(position.X + BEAM_OFFSET, Shape.Center.Y - MAX_LENGTH, BEAM_WIDTH, MAX_LENGTH);
                    break;
                case LaserDirection.DOWN:
                    _rotation = 0.0f;
                    _beamRect = new RectF(position.X + BEAM_OFFSET, Shape.Center.Y, BEAM_WIDTH, MAX_LENGTH);
                    break;
            }

            Shape.Position -= Shape.HalfSize;
        }

        public bool IsCollidingWithBeam(Body b)
        {
            if (b.Shape.X > _beamRect.X + _beamRect.Size.X || b.Shape.X + b.Shape.Size.X < _beamRect.X)
                return false;
            if (b.Shape.Y > _beamRect.Y + _beamRect.Size.Y || b.Shape.Y + b.Shape.Size.Y < _beamRect.Y)
                return false;
            return true;
        }

        public void ResolveCollisionWithBeam(Body b)
        {
            if (b is Player)
            {
                Player p = (Player)b;
                p.Die();
                return;
            }

            switch(_direct)
            {
                case LaserDirection.UP:
                    _beamRect.Y = b.Shape.Center.Y;
                    _beamRect.Height = Shape.Y - _beamRect.Y;
                    break;
                case LaserDirection.DOWN:
                    _beamRect.Height = b.Position.Y - Shape.Y - Shape.HalfHeight;
                    break;
                case LaserDirection.LEFT:
                    _beamRect.X = b.Shape.Center.X;
                    _beamRect.Width = Shape.X - _beamRect.X;
                    break;
                case LaserDirection.RIGHT:
                    _beamRect.Width = b.Position.X - Shape.X - Shape.HalfWidth;
                    break;
            }
        }

        public void ExtendBeam()
        {
            switch (_direct)
            {
                case LaserDirection.UP:
                    _beamRect.Y = Shape.Center.Y - MAX_LENGTH;
                    _beamRect.Height = MAX_LENGTH;
                    break;
                case LaserDirection.DOWN:
                    _beamRect.Height = MAX_LENGTH;
                    break;
                case LaserDirection.LEFT:
                    _beamRect.X = Shape.Center.X - MAX_LENGTH;
                    _beamRect.Width = MAX_LENGTH;
                    break;
                case LaserDirection.RIGHT:
                    _beamRect.Width = MAX_LENGTH;
                    break;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            _beamDestRect.X = (int)Shape.Center.X;
            _beamDestRect.Y = (int)Shape.Center.Y;
            _beamDestRect.Width = (int)BEAM_WIDTH;
            if (_direct == LaserDirection.RIGHT || _direct == LaserDirection.LEFT)
                _beamDestRect.Height = (int)_beamRect.Width;
            else
                _beamDestRect.Height = (int)_beamRect.Height;

            s.Draw(LaserBeamTexture, _beamDestRect, null, Color.White, _rotation, new Vector2(2, 0), SpriteEffects.None, 0);
            s.Draw(LaserBaseTexture, _baseDestRect, null, Color.White, _rotation, new Vector2(5,5), SpriteEffects.None, 0);
        }
    }
}
