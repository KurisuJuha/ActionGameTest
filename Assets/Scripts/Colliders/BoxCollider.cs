using UnityEngine;

namespace ActionGameTest.Physics
{
    public class BoxCollider
    {
        /// <summary>
        /// 左下の位置
        /// </summary>
        public readonly Vector2 position;
        public readonly Vector2 size;
        public readonly Vector2 halfSize;
        public readonly Vector2 centerPosition;

        public BoxCollider(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
            this.halfSize = size / 2f;
            this.centerPosition = position + size / 2f;
        }

        public bool IsHit(BoxCollider otherCollider)
        {
            // x
            if (Mathf.Abs(centerPosition.x - otherCollider.centerPosition.x) > (halfSize.x + otherCollider.halfSize.x)) return false;

            // y
            if (Mathf.Abs(centerPosition.y - otherCollider.centerPosition.y) > (halfSize.y + otherCollider.halfSize.y)) return false;

            return true;
        }
    }
}
