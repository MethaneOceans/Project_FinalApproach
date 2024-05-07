

using System;

namespace GXPEngine.Physics
{
	/// <summary>
	/// Class for objects with my physics implementations. To get sprites and such the object should get them as child.
	/// </summary>
	internal abstract class PhysicsObject : GameObject
	{
		protected Collider PCollider;

		// Collision event and the invoke method so the physics manager can invoke it.
		public EventHandler<CollisionInfo> CollisionHandler;
		public void InvokeCollision(object caller, CollisionInfo collisionInfo)
		{
			CollisionHandler?.Invoke(caller, collisionInfo);
		}

		// Properties that define behavior for the collider in the physics update loop
		public bool IsStatic
		{
			get => PCollider.Static;
			set => PCollider.Static = value;
		}
		public bool IsTrigger
		{
			get => PCollider.Trigger;
			set => PCollider.Trigger = value;
		}
		public bool IsSolid
		{
			get => PCollider.Solid;
			set => PCollider.Solid = value;
		}

		public Vector2 Position
		{
			get => PCollider.Position;
			set
			{
				PCollider.Position = value;
				SetXY(value.x, value.y);
			}
		}
		public Vector2 Velocity
		{
			get => PCollider.Velocity;
			set
			{
				PCollider.Velocity = value;
			}
		}

		public PhysicsObject(Collider collider, Vector2 position = new Vector2(), Vector2 velocity = new Vector2())
		{
			PCollider = collider;
			Position = position;
			Velocity = velocity;

			IsSolid = true;
		}

		
	}
}
