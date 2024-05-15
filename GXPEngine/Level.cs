﻿using GXPEngine.Control;
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
		public List<Mirror> mirrors;
		public List<Goal> goals;
		public List<Block> blocks;
		public List<Prism> prisms;

		private Beam laser;

		protected int ThreeStarScore;
		protected int TwoStarScore;
		protected int OneStarScore;

		protected int prismsShot;
		protected int currentGoalsHit;

		protected void Initialize(List<ALevelObject> objectList)
		{
			mirrors = new List<Mirror>();
			goals = new List<Goal>();
			blocks = new List<Block>();
			prisms = new List<Prism>();

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

					if (obj is Mirror mirror)
					{
						// Add mirror
						mirrors.Add(mirror);
					}
					else if (obj is Goal goal)
					{
						// Add goal
						goals.Add(goal);
						goal.OnLaserHit += GoalHit;
					}
					else if (obj is Block block)
					{
						// Add block
						blocks.Add(block);
					}
				}
			}
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
				Ray laserStart = new Ray(from, to);

				laser.RecalcPath(laserStart);
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
			currentGoalsHit++;
		}

		public EventHandler Win;
		public EventHandler LostStar;
	}
}