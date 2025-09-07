namespace Lab09
{
	public class Program
	{
        public static void Main(string[] args)
        {

            // Jagged Array
            int[][] jagged = new int[3][];
            jagged[0] = new int[2] { 1, 2 };
            jagged[1] = new int[3] { 3, 4, 5 };
            jagged[2] = new int[4] { 6, 7, 8, 9 };
            Console.WriteLine("Jagged Array");
            foreach (var item in jagged)
            {
                foreach (var i in item)
                {
                    Console.Write(i + " ");
                }
                Console.WriteLine();
            }
            // Rectangular Array
            int[,] rectangular = new int[3, 4]
            {
                {1,2,3,4 },
                {5,6,7,8 },
                {9,10,11,12 }
            };
            Console.WriteLine("Rectangular Array");
            for (int i = 0; i < rectangular.GetLength(0); i++)
            {
                for (int j = 0; j < rectangular.GetLength(1); j++)
                {
                    Console.Write(rectangular[i, j] + " ");
                }
                Console.WriteLine();
            }



            var number = new [] { 3,7,9,2,14,6 };

            // -------------------------------------------------Length
            Console.WriteLine("Length : " + number.Length);

            //number.IndexOf  // This is not a method of array class

            // -------------------------------------------------IndexOf()
            var index = Array.IndexOf(number, 9);
            Console.WriteLine("Index of 9 : " + index);

            // -------------------------------------------------Clear()
            Array.Clear(number, 0, 2);
            Console.WriteLine("Effect of Clear()");
            foreach (var item in number)
            {
                Console.WriteLine(item);
            }
            // -------------------------------------------------Copy()
            var another = new int[3];
            Array.Copy(number, another, 3);
            Console.WriteLine("Effect of Copy()");
            foreach (var item in another)
            {
                Console.WriteLine(item);
            }
            // -------------------------------------------------Sort()
            Array.Sort(number);
            Console.WriteLine("Effect of Sort()");
            foreach (var item in number)
            {
                Console.WriteLine(item);
            }
            // -------------------------------------------------Reverse()
            Array.Reverse(number);
            Console.WriteLine("Effect of Reverse()");
            foreach (var item in number)
            {
                Console.WriteLine(item);
            }
        }
    }
}