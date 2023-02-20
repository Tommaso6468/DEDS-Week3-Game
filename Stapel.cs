namespace Main;

public class Stapel<T>
{
    private DataFragment<T>? top;

    public void Duw(ref T waarde)
    {
        top = new DataFragment<T> { data = waarde, volgende = top };
    }

    public T Pak()
    {
        if (top == null) return default;
        DataFragment<T> oudeTop = top;
        top = top.volgende;
        return oudeTop.data;
    }

    public class DataFragment<T> {
        public DataFragment<T>? volgende { get; set; }
        public T data { get; set; }
    }
}