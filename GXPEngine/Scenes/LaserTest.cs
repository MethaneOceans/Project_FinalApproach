using GXPEngine.Control;
using GXPEngine.Physics;
using GXPEngine.Primitives;
using System;
using System.Collections.Generic;

namespace GXPEngine.Scenes
{
	internal class LaserTest : Scene
	{
		List<PhysicsObject> boxes;
		EasyDraw LaserDebug;
		Ray InitialRay;

		public override void Initialize()
		{
			base.Initialize();

			boxes = new List<PhysicsObject>() {
				new EDBox(new Vector2(2 * width / 3, height / 2), new Vector2(200, 500), 0),
				new EDBox(new Vector2(width / 4, height / 3), new Vector2(200, 295), 0),
				new EDBox(new Vector2(width / 4, height - height / 3), new Vector2(200, 295), 0),
			};
			LaserDebug = new EasyDraw(width, height);
			InitialRay = new Ray(new Vector2(100, height / 2), new Vector2(1, 0));

			foreach (EDBox box in boxes) AddChild(box);
			AddChild(LaserDebug);
		}

		public void Update()
		{
			if (Input.GetKey(Key.Q)) boxes[0].Rotation += 0.5f;
			if (Input.GetKey(Key.E)) boxes[0].Rotation -= 0.5f;

			Beam beam = new Beam(InitialRay, boxes, new List<PhysicsObject>(), 5);

			if (Input.GetKeyDown(Key.SPACE))
			{
				Console.WriteLine(beam.StringPath());

				Console.WriteLine();
			}

			LaserDebug.ClearTransparent();

			Vector2 a = InitialRay.Origin;
			foreach (var v in beam.Path)
			{
				Vector2 b = v.p;
				LaserDebug.Line(a.x, a.y, b.x, b.y);
				a = b;
			}
		}
	}
}
