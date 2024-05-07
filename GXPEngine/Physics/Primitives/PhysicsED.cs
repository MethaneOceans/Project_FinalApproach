using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Physics.Primitives
{
	internal abstract class PhysicsED : PhysicsObject
	{
		protected EasyDraw ED;
		protected bool DirtyFlag;

		// NOTE: Should this abstract object have a public or protected constructor?
		protected PhysicsED (
			int width, 
			int height, 
			Collider collider, 
			Vector2 position = new Vector2(), 
			Vector2 velocity = new Vector2()) : base(collider, position, velocity)
		{
			ED = new EasyDraw (width, height);
			ED.SetOrigin(width / 2, height / 2);
			AddChild(ED);
		}
	}
}
