using Micro.Common.Requests;

namespace Micro.Console;

public class Test
{
    [RequestHandler]
    public void Meow() 
    {
        System.Console.WriteLine("plpops");    
    }
}

public class Program
{
    public static void Main()
    {
        //var x = Plops.Meow;
    }
}
