using System.Collections.Generic;
using System;

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
			if (!ActiveStep)
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
				if (obj.Behavior == ACollider.ColliderType.Rigid)
				{
					foreach (ACollider trigger in triggerColliders)
					{
						if (obj.Overlapping(trigger))
						{
							// Should apply forces
							trigger.TriggerMethod(trigger, obj);
						}
					}
				}
			}
			
			ActiveStep = false;
		}

		private void Step(ACollider collider)
		{

		}

		private void BehaviorChangeHandler(object sender, ACollider.BehaviorChangeEvent args)
		{
			ACollider.ColliderType oldB = args.OldBehavior;
			ACollider.ColliderType newB = args.NewBehavior;

			if (oldB == ACollider.ColliderType.Rigid) rigidColliders.Remove((ACollider)sender);
			else if (oldB == ACollider.ColliderType.Trigger) triggerColliders.Remove((ACollider)sender);
			else if (oldB == ACollider.ColliderType.Rigid) rigidColliders.Remove((ACollider)sender);

			if (newB == ACollider.ColliderType.Rigid) rigidColliders.Add((ACollider)sender);
			else if (newB == ACollider.ColliderType.Trigger) triggerColliders.Add((ACollider)sender);
			else if (newB == ACollider.ColliderType.Rigid) rigidColliders.Add((ACollider)sender);
		}
	}
}
