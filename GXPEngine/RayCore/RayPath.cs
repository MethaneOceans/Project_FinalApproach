using System.Collections.Generic;

namespace GXPEngine
{
	internal class RayPath
	{
		private const int maxBounces = 100000;
		public List<Vector2> Points;

		public RayPath(Ray ray, IRayCollider collider)
		{
			Points = new List<Vector2>
			{
				ray.Origin
			};
			Trace(ray, collider, 0);
		}
		private void Trace(Ray ray, IRayCollider collider, int bounces)
		{
			// Trace path
			HitRecord rec = collider.RayCast(ray);
			if (rec.Hit && rec.t > 0 && bounces < maxBounces)
			{
				Points.Add(ray.At(rec.t));

				// TODO: Create a new reflected, scattered or refracted ray and trace its path.
				Vector2 reflectDirection;
				Vector2 normal = rec.Normal;
				Vector2 q = Vector2.Dot(ray.Direction, normal) * normal;
				reflectDirection = (ray.Direction - 2 * q).Normalized();

				Ray newRay = new Ray(ray.At(rec.t) + 0.001f * rec.Normal, reflectDirection);
				Trace(newRay, collider, bounces + 1);
			}
			else
			{
				Points.Add(ray.At(2000));
			}
		}
	}
}
