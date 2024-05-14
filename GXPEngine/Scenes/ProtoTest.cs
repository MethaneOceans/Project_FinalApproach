using GXPEngine.Control;
using GXPEngine.Physics;
using GXPEngine.Primitives;
using System.Collections.Generic;

namespace GXPEngine.Scenes
{
	internal class ProtoTest : Scene
	{
		private List<PhysicsObject> objects;
		private List<ACollider> bodies;

		private Vector2 playerPosition;

		public override void Initialize()
		{
			base.Initialize();

			playerPosition = new Vector2(100, height - height / 3);

			objects = new List<PhysicsObject>()
			{
				new EDBox(new Vector2(width / 2f, height - 100), new Vector2(1200, 100), 0)
			};
			bodies = new List<ACollider>();


			foreach (PhysicsObject obj in objects)
			{
				AddChild(obj);
				bodies.Add(obj.body);
			}
		}

		public void Update()
		{
			foreach(ACollider collider in bodies)
			{
				collider.Step(bodies);
			}

			if (Input.GetMouseButtonDown(0))
			{
				Vector2 aimAt = Input.mousePos;
				Vector2 initVelocity = (aimAt - playerPosition) / 30;

				Prism prism = new Prism(playerPosition, initVelocity, 5000);

				AddChild(prism);
				bodies.Add(prism.body);
			}
			if (Input.GetMouseButtonDown(1))
			{
				Ray r = new Ray(playerPosition, (Input.mousePos - playerPosition));
				Beam beam = new Beam(r, objects, new List<PhysicsObject>(), 5);
			}
		}
	}
}
