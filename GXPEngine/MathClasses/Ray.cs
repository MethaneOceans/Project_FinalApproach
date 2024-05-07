using System;

namespace GXPEngine
{
	internal class Ray
	{
		public Vector2 Origin;
		public Vector2 Direction;

		public Ray()
		{
			Origin = new Vector2();
			Direction = new Vector2();
		}
		public Ray(Vector2 origin, Vector2 direction)
		{
			Origin = origin;
			Direction = direction;
		}

		public Vector2 At(float t) => Origin + t * Direction;
	}
}
