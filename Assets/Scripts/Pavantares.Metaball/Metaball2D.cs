using UnityEngine;
using static UnityEngine.Mathf;

namespace Pavantares.Metaball
{
    internal static class Metaball2D
    {
        private const float HalfPI = 0.5f * PI;

        public static Data Calculate(float r1, float r2, Vector2 c1, Vector2 c2, float limit, float handleSize, float v)
        {
            var d = Vector2.Distance(c1, c2);

            if (r1 == 0 || r2 == 0 || d > limit || d <= Abs(r1 - r2))
            {
                return default;
            }

            var u1 = 0f;
            var u2 = 0f;

            if (d < r1 + r2)
            {
                u1 = Acos((r1 * r1 + d * d - r2 * r2) / (2 * r1 * d));
                u2 = Acos((r2 * r2 + d * d - r1 * r1) / (2 * r2 * d));
            }

            var angleBetweenCenters = Atan2(c2.y - c1.y, c2.x - c1.x);
            var maxSpread = Acos((r1 - r2) / d);

            var a1 = angleBetweenCenters + u1 + (maxSpread - u1) * v;
            var a2 = angleBetweenCenters - u1 - (maxSpread - u1) * v;
            var a3 = angleBetweenCenters + PI - u2 - (PI - u2 - maxSpread) * v;
            var a4 = angleBetweenCenters - PI + u2 + (PI - u2 - maxSpread) * v;

            var p1 = GetPoint(c1, a1, r1);
            var p2 = GetPoint(c1, a2, r1);
            var p3 = GetPoint(c2, a3, r2);
            var p4 = GetPoint(c2, a4, r2);

            var totalRadius = r1 + r2;
            var d2Base = Min(v * handleSize, Vector2.Distance(p1, p3) / totalRadius);
            var d2 = d2Base * Min(1, 2 * d / totalRadius);

            r1 *= d2;
            r2 *= d2;

            var h1 = GetPoint(p1, a1 - HalfPI, r1);
            var h2 = GetPoint(p2, a2 + HalfPI, r1);
            var h3 = GetPoint(p3, a3 + HalfPI, r2);
            var h4 = GetPoint(p4, a4 - HalfPI, r2);

            return new Data(p1, p2, p3, p4, h1, h2, h3, h4);
        }

        private static Vector2 GetPoint(Vector2 originPoint, float angleRad, float radius)
        {
            return originPoint + AngleAsDirection(angleRad) * radius;
        }

        private static Vector2 AngleAsDirection(float angleRad)
        {
            return new Vector2(Cos(angleRad), Sin(angleRad));
        }

        public struct Data
        {
            public Vector2 P1;
            public Vector2 P2;
            public Vector2 P3;
            public Vector2 P4;
            public Vector2 H1;
            public Vector2 H2;
            public Vector2 H3;
            public Vector2 H4;

            public Data(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 h1, Vector2 h2, Vector2 h3, Vector2 h4)
            {
                P1 = p1;
                P2 = p2;
                P3 = p3;
                P4 = p4;
                H1 = h1;
                H2 = h2;
                H3 = h3;
                H4 = h4;
            }

            public bool IsNull()
            {
                return P1.sqrMagnitude == 0 && P2.sqrMagnitude == 0 && P3.sqrMagnitude == 0 && P4.sqrMagnitude == 0;
            }
        }
    }
}