using Godot;

namespace FlappyBird
{
    public class Title : Control
    {
        [Signal] public delegate void GameStarted();
        private Button _startGame;

        public override void _Ready()
        {
            _startGame = GetNode("MarginContainer/CenterContainer/StartGame") as Button;
            _startGame.Connect("pressed", this, nameof(_StartPressed));
        }

        private void _StartPressed()
        {
            EmitSignal(nameof(GameStarted));
        }
    }
}