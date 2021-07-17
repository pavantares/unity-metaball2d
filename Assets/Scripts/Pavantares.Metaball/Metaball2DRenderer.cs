using System;
using UnityEngine;
using static UnityEngine.Mathf;

namespace Pavantares.Metaball
{
    public class Metaball2DRenderer
    {
        private const float Resolution = 20;
        private const float CircleLength = 2 * PI;

        private readonly LineRenderer line;

        private float r1;
        private float r2;

        public Metaball2DRenderer(LineRenderer line, float r1 = 2, float r2 = 1)
        {
            this.r1 = r1;
            this.r2 = r2;
            this.line = line;
        }

        public void Render(Vector3 c1, Vector3 c2, float limit, float handleSize = 2.4f, float v = 0.5f)
        {
            var metaballData = Metaball2D.Calculate(r1, r2, c1, c2, limit, handleSize, v);

            if (metaballData.IsNull())
            {
                var circlePoints = CalculateCirclePoints(c1);
                line.positionCount = circlePoints.Length;
                line.SetPositions(circlePoints);

                return;
            }

            var arcPoints0 = CalculateArcPoints(r1, c1, metaballData.P1, metaballData.P2);
            var arcPoints1 = CalculateArcPoints(r2, c2, metaballData.P4, metaballData.P3);

            var spline0 = CalculateSpline(metaballData.P1, metaballData.H1, metaballData.H3, metaballData.P3);
            var spline1 = CalculateSpline(metaballData.P2, metaballData.H2, metaballData.H4, metaballData.P4);

            Array.Reverse(spline0);

            var length = arcPoints0.Length + arcPoints1.Length + spline0.Length + spline1.Length;
            var result = new Vector3[length];

            arcPoints0.CopyTo(result, 0);
            spline1.CopyTo(result, arcPoints0.Length);
            arcPoints1.CopyTo(result, arcPoints0.Length + spline1.Length);
            spline0.CopyTo(result, arcPoints0.Length + spline1.Length + arcPoints1.Length);

            line.positionCount = result.Length;
            line.SetPositions(result);
        }

        public void SetR1(float r1)
        {
            this.r1 = r1;
        }

        public void SetR2(float r2)
        {
            this.r2 = r2;
        }

        private Vector3[] CalculateCirclePoints(Vector3 c1)
        {
            const float ArcResolution = 4 * Resolution;

            var segmentsCount = RoundToInt(ArcResolution * r1);

            var deltaTheta = CircleLength / (segmentsCount - 1);
            var theta = 0f;

            var points = new Vector3[segmentsCount];

            for (var i = 0; i < segmentsCount; i++)
            {
                var x = r1 * Cos(theta);
                var y = r1 * Sin(theta);
                var point = c1 + new Vector3(x, y);
                points[i] = point;

                theta += deltaTheta;
            }

            return points;
        }

        private static Vector3[] CalculateSpline(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var distance = Vector3.Distance(p0, p3);
            var segmentsCount = RoundToInt(Resolution * distance);
            var points = new Vector3[segmentsCount];
            var time = 0f;
            var deltaTime = 1f / segmentsCount;

            for (var i = 0; i < segmentsCount; i++)
            {
                points[i] = CalculateCubicBezierPoint(time, p0, p1, p2, p3);

                time += deltaTime;
            }

            return points;
        }

        private static Vector3[] CalculateArcPoints(float r, Vector3 c, Vector3 p0, Vector3 p1)
        {
            const float ArcResolution = 4 * Resolution;

            var segmentsCount = RoundToInt(ArcResolution * r);
            var startAngle = GetAngle(c, p0);
            var endAngle = GetAngle(c, p1);

            var arcLength = endAngle - startAngle;
            arcLength = arcLength < 0 ? arcLength + CircleLength : arcLength;

            var deltaTheta = arcLength / segmentsCount;
            var theta = startAngle;

            var points = new Vector3[segmentsCount];

            for (var i = 0; i < segmentsCount; i++)
            {
                var x = r * Cos(theta);
                var y = r * Sin(theta);
                points[i] = c + new Vector3(x, y);

                theta += deltaTheta;
            }

            return points;
        }

        private static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var u = 1 - t;
            var tt = t * t;
            var uu = u * u;
            var uuu = uu * u;
            var ttt = tt * t;

            var p = uuu * p0;
            p += 3 * uu * t * p1;
            p += 3 * u * tt * p2;
            p += ttt * p3;

            return p;
        }

        private static float GetAngle(Vector3 c1, Vector3 c2)
        {
            return Atan2(c2.y - c1.y, c2.x - c1.x);
        }
    }
}