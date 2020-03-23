namespace UI
{
    public interface ILineHighlighter
    {
        void SetLineColor(int lineIndex);
        void ResetColorLines();

        void Refresh();
    }
}