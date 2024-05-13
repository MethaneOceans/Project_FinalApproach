using System.Collections.Generic;
using GXPEngine.Primitives;

namespace GXPEngine.Physics
{
	internal abstract class ACollider
	{
		public GameObject Owner;
		public Vector2 Position
		{
			get => _position;
			set {
				_position = value;
				Invalidate();
			}
		}
		Vector2 _position;
		bool _positionChanged;

		public Vector2 Velocity;
		public float Angle
		{
			get => _angle;
			set
			{
				_angle = value;
				Invalidate();
			}
		}
		private float _angle;
		private bool _angleChanged;

		public bool IsColliding;
		public CollisionInfo LastCollision;

		public ACollider(PhysicsObject owner)
		{
			Owner = owner;
			IsColliding = false;
		}

		public abstract bool Overlapping(ACollider other);
		public abstract (float min, float max) MinMaxBounds(Vector2 axis);
		public abstract float RayCast(Ray ray);
		public abstract Vector2 NormalAt(Vector2 point);

		public void Step(ICollection<ACollider> colliderList)
		{
			Position += Velocity;

			bool collided = false;
			CollisionInfo bestCol = new CollisionInfo();

			foreach (ACollider collider in colliderList)
			{
				if (Overlapping(collider))
				{
					CollisionInfo colInfo = LastCollision;
					if (colInfo.Depth > bestCol.Depth)
					{
						bestCol = colInfo;
					}
					collided = true;
				}
			}
			if (collided)
			{
				Position -= bestCol.Normal * bestCol.Depth;
			}

			Owner.Position = Position;
		}
		protected abstract void Invalidate();
	}
}
