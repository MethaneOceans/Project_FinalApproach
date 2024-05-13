namespace GXPEngine.Physics
{
	public class CollisionInfo
	{
		public Vector2 Normal;
		public float Depth;

		public CollisionInfo(Vector2 normal, float depth)
		{
			Normal = normal;
			Depth = depth;
		}
		public CollisionInfo() : this(new Vector2(), -1000) { }
	}
}
