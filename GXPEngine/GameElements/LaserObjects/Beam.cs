﻿using GXPEngine.GameElements;
using GXPEngine.Physics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GXPEngine
{
	internal class Beam : GameObject
	{
		private Ray _ray;
		private List<ALevelObject> objects;

		public int MaxBounces;
		public int MaxDepth;

		private readonly Level _level;

		private LaserSprite[] laserSprites;

		public Beam(int maxBounces, int maxDepth, Level level)
		{
			MaxBounces = maxBounces;
			MaxDepth = maxDepth;
			_level = level;

			laserSprites = new LaserSprite[maxDepth * maxBounces];
			for (int i = 0; i < laserSprites.Length; i++)
			{
				laserSprites[i] = new LaserSprite()
				{
					visible = false,
				};
				AddChild(laserSprites[i]);
			}
		}

		private (ALevelObject obj, float t) Cast(Ray ray)
		{
			float closest_t = float.PositiveInfinity;
			ALevelObject closest_obj = null;

			foreach (ALevelObject obj in objects)
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

		public void RecalcPath(Ray ray)
		{
			_ray = ray;

			List<(Vector2 start, float angle, float t)> path = CalcPath(ray, 0, new List<Prism>());

			for (int i = 0; i < laserSprites.Length; i++)
			{
				if (i < path.Count)
				{
					var a = path[i];

					laserSprites[i].visible = true;
					laserSprites[i].Position = a.start;
					laserSprites[i].Rotation = a.angle;
					laserSprites[i].width = (int)a.t;
				}
				else if (laserSprites[i].visible) laserSprites[i].visible = false;
				else break;
			}
		}
		private List<(Vector2 start, float angle, float t)> CalcPath(Ray ray, int depth, List<Prism> prismsHit)
		{
			int bounces = 0;
			List<(Vector2 start, float angle, float t)> path = new List<(Vector2 start, float angle, float t)>();

			Ray currentRay = ray;

			while (bounces < MaxBounces)
			{
				bounces++;

				// Get new raycast results
				var (obj, t) = Cast(currentRay);
				// Add results to path list
				path.Add((currentRay.Origin, currentRay.Direction.Degrees, t));

				// Check what object the ray intersects with and continue as appropriate
				if (obj is Prism prism && !prismsHit.Contains(prism))
				{
					if (!prismsHit.Contains(prism))
					{
						prismsHit.Add(prism);

						// Send out a new ray for each normal of the prism body
						foreach (var normal in (prism.body as OBCollider).Normals)
						{
							OBCollider pBody = (prism.body as OBCollider);

							Vector2 subOrigin = pBody.Position + normal * (pBody.Size.x / 2);
							Ray subRay = new Ray(subOrigin, normal);

							List<(Vector2 start, float angle, float t)> subPath = CalcPath(subRay, depth + 1, prismsHit);
							subPath.ForEach(a => path.Add(a));
						}
					}

					break;
				}
				else if (obj is Mirror mirror)
				{
					Vector2 point = ray.At(t);
					Vector2 normal = mirror.body.NormalAt(point);

					Vector2 q = Vector2.Dot(ray.Direction, normal) * normal;
					Vector2 newDirection = currentRay.Direction - 2 * q;
					Vector2 newPoint = point + 0.001f * normal;
					currentRay = new Ray(newPoint, newDirection);
				}
				else break;
			}

			return path;
		}
	}
}
