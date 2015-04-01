using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gravity
{
    // Static Class that handles collision stuff
    public class Physics
    {
        // Gravity Constant
        public const float G = 0.4f;
        private const float CorrectionPercent = 0.8f;
        private const float CorrectionSlop = 0.1f;

        private static bool AABBvsAABB(Manifold m)
        {
            // Broadphase
            if (m.A.Shape.X > m.B.Shape.X + m.B.Shape.Width || m.A.Shape.X + m.A.Shape.Width < m.B.Shape.X)
                return false;

            RectF A = m.A.Shape;
            RectF B = m.B.Shape;

            // get overlap if it exists
            float overlapX, overlapY;
            
            // vector pointing from A to B
            Vector2 n = A.Center - B.Center;

            // get x overlap
            overlapX = A.HalfWidth + B.HalfWidth - Math.Abs(n.X);
            if (overlapX < 0)
                return false;

            // get y overlap
            overlapY = A.HalfHeight + B.HalfHeight - Math.Abs(n.Y);
            if (overlapY < 0)
                return false;

            if (overlapX < overlapY) // Horizontal Collision
            {
                m.PenatrationDepth = overlapX;

                if (n.X > 0)
                    m.Normal = new Vector2(-1f, 0f);
                else
                    m.Normal = new Vector2(1f, 0f);
            }
            else // Vertical Collision
            {
                m.PenatrationDepth = overlapY;

                if (n.Y > 0)
                    m.Normal = new Vector2(0f, -1f);
                else
                    m.Normal = new Vector2(0f, 1f);
            }

            return true;
        }

        public static bool DetectCollision(Manifold m)
        {
            // Eventually decide on which method to use (AABBvsAABB, CirclevsCircle, etc.)
            return AABBvsAABB(m);
        }

        public static void ResolveCollision(Manifold m)
        {
            // Calculate relative velocity
            Vector2 rv = m.B.Velocity - m.A.Velocity;

            // Calculate velocity along normal
            float vNorm = Vector2.Dot(rv, m.Normal);

            // do not resolve if objects are separating
            if (vNorm > 0)
                return;

            // Calculate restitution
            float e = Math.Min(m.A.Restitution, m.B.Restitution);

            // Calculate impulse scalar
            float j = (-(1.0f + e) * vNorm) / (m.A.InvMass + m.B.InvMass);

            // Apply Impulse
            Vector2 impulse = j * m.Normal;
            m.A.Velocity -= m.A.InvMass * impulse;
            m.B.Velocity += m.B.InvMass * impulse;

            // Find Tangent Vector (Friction)
            Vector2 tangent = rv - (vNorm * m.Normal);
            if (tangent == Vector2.Zero)
                return;

            tangent.Normalize();

            Vector2 frictionImpulse = tangent * ((m.A.KineticFriction + m.B.KineticFriction) / 2f);

            // Apply Impulse
            m.A.Velocity += m.A.InvMass * frictionImpulse;
            m.B.Velocity -= m.B.InvMass * frictionImpulse;

            // Apply Positional Correction
            PositionalCorrection(m);
        }

        public static void PositionalCorrection(Manifold m)
        {            
            // allow small penetration (slop) to prevent objects from "jiggling"
            if (m.PenatrationDepth < CorrectionSlop)
                return;

            Vector2 correction = ((m.PenatrationDepth - CorrectionSlop) / (m.A.InvMass + m.B.InvMass)) * CorrectionPercent * m.Normal;
            m.A.Position -= m.A.InvMass * correction;
            m.B.Position += m.B.InvMass * correction;
        }
    }
}
