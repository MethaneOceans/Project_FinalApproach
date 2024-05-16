using System.Collections.Generic;
using GXPEngine.GameElements;

namespace GXPEngine.Scenes
{
	internal class TestLevel : Level
	{
		public override void Initialize()
		{
			base.Initialize();

			List<ALevelObject> levelObjects = new List<ALevelObject>()
			{
				new Player(new Vector2(100, height - height / 3)),

				new Block(new Vector2(width / 2, height - 100), new Vector2(1200, 200), 0),
				
				new Mirror(new Vector2(width / 2, 100), 90),
				
				new Goal(new Vector2(width - 100, height - height / 3)),
			};

			Initialize(levelObjects);
		}
	}
}

