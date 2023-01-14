// CHANGE the namespace to match the one in your project!
namespace ConsoleCompare_Demo
{
    /// <summary>
    /// Author: Erika S. Mesh. Demo/helper code written for RIT GDAPS 1 and 2.
    /// 
    /// The SmartConsole class provides "helper" methods to perform common
    /// behaviors for us. This keeps code elsewhere in the project simpler.
    /// 
    /// All of the methods in this class are defined as "static". This isn't 
    /// a keyword we discussed in GDAPS1 and it won't be on the exam, but 
    /// it's worth being aware of.
    /// 
    /// Static methods are defined in a class, but don't require a specific 
    /// object (instance of the class) in order to execute them. Instead,
    /// we call them using the class type. I.e. SmartConsole.<method name>.
    /// 
    /// This is how the Math and Console methods you are used to using work.
    /// </summary>
    class SmartConsole
    {
        private static ConsoleColor DefaultColor = ConsoleColor.White;
        private static ConsoleColor InputColor = ConsoleColor.Cyan;


        /// <summary>
        /// This enum is defined *inside* the class as private
        /// because I don't intend for any other class to use it.
        /// It's a "helper" for the GetYesNoInput.
        /// </summary>
        private enum YesNoParse
        {
            Yes,
            No,
            Invalid
        }

        /// <summary>
        /// Displays the given prompt, waits for input, and
        /// returns the trimmed input.
        /// </summary>
        /// <param name="prompt">A string prompt to display to the user.</param>
        /// <returns>Trimmed string</returns>
        public static string GetPromptedInput(string prompt)
        {
            Console.ForegroundColor = DefaultColor;
            Console.Write(prompt + " ");
            Console.ForegroundColor = InputColor;
            string input = Console.ReadLine().Trim();
            Console.ForegroundColor = DefaultColor;
            return input;
        }

        /// <summary>
        /// Displays the given prompts, waits for input, and
        /// tries to parse the input as an int. Loops until the
        /// provided input is a valid int within the range given (inclusive)
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetIntRangeInput(string prompt, int min, int max)
        {
            int result = int.MinValue;
            bool success = int.TryParse(GetPromptedInput(prompt), out result);
            while (!success || result < min || result > max)
            {
                success = int.TryParse(
                    GetPromptedInput(String.Format("Please enter a whole number in the range {0}-{1}: ", min, max)), 
                    out result);
            }
            return result;
        }

        /// <summary>
        /// Displays the given prompts, waits for input, and
        /// tries to parse the input as a double. Loops until the
        /// provided input is a valid double within the range given (inclusive)
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double GetDoubleRangeInput(string prompt, double min, double max)
        {
            double result = double.MinValue;
            bool success = double.TryParse(GetPromptedInput(prompt), out result);
            while (!success || result < min || result > max)
            {
                success = double.TryParse(
                    GetPromptedInput(String.Format("Please enter a whole number in the range {0}-{1}: ", min, max)),
                    out result);
            }
            return result;
        }

        /// <summary>
        /// Internal helper method to convert a string input to
        /// our enum value. Not really neccessary (a compound if with strings
        /// would be fine), but I wanted to now this switch statement.
        /// </summary>
        /// <param name="choice"></param>
        /// <returns></returns>
        private static YesNoParse ParseYesNoChoice(string choice)
        {
            choice = choice.ToLower();
            YesNoParse result = YesNoParse.Invalid;
            switch (choice)
            {
                case "yes":
                case "y":
                    result = YesNoParse.Yes;
                    break;

                case "no":
                case "n":
                    result = YesNoParse.No;
                    break;

                default:
                    Console.WriteLine("Please enter Yes, No, Y or N.");
                    break;
            }
            return result;
        }

        /// <summary>
        /// Display the given prompt and return a bool based on a
        /// yes/y/no/n input (and doesn't take anything else)
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns>True if yes</returns>
        public static bool GetYesNoInput(string prompt)
        {
            string choice = GetPromptedInput(prompt);
            YesNoParse result = ParseYesNoChoice(choice);
            while (result == YesNoParse.Invalid)
            {
                choice = GetPromptedInput(prompt);
                result = ParseYesNoChoice(choice);
            }
            return result == YesNoParse.Yes;
        }
    }
}
