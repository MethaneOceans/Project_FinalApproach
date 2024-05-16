using GXPEngine.GameElements;
using GXPEngine.Physics;

namespace GXPEngine
{
	internal class Mirror : ALevelObject
	{
		public Mirror(Vector2 position, float angle) : base("Textures/mirror.png")
		{
			body = new OBCollider(position, new Vector2(sprite.width, sprite.height), angle, this);

			Position = position;
			Rotation = angle;
		}
	}
}
