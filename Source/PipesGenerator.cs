using Godot;

namespace FlappyBird
{
    public class PipesGenerator : Node2D
    {
        private Area2D _topPipe;
        private Area2D _bottomPipe;
        private Area2D _scoreArea;
        private Pipe _topPipeSprite;
        private Pipe _bottomPipeSprite;
        private AudioStreamPlayer _hitSound;
        private AudioStreamPlayer _fallSound;
        private AudioStreamPlayer _scoreSound;
        private RandomNumberGenerator _randomNumberGenerator;
        private const float _Speed = 178.8F;
        private const uint _PipeHalfWidth = 52 / 2;
        private const float _MaxHeight = 90.0F;
        private const float _MinHeight = -120.0F;

        public override void _Ready()
        {
            _randomNumberGenerator = new RandomNumberGenerator();
            _topPipe = GetNode("TopPipe") as Area2D;
            _bottomPipe = GetNode("BottomPipe") as Area2D;
            _scoreArea = GetNode("ScoreArea") as Area2D;
            _topPipeSprite = _topPipe.GetNode("Pipe") as Pipe;
            _bottomPipeSprite = _bottomPipe.GetNode("Pipe") as Pipe;
            _hitSound = GetNode("HitSound") as AudioStreamPlayer;
            _fallSound = GetNode("FallSound") as AudioStreamPlayer;
            _scoreSound = GetNode("ScoreSound") as AudioStreamPlayer;

            _topPipe.Connect("body_entered", this, nameof(_PlayerHitPipe));
            _bottomPipe.Connect("body_entered", this, nameof(_PlayerHitPipe));
            _scoreArea.Connect("body_entered", this, nameof(_PlayerScored));

            _randomNumberGenerator.Randomize();
            GlobalPosition += new Vector2(0, _randomNumberGenerator.RandfRange(_MinHeight, _MaxHeight));
        }

        public override void _Process(float delta)
        {
            if (GlobalPosition.x <= -_PipeHalfWidth)
            {
                QueueFree();
                SetProcess(false);
            }

            GlobalPosition -= new Vector2(_Speed * delta, 0);
        }

        public void GreenToRed()
        {
            _topPipeSprite.GreenToRed();
            _bottomPipeSprite.GreenToRed();
        }

        public void RedToGreen()
        {
            _topPipeSprite.RedToGreen();
            _bottomPipeSprite.RedToGreen();
        }

        public void ToRed()
        {
            _topPipeSprite.ToRed();
            _bottomPipeSprite.ToRed();
        }

        private void _PlayerHitPipe(Player player)
        {
            _hitSound.Play();
            _fallSound.Play();
            player.IsDead = true;
        }

        private void _PlayerScored(Player player)
        {
            _scoreSound.Play();
            player.Score++;
        }
    }
}