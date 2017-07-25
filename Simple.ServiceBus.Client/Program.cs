﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Simple.ServiceBus.Logging;
using Simple.ServiceBus.Messages;

namespace Simple.ServiceBus.Client
{
    class Program
    {
        static PublishClient client = new PublishClient();

        static void Main(string[] args)
        {
#if DEBUG
            Trace.Listeners.Add(new BusDefaultTraceListener());
#else
            Trace.Listeners.Add(new FileLogTraceListener());
#endif
            DoTest(args);
            
            Console.WriteLine("\nPress Any Key To Exit...");
            Console.ReadLine();
        }

        static void DoTest(string[] args)
        {
            var input = args.Length > 0 ? args[0] ?? Console.ReadLine() : Console.ReadLine();
            var key = "abc";
            var subKey = "s";
            if (input.Contains("="))
            {
                var inputs = input.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                if (inputs.Length > 1)
                {
                    subKey = inputs[0];
                    key = inputs[1];
                }
            }

            if (string.Equals(subKey, "s", StringComparison.OrdinalIgnoreCase))
            {
                Console.Title = "SubKey=" + key + " Start:" + DateTime.Now.ToString("yyyyMMddHHmmss");
                SubTest(key);
            }
            else
            {
                Console.Title = "PubKey=" + key + " Start:" + DateTime.Now.ToString("yyyyMMddHHmmss");
                PubTest(key);
            }
        }

        static void SubTest(string key)
        {
            for (int i = 0; i < 5; i++)
            {
                SubscribeClient client = new SubscribeClient();
                client.Register(new Test1Handler());
                client.Register(new Test3Handler());

                client.Subscribe(key);

                Thread.Sleep(1000);
            }
        }

        static void PubTest(string key)
        {
            var input = string.Empty;
            var process = Process.GetCurrentProcess();
            Trace.WriteLine("current process id:" + process.Id);

            do
            {
                Console.WriteLine(DateTime.Now + ">>");
                try
                {
                    for (int i = 0; i < 100000000; i++)
                    {
                        Thread.Sleep(100);
                        var header = new MessageHeader { RequestKey = key, MessageKey = process.Id.ToString() + "_" + i.ToString() };

                        header.RouteType = RouteType.Single;

                        if (i % 3 == 0)
                        {
                            SendTest1(header, i);
                            continue;
                        }

                        if (i % 4 == 0)
                        {
                            SendTest2(header, i);
                            continue;
                        }

                        if (i % 5 == 0)
                        {
                            header.RouteType = RouteType.All;
                            SendAsyncTest3(new Test3InCommand() { Index = i }, header, i);
                            continue;
                        }

                        SendTest3(new Test3InCommand() { Index = i }, header, i);                        
                    }

                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Exception:" + ex.Message);
                }

                Trace.WriteLine("send finish");
                input = Console.ReadLine();

            } while (input != "q");
        }

        static void SendTest1(MessageHeader header, Int64 index)
        {
            var msg = new Message<Test1Command>(new Test1Command(), header);
            var result = client.Send<Test1Command, Test1Command>(msg);

            Trace.WriteLine(index.ToString().PadLeft(3, '0') + "_" + result.Body.ToString());
        }

        static void SendTest2(MessageHeader header, Int64 index)
        {
            var msg = new Message<Test2Command>(new Test2Command(), header);
            var result = client.Send<Test2Command, Test2ResultCommand>(msg);

            Trace.WriteLine(index.ToString().PadLeft(3, '0') + "_" + result.Body.ToString());
        }

        static void SendTest3(Test3InCommand cmd, MessageHeader header, Int64 index)
        { 
            var msg = new Message<Test3InCommand>(cmd, header);
            var result = client.Send<Test3InCommand, Test3OutCommand>(msg);
            Trace.WriteLine(index.ToString().PadLeft(3, '0') + "_" + DateTime.Now.ToString("HH:mm:ss") + ">> " + result.Body.ToString());
        }

        static void SendAsyncTest3(Test3InCommand cmd, MessageHeader header, Int64 index)
        {
            var msg = new Message<Test3InCommand>(cmd, header);
            client.SendAsync(msg);            
        }

    }
}
