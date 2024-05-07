using System;
using System.Collections.Generic;
using System.Linq;

namespace GXPEngine.Physics
{
	internal abstract class Collider
	{
		public float Mass;
		public bool Static;
		public bool Trigger;
		public bool Solid;

		public bool didNotMove;

		public ColliderType ColType
		{
			get
			{
				if (Static && Solid) return ColliderType.Static;
				else if (!Static && Solid) return ColliderType.Rigid;
				else return ColliderType.Ghost;
			}
		}

		public PhysicsObject Owner;

		public Vector2 OldPosition;
		public Vector2 Position;
		public Vector2 Velocity;

		public Collider()
		{
			Solid = true;
		}

		public abstract bool Overlaps(Collider other);
		public abstract CollisionInfo GetCollisionInfo(Collider other);

		// Method to check multiple colliders and add them to the referenced array
		// NOTE: This is not very easy to read when used somewhere else
		public void Overlaps<T>(T others, ref T hitlist) where T : ICollection<Collider>
		{
			foreach (Collider other in others)
			{
				if (other == this) continue;
				if (Overlaps(other)) hitlist.Append(other);
			}
		}

		// Check if overlapping with any colliders from the supplied collection
		public List<Collider> Overlaps(ICollection<Collider> others)
		{
			List<Collider> overlapping = new List<Collider>();
			foreach (Collider other in others)
			{
				if (other == this) continue;
				if (Overlaps(other)) overlapping.Add(other);
			}
			return overlapping;
		}

		// Calculate the collision with the earliest time of impact
		public CollisionInfo EarliestCollision(ICollection<Collider> overlapping)
		{
			CollisionInfo earliest = new CollisionInfo(this, null, new Vector2(), float.PositiveInfinity);
			foreach (Collider other in overlapping)
			{
				CollisionInfo current = GetCollisionInfo(other);
				if (current.TimeOfImpact < earliest.TimeOfImpact) earliest = current;
			}
			return earliest;
		}
		
		public class TriggerEventArgs : EventArgs
		{
			public Collider Other;
			public TriggerEventArgs(Collider other)
			{
				Other = other;
			}
		}
	}
}
