using System;
using System.Threading;
 
namespace WidgetToolkit
{
    class ConfigurableCLUISpinner : IDisposable
    {
        /*
        private int _currentAnimationFrame;
 
        public CLUISpinner()
        {
            SpinnerAnimationFrames = new[]
                                     {
                                         '|',
                                         '/',
                                         '-',
                                         '\\'
                                     };
        }
 
        public char[] SpinnerAnimationFrames { get; set; }
 
        public void UpdateProgress()
        {
	        // Console.CursorVisible = false;
            // Store the current position of the cursor
            var originalX = Console.CursorLeft;
            var originalY = Console.CursorTop;
 
            // Write the next frame (character) in the spinner animation
            Console.Write(SpinnerAnimationFrames[_currentAnimationFrame]);
 
            // Keep looping around all the animation frames
            _currentAnimationFrame++;
            if (_currentAnimationFrame == SpinnerAnimationFrames.Length)
            {
                _currentAnimationFrame = 0;
            }
 
            // Restore cursor to original position
            Console.SetCursorPosition(originalX, originalY);
        }

	    public void Stop(){
		    Console.CursorVisible = true;
        }

        public void Dispose()
        {
            Stop();
        }

        */
        private string Sequence = "";

        private int SEQlength = 0;
        private int counter = 0;
        private readonly int left;
        private readonly int top;
        private readonly int delay;
        private bool active;
        private readonly Thread thread;

        public ConfigurableCLUISpinner(int left = 0, int top = 0, int delay = 100, string sequent = @"/-\|")
        {
            SEQlength = sequent.Length;

            if (SEQlength != 4)
            {
                throw new IndexOutOfRangeException("Invalid spinner design, 4 charcters permited in spinner design");
            }

            Sequence = sequent;
            this.left = left;
            this.top = top;
            this.delay = delay;
            thread = new Thread(Spin);
        }

        public void Start()
        {
           active = true;
           if (!thread.IsAlive)
              thread.Start();
        }

        public void Stop()
        {
           active = false;
           Console.ForegroundColor = ConsoleColor.White;
           Draw(' ');
        }

        private void Spin()
        {
           while (active)
           {
              Turn();
              Thread.Sleep(delay);
           }
        }

        private void Draw(char c)
        {
           Console.SetCursorPosition(left, top);
           Console.ForegroundColor = ConsoleColor.Green;
           Console.Write(c);
        }

        private void Turn()
        {
           Draw(Sequence[++counter % Sequence.Length]);
        }

        public void UpdateProgress()
        {
            Draw(Sequence[++counter % Sequence.Length]);
        }

        public void Dispose()
        {
           Stop();
        }

	}

    class CLUISpinner : IDisposable
    {
        private int _currentAnimationFrame;

        private int SEQLength;

        private int delay;

        private readonly Thread thread;
 
        public CLUISpinner(string SEQFrame = @"/-\|", int delay = 100)
        {

            SEQLength = SEQFrame.Length;
            if(SEQLength != 4)
            {
                throw new IndexOutOfRangeException("Invalid spinner design, 4 charcters permited in spinner design");
            }

            this.delay = delay;

            SpinnerAnimationFrames = new[]
                                     {
                                         SEQFrame[0],
                                         SEQFrame[1],
                                         SEQFrame[2],
                                         SEQFrame[3]
                                     };

            thread = new Thread(UpdateProgress);
        }
 
        public char[] SpinnerAnimationFrames { get; set; }
 
        public void UpdateProgress()
        {
            // Store the current position of the cursor
            var originalX = Console.CursorLeft;
            var originalY = Console.CursorTop;
 
            // Write the next frame (character) in the spinner animation
            Console.Write(SpinnerAnimationFrames[_currentAnimationFrame]);
 
            // Keep looping around all the animation frames
            _currentAnimationFrame++;
            if (_currentAnimationFrame == SpinnerAnimationFrames.Length)
            {
                _currentAnimationFrame = 0;
            }
 
            // Restore cursor to original position
            Console.SetCursorPosition(originalX, originalY);
        }

        public void Start()
        {
            if (!thread.IsAlive)
              thread.Start();
        }

        public void Stop()
        {
        }

        public void Dispose()
        {
            Stop();
        }
    }
}