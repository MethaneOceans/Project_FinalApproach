using GXPEngine.Control;
using GXPEngine.Core;
using GXPEngine.GameElements;
using GXPEngine.Physics;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;

namespace GXPEngine
{
	internal abstract class Level : Scene
	{
		private readonly Dictionary<string, (Sound sound, SoundChannel channel)> _sounds = new Dictionary<string, (Sound sound, SoundChannel channel)>
		{
			{ "ambient", (new Sound("Sounds/Ambient Loop.mp3", true, true), null) },
			{ "victoryMusic", (new Sound("Sounds/Victory Music.mp3", false, true), null) },
			{ "ambientMusic", (new Sound("Sounds/Croak Quest Music.mp3", true, true), null) },

			{ "chargeLaunch", (new Sound("Sounds/Spit Setup.mp3", false, false), null) },
			{ "prismLaunched", (new Sound("Sounds/Spit Launch.mp3", false, false), null) },
			
			{ "prismFreeze", (new Sound("Sounds/Crystal Freeze.mp3", false, false), null) },
			{ "prismMidAir", (new Sound("Sounds/Crystal Midair Whistle.mp3", false, false), null) },
			{ "rockBounce", (new Sound("Sounds/Rock Bounce.mp3", false, false), null) },

			{ "weavilLaugh", (new Sound("Sounds/Weavil Laugh.mp3", false, false), null) },
			{ "weavilHit", (new Sound("Sounds/Weavil Hit.mp3", false, false), null) },

		};

		protected PhysicsManager physics;

		private Player player;

		public List<ALevelObject> allObjects;

		private Beam laser;

		protected int ThreeStarScore = 1;
		protected int TwoStarScore = 2;
		protected int OneStarScore = 3;
		private int starGoal;

		private bool launchCharging;
		private bool launchPrism;

		protected int prismsShot;
		private int currentGoalsHit;
		private int goalsCount;

		private bool gameWon;

		protected void Initialize(List<ALevelObject> objectList)
		{
			physics = new PhysicsManager();
			allObjects = new List<ALevelObject>();

			gameWon = false;
			prismsShot = 0;
			starGoal = ThreeStarScore;
			currentGoalsHit = 0;
			goalsCount = 0;

			launchCharging = false;
			launchPrism = false;

			int maxBounces = 10;
			int maxDepth = 5;
			laser = new Beam(maxBounces, maxDepth, this);

			// Add objects to level and add event handlers if necessary
			foreach (ALevelObject obj in objectList)
			{
				AddChild(obj);

				obj.body.OnDestroy += (sender, args) =>
				{
					allObjects.Remove(obj);
					obj.Destroy();
				};

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
						goalsCount++;
					}
				}
			}
			AddChild(laser);

			// Configure sound
			
			PlaySound("ambient", false);
			PlaySound("ambientMusic");
		}

		public virtual void Update()
		{
			if (!gameWon)
			{
				currentGoalsHit = 0;
				physics.Step();

				if (launchPrism)
				{
					LaunchPrismAction();

					launchPrism = false;
				}

				// Fire laser
				if (Input.GetMouseButton(0))
				{
					Vector2 from = player.Position;
					Vector2 to = Input.mousePos;
					Ray laserStart = new Ray(from, to - from);

					laser.RecalcPath(laserStart);

					laser.visible = true;
				}
				else
				{
					laser.visible = false;
				}
				// Fire prism
				if (Input.GetMouseButtonDown(1) && !launchCharging)
				{
					ChargeLaunch();
					//LaunchPrism();
				}

				if (currentGoalsHit == goalsCount)
				{
					gameWon = true;
					LevelWon();
				}
			}
			else
			{
				//if (!_sounds["victoryMusic"].channel.IsPlaying)
				//{
				//	NextLevel();
				//}
			}
		}

		private void GoalHit(object sender, EventArgs args)
		{
			Console.WriteLine("Hit Goal!!!");
			PlaySound("weavilHit");
			currentGoalsHit++;
		}

		protected virtual void LevelWon()
		{
			OnLevelWon?.Invoke(this, new EventArgs());

			PlaySound("victoryMusic", true);
			StopSound("ambient");
			StopSound("ambientMusic");
		}

		protected virtual void ChargeLaunch()
		{
			PlaySound("chargeLaunch");
			launchCharging = true;
			Timer launchTimer = new Timer((_) =>
			{
				launchCharging = false;
				launchPrism = true;
			}, null, 500, Timeout.Infinite);

			Vector2 from = player.Position;
			Vector2 to = Input.mousePos;

			LaunchPrismAction = () =>
			{
				LaunchPrism(from, to);
			};
		}
		protected virtual void LaunchPrism(Vector2 from, Vector2 to)
		{
			OnPrismLaunched?.Invoke(this, new EventArgs());

			Vector2 velocity = (to - from) / 30;
			Prism newPrism = new Prism(player.Position, velocity, 2000);
			allObjects.Add(newPrism);
			physics.Add(newPrism.body);
			AddChild(newPrism);

			newPrism.OnPrismFreeze += (sender, args) =>
			{
				_sounds["prismFreeze"].sound.Play(volume: myGame.SoundVolume);
			};

			newPrism.body.OnCollision += (sender, args) =>
			{
				if (args.other.Owner is Block)
				{
					_sounds["rockBounce"].sound.Play(volume: myGame.SoundVolume);
				}
			};

			PlaySound("prismLaunched", true);
			_sounds["prismMidAir"].sound.Play(volume: myGame.SoundVolume);

			prismsShot++;
			if (prismsShot - 1 == starGoal && prismsShot > starGoal)
			{
				OnStarLost?.Invoke(this, new EventArgs());
				PlaySound("weavilLaugh", true);

				if (starGoal == ThreeStarScore) starGoal = TwoStarScore;
				else if (starGoal == TwoStarScore) starGoal = OneStarScore;
				else starGoal = prismsShot + 5;
			}
		}
		private Action LaunchPrismAction;

		public EventHandler OnLevelWon;
		public EventHandler OnStarLost;
		public EventHandler OnPrismLaunched;

		protected void PlaySound(string soundID, bool forceRestart = false)
		{
			(Sound sound, SoundChannel channel) = _sounds[soundID];
			
			
			if (channel != null && channel.IsPlaying && forceRestart) channel.Stop();
			if (channel == null || (channel != null && !channel.IsPlaying) || forceRestart)
			{
				SoundChannel newChannel = sound.Play(volume: myGame.SoundVolume);
				_sounds[soundID] = (sound, newChannel);
			}
		}
		protected void StopSound(string soundID)
		{
			(Sound sound, SoundChannel channel) = _sounds[soundID];

			if (channel != null && channel.IsPlaying)
			{
				channel.Stop();
				_sounds[soundID] = (sound, null);
			}
		}

		public override void Unload()
		{
			base.Unload();

			if (_sounds != null)
			{
				var keys = _sounds.Keys.ToArray();
				for (int i = _sounds.Count - 1; i >= 0; i--)
				{
					StopSound(keys[i]);
				}
			}
		}

		protected virtual void NextLevel()
		{
			myGame.sceneManager.Reload();
		}
	}
}
