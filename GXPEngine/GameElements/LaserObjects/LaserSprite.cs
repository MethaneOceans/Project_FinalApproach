namespace GXPEngine.GameElements
{
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

		public LaserSprite() : base("Textures/Laser.png")
		{
			SetOrigin(0, height / 2);

			texture.wrap = true;
			scale = 50f / height;
			_baseWidth = width;
		}
	}
}
