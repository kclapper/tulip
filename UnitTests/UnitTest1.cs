namespace UnitTests;
using Tulip.Models;


public class Tests
{
    [SetUp]
    public void Setup()	
    {
	
    }

    [Test]
    public void BadgesConstructorTest()
    {
    	//Constructor Sanity Check
    	
	Badges b = new Badges("Speed");
	Assert.That(b.Badge, Is.EqualTo("Speed"));
	
    }
}
