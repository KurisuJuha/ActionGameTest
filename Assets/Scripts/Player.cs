using System.Collections.ObjectModel;
using ActionGameTest.Physics;

namespace ActionGameTest
{
    public class Player
    {
        private readonly Input input;
        private readonly ReadOnlyCollection<Box> fieldBoxes;

        public Player(Input input, Box[] fieldBoxes)
        {
            this.input = input;
            this.fieldBoxes = new(fieldBoxes);
        }

        public void Update()
        {

        }
    }
}