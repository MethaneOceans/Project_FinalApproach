using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Physics
{
	internal class CollisionEvent : EventArgs
	{
		public PhysicsObject thisObject;
		public PhysicsObject otherObject;
		public Vector2 velocity;
	}
}
