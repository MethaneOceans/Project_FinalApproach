using System;

namespace GXPEngine.RayCore
{
	internal class LineSegment : IRayCollider
	{
		public Vector2 lineStart;
		public Vector2 lineEnd;

		public HitRecord RayCast(Ray ray)
		{
			//float sdSegment(in vec2 p, in vec2 a, in vec2 b)
			//{
			//	vec2 pa = p - a, ba = b - a;
			//	float h = clamp(dot(pa, ba) / dot(ba, ba), 0.0, 1.0);
			//	return length(pa - ba * h);
			//}

			throw new NotImplementedException();
		}
	}
}
