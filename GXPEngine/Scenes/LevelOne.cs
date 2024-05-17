using GXPEngine.GameElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Scenes
{
    internal class LevelOne : Level
    {
        public override void Initialize()
        {
            base.Initialize();

            List<ALevelObject> levelObjects = new List<ALevelObject>()
            {
                new Player(new Vector2(100, 750)),

                //floor
                new Block(new Vector2(800, 850), new Vector2(1700, 100), 0),

                //left wall
                new Block(new Vector2(0, 550), new Vector2(100, 500), 0),

                //right wall
                new Block(new Vector2(1500, 550), new Vector2(300, 500), 0),

                //Weavil left wall
                new Block(new Vector2(740, 730), new Vector2(30, 150), 0),

                //Weavil roof
                new Block(new Vector2(770, 670), new Vector2(30, 50), 90),

                new Goal(new Vector2(800, 750)),
            };

            Initialize(levelObjects);
        }
    }
}
