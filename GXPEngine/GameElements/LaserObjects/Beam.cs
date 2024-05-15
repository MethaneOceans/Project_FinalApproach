using GXPEngine.GameElements;
using GXPEngine.Physics;
using System;
using System.Collections.Generic;

namespace GXPEngine
{
	internal class Beam
	{
		Ray Ray;
		public readonly List<(PhysicsObject obj, Vector2 p, float t, Ray ray)> Path;
		private List<PhysicsObject> objects;

		
		public Beam(Ray ray, List<Mirror> mirrors, List<Block> blocks, List<Prism> prisms, int maxBounces)
		{
			Ray = ray;
			Path = new List<(PhysicsObject obj, Vector2 p, float t, Ray ray)>();
			
			objects = new List<PhysicsObject>();
			mirrors.ForEach(a => objects.Add(a));
			blocks.ForEach(a => objects.Add(a));
			prisms.ForEach(a => objects.Add(a));

			int bounces = 0;
			Ray currentRay = Ray;
			while (bounces <= maxBounces)
			{
				bounces++;

				(PhysicsObject obj, float t) = Cast(currentRay);

				if (t == float.NegativeInfinity || t == -1 || t > 2000)
				{
					// Ray never hits
					t = 2000;
					bounces = maxBounces + 1;
				}

				Vector2 point = currentRay.At(t);

				Path.Add((obj, point, t, currentRay));

				if (obj is Prism || obj is Block || t == 2000) return;


				Vector2 normal = obj.body.NormalAt(point);

				Vector2 q = Vector2.Dot(currentRay.Direction, normal) * normal;
				Vector2 reflected = currentRay.Direction - (2 * q);

				currentRay = new Ray(point + 1.1f * normal, reflected);
			}
		}

		private (PhysicsObject obj, float t) Cast(Ray ray)
		{
			float closest_t = float.PositiveInfinity;
			PhysicsObject closest_obj = null;

			foreach (PhysicsObject obj in objects)
			{
				float t = obj.body.RayCast(ray);
				if (t < closest_t && t >= 0)
				{
					closest_t = t;
					closest_obj = obj;
				}
			}

			return (closest_obj, closest_t);
		}

		public string StringPath()
		{
			string result = string.Empty;

			for (int i = 0; i < Path.Count; i++)
			{
				(PhysicsObject obj, Vector2 point, float t, Ray ray) = Path[i];
				result += $"Bounce #{i}\n";
				result += $"Ray cast:\n";
				result += $"\tOrigin: {ray.Origin}\n";
				result += $"\tDirection: {ray.Direction}\n";
				result += $"Hit after t: {t} \n";
				result += $"Hit at {point}\n\n";
			}

			return result;
		}
	}
}
