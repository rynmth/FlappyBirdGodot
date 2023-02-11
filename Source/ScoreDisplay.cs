using Godot;

namespace FlappyBird
{
    public class ScoreDisplay : Control
    {
        private Label _currentScore;

        public uint Score
        {
            set
            {
                _currentScore.Text = value.ToString();
            }
        }

        public override void _Ready()
        {
            _currentScore = GetNode("MarginContainer/CurrentScore") as Label;
        }
    }
}
