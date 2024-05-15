﻿using GXPEngine.GameElements;
using GXPEngine.Physics;

namespace GXPEngine
{
	internal class Mirror : ALevelObject
	{
		public Mirror(Vector2 position, float angle) : base("mirror.png")
		{
			Position = position;
			Rotation = angle;

			body = new OBCollider(position, new Vector2(sprite.width, sprite.height), angle, this);
		}

		public override void LaserHit()
		{
			// Do nothing
		}
	}
}
