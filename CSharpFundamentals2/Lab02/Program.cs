namespace Lab02
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            //Customer customer = new Customer();
            //var customer = new Customer(1, "Soubhik");
            var customer = new Customer();
            customer.Id = 1;
            customer.Name = "Soubhik";

            var order = new Order();

            customer.Orders = new List<Order>();
            customer.Orders.Add(order); 




            Console.WriteLine(customer.Id);
            Console.WriteLine(customer.Name);
        }
    }
}