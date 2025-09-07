namespace Lab4
{
    partial class Part1
    {
        static void Main(string[] args)
        {
            int hour = 10;
            if (hour > 0 && hour < 12)
            {
                Console.WriteLine("It is morning");
            }
            else if (hour >= 12 && hour < 18)
            {
                Console.WriteLine("It is afternoon");
            }
            else
            {
                Console.WriteLine("It is evening");
            }



            bool isGoldCustomer = true;
            //float price;

            //if (isGoldCustomer)
            //    price = 19.95f;
            //else
            //    price = 29.95f;

            float finalPrice = (isGoldCustomer) ? 19.95f : 29.95f;
            Console.WriteLine(finalPrice);



            var season = Season.Autumn;

            switch (season)
            {
                case Season.Autumn:
                case Season.Spring:
                    Console.WriteLine("It's beautiful season");
                    break;
                case Season.Summer:
                    Console.WriteLine("It's Summer and it's hot outside");
                    break;
                case Season.Winter:
                    Console.WriteLine("It's Winter and it's snowing");
                    break;
                default:
                    Console.WriteLine("I don't understand that season!");
                    break;
            }
        }
    }
}