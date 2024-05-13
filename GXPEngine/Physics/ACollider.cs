namespace GXPEngine.Physics
{
	internal abstract class ACollider
	{
		public Vector2 Position;
		public float Angle;
		public CollisionInfo LastCollision;

		public abstract bool Overlapping(ACollider other);
		public abstract (float min, float max) MinMaxBounds(Vector2 axis);
		public abstract float RayCast(Ray ray);
		public abstract Vector2 NormalAt(Vector2 point);
	}
}
