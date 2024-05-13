using GXPEngine.Control;
using GXPEngine.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GXPEngine.Scenes
{
	// NOTE: PHYSICS BRANCH - This is a test for oriented boxes
	internal class RayTest : Scene
	{
		// Debug properties
		private bool showCorners = false;
		private bool showColInfo = false;
		private bool enableResolve = true;

		private EasyDraw debugLayer;

		private EDBox BoxA;
		private Ray ray;

		public override void Initialize()
		{
			// Base is called because it removes the child objects so there will be no duplicates
			base.Initialize();

			debugLayer = new EasyDraw(width, height);

			//Vector2 size = new Vector2(50, 50);
			ray = Ray.Horizontal;
			ray.Origin = new Vector2(100, height / 2f);

			BoxA = new EDBox(new Vector2(width / 2f, height / 2f), new Vector2(200, 200), 0);

			AddChild(BoxA);
			AddChild(debugLayer);
		}

		public void Update()
		{
			debugLayer.ClearTransparent();
			debugLayer.Line(ray.Origin.x, ray.Origin.y, ray.At(1000).x, ray.At(1000).y);
			HandleInput();

			Vector2 mouse = new Vector2(Input.mouseX, Input.mouseY);
			ray.Direction = (mouse - ray.Origin).Normalized();

			//BoxA.Rotation -= 1;
			float t = BoxA.body.RayCast(ray);
			if (t > 0)
			{
				BoxA.ED.SetColor(1, 0, 0);

				// Draw hit point
				Vector2 p = ray.At(t);
				debugLayer.NoStroke();
				debugLayer.Fill(Color.Purple);
				debugLayer.Ellipse(p.x, p.y, 10, 10);

				// Draw normal
				Vector2 n = BoxA.body.NormalAt(p);
				Vector2 a = p;
				Vector2 b = p + 100 * n;
				debugLayer.Stroke(Color.Green);
				debugLayer.Line(a.x, a.y, b.x, b.y);
			}
			else
			{
				BoxA.ED.SetColor(0, 0, 1);
			}

			UpdateDebug();
		}

		private void HandleInput()
		{
			if (Input.GetKey(Key.E)) BoxA.Rotation += 1;
			if (Input.GetKey(Key.Q)) BoxA.Rotation -= 1;
			if (Input.GetKeyDown(Key.F1)) showCorners = !showCorners;
			if (Input.GetKeyDown(Key.F2)) showColInfo = !showColInfo;
			if (Input.GetKeyDown(Key.F3)) enableResolve = !enableResolve;
		}

		// Updates the debug easydraw object
		private void UpdateDebug()
		{

			debugLayer.Stroke(Color.Yellow);
			

			// Shows the collision info
			//if (showColInfo && false)
			//{

			//	debugLayer.StrokeWeight(2);
			//	debugLayer.Stroke(Color.Green);
			//	debugLayer.Line(a.x, a.y, b.x, b.y);
			//}

			// Enables collision resolving
			//if (enableResolve)
			//{
			//	var colInfo = BoxC.rigidCollider.LastCollision;
			//	BoxC.Position += colInfo.Normal * colInfo.PenetrationDepth;
			//}

			// Show the corners of boxes
			if (showCorners)
			{
				List<EDBox> boxes = new List<EDBox>() { BoxA };

				debugLayer.NoStroke();
				debugLayer.Fill(Color.Green);
				foreach (EDBox box in boxes)
				{
					box.body.DrawCorners(debugLayer);
				}

				debugLayer.Stroke(Color.Purple);
				foreach (EDBox box in boxes)
				{
					box.body.DrawNormals(debugLayer);
				}
			}
		}
	}
}
