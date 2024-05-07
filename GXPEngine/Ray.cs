using System;

namespace GXPEngine
{
	internal class Ray
	{
		public Vector2 Origin;
		public Vector2 Direction;

		public Vector2 At(float t) => Origin + t * Direction;
	}
}
