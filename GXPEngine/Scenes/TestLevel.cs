using GXPEngine.Control;
using GXPEngine.RayCore;
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

		IRayCollider rayCollider;

		List<Ray> rays;
		List<LaserEmitter> emitters;
		
		public override void Initialize()
		{
			// Base is called because it removes the child objects so there will be no duplicates
			base.Initialize();

			// Configure and add the layer to draw rays on
			rayLayer = new EasyDraw(width, height);
			rays = new List<Ray>();
			AddChild(rayLayer);

			emitters = new List<LaserEmitter>()
			{
				new LaserEmitter(new Vector2(100, height / 2), new Vector2(1, 0)),
			};
			//float spacing = 0.1f;
			//int numLasers = 1000;
			//for (int i = 0; i < numLasers; i++)
			//{
			//	float y = height / 2f - (i - numLasers / 2) * spacing;
			//	emitters.Add(new LaserEmitter(new Vector2(100, y), new Vector2(1, 0)));
			//}
			foreach (LaserEmitter emitter in emitters)
			{
				AddChild(emitter);
				rays.Add(emitter.ray);
			}

			// Configure and add catapult
			catapult = new Catapult()
			{
				Position = new Vector2(100, game.height - 100),
			};
			AddChild(catapult);

			Circle a = new Circle(50)
			{
				Position = new Vector2(width / 2, height / 2 + 75),
			};
			Circle b = new Circle(50)
			{
				Position = new Vector2(width / 2, height / 2 - 75),
			};
			Circle c = new Circle(50)
			{
				Position = new Vector2(width / 2 + 125, height / 2),
			};
			RayColliderList list = new RayColliderList();
			list.Colliders.Add(a);
			list.Colliders.Add(b);
			list.Colliders.Add(c);
			rayCollider = list;
		}

		public void Update()
		{
			// TEST: Rotate the laser to test reflection
			foreach (LaserEmitter laser in emitters)
			{
				laser.rotation = Sin(Time.time / 10000f) * 4f;
			}

			// Get ray paths
			List<RayPath> paths = new List<RayPath>();
			foreach (Ray ray in rays)
			{
				paths.Add(new RayPath(ray, rayCollider));
			}

			rayLayer.ClearTransparent();
			rayLayer.NoFill();
			rayLayer.Stroke(Color.Red);
			rayLayer.StrokeWeight(1);
			foreach (RayPath path in paths)
			{
				List<Vector2> points = path.Points;
				for (int i = 0; i < points.Count - 1; i++)
				{
					Vector2 a = points[i];
					Vector2 b = points[i + 1];

					rayLayer.Line(a.x, a.y, b.x, b.y);
				}
			}
		}

		// Clears the ray layer and draws all rays in the rays list.
		// TODO: Replace ray with object that stores info for a raycast instead.
	}
}
