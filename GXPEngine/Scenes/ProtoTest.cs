using GXPEngine.Control;
using GXPEngine.Physics;
using GXPEngine.Primitives;
using System.Collections.Generic;

namespace GXPEngine.Scenes
{
	internal class ProtoTest : Scene
	{
		private List<ACollider> colliders;

		public override void Initialize()
		{
			base.Initialize();

			EDBox floor = new EDBox(new Vector2(width / 2f, height - 100), new Vector2(1200, 100), 0);
			AddChild(floor);

			colliders = new List<ACollider>()
			{
				floor.body,
			};
		}

		public void Update()
		{
			foreach(ACollider collider in colliders)
			{
				collider.Step(colliders);
			}

			if (Input.GetMouseButtonDown(0))
			{
				Vector2 aimFrom = new Vector2(100, height - height / 3);
				Vector2 aimAt = Input.mousePos;
				Vector2 initVelocity = (aimAt - aimFrom) / 30;

				Prism prism = new Prism(aimFrom, initVelocity, 5000);

				AddChild(prism);
				colliders.Add(prism.body);
			}
		}
	}
}
