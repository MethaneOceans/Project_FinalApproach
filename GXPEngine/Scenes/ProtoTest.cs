using GXPEngine.Control;
using GXPEngine.Physics;
using GXPEngine.Primitives;
using System.Collections.Generic;

namespace GXPEngine.Scenes
{
	internal class ProtoTest : Scene
	{
		private List<ACollider> staticColliders;
		private List<ACollider> movingColliders;

		public override void Initialize()
		{
			base.Initialize();

			EDBox floor = new EDBox(new Vector2(width / 2f, height - 100), new Vector2(1200, 100), 0);
			AddChild(floor);

			staticColliders = new List<ACollider>()
			{
				floor.body,
			};
			movingColliders = new List<ACollider>();
		}

		public void Update()
		{
			foreach(ACollider collider in movingColliders)
			{
				collider.Velocity += new Vector2(0, 0.1f);
				collider.Step(staticColliders);
			}

			if (Input.GetMouseButton(0))
			{
				EDBox newBox = new EDBox(Input.mousePos, new Vector2(50, 50), 0.1f);
				AddChild(newBox);
				movingColliders.Add(newBox.body);
			}
		}
	}
}
