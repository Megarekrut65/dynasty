
public class ResizingData{
    private int minFontSize;
    public int MinFontSize{
        get{
            return minFontSize;
        }
        set{
            lock (_lock){
                if(value < minFontSize){
                    minFontSize = value;
                    OnChanged?.Invoke();
                }
            }
        }
    }
    public readonly object _lock = new object();
    public delegate void ChangeFontSize();
    public event ChangeFontSize OnChanged;
    public ResizingData(int minFontSize){
        this.minFontSize = minFontSize;
    }
}