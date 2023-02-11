using Godot;

namespace FlappyBird
{
    public class Background : Control
    {
        private TextureRect _backgroundDay;
        private TextureRect _backgroundNight;
        private AnimationPlayer _animationPlayer;

        public override void _Ready()
        {
            _backgroundDay = GetNode("BackgroundDay") as TextureRect;
            _backgroundNight = GetNode("BackgroundNight") as TextureRect;
            _animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
        }

        public void TimeToNightfall()
        {
            _animationPlayer.Play("Nightfall");
        }

        public void TimeToDawn()
        {
            _animationPlayer.Play("Dawn");
        }
    }
}