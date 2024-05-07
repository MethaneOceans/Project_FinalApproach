using GXPEngine.Control;
using System.Collections.Generic;
using System.Drawing;
using static GXPEngine.Mathf;

namespace GXPEngine.Scenes
{
	internal class TestLevel : Scene
	{
		EasyDraw rayLayer;
		Catapult catapult;

		// NOTE: Temporary, this is hard to work with if a larger amount of rays is used.
		List<Ray> rays;
		
		public override void Initialize()
		{
			// Base is called because it removes the child objects so there will be no duplicates
			base.Initialize();

			// Configure and add the layer to draw rays on
			rayLayer = new EasyDraw(width, height);
			rays = new List<Ray>();
			AddChild(rayLayer);

			// Configure and add catapult
			catapult = new Catapult()
			{
				Position = new Vector2(100, game.height - 100),
			};
			AddChild(catapult);

			rays.Add(catapult.aim);
		}

		public void Update()
		{
			UpdateRayLayer();	
		}

		private void UpdateRayLayer()
		{
			rayLayer.ClearTransparent();
			rayLayer.Stroke(Color.Red);
			rayLayer.StrokeWeight(3);
			DrawRays(rays);
		}
		private void DrawRays(ICollection<Ray> rays)
		{
			foreach (Ray r in rays) DrawRay(r);
		}
		private void DrawRay(Ray ray)
		{
			rayLayer.Line(ray.Origin.x, ray.Origin.y, ray.Origin.x + 2000 * ray.Direction.x, ray.Origin.y + 2000 * ray.Direction.y);
		}
	}

	internal class Catapult : GameObject
	{
		private EasyDraw body;
		private EasyDraw barrel;
		public readonly Ray aim;

		public Catapult()
		{
			body = new EasyDraw(75, 75);
			body.SetOrigin(body.width / 2, body.height / 2);
			body.Clear(Color.Gray);
			AddChild(body);

			barrel = new EasyDraw(75, 40);
			barrel.SetOrigin(barrel.height / 2, barrel.height / 2);
			barrel.Clear(Color.DarkGray);
			AddChild(barrel);

			aim = new Ray(Position, Vector2.GetUnitVectorDeg(barrel.rotation));
		}

		public void Update()
		{
			barrel.rotation = Vector2.Rad2Deg(Atan2(Input.mouseY - y, Input.mouseX - x));
			aim.Origin = Position;
			aim.Direction = Vector2.GetUnitVectorDeg(barrel.rotation);
		}
	}
}
