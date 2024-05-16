using GXPEngine.Physics;

namespace GXPEngine.GameElements
{
	internal class Goal : ALevelObject
	{
		public Goal(Vector2 position) : base("Textures/triangle.png")
		{
			body = new OBCollider(position, new Vector2(sprite.width, sprite.height), 0, this);
			Position = position;
		}
	}
}
