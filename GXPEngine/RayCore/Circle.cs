using static GXPEngine.Mathf;

namespace GXPEngine
{
	internal class Circle : GameObject, IRayCollider
	{
		public float Radius;

		public Circle(float radius)
		{
			Radius = radius;
		}

		public HitRecord RayCast(Ray ray)
		{
			HitRecord rec = new HitRecord();
			Vector2 relative = Position - ray.Origin;

			float a = Vector2.Dot(ray.Direction, ray.Direction);
			float b = Vector2.Dot(-2 * ray.Direction, relative);
			float c = Vector2.Dot(relative, relative) - Radius * Radius;
			float D = MathUtils.GetDiscriminant(a, b, c);
			if (D < 0) return rec;

			rec.Hit = true;

			// Get roots
			(float rootA, float rootB) = MathUtils.GetRoots(D, a, b);
			rec.t = Min(rootA, rootB);

			if (rec.t < 0)
			{
				// Earliest root is invalid, check other root
				rec.t = Max(rootA, rootB);
				if (rec.t < 0)
				{
					rec.Hit = false;
					return rec;
				}
			}

			rec.Normal = Normal(ray.At(rec.t));

			return rec;
		}

		private Vector2 Normal(Vector2 incoming) => (incoming - Position).Normalized();
	}
}
