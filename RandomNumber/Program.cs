using System;

namespace RandomNumber;

public class Program
{
    static void Main(string[] args)
    {
        MyRandomNumber myNumber = new MyRandomNumber();      
        int myInt = myNumber.MyRandom();
        Console.WriteLine($"My random number between 0 and 100 is {myInt}");
    }
}
