using System.Collections.Generic;
using System;
using GXPEngine.Core;
using static GXPEngine.Physics.ACollider;

namespace GXPEngine.Physics
{
	internal class PhysicsManager
	{
		public List<ACollider> Objects => bodies;
		private List<ACollider> bodies;

		private List<ACollider> staticColliders;
		private List<ACollider> triggerColliders;
		private List<ACollider> rigidColliders;

		private bool ActiveStep;

		public PhysicsManager()
		{
			bodies = new List<ACollider>();
			staticColliders = new List<ACollider>();
			triggerColliders = new List<ACollider>();
			rigidColliders = new List<ACollider>();

			ActiveStep = false;
		}
		
		public void Add(ACollider obj)
		{
			if (ActiveStep)
			{
				throw new InvalidOperationException("Currently running a step, cannot add object");
			}
			else
			{
				bodies.Add(obj);

				if (obj.Behavior == ACollider.ColliderType.Static) staticColliders.Add(obj);
				else if (obj.Behavior == ACollider.ColliderType.Rigid) rigidColliders.Add(obj);
				else if (obj.Behavior == ACollider.ColliderType.Trigger) triggerColliders.Add(obj);

				obj.BehaviorChanged += BehaviorChangeHandler;
			}
		}

		public void Step()
		{
			ActiveStep = true;

			foreach (ACollider obj in rigidColliders)
			{
				Step(obj);
			}
			
			ActiveStep = false;
		}

		private void Step(ACollider obj)
		{
			if (obj.Behavior == ColliderType.Rigid)
			{
				foreach (ACollider trigger in triggerColliders)
				{
					if (obj.Overlapping(trigger))
					{
						// Should apply forces
						trigger.TriggerMethod(trigger, obj);
					}
				}

				// Copied from ACollider
				obj.Position += obj.Velocity;

				bool collided = false;
				CollisionInfo bestCol = new CollisionInfo();

				foreach (ACollider collider in bodies)
				{
					if (collider == obj || collider.Behavior == ColliderType.Trigger) continue;
					if (obj.Overlapping(collider))
					{
						CollisionInfo colInfo = obj.LastCollision;
						if (colInfo.Depth > bestCol.Depth)
						{
							bestCol = colInfo;
						}
						collided = true;
					}
				}
				if (collided)
				{
					obj.Position -= bestCol.Normal * bestCol.Depth;

					Vector2 q = Vector2.Dot(bestCol.Normal, obj.Velocity) * bestCol.Normal;
					obj.Velocity -= (2 * 0.9f) * q;
				}

				obj.Owner.Position = obj.Position;
			}
		}

		private void BehaviorChangeHandler(object sender, ACollider.BehaviorChangeEvent args)
		{
			ColliderType oldB = args.OldBehavior;
			ColliderType newB = args.NewBehavior;

			if (oldB == ColliderType.Rigid) rigidColliders.Remove((ACollider)sender);
			else if (oldB == ColliderType.Trigger) triggerColliders.Remove((ACollider)sender);
			else if (oldB == ColliderType.Rigid) rigidColliders.Remove((ACollider)sender);

			if (newB == ColliderType.Rigid) rigidColliders.Add((ACollider)sender);
			else if (newB == ColliderType.Trigger) triggerColliders.Add((ACollider)sender);
			else if (newB == ColliderType.Rigid) rigidColliders.Add((ACollider)sender);
		}
	}
}
