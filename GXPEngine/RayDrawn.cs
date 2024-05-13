using static GXPEngine.Mathf;
using System.Drawing;

namespace GXPEngine
{
	internal class RayDrawn : EasyDraw
	{
		public Vector2 Origin
		{
			get => Origin;
			set
			{
				Origin = value;
				x = value.x;
				y = value.y;
			}
		}
		public Vector2 Direction
		{
			get => Direction;
			set
			{
				Direction = value;
				Rotation = Vector2.Rad2Deg(Atan2(value.y, value.x));
			}
		}
		private readonly Ray R;
		
		public RayDrawn(Vector2 Origin, Vector2 Direction) : base(1, 1)
		{
			R = new Ray();

			width = (int)Sqrt(game.width * game.width + game.height * game.height);
			Clear(Color.Red);

			R.Origin = Origin;
			R.Direction = Direction;
		}
	}
}
