using System;
using System.Collections.Generic;
using GXPEngine.Control;
using GXPEngine.Physics;
using GXPEngine.Primitives;

namespace GXPEngine.Scenes
{
	internal class TriggerTest : Scene
	{
		PhysicsManager PhysManager;
		List<PhysicsObject> Objects;

		public override void Initialize()
		{
			base.Initialize();

			PhysManager = new PhysicsManager();

			Objects = new List<PhysicsObject>()
			{
				new EDBox(new Vector2())
			};
		}
	}
}
