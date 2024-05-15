using GXPEngine.Physics;
using System;


namespace GXPEngine.GameElements
{
	internal class Player : ALevelObject
	{
		public Player(Vector2 position) : base("colors.png")
		{
			body = new OBCollider(position, new Vector2(sprite.width, sprite.height), 0, this);
			Position = position;
		}
	}
}
