using GXPEngine.Control;
using GXPEngine.Physics;
using GXPEngine.Primitives;
using System.Collections.Generic;

namespace GXPEngine.Scenes
{
	internal class ProtoTest : Scene
	{
		private List<EasyDraw> EDs;
		private List<ACollider> staticColliders;

		public override void Initialize()
		{
			base.Initialize();

			EDBox floor = new EDBox(new Vector2(width / 2, height / 2), new Vector2(500, 100), 0);
			EDs = new List<EasyDraw>()
			{
				floor.ED,
			};
			foreach (EasyDraw draw in EDs)
			{
				AddChild(draw);
			}

			staticColliders = new List<ACollider>()
			{
				floor.body,
			};
		}
	}
}
