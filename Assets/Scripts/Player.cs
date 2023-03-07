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
            AddGravityToVelocity();
            Move();
            CheckOnWall();
            ApplyVelocity();

        private void CheckOnWall()
        {
            Box leftBox = new(position + new Vector2(0, 0.05f), new(0, 0.8f));
            Box rightBox = new(position + new Vector2(1, 0.05f), new(0, 0.8f));

            foreach (var fieldBox in fieldBoxes)
            {
                if (fieldBox.IsHit(leftBox))
                {
                    // 左壁に当たっていることを示す
                    onRightWall = true;

                    // めり込みの解消
                    position = new(fieldBox.position.x + fieldBox.size.x, position.y);
                }

                if (fieldBox.IsHit(rightBox))
                {
                    // 右壁に当たっていることを示す
                    onLeftWall = true;

                    // めり込みの解消
                    position = new(fieldBox.position.x - 1, position.y);
                }
            }

            onGround = false;

        }

        private void Move()
        {
            float inputVec = (input.left ? -1 : 0) + (input.right ? 1 : 0);
            position += new Vector2(1, 0) * inputVec * moveSpeed;

            if (onGround && input.upDown) velocity = new Vector2(0, 1) * jumpPower;
        }

        private void ApplyVelocity()
        {
            position += velocity;
        }

        private void AddGravityToVelocity()
        {
            velocity -= new Vector2(0, gravityScale);
        }
    }
}