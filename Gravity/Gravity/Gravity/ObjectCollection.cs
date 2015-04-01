using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gravity
{
    public class ObjectCollection
    {
        List<Body> _bodies;
        List<Manifold> _mans;
        List<Laser> _lasers;
        WinArea _winArea;
        Player _player;

        public ObjectCollection()
        {
            _bodies = new List<Body>();
            _mans = new List<Manifold>();
            _lasers = new List<Laser>();
            _player = null;
            _winArea = null;
        }

        public WinArea WinArea
        {
            get { return _winArea; }
            set { _winArea = value; }
        }

        public void Add(Body newBody)
        {
            if (newBody is Player)
                _player = (Player)newBody;

            // add a manifold connecting the new body to each one that already exists
            foreach (Body b in _bodies)
            {
                if (b.InvMass + newBody.InvMass > 0) // don't add manifolds for two objects with infinite mass
                {
                    _mans.Add(new Manifold(b, newBody));
                }
            }

            _bodies.Add(newBody);
            if (newBody is Laser)
                _lasers.Add((Laser)newBody);
        }

        public void Remove(Body body)
        {
            // remove all manifolds that involve this body
            for(int i = 0; i < _mans.Count; i++)
            {
                Manifold m = _mans[i];
                if (m.A == body || m.B == body)
                    _mans.Remove(m);
            }

            _bodies.Remove(body);
            if (body is Laser)
                _lasers.Remove((Laser)body);
        }

        public void AvoidCollision(Body b1, Body b2)
        {
            for (int i = 0; i < _mans.Count; i++)
            {
                Manifold m = _mans[i];
                if ((m.A == b1 && m.B == b2) || (m.A == b2 && m.B == b1))
                {
                    _mans.Remove(m);
                    return;
                }
            }
        }

        public void ReinstateCollision(Body b1, Body b2)
        {
            _mans.Add(new Manifold(b1, b2));
        }

        public void UpdateAll(GameTime gt)
        {
            // Resolve Collisions
            foreach (Manifold m in _mans)
            {
                if (Physics.DetectCollision(m))
                {
                    Physics.ResolveCollision(m);
                    if (m.A is Player && m.Normal.Y > 0) // player landed on top of something
                    {
                        Player p = (Player)m.A;
                        p.Land();
                    }
                }
            }

            // Resolve Beam Collisions
            foreach (Laser l in _lasers)
            {
                bool collision = false;
                //l.ExtendBeam();
                foreach (Body b in _bodies)
                {
                    if (!(b is Laser))
                    {
                        if (l.IsCollidingWithBeam(b))
                        {
                            l.ResolveCollisionWithBeam(b);
                            collision = true;
                        }
                    }
                }
                if (!collision)
                    l.ExtendBeam();
            }

            // Call the update functions
            foreach(Body b in _bodies)
            {
                b.Update(gt);
            }

            // Check for win condition
            if (_winArea != null)
                _winArea.CheckForWin(_player);
        }

        public void DrawAll(SpriteBatch s)
        {
            foreach(Body b in _bodies)
            {
                b.Draw(s);
            }
            if (_winArea != null)
                _winArea.Draw(s);
        }

        public Body CheckCursorOverObject(Cursor c)
        {
            foreach(Body b in _bodies)
            {
                if (b.IsPullable)
                {
                    if (b.Shape.ContainsPoint(c.Position))
                        return b;
                }
            }
            return null;
        }
    }
}
