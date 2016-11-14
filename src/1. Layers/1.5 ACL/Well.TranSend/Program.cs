namespace PH.Well.TranSend
{
    using Infrastructure;

    public class Program
    {
        static void Main(string[] args)
        {
            var container = DependancyRegister.InitIoc();

            new Import().Process(container);      
        }
    }
}
