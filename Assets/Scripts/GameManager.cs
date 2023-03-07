using System.Collections.Generic;
using JuhaKurisu.PopoTools.Utility;
using ActionGameTest.Physics;
using UnityEngine;

namespace ActionGameTest
{
    public class GameManager : PopoBehaviour
    {
        private Box[] fieldBoxes;
        private Player player;
        private Input input;

        protected override void Start()
        {
            input = new();
            player = new(input, fieldBoxes);
        }

        protected override void FixedUpdate()
        {
            input.Update();
            player.Update();
        }
    }
}