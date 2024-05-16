
using System.Drawing;

namespace GXPEngine
{
	internal class NineSlice : SpriteBatch
	{
		public NineSlice(string filepath, int width, int height)
		{
			Bitmap texture = (Bitmap)Image.FromFile(filepath);
			int tileWidth = texture.Width / 3;
			int tileHeight = texture.Height / 3;

			int cols = width / tileWidth;
			int rows = height / tileHeight;

			AnimationSprite[,] sprites = new AnimationSprite[cols, rows];

			// Generate tiles
			for (int j = 0; j < rows; j++)
			{
				for (int i = 0; i < cols; i++)
				{
					AnimationSprite currentTile = new AnimationSprite(texture, 3, 3)
					{
						Position = new Vector2(i * tileWidth, j * tileHeight),
					};

					int selectX;
					int selectY;

					if (i == 0) selectX = 0;
					else if (i == cols - 1) selectX = 2;
					else selectX = 1;

					if (j == 0) selectY = 0;
					else if (j == rows - 1) selectY = 2;
					else selectY = 1;

					currentTile.SetFrame(selectY * 3 + selectX);
					AddChild(currentTile);
				}
			}

			Freeze();
		}
	}
}
