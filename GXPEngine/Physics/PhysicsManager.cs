using System.Collections.Generic;
using static GXPEngine.Mathf;

namespace GXPEngine.Physics
{
	internal class PhysicsManager
	{
		Game game;

		private List<Collider> allColliders;

		public Vector2 GlobalForce;
		public float Bounciness = 0.65f;

		private EasyDraw PathTraceLayer;
		public bool PathTraceEnabled;

		public PhysicsManager(Game game)
		{
			this.game = game;

			allColliders = new List<Collider>();

			PathTraceLayer = new EasyDraw(game.width, game.height);
			PathTraceEnabled = false;
			game.AddChild(PathTraceLayer);
		}

		// Steps all objects by 1 frame
		public void Step()
		{
			foreach (Collider mover in allColliders.FindAll(a => { return a.ColType == ColliderType.Rigid; }))
			{
				// Update velocity and save position
				mover.Velocity += GlobalForce;
				mover.OldPosition = mover.Position;

				// StepCollider takes a list of colliders to check with, this can be used for pruning a lot of colliders in advance
				StepCollider(mover, allColliders.FindAll(col => { return col.ColType != ColliderType.Ghost; }));
			}
		}
		public void StepCollider(Collider col, ICollection<Collider> colliders, float dt = 1, int recursionCounter = 0)
		{
			// Try to move the object by it's velocity multiplied by the remaining time
			col.Position += dt * col.Velocity;

			List<Collider> overlaps = col.Overlaps(colliders);
			if (overlaps.Count == 0) return;

			// Get the collision info for collisions with the overlapping objects
			CollisionInfo collision = col.EarliestCollision(overlaps);

			// Object didnt move this frame
			if (collision.TimeOfImpact < 0.0001f && recursionCounter == 0)
			{
				col.Position = col.OldPosition;
				return;
			}

			// Make sure the TOI is limited between 0 and the remaining time
			collision.TimeOfImpact = Clamp(collision.TimeOfImpact, 0, dt);

			ResolveCollision(collision);
			float remaining_dt = dt - collision.TimeOfImpact;
			
			// If the time left is close to zero it has no use to continue.
			if (remaining_dt > 0.01f && recursionCounter < 10)
			{
				StepCollider(col, colliders, remaining_dt, recursionCounter + 1);
			}
		}

		// Resolves a collision based on the supplied collisionInfo
		public void ResolveCollision(CollisionInfo collision)
		{
			Collider thisCol = collision.thisCollider;
			Collider otherCol = collision.otherCollider;

			// Invoke the collision event of the collider if it is bound
			thisCol.Owner?.InvokeCollision(this, collision);

			// Destructure the collision info and calculate the tangent vector
			Vector2 normal = collision.Normal;
			float toi = collision.TimeOfImpact;
			toi = Clamp(toi, 0, 1);
			Vector2 tangent = normal.Normal();

			thisCol.Position = thisCol.OldPosition + toi * thisCol.Velocity;

			// Get speed along the normal and tangent of collision
			float p_this = Vector2.Dot(tangent, thisCol.Velocity);
			float q_this = Vector2.Dot(normal, thisCol.Velocity);

			// Can't resolve for other object if it's static or non existent
			if (otherCol == null || otherCol.Static)
			{
				thisCol.Velocity = p_this * tangent - q_this * normal;
				return;
			}

			// Invoke the collision event of the collider if it is bound
			otherCol.Owner?.InvokeCollision(this, collision);

			// Get speed along the normal and tangent of collision
			float p_other = Vector2.Dot(tangent, otherCol.Velocity);
			float q_other = Vector2.Dot(normal, otherCol.Velocity);

			// Get the momentum in along the q component
			float q_momentum = q_this * thisCol.Mass + q_other * otherCol.Mass;
			float q_com = q_momentum / (thisCol.Mass + otherCol.Mass);

			// Get the velocities along p and q. p only with the bounciness. q changes according to the momentum of both objects
			float p_result_this = Bounciness * p_this;
			float q_result_this = q_com - Bounciness * (q_this - q_com);
			Vector2 thisNewV = p_result_this * tangent + q_result_this * normal;
			float p_result_other = Bounciness * p_other;
			float q_result_other = q_com - Bounciness * (q_other - q_com);
			Vector2 otherNewV = p_result_other * tangent + q_result_other * normal;

			thisCol.Velocity = thisNewV;
			otherCol.Velocity = otherNewV;
		}

		public void AddCollider(Collider collider) => allColliders.Add(collider);
		public bool RemoveCollider(Collider collider) => allColliders.Remove(collider);
	}
}
