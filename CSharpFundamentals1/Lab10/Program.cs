namespace Lab10
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var numbers = new List<int>();
            //var nameList = new List<string>();
            //var charetcers = new List<char>();

            var numbers = new List<int>() { 1, 2, 3, 4, 5 };

            // add numbers to the list
            numbers.Add(1);
            
            numbers.AddRange(new int[3] { 7, 8, 9 });

            foreach (var number in numbers)
            {
                Console.WriteLine(number);
            }


            Console.WriteLine("Index of number of 1: " + numbers.IndexOf(1));
            Console.WriteLine("Index of last number of 1: " + numbers.LastIndexOf(1));
            Console.WriteLine("Count : " + numbers.Count);

            //numbers.Remove(1); //---------------------------------------------- only removed initial no 1 , not second one

            //foreach (var number in numbers)
            //{
            //    if(number == 1)
            //    {
            //        numbers.Remove(1); //------------------------------------- in loop modification not allowed
            //    }
            //}

            for(var i=0; i<numbers.Count; i++)
            {
                if(numbers[i] == 1)
                {
                    numbers.RemoveAt(i);
                    i--; // to adjust the index after removal
                }
            }

            foreach (var number in numbers)
            {
                Console.WriteLine(number);
            }


            numbers.Clear();
            Console.WriteLine("Count : " + numbers.Count);
        }
    }
}