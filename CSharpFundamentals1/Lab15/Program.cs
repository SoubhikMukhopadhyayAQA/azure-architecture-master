namespace Lab15
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //----------------------------------------------------- Example 1
            Console.WriteLine("What's you name? ");
            var name = Console.ReadLine();

            //var array = new char[name.Length];
            //for(var i = name.Length - 1; i >= 0; i--)
            //    array[name.Length - 1 - i] = name[i];

            //var reversed = new string(array);

            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Name cannot be null or empty.");
                return;
            }

            var reversed = ReverseName(name);


            Console.WriteLine($"Reversed name, {reversed}!");


            //----------------------------------------------------- Example 2

            var numbers = new List<int>();

            while (true)
            {
                Console.Write("Enter a number (or 'Quit' to exit): ");
                var input = Console.ReadLine();

                if (input?.ToLower() == "quit")
                    break;

                numbers.Add(Convert.ToInt32(input));
            }
            //var unique = new List<int>();
            //foreach (var number in numbers)
            //{
            //    if (!unique.Contains(number))
            //        unique.Add(number);
            //}

            //var unique = GetUniqueNumbers(numbers);

            Console.WriteLine("Unique numbers:");
            foreach (var number in GetUniqueNumbers(numbers))
                Console.WriteLine(number);

        }
        //----------------------------------------------------- Example 1
        public static string ReverseName(string name)
        {
            var array = new char[name.Length];
            for (var i = name.Length - 1; i >= 0; i--)
                array[name.Length - 1 - i] = name[i];
            return new string(array);
        }
        //----------------------------------------------------- Example 2

        public static List<int> GetUniqueNumbers(List<int> numbers)
        {
            var unique = new List<int>();
            foreach (var number in numbers)
            {
                if (!unique.Contains(number))
                    unique.Add(number);
            }
            return unique;
        }
    }
}