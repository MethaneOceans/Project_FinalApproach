using GXPEngine.Physics;

namespace GXPEngine.GameElements
{
	internal class Block : ALevelObject
	{
		public Block(Vector2 position, Vector2 size, float angle) : base("Textures/Square.png")
		{
			body = new OBCollider(position, size, 0, this);
			Rotation = angle;
			Position = position;
			sprite.width = (int)size.x;
			sprite.height = (int)size.y;
		}
	}
}
