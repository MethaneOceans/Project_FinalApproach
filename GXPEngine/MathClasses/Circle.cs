using static GXPEngine.Mathf;

namespace GXPEngine.MathClasses
{
	internal class Circle : GameObject
	{
		public float Radius;

		public Circle(float radius)
		{
			Radius = radius;
		}

		public float Intersect(Ray ray)
		{
			Vector2 relative = Position - ray.Origin;

			float a = Vector2.Dot(ray.Direction, ray.Direction);
			float b = Vector2.Dot(-2 * ray.Direction, relative);
			float c = Vector2.Dot(relative, relative) - Radius * Radius;
			float D = MathUtils.GetDiscriminant(a, b, c);
			if (D < 0) return -1;

			(float rootA, float rootB) = MathUtils.GetRoots(D, a, b);
			float root = Min(rootA, rootB);
			if (root < 0)
			{
				// Earliest root is invalid, check other root
				root = Max(rootA, rootB);
				if (root < 0) return -1;
			}
			return root;
		}
	}
}
