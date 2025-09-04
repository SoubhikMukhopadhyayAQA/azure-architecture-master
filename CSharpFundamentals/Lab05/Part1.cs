namespace Lab05
{
    class Part1
    {
        static void Main(string[] args)
        {
            //-------------------------------------------------------------- case 1
            Console.WriteLine("Please Enter the Number");
            int number = Convert.ToInt32(Console.ReadLine());


            if (number >= 1 && number <= 10)
            {
                Console.WriteLine("Valid No");
            }
            else
            {
                Console.WriteLine("Invalid No");
            }


            //-------------------------------------------------------------- case 2
            Console.WriteLine("Please provide two numbers");
            int number1 = Convert.ToInt32(Console.ReadLine());
            int number2 = Convert.ToInt32(Console.ReadLine());


            if (number1 > number2)
            {
                Console.WriteLine("First Number is Greater");
            }
            else if (number1 < number2)
            {
                Console.WriteLine("Second Number is Greater");
            }
            else
            {
                Console.WriteLine("Both are Equal");
            }


            //-------------------------------------------------------------- case 3

            Console.WriteLine("Please provide the width and height of the image");
            int width = Convert.ToInt32(Console.ReadLine());
            int height = Convert.ToInt32(Console.ReadLine());
            if (width > height)
            {
                Console.WriteLine("Landscape");
            }
            else
            {
                Console.WriteLine("Portrait");
            }



            //-------------------------------------------------------------- case 4


            Console.WriteLine("Please provide the speed limit and the speed of the car");
            int speedLimit = Convert.ToInt32(Console.ReadLine());
            int carSpeed = Convert.ToInt32(Console.ReadLine());
            if (carSpeed <= speedLimit)
            {
                Console.WriteLine("Ok");
            }
            else
            {
                int demeritPoints = (carSpeed - speedLimit) / 5;
                if (demeritPoints > 12)
                {
                    Console.WriteLine("License Suspended");
                }
                else
                {
                    Console.WriteLine("Demerit Points: " + demeritPoints);
                }
            }
        }
    }
}