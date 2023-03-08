using UnityEngine;

namespace ActionGameTest
{
    public class GameInput
    {
        public bool up { get; private set; }
        public bool upUp { get; private set; }
        public bool upDown { get; private set; }

        public bool right { get; private set; }
        public bool rightUp { get; private set; }
        public bool rightDown { get; private set; }

        public bool left { get; private set; }
        public bool leftUp { get; private set; }
        public bool leftDown { get; private set; }

        public bool jump { get; private set; }
        public bool jumpUp { get; private set; }
        public bool jumpDown { get; private set; }

        public void Update()
        {
            bool newUp = false;
            bool newRight = false;
            bool newLeft = false;
            bool newJump = false;

            newUp |= Input.GetKey(KeyCode.UpArrow);
            newUp |= Input.GetKey(KeyCode.W);
            newRight |= Input.GetKey(KeyCode.RightArrow);
            newRight |= Input.GetKey(KeyCode.D);
            newLeft |= Input.GetKey(KeyCode.LeftArrow);
            newLeft |= Input.GetKey(KeyCode.A);
            newJump |= Input.GetKey(KeyCode.Z);

            upUp = up && !newUp;
            upDown = !up && newUp;
            rightUp = right && !newRight;
            rightDown = !right && newRight;
            leftUp = left && !newLeft;
            leftDown = !left && newLeft;
            jumpUp = jump && !newJump;
            jumpDown = !jump && newJump;

            up = newUp;
            right = newRight;
            left = newLeft;
            jump = newJump;
        }
    }
}