using System;

namespace RandomNumber;

public class MyRandomNumber
{
    private readonly Random _myRandom = new();

    public int MyRandom()
    {
        int randomNumber = _myRandom.Next(101);
        return randomNumber;
    }
}
