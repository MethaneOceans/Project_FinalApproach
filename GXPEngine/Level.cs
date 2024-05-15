using GXPEngine.Control;
using GXPEngine.GameElements;
using GXPEngine.Physics;
using System;
using System.Collections.Generic;

namespace GXPEngine
{
	internal abstract class Level : Scene
	{
		protected PhysicsManager physics;

		private Player player;

		public List<ALevelObject> allObjects;

		private Beam laser;

		protected int ThreeStarScore;
		protected int TwoStarScore;
		protected int OneStarScore;

		protected int prismsShot;
		protected int currentGoalsHit;

		protected void Initialize(List<ALevelObject> objectList)
		{
			physics = new PhysicsManager();
			allObjects = new List<ALevelObject>();

			int maxBounces = 10;
			int maxDepth = 5;
			laser = new Beam(maxBounces, maxDepth, this);

			foreach (ALevelObject obj in objectList)
			{
				AddChild(obj);

				if (obj is Player playerObj)
				{
					// Assign player object
					player = playerObj;
				}
				else
				{
					physics.Add(obj.body);
					allObjects.Add(obj);

					if (obj is Goal goal)
					{
						goal.OnLaserHit += GoalHit;
					}
				}
			}
			AddChild(laser);
		}

		public virtual void Update()
		{
			currentGoalsHit = 0;
			physics.Step();


			// Fire laser
			if (Input.GetMouseButton(0))
			{
				Vector2 from = player.Position;
				Vector2 to = Input.mousePos;
				Ray laserStart = new Ray(from, to - from);

				laser.RecalcPath(laserStart);

				laser.visible = true;
			}
			else laser.visible = false;
			// Fire prism
			if (Input.GetMouseButtonDown(1))
			{
				Vector2 from = player.Position;
				Vector2 to = Input.mousePos;


			}
		}

		private void GoalHit(object sender, EventArgs args)
		{
			Console.WriteLine("Hit Goal!!!");
			currentGoalsHit++;
		}

		public EventHandler Win;
		public EventHandler LostStar;
	}
}
