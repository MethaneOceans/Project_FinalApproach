using GXPEngine.Physics;

namespace GXPEngine.Primitives
{
	internal class Box : GameObject
	{
		public OBCollider rigidCollider;

		public new Vector2 Position
		{
			get => base.Position;
			set
			{
				base.Position = value;
				rigidCollider.Position = value;
			}
		}
		public new float Rotation
		{
			get => base.Rotation;
			set
			{
				base.Rotation = value;
				rigidCollider.Angle = value;
			}
		}

		public Box(Vector2 position, Vector2 size, float angle)
		{
			rigidCollider = new OBCollider(position, size, angle);

			Position = position;
		}
	}
}
