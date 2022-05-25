public class ResizingData {
    private int minFontSize;
    public int MinFontSize {
        get => minFontSize;
        set {
            lock (@lock) {
                if (value < minFontSize) {
                    minFontSize = value;
                    OnChanged?.Invoke();
                }
            }
        }
    }

    private readonly object @lock = new object();
    public delegate void ChangeFontSize();
    public event ChangeFontSize OnChanged;

    public ResizingData(int minFontSize) {
        this.minFontSize = minFontSize;
    }
}