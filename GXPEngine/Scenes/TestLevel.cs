using System;
using System.Collections.Generic;
using GXPEngine.Control;
using GXPEngine.Physics;
using GXPEngine.Primitives;

namespace GXPEngine.Scenes
{
	internal class TestLevel : Scene
	{
		PhysicsManager PhysManager;
		List<PhysicsObject> Objects;

		public override void Initialize()
		{
			base.Initialize();

			PhysManager = new PhysicsManager();

			Objects = new List<PhysicsObject>()
			{
				new EDBox(new Vector2(200, height / 2), new Vector2(400, height), 0),
				new EDBox(new Vector2(width - 200, height / 2), new Vector2(400, height), 180),
				new EDCircle(new Vector2(width / 2, height / 2), 50),
			};

			Objects[0].body.TriggerMethod = BorderForce;
			Objects[0].body.Behavior = ACollider.ColliderType.Trigger;
			Objects[1].body.TriggerMethod = BorderForce;
			Objects[1].body.Behavior = ACollider.ColliderType.Trigger;
			Objects[2].body.Behavior = ACollider.ColliderType.Rigid;
			Objects[2].body.Velocity = new Vector2(5, 0);

			foreach (var obj in Objects)
			{
				AddChild(obj);
				PhysManager.Add(obj.body);
			}
		}

		public void Update()
		{
			if ((Objects[0] as EDBox).ED.color < 0xFFFFFF)
			{
				uint col = (Objects[0] as EDBox).ED.color;
				(Objects[0] as EDBox).ED.color = ColorReturnToWhite(col, 16);
			}
			if ((Objects[1] as EDBox).ED.color < 0xFFFFFF)
			{
				uint col = (Objects[1] as EDBox).ED.color;
				(Objects[1] as EDBox).ED.color = ColorReturnToWhite(col, 16);
			}

			PhysManager.Step();
		}

		private (byte r, byte g, byte b) I32ToColor(uint color)
		{
			byte red = (byte)(color >> 16);
			byte green = (byte)(color >> 8);
			byte blue = (byte)color;

			return (red, green, blue);
		}
		private uint ColorReturnToWhite(uint color, byte increaseBy)
		{
			(byte r, byte g, byte b) = I32ToColor(color);

			if (r < 255 - increaseBy) r += increaseBy;
			else if (r < 255) r = 255;
			if (g < 255 - increaseBy) g += increaseBy;
			else if (g < 255) g = 255;
			if (b < 255 - increaseBy) b += increaseBy;
			else if (b < 255) b = 255;

			uint rComp = (uint)r << 16;
			uint gComp = (uint)g << 8;
			uint bComp = b;

			return rComp + gComp + bComp;
		}

		private void BorderForce(ACollider trigger, ACollider other)
		{
			other.Velocity += Vector2.GetUnitVectorDeg(trigger.Angle) * 0.1f;
			(trigger.Owner as EDBox).ED.SetColor(1, 0, 0);
			return;
		}
	}
}

