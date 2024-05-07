using System;

namespace GXPEngine.Physics.Primitives
{
	internal class AABBCollider : Collider
	{
		public AABBCollider(Vector2 position, float width, float height)
		{

		}

		public override CollisionInfo GetCollisionInfo(Collider other)
		{
			throw new NotImplementedException();
		}

		public override bool Overlaps(Collider other)
		{
			throw new NotImplementedException();
		}
	}
}
