namespace GXPEngine
{
    public class Mirror : Sprite
    {
        public Mirror() : base("mirror.png")
        {
            x = 200;
            y = 200;
        }

        private void Update()
        {
            Rotate();
        }

        private void Rotate()
        {
            if (Input.GetKey(Key.LEFT)) 
            {
                Rotation += 1;
            }
            if (Input.GetKey(Key.RIGHT))
            {
                Rotation -= 1;
            }
        }
    }
}
