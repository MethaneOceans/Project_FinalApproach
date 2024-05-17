using GXPEngine.GameElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Scenes
{
    internal class LevelThree : Level
    {
        public override void Initialize()
        {
            base.Initialize();

            List<ALevelObject> levelObjects = new List<ALevelObject>()
            {
                new Player(new Vector2(400, 600)),

                //floor
                new Block(new Vector2(800, 850), new Vector2(500, 100), 0),

                //left block
                new Block(new Vector2(250, 850), new Vector2(500, 400), 0),

                //left block lower part
                new Block(new Vector2(550, 850), new Vector2(100, 300), 0),

                //floating wall
                new Block(new Vector2(650, 350), new Vector2(70, 500), 0),

                //high middle wall
                new Block(new Vector2(1000, 525), new Vector2(100, 550), 0),

                //Weavil block
                new Block(new Vector2(1325, 900), new Vector2(550, 500), 0),

                //bottom mirror
                new Mirror(new Vector2(900, 750), 50),

                //top mirror
                new Mirror(new Vector2(1400, 150), 135),

                //top mirror 2
                new Mirror(new Vector2(1355, 105), 135),

                new Goal(new Vector2(1350, 600))
            };

            Initialize(levelObjects);
        }
    }
}
