using GXPEngine.Control;
using GXPEngine.Primitives;
using System.Collections.Generic;
using System.Drawing;

namespace GXPEngine.Scenes
{
	// NOTE: PHYSICS BRANCH - This is a test for oriented boxes
	internal class CollisionTest : Scene
	{
		// Debug properties
		private bool showCorners = false;
		private bool showColInfo = false;
		private bool enableResolve = true;

		private EasyDraw debugLayer;

		private EDBox BoxA;
		private EDBox BoxB;
		private EDBox BoxC;

		public override void Initialize()
		{
			// Base is called because it removes the child objects so there will be no duplicates
			base.Initialize();

			debugLayer = new EasyDraw(width, height);

			Vector2 size = new Vector2(50, 50);

			BoxA = new EDBox(new Vector2(width / 2f, height / 2f), new Vector2(200, 200), 0);
			BoxB = new EDBox(new Vector2(), size, 0);
			BoxC = new EDBox(BoxB.Position, BoxB.rigidCollider.Size, BoxB.Rotation);

			AddChild(BoxA);
			AddChild(BoxB);
			AddChild(BoxC);
			AddChild(debugLayer);
		}

		public void Update()
		{
			HandleInput();

			BoxA.Rotation -= 1;
			BoxB.Position = new Vector2(Input.mouseX, Input.mouseY);

			BoxC.Rotation = BoxB.Rotation;
			BoxC.Position = BoxB.Position + Vector2.GetUnitVectorDeg(BoxB.Rotation) * 200;
			
			if (BoxC.rigidCollider.Overlapping(BoxA.rigidCollider))
			{
				BoxA.ED.SetColor(1, 0, 0);
				BoxC.ED.SetColor(1, 0, 0);
			}
			else
			{
				BoxA.ED.SetColor(0, 0, 1);
				BoxC.ED.SetColor(0, 0, 1);
			}

			UpdateDebug();
		}

		private void HandleInput()
		{
			if (Input.GetKey(Key.E)) BoxB.Rotation += 1;
			if (Input.GetKey(Key.Q)) BoxB.Rotation -= 1;
			if (Input.GetKeyDown(Key.F1)) showCorners = !showCorners;
			if (Input.GetKeyDown(Key.F2)) showColInfo = !showColInfo;
			if (Input.GetKeyDown(Key.F3)) enableResolve = !enableResolve;
		}

		// Updates the debug easydraw object
		private void UpdateDebug()
		{
			debugLayer.ClearTransparent();

			// Shows the collision info
			if (showColInfo)
			{
				var colInfo = BoxC.rigidCollider.LastCollision;

				Vector2 a = BoxC.Position;
				Vector2 b = BoxC.Position + colInfo.Normal * colInfo.PenetrationDepth;

				debugLayer.StrokeWeight(2);
				debugLayer.Stroke(Color.Green);
				debugLayer.Line(a.x, a.y, b.x, b.y);
			}

			// Enables collision resolving
			if (enableResolve)
			{
				var colInfo = BoxC.rigidCollider.LastCollision;
				BoxC.Position += colInfo.Normal * colInfo.PenetrationDepth;
			}

			// Show the corners of boxes
			if (showCorners)
			{
				List<EDBox> boxes = new List<EDBox>() { BoxA, BoxB, BoxC };

				debugLayer.NoStroke();
				debugLayer.Fill(Color.Green);
				foreach (EDBox box in boxes)
				{
					box.rigidCollider.DrawCorners(debugLayer);
				}

				debugLayer.Stroke(Color.Purple);
				foreach (EDBox box in boxes)
				{
					box.rigidCollider.DrawNormals(debugLayer);
				}
			}
		}
	}
}
