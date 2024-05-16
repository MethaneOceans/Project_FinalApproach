using GXPEngine.Physics;
using System;

namespace GXPEngine.GameElements
{
	internal abstract class ALevelObject : PhysicsObject
	{
		protected Sprite sprite;

		public ALevelObject(string path)
		{
			sprite = new Sprite(path);
			sprite.SetOrigin(sprite.width / 2, sprite.height / 2);
			AddChild(sprite);
		}
		public ALevelObject(Sprite sprite)
		{
			this.sprite = sprite;
			this.sprite.SetOrigin(sprite.width / 2, sprite.height / 2);
			AddChild(sprite);
		}

		public EventHandler OnLaserHit;
		public void HitByLaser()
		{
			OnLaserHit?.Invoke(this, new EventArgs());
		}
	}
}
