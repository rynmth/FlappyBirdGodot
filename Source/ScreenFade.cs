using Godot;

namespace FlappyBird
{
    public class ScreenFade : Control
    {
        [Signal] public delegate void FadeInFinished();
        [Signal] public delegate void FadeOutFinished();
        private AnimationPlayer _animationPlayer;

        public override void _Ready()
        {
            _animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
        }

        public void FadeIn()
        {
            _animationPlayer.Play("FadeIn");
        }

        public void FadeOut()
        {
            _animationPlayer.Play("FadeOut");
        }

        private void _FadeInFinished()
        {
            EmitSignal(nameof(FadeInFinished));
        }

        private void _FadeOutFinished()
        {
            EmitSignal(nameof(FadeOutFinished));
        }
    }
}
