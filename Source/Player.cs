using Godot;

namespace FlappyBird
{
    public class Player : KinematicBody2D
    {
        [Signal] public delegate void Died();
        [Signal] public delegate void Scored();
        private AnimatedSprite _birds;
        private AudioStreamPlayer _wingSound;
        private Tween _deathTween;
        private CollisionShape2D _collision;
        private RandomNumberGenerator _randomNumberGenerator;
        private Vector2 _velocity;
        private bool _flap;
        private bool _isDead;
        private uint _score;
        private bool _waitingClick;
        private const float _Gravity = 572.2F;
        private const float _FlapHeight = 232.42F;
        private const float _RotationSpeed = 2.11F;

        public bool IsDead
        {
            get { return _isDead; }
            set
            {
                _isDead = value;
                if (_isDead)
                {
                    SetPhysicsProcess(false);
                    _collision.SetDeferred("disabled", true);
                    _birds.Playing = false;

                    var tween = CreateTween().SetParallel(true);

                    tween.TweenProperty(this, "global_position:y", 430.0F, 0.3F);
                    tween.TweenProperty(this, "rotation", Mathf.Deg2Rad(90), 0.2F);
                    EmitSignal(nameof(Died));
                }
            }
        }

        public uint Score
        {
            get { return _score; }
            set
            {
                _score = value;
                EmitSignal(nameof(Scored)); 
            }
        }

        public bool WaitingClick
        {
            get { return _waitingClick; }
            set
            {
                _waitingClick = value;
                SetPhysicsProcess(!_waitingClick);
            }
        }

        public override void _Ready()
        {
            _randomNumberGenerator = new RandomNumberGenerator();
            _birds = GetNode("Birds") as AnimatedSprite;
            _wingSound = GetNode("WingSound") as AudioStreamPlayer;
            _deathTween = GetNode("DeathTween") as Tween;
            _collision = GetNode("Collision") as CollisionShape2D;
            _velocity = Vector2.Zero;
            _flap = false;
            _isDead = false;
            _score = 0;
            WaitingClick = true;

            _randomNumberGenerator.Randomize();
            switch (_randomNumberGenerator.RandiRange(0, 2))
            {
                case 0:
                    _birds.Animation = "YellowBird";
                    break;

                case 1:
                    _birds.Animation = "RedBird";
                    break;

                case 2:
                    _birds.Animation = "BlueBird";
                    break;
            }
        }

        private void Flap()
        {
            _wingSound.Play();
            _velocity.y = -_FlapHeight;
            RotationDegrees = -45;
        }

        public override void _PhysicsProcess(float delta)
        {
            _velocity.y += _Gravity * delta;

            if (_flap)
            {
                _flap = false;
                Flap();
            }

            Rotation += _RotationSpeed * delta;
            Rotation = Mathf.Clamp(Rotation, Mathf.Deg2Rad(-45), Mathf.Deg2Rad(90));

            MoveAndSlide(_velocity);
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseButton @button)
            {
                if (@button.Pressed && WaitingClick)
                {
                    WaitingClick = false;
                    Flap();
                }
                else if (@button.Pressed)
                {
                    _flap = true;
                }
            }
            else if (@event is InputEventScreenTouch @touch)
            {
                if (@touch.Pressed && WaitingClick)
                {
                    WaitingClick = false;
                    Flap();
                }
                else if (@touch.Pressed)
                {
                    _flap = true;
                }
            }
        }
    }
}