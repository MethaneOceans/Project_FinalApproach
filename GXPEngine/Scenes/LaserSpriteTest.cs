using System;
using System.Drawing;
using GXPEngine.Control;
using GXPEngine.GameElements;

namespace GXPEngine.Scenes
{
	internal class LaserSpriteTest : Scene
	{
		LaserSprite lsSprite;
		EasyDraw debugLayer;

		Vector2 origin;
		public override void Initialize()
		{
			base.Initialize();

			lsSprite = new LaserSprite();
			AddChild(lsSprite);
			//lsSprite.SetOrigin(0, lsSprite.height);

			lsSprite.width = 200;
			lsSprite.Position = new Vector2(width / 2, height / 2);

			origin = new Vector2(0, 0);

			debugLayer = new EasyDraw(width, height);
			AddChild(debugLayer);
			//debugLayer.SetOrigin(width / 2, height / 2);
			debugLayer.Fill(Color.Blue);
			debugLayer.Ellipse(width / 2, height / 2, 5, 5);
		}

		public void Update()
		{
			if (Input.GetKey(Key.LEFT_SHIFT)) lsSprite.width += 10;
			if (Input.GetKey(Key.SPACE)) lsSprite.width -= 10;

			if (Input.GetKey(Key.Q)) lsSprite.Rotation--;
			if (Input.GetKey(Key.E)) lsSprite.Rotation++;
		}
	}
}
