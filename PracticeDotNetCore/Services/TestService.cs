namespace PracticeDotNetCore.Services
{
    public class TestService : ITestService
    {
        public void TestDIService()
        {
            Console.WriteLine("This is DI test service called successfully;");
        }
    }
}
