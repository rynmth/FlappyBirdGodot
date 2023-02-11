using Godot;

namespace FlappyBird
{
    static class ScoreStorage
    {
        public static uint BestScore { get; private set; }
        private static uint _score;

        static ScoreStorage()
        {
            var file = new File();

            var error = file.OpenCompressed("user://data.dat", File.ModeFlags.Read, File.CompressionMode.Deflate);
            if (error == Error.Ok)
            {
                BestScore = file.Get32();
            }
            else
            {
                file.OpenCompressed("user://data.dat", File.ModeFlags.Write, File.CompressionMode.Deflate);
                file.Store32(_score);
                BestScore = _score;
            }

            file.Close();
        }

        public static uint Score
        {
            get { return _score; }
            set
            {
                _score = value;
                if (_score > BestScore)
                {
                    var file = new File();

                    file.OpenCompressed("user://data.dat", File.ModeFlags.Write, File.CompressionMode.Deflate);
                    file.Store32(value);
                    BestScore = value;

                    file.Close();
                }
            }
        }
    }
}