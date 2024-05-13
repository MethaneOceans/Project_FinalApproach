using GXPEngine.Control;
using GXPEngine.Physics;
using GXPEngine.Primitives;
using System.Collections.Generic;
using System.Drawing;

namespace GXPEngine.Scenes
{
	// NOTE: PHYSICS BRANCH - This is a test for oriented boxes
	internal class CircleTest : Scene
	{
		// Debug properties
		private bool showCorners = false;
		private bool showColInfo = false;
		private bool enableResolve = true;

		private EasyDraw debugLayer;

		//private EDBox BoxA;
		private EDCircle CircleA;
		private EDCircle CircleB;
		private EDCircle CircleC;

		public override void Initialize()
		{
			// Base is called because it removes the child objects so there will be no duplicates
			base.Initialize();

			debugLayer = new EasyDraw(width, height);

			CircleC = new EDCircle(new Vector2(width / 2f, height / 2f), 100);
			//BoxA = new EDBox(new Vector2(width / 2f, height / 2f), new Vector2(200, 200), 0);
			CircleA = new EDCircle(new Vector2(), 25);
			CircleB = new EDCircle(CircleA.Position, 25);
			CircleB.body.IsStatic = false;

			//AddChild(BoxA);
			AddChild(CircleA);
			AddChild(CircleB);
			AddChild(CircleC);
			AddChild(debugLayer);
		}

		public void Update()
		{
			HandleInput();

			//BoxA.Rotation -= 1;
			
			CircleA.Position = new Vector2(Input.mouseX, Input.mouseY);

			CircleB.Rotation = CircleA.Rotation;
			CircleB.Position = CircleA.Position;
			CircleB.body.Velocity = Vector2.GetUnitVectorDeg(CircleA.Rotation) * 200;

			CircleB.body.Step(new ACollider[] { CircleC.body });

			UpdateDebug();
		}

		private void HandleInput()
		{
			if (Input.GetKey(Key.E)) CircleA.Rotation += 1;
			if (Input.GetKey(Key.Q)) CircleA.Rotation -= 1;
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
				var colInfo = CircleB.body.LastCollision;

				Vector2 a = CircleB.Position;
				Vector2 b = CircleB.Position + colInfo.Normal * colInfo.Depth;

				debugLayer.StrokeWeight(2);
				debugLayer.Stroke(Color.Green);
				debugLayer.Line(a.x, a.y, b.x, b.y);
			}

			// Enables collision resolving
			//if (enableResolve)
			//{
			//	var colInfo = CircleB.body.LastCollision;
			//	CircleB.Position += colInfo.Normal * colInfo.Depth;
			//}

			// Show the corners of boxes
		}
	}
}
