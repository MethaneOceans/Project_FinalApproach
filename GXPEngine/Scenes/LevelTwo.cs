using GXPEngine.GameElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Scenes
{
    internal class LevelTwo : Level
    {
        public override void Initialize()
        {
            base.Initialize();

            Balloon balloon = new Balloon(new Vector2(750, 450), new Vector2(100, 100), 0);
            Block plank = new Block(new Vector2(750, 650), new Vector2(30, 250), 0);
            balloon.OnLaserHit += (object sender, EventArgs args) =>
            {
                balloon.body.Behavior = Physics.ACollider.ColliderType.Rigid;
                plank.body.Behavior = Physics.ACollider.ColliderType.Rigid;
            };

            List<ALevelObject> levelObjects = new List<ALevelObject>()
            {
                new Player(new Vector2(650, 250)),

                //floor left
                new Block(new Vector2(350, 850), new Vector2(700, 100), 0),

                //floor right
                new Block(new Vector2(1250, 850), new Vector2(800, 100), 0),

                //left wall
                new Block(new Vector2(150, 550), new Vector2(300, 500), 0),

                //Weavil roof/platform
                new Block(new Vector2(490, 320), new Vector2(30, 400), 90),

                //small hanging right wall
                new Block(new Vector2(670, 380), new Vector2(30, 100), 0),

                //balloon
                balloon,

                //wood plank
                plank,

                new Goal(new Vector2(400, 750)),
            };

            Initialize(levelObjects);
        }
    }
}
