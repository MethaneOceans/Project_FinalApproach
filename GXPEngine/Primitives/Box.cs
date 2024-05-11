using GXPEngine.Physics;
using System.Drawing;

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
	internal class EDBox : Box
	{
		public EasyDraw ED;

		public EDBox(Vector2 position, Vector2 size, float angle) : base(position, size, angle)
		{
			ED = new EasyDraw((int)size.x, (int)size.y);
			ED.SetOrigin(ED.width / 2, ED.height / 2);
			AddChild(ED);

			ED.Position = new Vector2(0, 0);
		}
	}
}
