using Godot;

namespace FlappyBird
{
    public class Pipe : Node2D
    {
        private AnimationPlayer _animationPlayer;

        public override void _Ready()
        {
            _animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
        }

        public void GreenToRed()
        {
            _animationPlayer.Play("GreenToRed");
        }

        public void RedToGreen()
        {
            _animationPlayer.Play("RedToGreen");
        }

        public void ToRed()
        {
            _animationPlayer.Play("Red");
        }
    }
}
