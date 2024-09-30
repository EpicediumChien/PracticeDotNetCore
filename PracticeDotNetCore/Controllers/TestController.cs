using Microsoft.AspNetCore.Mvc;

namespace PracticeDotNetCore.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [HttpGet("Threads")]
        public IActionResult PrintNumbersInThreads() 
        {
            Thread thread = new Thread(PrintNumbers);

            thread.Start();

            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"Main thread: {i}");
                Thread.Sleep(500);
            }
            thread.Join();

            return Content("Both threads are complete.");
        }

        [HttpGet("Async")]
        public async Task<IActionResult> GetDataFromAsync()
        {
            Console.WriteLine("Starting async operation...");

            // Await the asynchronous method
            string result = await GetDataAsync();

            Console.WriteLine(result);

            return Content("Async operation complete.");
        }

        private void PrintNumbers()
        {
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"Number: {i}");
                Thread.Sleep(1000);
            }
        }

        private async Task<string> GetDataAsync()
        {
            Console.WriteLine("Fetching data...");

            // Simulate a delay, e.g., waiting for data from an API
            await Task.Delay(2000); // 2 seconds delay

            return "Data fetched successfully!";
        }
    }
}
