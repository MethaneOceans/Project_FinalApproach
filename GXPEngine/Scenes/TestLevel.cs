﻿using System;
using System.Collections.Generic;
using GXPEngine.Control;
using GXPEngine.GameElements;
using GXPEngine.Physics;
using GXPEngine.Primitives;

namespace GXPEngine.Scenes
{
	internal class TestLevel : Level
	{
		public override void Initialize()
		{
			base.Initialize();

			Prism CenterPrism = new Prism(new Vector2(width / 2, height / 2), new Vector2(), 0)
			{
				Rotation = 15,
			};
			CenterPrism.OnLaserHit += (sender, args) =>
			{
				CenterPrism.body.ShouldRemove = true;
			};

			List<ALevelObject> levelObjects = new List<ALevelObject>()
			{
				new Player(new Vector2(100, height - height / 3)),

				//new Block(new Vector2(width / 2, height - 200), new Vector2(1200, 200), 0),
				new Mirror(new Vector2(width / 2, 100), 90),

				CenterPrism,

				new Goal(new Vector2(width - 100, height - height / 3)),
			};

			Initialize(levelObjects);
		}
	}
}

