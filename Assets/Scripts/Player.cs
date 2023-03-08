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
        private bool jumpBuffering;
        private bool coyoteTime;
        private bool onCeiling;
        private bool onGround;
        private bool onLeftWall;
        private bool onRightWall;
        private int wallJumpDirection;
        private float gravityVelocity;
        private double wallJumpStartTime;
        private double lastJumpButtonDownTime;
        private double lastOnGroundTime;

        private const float gravityScale = 0.02f;
        private const float moveSpeed = 0.15f;
        private const float jumpPower = 0.3f;
        private const double jumpBuffer = 0.18f;
        private const double coyoteTimeLength = 0.2f;
        private const double wallJumpTimeLength = 0.3f;
        private const float wallHangingFallSpeed = 0.1f;

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
            WallJump();
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
                    coyoteTime = true;
                    lastOnGroundTime = Time.fixedTimeAsDouble;

                    // gravityVelocityを0に
                    gravityVelocity = 0;

                    // めり込みの解消
                    position = new(position.x, fieldBox.position.y + fieldBox.size.y);
                }

                if (fieldBox.IsHit(upBox))
                {
                    // 天井に当たっていることを示す
                    onCeiling = true;

                    // gravityVelocityを0に
                    gravityVelocity = 0;

                    // めり込みの解消
                    position = new(position.x, fieldBox.position.y - 1);
                }
            }
        }

        private void CheckOnWall()
        {
            onRightWall = false;
            onLeftWall = false;
            Box leftBox = new(position + new Vector2(0, 0.05f), new(0, 0.8f));
            Box rightBox = new(position + new Vector2(1, 0.05f), new(0, 0.8f));

            foreach (var fieldBox in fieldBoxes)
            {
                if (fieldBox.IsHit(leftBox))
                {
                    // 左壁に当たっていることを示す
                    onLeftWall = true;

                    // めり込みの解消
                    position = new(fieldBox.position.x + fieldBox.size.x, position.y);
                }

                if (fieldBox.IsHit(rightBox))
                {
                    // 右壁に当たっていることを示す
                    onRightWall = true;

                    // めり込みの解消
                    position = new(fieldBox.position.x - 1, position.y);
                }
            }

            onGround = false;
        }

        private void Move()
        {
            if ((Time.fixedTimeAsDouble - wallJumpStartTime) < wallJumpTimeLength) return;

            int inputVec = (input.left ? -1 : 0) + (input.right ? 1 : 0);
            position += new Vector2(1, 0) * inputVec * moveSpeed;

            // ボタンが押されたら押されたタイミングを記録しておく
            if (input.jumpDown)
            {
                lastJumpButtonDownTime = Time.fixedTimeAsDouble;
                jumpBuffering = true;
            }
            if (input.jumpUp) jumpBuffering = false;

            // 地面に触れている かつ ボタンを押されたタイミングが猶予いないなら jumpを呼ぶ
            if (jumpBuffering && coyoteTime
                && (Time.fixedTimeAsDouble - lastJumpButtonDownTime) < jumpBuffer
                && (Time.fixedTimeAsDouble - lastOnGroundTime) < coyoteTimeLength)
            {
                jumpBuffering = false;
                coyoteTime = false;
                Jump();
            }
        }

        private void ApplyVelocity()
        {
            position += new Vector2(0, gravityVelocity);
        }

        private void AddGravityToVelocity()
        {
            gravityVelocity -= gravityScale;
        }

        private void WallJump()
        {
            int inputVec = (input.left ? -1 : 0) + (input.right ? 1 : 0);

            if ((onLeftWall || onRightWall) && !onGround)
            {
                // 左右キーのどちらかを押している かつ velocityが下向き なら壁すべり状態にする
                if (inputVec != 0 && gravityVelocity <= 0)
                {
                    gravityVelocity = 0;
                    position -= new Vector2(0, wallHangingFallSpeed);
                }

                // 壁つかまり状態にジャンプボタンを押したら壁と逆向きの方向にジャンプ
                if (input.jumpDown)
                {
                    wallJumpStartTime = Time.fixedTimeAsDouble;
                    wallJumpDirection = -((onLeftWall ? -1 : 0) + (onRightWall ? 1 : 0));
                    Jump();
                }
            }

            // 実際に動かす
            if ((Time.fixedTimeAsDouble - wallJumpStartTime) < wallJumpTimeLength)
            {
                // 逆向きの壁に当たっているならdirectionを0にする
                if ((wallJumpDirection == -1 && onLeftWall) || (wallJumpDirection == 1 && onRightWall)) wallJumpDirection = 0;
                position += new Vector2(moveSpeed * wallJumpDirection, 0);
            }
        }

        private void Jump()
        {
            gravityVelocity = jumpPower;
        }
    }
}