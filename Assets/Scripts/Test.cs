using System;
using UnityEngine;

public interface ITestInterface
{
    public int Test { get; }
}

public class TestClass : ITestInterface
{
    private int a;

    public TestClass()
    {
    }

    public int Test { get { return a; } private set { a = value; } }

    public void A()
    {
        Test = 1;
    }
}
