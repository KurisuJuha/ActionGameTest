using System.Collections.ObjectModel;
using ActionGameTest.Physics;
using JuhaKurisu.PopoTools.Utility;
using UnityEngine;

namespace ActionGameTest
{
    public class Player
    {
        public Vector2 position { get; private set; }
        private readonly GameInput input;
        private readonly ReadOnlyCollection<Box> fieldBoxes;
        private readonly ActionForOnlyOnce jump;
        private bool onCeiling;
        private bool onGround;
        private bool onLeftWall;
        private bool onRightWall;
        private Vector2 velocity;
        private double jumpButtonDownTime;

        private const float gravityScale = 0.03f;
        private const float moveSpeed = 0.2f;
        private const float jumpPower = 0.4f;
        private const double jumpBuffer = 0.1f;

        public Player(GameInput input, Box[] fieldBoxes)
        {
            this.input = input;
            this.fieldBoxes = new(fieldBoxes);
            position = new(-0.5f, -0.5f);
            jump = new(() =>
            {
                velocity = new Vector2(0, 1) * jumpPower;
            });
        }

        public void Update()
        {
            AddGravityToVelocity();
            Move();
            CheckOnWall();
            ApplyVelocity();
            CheckOnGround();
        }

        private void CheckOnGround()
        {
            onGround = false;
            Box upBox = new(position + new Vector2(0.05f, 1), new(0.8f, 0));
            Box downBox = new(position + new Vector2(0.05f, 0), new(0.8f, 0));

            foreach (var fieldBox in fieldBoxes)
            {
                if (fieldBox.IsHit(downBox))
                {
                    // 地面に当たっていることを示す
                    onGround = true;

                    // velocityを0に
                    velocity = new();

                    // めり込みの解消
                    position = new(position.x, fieldBox.position.y + fieldBox.size.y);
                }

                if (fieldBox.IsHit(upBox))
                {
                    // 天井に当たっていることを示す
                    onCeiling = true;

                    // velocityを0に
                    velocity = new();

                    // めり込みの解消
                    position = new(position.x, fieldBox.position.y - 1);
                }
            }
        }

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

            // ボタンが押されたら押されたタイミングを記録しておく
            if (input.upDown)
            {
                jumpButtonDownTime = Time.fixedTimeAsDouble;
                jump.Reset();
            }

            // 地面に触れている かつ ボタンを押されたタイミングが猶予いないなら jumpを呼ぶ
            if (onGround && (Time.fixedTimeAsDouble - jumpButtonDownTime) < jumpBuffer) jump.Invoke();
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