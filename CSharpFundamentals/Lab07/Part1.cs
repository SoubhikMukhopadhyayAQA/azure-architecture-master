namespace Lab06
{
    public class Part1
    {
        public static void Main(string[] args)
        {
            var random = new Random();

            //for (int i = 0; i < 10; i++)
            //    //Console.WriteLine(random.Next());
            //    Console.WriteLine(random.Next(1,10)); 




            //Console.WriteLine((int)'a');


            //for (int i = 0; i < 10; i++)
            //    Console.Write((char)random.Next(97, 122));

            //Console.WriteLine();




            //for (int i = 0; i < 10; i++)
            //    Console.Write((char)('a' + random.Next(0, 26)));

            //Console.WriteLine();

            const int passwordLength = 10;
            var buffer = new char[passwordLength];

            for (int i = 0; i < passwordLength; i++)
                buffer[i] = (char)('a' + random.Next(0, 26));

            var str = new string(buffer);
            Console.WriteLine(str);

        }
    }
}