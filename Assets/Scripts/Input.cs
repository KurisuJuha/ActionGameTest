using UnityInput = UnityEngine.Input;
using KeyCode = UnityEngine.KeyCode;

namespace ActionGameTest
{
    public class Input
    {
        public bool up { get; private set; }
        public bool right { get; private set; }
        public bool left { get; private set; }

        public void Update()
        {
            ResetInputs();

            up |= UnityInput.GetKey(KeyCode.UpArrow);
            up |= UnityInput.GetKey(KeyCode.W);
            right |= UnityInput.GetKey(KeyCode.RightArrow);
            right |= UnityInput.GetKey(KeyCode.D);
            left |= UnityInput.GetKey(KeyCode.LeftArrow);
            left |= UnityInput.GetKey(KeyCode.A);
        }

        private void ResetInputs()
        {
            up = false;
            right = false;
            left = false;
        }
    }
}