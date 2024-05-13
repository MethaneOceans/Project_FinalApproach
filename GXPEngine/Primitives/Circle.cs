using GXPEngine.Physics;
using System.Drawing;

namespace GXPEngine.Primitives
{
	internal class Circle : GameObject
	{
		public CircleCollider body;

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

		public Circle(Vector2 position, float radius)
		{
			body = new CircleCollider(position, radius);

			Position = position;
		}
	}
}
