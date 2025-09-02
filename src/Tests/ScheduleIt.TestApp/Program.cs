// See https://aka.ms/new-console-template for more information
using ScheduleIt;
using ScheduleIt.IoC;
using ScheduleIt.TestApp;

Console.WriteLine("TaskProcessing!");

TaskServer.Setup(s =>
{
    //// s.Resolver = new BasicDependencyResolver();
}).SetDefault();


TaskServer.Instance.Schedule(() => new TestTask("one"), s => s.Every(5).Seconds(), "one")
    .Schedule(() => new TestTask("two"), s => s.Every(10).Seconds(), "two")
    .Schedule(() => { Console.WriteLine("x"); }, s => s.In(TimeSpan.FromSeconds(5)).AndEvery(3).Seconds(), "x")
    .Schedule(() => { Console.WriteLine("x"); }, s => s.Every(3).Seconds(), "two")
    .Schedule<TestTask>(s => s.Every(5).Seconds());


Console.ReadLine();
