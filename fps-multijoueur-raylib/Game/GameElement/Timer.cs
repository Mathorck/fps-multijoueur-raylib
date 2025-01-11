namespace DeadOpsArcade3D.Game;

public class Timer
{
    private float time;
    private readonly float timeToWait;

    public Timer(float timeToWait)
    {
        this.timeToWait = timeToWait;
        time = 0;
        IsRunning = false;
        IsFinished = false; // Initialize IsFinished
    }

    public bool IsRunning { get; private set; }
    public bool IsFinished { get; private set; }

    // Start the timer
    public void Start()
    {
        IsRunning = true;
    }

    // Reset the timer
    public void Reset()
    {
        time = 0;
        IsFinished = false;
    }

    // Stop the timer
    public void Stop()
    {
        IsRunning = false;
    }

    // Update the timer
    public void Update()
    {
        if (IsRunning)
        {
            time += Raylib.GetFrameTime();
            if (time >= timeToWait)
            {
                IsRunning = false;
                IsFinished = true;
                time = 0;
            }
        }
    }
}