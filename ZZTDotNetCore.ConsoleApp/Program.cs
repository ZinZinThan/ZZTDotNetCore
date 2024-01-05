// See https://aka.ms/new-console-template for more information
using ZZTDotNetCore.ConsoleApp.AdoDotNetExamples;
using ZZTDotNetCore.ConsoleApp.DapperExamples;
using ZZTDotNetCore.ConsoleApp.EFCoreExamples;
using ZZTDotNetCore.ConsoleApp.HttpClientExamples;

Console.WriteLine("Hello World");

//AdoDotNetExample adoDotNetExample= new AdoDotNetExample();
//adoDotNetExample.Run();

//DapperExample dapperExample = new DapperExample();
//dapperExample.Run();

//EFCoreExample eFCoreExample = new EFCoreExample();
//eFCoreExample.Run();

Console.WriteLine("Please wait for api...");
Console.ReadKey();

HttpClientExample httpClientExample = new HttpClientExample();
await httpClientExample.Run();

Console.ReadKey();


