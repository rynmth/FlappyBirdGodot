using Godot;

namespace FlappyBird
{
    public class GameOver : Control
    {
        [Signal] public delegate void Continue();
        private AnimationPlayer _animationPlayer;
        private Label _currentScore;
        private Label _bestScore;
        private Button _continue;

        public uint Score
        {
            set
            {
                _currentScore.Text = value.ToString();
            }
        }

        public uint Best
        {
            set
            {
                _bestScore.Text = value.ToString();
            }
        }

        public override void _Ready()
        {
            var containerPath = "Results/MarginContainer/PanelContainer/MarginContainer/VBoxContainer/";

            _currentScore = GetNode(containerPath + "VBoxContainer/CurrentScore") as Label;
            _bestScore = GetNode(containerPath + "VBoxContainer2/BestScore") as Label;
            _animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
            _continue = GetNode("Results/Continue") as Button;

            _continue.Connect("pressed", this, nameof(_ContinuePressed));
        }

        private void _ContinuePressed()
        {
            EmitSignal(nameof(Continue));
        }
    }
}