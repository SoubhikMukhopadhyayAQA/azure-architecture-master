namespace Lab17
{
    class Program
    {
        public static void Main(string[] args)
        {
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6 };
            var smallests = GetSmallests(numbers, 3);

            foreach (var number in smallests)
                Console.WriteLine(number);
        }

        public static List<int> GetSmallests(List<int> list, int count)
        {
            if (list == null)
                throw new ArgumentNullException("list", "List cannot be null.");  // Added null check for the list (EDGE CASE)

            if (count > list.Count || count <= 0 )
                throw new ArgumentOutOfRangeException("count", "Count must be between 1 and the size of the list."); // Added check for count being less than or equal to 0 (EDGE CASE)

            var buffer = new List<int>(list); // Create a copy of the list to avoid modifying the original


            var smallests = new List<int>();

            while (smallests.Count < count)
            {
                var min = GetSmallest(buffer);
                smallests.Add(min);
                buffer.Remove(min);
            }

            return smallests;
        }

        public static int GetSmallest(List<int> list)
        {
            // Assume the first number is the smallest
            var min = list[0];
            for (var i = 1; i < list.Count; i++)
            {
                if (list[i] < min)    // Bug fix: changed from '>' to '<'
                    min = list[i];
            }
            return min;
        }
    }
}