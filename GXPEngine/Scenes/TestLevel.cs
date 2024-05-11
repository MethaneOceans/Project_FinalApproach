using GXPEngine.Control;
using GXPEngine.Physics;
using GXPEngine.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using static GXPEngine.Mathf;

namespace GXPEngine.Scenes
{
	// NOTE: PHYSICS BRANCH - This is a test for oriented boxes
	internal class TestLevel : Scene
	{
		// Debug properties
		private bool showCorners = false;

		private EasyDraw debugLayer;

		private EDBox BoxA;
		private EDBox BoxB;

		public override void Initialize()
		{
			// Base is called because it removes the child objects so there will be no duplicates
			base.Initialize();

			debugLayer = new EasyDraw(width, height);
			
			BoxA = new EDBox(new Vector2(width / 2f, height / 2f), new Vector2(200, 200), 0);
			BoxB = new EDBox(new Vector2(), new Vector2(200, 200), 0);

			BoxA.ED.Clear(Color.White);
			BoxA.ED.SetColor(0, 0, 1);
			BoxB.ED.Clear(Color.White);
			BoxB.ED.SetColor(0, 0, 1);

			AddChild(BoxA);
			AddChild(BoxB);
			AddChild(debugLayer);
		}

		public void Update()
		{
			BoxB.Position = new Vector2(Input.mouseX, Input.mouseY);
			
			if (BoxA.rigidCollider.Overlapping(BoxB.rigidCollider))
			{
				BoxA.ED.SetColor(1, 0, 0);
				BoxB.ED.SetColor(1, 0, 0);
			}
			else
			{
				BoxA.ED.SetColor(0, 0, 1);
				BoxB.ED.SetColor(0, 0, 1);
			}

			if (Input.GetKey(Key.E)) BoxB.rotation += 1;
			if (Input.GetKey(Key.Q)) BoxB.rotation -= 1;

			UpdateDebug();
		}

		private void UpdateDebug()
		{
			debugLayer.ClearTransparent();

			if (showCorners)
			{
				List<EDBox> boxes = new List<EDBox>() { BoxA, BoxB };
				foreach (EDBox box in boxes)
				{
					debugLayer.Fill(Color.Green);
					debugLayer.NoStroke();
					Vector2 position = box.rigidCollider.Position;
					for (int i = 1; i < 5; i++)
					{
						Vector2 corner = position + box.rigidCollider.GetCornerOffset(i);
						debugLayer.Ellipse(corner.x, corner.y, 10, 10);
					}
				}
			}
		}
	}
}
