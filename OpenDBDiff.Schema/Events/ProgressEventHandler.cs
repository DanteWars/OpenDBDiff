namespace OpenDBDiff.Schema.Events
{
    public class ProgressEventHandler
    {
        public delegate void ProgressHandler(ProgressEventArgs e);

        public static event ProgressHandler OnProgress;

        public static void RaiseOnChange(ProgressEventArgs e)
        {
            OnProgress?.Invoke(e);
        }
    }
}
