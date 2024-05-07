using GXPEngine.Physics;

namespace GXPEngine.Control
{
	internal abstract class PhysicsScene : Scene
	{
		protected PhysicsManager physics;
		public bool Paused;
		public bool ManualStep;
		
		public virtual void Update()
		{
			if (Paused) return;
			if (!ManualStep)
			{
				physics.Step();
			} 
			else if (Input.GetKeyDown(Key.SPACE)) {
				physics.Step();
			}

			if (Input.GetKeyDown(Key.S))
			{
				ManualStep = !ManualStep;
			}
		}
		public override void Initialize()
		{
			base.Initialize();
			physics = new PhysicsManager(game);
		}
	}
}
