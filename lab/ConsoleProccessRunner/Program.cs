using System;

namespace ProccessRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello");
            var runner = new Runner();
            runner.Run(@"C:\Repositories\Osobno\25.06.2019_NSeed\tests\smoke\TypicalSeedBucket.DotNetCore\bin\Debug\netcoreapp3.0\TypicalSeedBucket.exe", @"C:\Repositories\Osobno\25.06.2019_NSeed\tests\smoke\TypicalSeedBucket.DotNetCore\bin\Debug\netcoreapp3.0", new[] {"info" });
            //Console.Write(response.Output);
        }
    }
}
