using GXPEngine.Control;
using GXPEngine.GameObjects;
using GXPEngine.MathClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using static GXPEngine.Mathf;

namespace GXPEngine.Scenes
{
	// TODO: Add functionality for calling method on being hit by a ray.
	// TODO: Add a moving "prism" object
	// TODO: Stuff
	internal class TestLevel : Scene
	{
		EasyDraw rayLayer;
		Catapult catapult;

		LaserEmitter emitter;

		Circle circle;

		// NOTE: Temporary, this is hard to work with if a larger amount of rays is used.
		List<(Ray ray, float t)> rays;
		List<LaserEmitter> lasers;
		
		public override void Initialize()
		{
			// Base is called because it removes the child objects so there will be no duplicates
			base.Initialize();

			lasers = new List<LaserEmitter>();

			// Configure and add the layer to draw rays on
			rayLayer = new EasyDraw(width, height);
			rays = new List<(Ray, float)>();
			AddChild(rayLayer);

			// Configure and add catapult
			catapult = new Catapult()
			{
				Position = new Vector2(100, game.height - 100),
			};
			AddChild(catapult);

			emitter = new LaserEmitter()
			{
				Position = new Vector2(50, height / 2),
			};
			lasers.Add(emitter);
			AddChild(emitter);

			rays.Add((emitter.ray, float.NaN));

			//int laserCount = 50;
			//float laserSpread = 600;
			//for (int i = 0; i < laserCount; i++)
			//{
			//	float laserMin = (height - laserSpread) / 2f;
			//	float laserY = laserMin + (laserSpread / laserCount) * i;
			//	LaserEmitter emitter = new LaserEmitter()
			//	{
			//		Position = new Vector2(50, laserY),
			//	};
			//	lasers.Add(emitter);
			//	rays.Add((emitter.ray, float.NaN));
			//	AddChild(emitter);
			//}

			circle = new Circle(50)
			{
				Position = new Vector2(width / 2, height / 2),
			};
		}

		public void Update()
		{
			UpdateRayLayer();

			// Move emitter
			float angleOffset = Sin(Time.time / 1000f) * 20;
			foreach (LaserEmitter emitter in lasers)
			{
				emitter.rotation = angleOffset;
			}

			// Update ray intersections
			for (int i = 0; i < rays.Count; i++)
			{
				Ray ray = rays[i].ray;
				float t = circle.Intersect(ray);
				rays[i] = (ray, t);
			}
		}

		// Clears the ray layer and draws all rays in the rays list.
		// TODO: Replace ray with object that stores info for a raycast instead.
		private void UpdateRayLayer()
		{
			rayLayer.ClearTransparent();
			rayLayer.Stroke(Color.Red);
			rayLayer.StrokeWeight(3);
			DrawRays(rays);
		}
		private void DrawRays(ICollection<(Ray, float)> rays)
		{
			foreach ((Ray r, float t) in rays) DrawRay(r, t);
		}
		private void DrawRay(Ray ray, float t)
		{
			if (t == float.NaN || t == -1)
			{
				rayLayer.Line(ray.Origin.x, ray.Origin.y, ray.At(2000).x, ray.At(2000).y);
			}
			else if (t > 0)
			{
				rayLayer.Line(ray.Origin.x, ray.Origin.y, ray.At(t).x, ray.At(t).y);
			}
			else
			{
				Console.WriteLine("t = {0}", t);
			}
		}
	}
}
