using GXPEngine.Physics;

namespace GXPEngine.Primitives
{
	internal class Box : GameObject
	{
		public OBCollider body;

		public new Vector2 Position
		{
			get => base.Position;
			set
			{
				base.Position = value;
				body.Position = value;
			}
		}
		public new float Rotation
		{
			get => base.Rotation;
			set
			{
				base.Rotation = value;
				body.Angle = value;
			}
		}

		public Box(Vector2 position, Vector2 size, float angle)
		{
			body = new OBCollider(position, size, angle);

			Position = position;
		}
	}
}
