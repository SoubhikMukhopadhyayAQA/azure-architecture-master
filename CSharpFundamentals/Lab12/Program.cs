namespace Lab12
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //DateTime startTime = DateTime.Now;
            var  dateTime = new DateTime(2025, 09, 06, 0, 0, 0);
            var now = DateTime.Now;
            var today = DateTime.Today; 
            
            Console.WriteLine("Hours : "+now.Hour);
            Console.WriteLine("Minutes : " + now.Minute);


            //var newTime = now.AddHours(2);
            var newDate = dateTime.AddDays(10);
            var newTime = now.AddMinutes(30).AddHours(2);
            Console.WriteLine("After adding 2 hours and 30 minutes : " + newTime);


            Console.WriteLine(now.ToLongDateString());
            Console.WriteLine(now.ToShortDateString());
            Console.WriteLine(now.ToLongTimeString());
            Console.WriteLine(now.ToShortTimeString());
            Console.WriteLine(now.ToString("yyyy-MM-dd HH:mm:ss.ff tt"));



            var timeSpan = new TimeSpan(1, 2, 3);
            var timeSpan1 = new TimeSpan(1, 0, 0);
            var timeSpan2 = TimeSpan.FromHours(1);

            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddMinutes(2);

            var duration = endTime - startTime;
            Console.WriteLine("Duration : " + duration);



            // Properties of TimeSpan

            Console.WriteLine("Minutes: " + timeSpan.Minutes);
            Console.WriteLine("Total Minutes: " + timeSpan.TotalMinutes);

            // Operation on TimeSpans
            Console.WriteLine("Add TimeSpan: " + timeSpan.Add(timeSpan1));
            Console.WriteLine("Subtract TimeSpan: " + timeSpan.Subtract(timeSpan1));
            Console.WriteLine("Multiply TimeSpan: " + timeSpan.Multiply(2));
            Console.WriteLine("Divide TimeSpan: " + timeSpan.Divide(2));
            Console.WriteLine("ToString: " + timeSpan.ToString());


            // Parse TimeSpan
            Console.WriteLine("Parse: " + TimeSpan.Parse("01:02:03"));
        }
    }
}