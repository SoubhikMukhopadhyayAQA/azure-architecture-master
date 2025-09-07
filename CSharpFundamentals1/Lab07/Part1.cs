namespace Lab06
{
    public class Part1
    {
        public static void Main(string[] args)
        {
            var random = new Random();

            const int passwordLength = 10;
            var buffer = new char[passwordLength];

            for (int i = 0; i < passwordLength; i++)
                buffer[i] = (char)('a' + random.Next(0, 26));

            var str = new string(buffer);
            Console.WriteLine(str);



            //---------------------------------------------------------------------------------Exercise1
            int count = 0;
            for (int i = 1; i <= 100; i++)
            {
                if (i % 3 == 0)
                {
                    count++;
                }
            }
            Console.WriteLine("Total count : " + count);

            //---------------------------------------------------------------------------------Exercise2
            var sum = 0;
            while (true)
            {
                Console.WriteLine("Sum of all numbers is: " + sum);


                Console.Write("Enter a number (or 'ok' to exit): ");
                var input = Console.ReadLine();

                if (input.ToLower() == "ok")
                    break;

                sum += Convert.ToInt32(input);
            }
            Console.WriteLine("Sum of all numbers is: " + sum);

            //---------------------------------------------------------------------------------Exercise3

            Console.Write("Enter a number to calculate its factorial: ");
            var number = Convert.ToInt32(Console.ReadLine());
            var factorial = 1;
            for (var i = 1; i <= number; i++)
                factorial *= i;
            Console.WriteLine("Factorial of " + number + " is " + factorial);

            //---------------------------------------------------------------------------------Exercise4
            var random1 = new Random();
            var randomNumber = random1.Next(1, 10);
            var attempts = 4;
            for (var i = 0; i < attempts; i++)
            {
                Console.Write("Guess the number between 1 and 10: ");
                var guess = Convert.ToInt32(Console.ReadLine());
                if (guess == randomNumber)
                {
                    Console.WriteLine("You won!");
                    break;
                }
            }

            //---------------------------------------------------------------------------------Exercise5
            Console.Write("Enter commoa separated numbers: ");
            var input2 = Console.ReadLine();

            var numbers = input2.Split(',');

            // Assume the first number is the max 
            var max = Convert.ToInt32(numbers[0]);

            foreach (var str2 in numbers)
            {
                var number2 = Convert.ToInt32(str2);
                if (number2 > max)
                    max = number2;
            }

            Console.WriteLine("Max is " + max);
        }
    }
}