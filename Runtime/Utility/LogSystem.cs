using TeamZero.Core.Logging;

namespace TeamZero.SceneManagement
{
    public static class LogSystem
    {
        private static Log _log;
        public static Log Main => _log ?? Log.Default();

        public static void Init(Log log) => _log = log;
    }
}
