using GXPEngine.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.GameElements
{
	internal class PathSprite : GameObject
	{
		public readonly int MaxBounces;
		private LaserSprite[] _laserSprites;

		public PathSprite(int maxBounces)
		{
			MaxBounces = maxBounces;

			_laserSprites = new LaserSprite[MaxBounces];
			for (int i = 0; i < _laserSprites.Length; i++)
			{
				_laserSprites[i] = new LaserSprite();
				AddChild(_laserSprites[i]);
				_laserSprites[i].visible = false;
			}
		}

		public void SetPath(List<(PhysicsObject obj, Vector2 p, float t, Ray ray)> path)
		{
			if (path.Count > MaxBounces + 1)
			{
				throw new Exception($"Path is more bounces than MaxBounces({MaxBounces})");
			}

			for (int i = 0; i < MaxBounces; i++)
			{
				if (i < path.Count)
				{
					(PhysicsObject obj, Vector2 p, float t, Ray ray) = path[i];

					float angle = ray.Direction.Degrees;
					_laserSprites[i].rotation = angle;
					_laserSprites[i].width = (int)t;
					_laserSprites[i].visible = true;
					_laserSprites[i].Position = ray.Origin;
				}
				else
				{
					_laserSprites[i].visible = false;
				}
			}
		}
	}
	internal class LaserSprite : Sprite
	{
		public new int width
		{
			get => base.width;
			set
			{
				base.width = value;

				float left = _mirrorX ? width / _baseWidth : 0.0f;
				float right = _mirrorX ? 0.0f : width / _baseWidth;
				float top = _mirrorY ? 1.0f : 0.0f;
				float bottom = _mirrorY ? 0.0f : 1.0f;

				_uvs = new float[8] { left, top, right, top, right, bottom, left, bottom };
			}
		}

		private float _baseWidth;

		public LaserSprite() : base("Laser.png")
		{
			SetOrigin(0, height / 2);

			texture.wrap = true;
			scale = 50f / height;
			_baseWidth = width;
		}
	}
}
