using Godot;

namespace FlappyBird
{
    public class GetReady : Control
    {
        [Signal] public delegate void Playing();
        private AnimationPlayer _animationPlayer;
        private bool _fading;

        public override void _Ready()
        {
            _animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
            _fading = false;
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseButton button) if (button.Pressed && !_fading)
            {
                _animationPlayer.Play("FadeIn");
                _fading = true;
            }
            else if (@event is InputEventScreenTouch @touch) if (@touch.Pressed && !_fading)
            {
                _animationPlayer.Play("FadeIn");
                _fading = true;
            }
        }

        private void _FadingFinished()
        {
            EmitSignal(nameof(Playing));
            QueueFree();
        }
    }
}
