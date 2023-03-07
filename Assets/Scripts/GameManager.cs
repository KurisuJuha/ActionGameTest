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
        private GameInput input;

        [SerializeField] private Transform playerTransform;

        protected override void Start()
        {
            RegisterFieldBoxes();
            input = new();
            player = new(input, fieldBoxes);
        }

        protected override void FixedUpdate()
        {
            input.Update();
            player.Update();

            SetView();
        private void RegisterFieldBoxes()
        {
            List<Box> boxes = new();

            foreach (var fieldTransform in fieldTransforms)
            {
                boxes.Add(new(fieldTransform.position - fieldTransform.localScale / 2f, fieldTransform.localScale));
            }

            fieldBoxes = boxes.ToArray();
        }

        private void SetView()
        {
            playerTransform.position = player.position + new Vector2(0.5f, 0.5f);
        }
    }
}