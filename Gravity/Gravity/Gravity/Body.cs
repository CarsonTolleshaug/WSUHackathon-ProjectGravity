using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gravity
{
    public class Body
    {
        RectF _shape;
        Vector2 _vel, _force;
        float _restitution, _invMass, _staticF, _kineticF;
        bool _gravityEnabled, _pullable;

        public Body(RectF shape, float restitution, float invMass, float kineticF)
        {
            _shape = shape;
            _restitution = restitution;
            _vel = _force = Vector2.Zero;
            _invMass = invMass;
            _kineticF = kineticF;
            _gravityEnabled = invMass > 0; // By default, don't apply gravity to objects with infinite mass
            _pullable = false; // By default, objects are not pullable
        }

        public RectF Shape
        {
            get { return _shape; }
        }

        public float Restitution
        {
            get { return _restitution; }
        }

        public float InvMass
        {
            get { return _invMass; }
        }

        public float KineticFriction
        {
            get { return _kineticF; }
        }

        public bool GravityEnabled
        {
            get { return _gravityEnabled; }
            set { _gravityEnabled = value; }
        }

        public bool IsPullable
        {
            get { return _pullable; }
            protected set { _pullable = value; }
        }

        public Vector2 Position
        {
            get { return _shape.Position; }
            set { _shape.Position = value; }
        }

        public float X_Velocity
        {
            get { return _vel.X; }
            set { _vel.X = value; }
        }

        public float Y_Velocity
        {
            get { return _vel.Y; }
            set { _vel.Y = value; }
        }

        public Vector2 Velocity
        {
            get { return _vel; }
            set { _vel = value; }
        }

        public float X_Force
        {
            get { return _force.X; }
            set { _force.X = value; }
        }

        public float Y_Force
        {
            get { return _force.Y; }
            set { _force.Y = value; }
        }

        public Vector2 Force
        {
            get { return _force; }
            set { _force = value; }
        }

        private void ApplyGravity()
        {
            _vel.Y += Physics.G;
        }

        public virtual void Update(GameTime t)
        {
            _vel += _force * _invMass;

            // Prevent tiny impulses from collision resolution from making objects "jitter"
            if (_vel.X > _restitution || _vel.X < -_restitution)
                _shape.X += _vel.X;
            if (_vel.Y > _restitution || _vel.Y < -_restitution)
                _shape.Y += _vel.Y;

            if (_gravityEnabled)
               ApplyGravity();
        }

        public virtual void Draw(SpriteBatch s)
        {
            // Override to enable drawing
        }
    }
}
