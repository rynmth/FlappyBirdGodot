using Godot;
using static Godot.GD;

namespace FlappyBird
{
    public class Game : Node2D
    {
        private Title _title;
        private ScreenFade _screenFade;
        private AudioStreamPlayer _swooshSound;
        private Timer _pipeTimer;
        private Ground _ground;
        private Node2D _pipes;
        private CanvasLayer _ui;
        private PackedScene _backgroundScene;
        private Background _background;
        private PackedScene _playerScene;
        private Player _player;
        private PackedScene _pipesGeneratorScene;
        private PackedScene _getReadyScene;
        private GetReady _getReady;
        private PackedScene _scoreDisplayScene;
        private ScoreDisplay _scoreDisplay;
        private PackedScene _lightFlashScene;
        private LightFlash _lightFlash;
        private PackedScene _gameOverScene;
        private GameOver _gameOver;
        private uint _currentCycle;
        private bool _isNight;
        private const uint _DayCycleScore = 25;
        private const float _DefaultGroundSpeed = 0.5F;

        public override void _Ready()
        {
            _title = GetNode("UI/Title") as Title;
            _screenFade = GetNode("UI/ScreenFade") as ScreenFade;
            _swooshSound = GetNode("SwooshSound") as AudioStreamPlayer;
            _pipeTimer = GetNode("PipeTimer") as Timer;
            _ground = GetNode("Ground") as Ground;
            _pipes = GetNode("Pipes") as Node2D;
            _ui = GetNode("UI") as CanvasLayer;
            _backgroundScene = Load("res://UI/Background.tscn") as PackedScene;
            _playerScene = Load("res://Scenes/Player.tscn") as PackedScene;
            _pipesGeneratorScene = Load("res://Scenes/PipesGenerator.tscn") as PackedScene;
            _getReadyScene = Load("res://UI/GetReady.tscn") as PackedScene;
            _scoreDisplayScene = Load("res://UI/ScoreDisplay.tscn") as PackedScene;
            _lightFlashScene = Load("res://UI/LightFlash.tscn") as PackedScene;
            _gameOverScene = Load("res://UI/GameOver.tscn") as PackedScene;
            _currentCycle = 1;
            _isNight = false;

            _title.Connect(nameof(Title.GameStarted), this, nameof(_GameStarted));
            _screenFade.Connect(nameof(ScreenFade.FadeInFinished), this, nameof(_FadeInFinishedToGame));
            _pipeTimer.Connect("timeout", this, nameof(_CreatePipe));
        }

        private void _GameStarted()
        {
            _swooshSound.Play();
            _screenFade.FadeIn();
        }

        private void _FadeInFinishedToGame()
        {
            _player = _playerScene.Instance() as Player;
            _player.Connect(nameof(Player.Died), this, nameof(_PlayerDied));
            _player.Connect(nameof(Player.Scored), this, nameof(_PlayerScored));
            AddChild(_player);

            _background = _backgroundScene.Instance() as Background;
            AddChild(_background);

            _scoreDisplay = _scoreDisplayScene.Instance() as ScoreDisplay;
            _ui.AddChild(_scoreDisplay);
            
            _getReady = _getReadyScene.Instance() as GetReady;
            _getReady.Connect(nameof(GetReady.Playing), this, nameof(_Playing));
            _ui.AddChild(_getReady);
            
            _lightFlash = _lightFlashScene.Instance() as LightFlash;
            _lightFlash.Connect(nameof(LightFlash.FlashInFinished), this, nameof(_FlashInFinished));
            _lightFlash.Connect(nameof(LightFlash.FlashOutFinished), this, nameof(_FlashOutFinished));
            _ui.AddChild(_lightFlash);
  
            _title.Visible = false;
            _screenFade.FadeOut();
        }

        private void _Playing()
        {
            _pipeTimer.Start();
        }

        private void _CreatePipe()
        {
            var pipe = _pipesGeneratorScene.Instance() as PipesGenerator;

            _pipes.AddChild(pipe);
            if (_isNight)
            {
                pipe.ToRed();
            }
        }

        private void _PlayerScored()
        {
            var score = _player.Score;

            _scoreDisplay.Score = score;
            if (score / _currentCycle == _DayCycleScore)
            {
                _currentCycle++;
                _isNight = !_isNight;

                if (_isNight)
                {
                    _background.TimeToNightfall();
                
                    foreach (PipesGenerator pipe in _pipes.GetChildren())
                    {
                        pipe.GreenToRed();
                    }
                }
                else
                {
                    _background.TimeToDawn();

                    foreach (PipesGenerator pipe in _pipes.GetChildren())
                    {
                        pipe.RedToGreen();
                    }
                }
            }
        }

        private void _PlayerDied()
        {
            _pipeTimer.Stop();
            _lightFlash.FlashIn();
            _isNight = false;
            _currentCycle = 1;
        }

        private void _FlashInFinished()
        {
            foreach (PipesGenerator pipe in _pipes.GetChildren())
            {
                pipe.SetProcess(false);
            }

            _ground.AnimationSpeed = 0;
            _scoreDisplay.QueueFree();
            _lightFlash.FlashOut();
        }

        private void _FlashOutFinished()
        {
            _gameOver = _gameOverScene.Instance() as GameOver;
            _gameOver.Connect(nameof(GameOver.Continue), this, nameof(_ContinueToTitle));
            _ui.AddChild(_gameOver);
            _ui.MoveChild(_gameOver, 0);

            ScoreStorage.Score = _player.Score;
            _gameOver.Score = ScoreStorage.Score;
            _gameOver.Best = ScoreStorage.BestScore;

            _lightFlash.QueueFree();
        }

        private void _ContinueToTitle()
        {
            _screenFade.Disconnect(nameof(ScreenFade.FadeInFinished), this, nameof(_FadeInFinishedToGame));
            _screenFade.Connect(nameof(ScreenFade.FadeInFinished), this, nameof(_FadeInFinishedToTile));

            _swooshSound.Play();
            _screenFade.FadeIn();
        }

        private void _FadeInFinishedToTile()
        {
            _screenFade.Disconnect(nameof(ScreenFade.FadeInFinished), this, nameof(_FadeInFinishedToTile));
            _screenFade.Connect(nameof(ScreenFade.FadeInFinished), this, nameof(_FadeInFinishedToGame));
            
            _gameOver.QueueFree();
            _player.QueueFree();
            _title.Visible = true;

            foreach (PipesGenerator pipe in _pipes.GetChildren())
            {
                pipe.QueueFree();
            }

            _ground.AnimationSpeed = _DefaultGroundSpeed;
            _screenFade.FadeOut();
        }
    }
}