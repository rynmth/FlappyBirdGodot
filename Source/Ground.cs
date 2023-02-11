using Godot;

namespace FlappyBird
{
    public class Ground : Area2D
    {
        private AudioStreamPlayer _hitSound;
        private Sprite _groundSprite;

        public float AnimationSpeed
        {
            set
            {
                var material = _groundSprite.Material as ShaderMaterial;

                material.SetShaderParam("speed", value);
            }
        }

        public override void _Ready()
        {
            _hitSound = GetNode("HitSound") as AudioStreamPlayer;
            _groundSprite = GetNode("GroundSprite") as Sprite;

            Connect("body_entered", this, nameof(_PlayerEntered));
        }

        private void _PlayerEntered(Player player)
        {
            _hitSound.Play();
            player.IsDead = true;
        }
    }
}
