using System.Drawing;
using static GXPEngine.Mathf;

namespace GXPEngine
{
	internal class Catapult : GameObject
	{
		private EasyDraw body;
		private EasyDraw barrel;

		public Catapult()
		{
			body = new EasyDraw(75, 75);
			body.SetOrigin(body.width / 2, body.height / 2);
			body.Clear(Color.Gray);
			AddChild(body);

			barrel = new EasyDraw(75, 40);
			barrel.SetOrigin(barrel.height / 2, barrel.height / 2);
			barrel.Clear(Color.DarkGray);
			AddChild(barrel);
		}

		public void Update()
		{
			barrel.rotation = Vector2.Rad2Deg(Atan2(Input.mouseY - y, Input.mouseX - x));
		}
	}
}
