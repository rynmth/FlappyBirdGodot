using Godot;

namespace FlappyBird
{
    public class LightFlash : Control
    {
        [Signal] public delegate void FlashInFinished();
        [Signal] public delegate void FlashOutFinished();
        private AnimationPlayer _animationPlayer;

        public override void _Ready()
        {
            _animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
        }

        public void FlashIn()
        {
            _animationPlayer.Play("FlashIn");
        }
        
        public void FlashOut()
        {
            _animationPlayer.Play("FlashOut");
        }

        private void _FlashInFinished()
        {
            EmitSignal(nameof(FlashInFinished));
        }

        private void _FlashOutFinished()
        {
            EmitSignal(nameof(FlashOutFinished));
        }
    }
}
