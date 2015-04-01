using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Gravity
{
    public class Player : Body
    {
        private const float RESTITUTION = 0.2f;
        private const float INV_MASS = 0.5f;
        private const float KINETIC_F = 0.3f;
        
        private const float WIDTH = 30f;
        private const float HEIGHT = 60f;
        private const float WALK_SPEED = 3.2f;
        private const float JUMP_FORCE = 6.2f;
        private const float HOLD_DISTANCE = 20f;
        private const float THROW_FORCE = 18.0f;
        private const float PULL_FORCE = 0.5f;

        public static Texture2D PlayerTexture;

        public event EventHandler Died;
        public event EventHandler Won;

        bool _jumping;
        bool _pulling;
        Body _capturedObj;

        public Player(Vector2 position) : base(new RectF(position, new Vector2(WIDTH,HEIGHT)), RESTITUTION, INV_MASS, KINETIC_F)
        {
            _jumping = true;
            _pulling = false;
        }

        public bool Pulling
        {
            get { return _pulling; }
            set { _pulling = value; }
        }

        public void MoveLeft()
        {
            if (!_pulling)
                X_Velocity = -WALK_SPEED;
        }

        public void MoveRight()
        {
            if (!_pulling)
                X_Velocity = WALK_SPEED;
        }

        public void Jump()
        {
            if (!_jumping && !_pulling)
            {
                Y_Velocity = -JUMP_FORCE;
                _jumping = true;
            }
        }

        public void Land()
        {
            _jumping = false;
        }

        public void CaptureObject(Body obj)
        {
            _capturedObj = obj;
            _pulling = true;
        }

        public Body ReleaseObject(Cursor c, bool throwObj)
        {
            if (_capturedObj == null)
                return null;

            _pulling = false;
            
            Body b = _capturedObj;
            _capturedObj = null;

            b.GravityEnabled = true;

            if (throwObj)
            {
                Vector2 direction = c.Position - b.Shape.Center;
                direction.Normalize();
                b.Velocity = direction * THROW_FORCE * b.InvMass;
            }
            else
                b.Velocity /= 4f;

            return b;
        }

        public void PullObject(Cursor c)
        {
            Body obj = _capturedObj;
            Vector2 direction = Shape.Center - obj.Shape.Center;

            float dl = direction.Length();
            if (dl < Shape.HalfWidth + obj.Shape.HalfWidth + HOLD_DISTANCE)
            {
                // While holding, cancel gravity
                obj.GravityEnabled = false;
                obj.X_Velocity = 0;
                obj.Force = Vector2.Zero;

                // Move with cursor
                if (obj.Shape.Center.X < this.Shape.Center.X && this.Shape.Center.X < c.Position.X)
                    obj.Shape.X = this.Shape.X + this.Shape.Size.X + HOLD_DISTANCE - 0.1f;
                else if (obj.Shape.Center.X > this.Shape.Center.X && this.Shape.Center.X > c.Position.X)
                    obj.Shape.X = this.Shape.X - obj.Shape.Size.X - HOLD_DISTANCE + 0.1f;

                return;
            }

            direction.Normalize();
            obj.Velocity += direction * PULL_FORCE;
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(PlayerTexture, Shape.DestinationRect, Color.White);
        }

        public void Die()
        {
            if (Died != null)
                Died(this, new EventArgs());
        }

        public void Win()
        {
            if (Won != null)
                Won(this, new EventArgs());
        }
    }
}
