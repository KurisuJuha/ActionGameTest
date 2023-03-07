using System.Collections.ObjectModel;
using ActionGameTest.Physics;
using UnityEngine;

namespace ActionGameTest
{
    public class Player
    {
        public Vector2 position { get; private set; }
        private readonly GameInput input;
        private readonly ReadOnlyCollection<Box> fieldBoxes;
        private bool onCeiling;
        private bool onGround;
        private bool onLeftWall;
        private bool onRightWall;
        private Vector2 velocity;

        private const float gravityScale = 0.03f;
        private const float moveSpeed = 0.2f;
        private const float jumpPower = 0.4f;

        public Player(GameInput input, Box[] fieldBoxes)
        {
            this.input = input;
            this.fieldBoxes = new(fieldBoxes);
            position = new(-0.5f, -0.5f);
        }

        public void Update()
        {

        }
    }
}