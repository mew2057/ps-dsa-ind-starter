namespace ConsoleCompare_Demo
{
    /// <summary>
    /// This project demos the use of Prof. Chris Cascioli's 
    /// new ConsoleCompare extension for VS 2022: https://github.com/vixorien/ConsoleCompare
    /// 
    /// To use it:
    ///  1. Download the vsix file from latest release: https://github.com/vixorien/ConsoleCompare/releases
    ///  2. Install the vsix file
    ///  3. In Visual Studio, go to Extensions -> Console Compare -> Open Console Compare Window
    ///  4. Load the .simile file in the Console Compare window
    ///  5. Click Run in the console compare window (the |> icon)
    /// 
    /// When possible, I'll give you .simile files to go with PEs and Homeworks so you can test your
    /// output against what we expect. These in NO WAY should replace you manually testing your
    /// work! The tests we give you will NOT cover every possible situation!
    /// 
    /// ConsoleCompare also checks how many classes and methods have XML headers like this
    /// (Create them by typing 3 slashes "///" on the line above a class or method declaration.)
    /// We WILL be using this when grading homeworks and practicals as a check for how many
    /// comment headers you have!
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main has some hard-coded output + output dependent on an input string.
        /// 
        /// It also prints a random number that might be outside the range the .simile
        /// file expects so that you can see what a failed test looks like.
        /// 
        /// Finally, it reads and lists the contents of a file (mostly to make sure
        /// file io works with the extension).
        /// 
        /// NOTE that ConsoleCompare is VERY picky about output matching exactly.
        /// Extra whitespace of any kind (new line, tabs vs spaces, etc.) will make
        /// the tests fail. And if the whitespace means input is requested on a line
        /// that doesn't have a {{ }} input marker in the .simile file, ConsoleCompare
        /// will hang.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Variables we'll need
            string name;
            StreamReader input = null;
            Random rng = new Random();

            // Greeting
            Console.WriteLine("Hello GDAPS!\n");

            // Get and use a name
            name = SmartConsole.GetPromptedInput("Who are you?");
            Console.WriteLine($"\nHello {name.ToUpper()}!\n" +
                "This is pretty cool!\n");

            // Should print out a random # from 7 to 42
            // Intentionally going WAY out of range so sometimes this
            // line fails the compare test
            Console.WriteLine("Your random ID is " + rng.Next(7, 100)+"\n");

            // Test file IO
            Console.WriteLine("InputFile contents:");
            try
            {
                input = new StreamReader("../../../InputFile.txt");
                string line = null;
                while((line = input.ReadLine()) != null)
                {
                    Console.WriteLine(" - " + line);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Uhoh: " + e.Message);
            }
            
        }
    }
}