﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using Simple.ServiceBus.Configuration;
using Simple.ServiceBus.Inspect;
using Simple.ServiceBus.Messages;

namespace Simple.ServiceBus.Client
{
    public class PublishClient
    {
        protected ChannelFactory<IPublishService> Factory { get; private set; }
        protected ConcurrentQueue<Message> Queue { get; private set; }

        public PublishClient()
        {
            Queue = new ConcurrentQueue<Message>();
            Factory = CreateChannelFactory();

            InitWorker(Queue, 5);
        }

        protected ChannelFactory<IPublishService> CreateChannelFactory()
        {
            EndpointAddress endpointAddress = new EndpointAddress(NetSetting.PubAddress);
            
            var binding = NetSetting.GetBinding();
            
            var result = new ChannelFactory<IPublishService>(binding, endpointAddress);

            result.Endpoint.Behaviors.Add(new BusClientBehavior());
            
            return result;
        }

        protected virtual Message DoSend(ChannelFactory<IPublishService> factory, Message message)
        {
            Message result = null;
            var proxy = factory.CreateChannel();
            bool success = false;

            try
            {
                result = proxy.Publish(message);

                ((IClientChannel)proxy).Close();
                success = true;
            }
            finally
            {
                if (!success)
                {
                    ((IClientChannel)proxy).Abort();
                }
            }

            return result;
        }

        public virtual Message Send(Message message)
        {
            return DoSend(Factory, message);
        }

        public virtual Message<TOut> Send<TIn, TOut>(Message<TIn> message)
            where TIn : class, ICommand
            where TOut : class, ICommand
        {
            Message dto = new Message() { Header = message.Header, Body = message.Body, TypeName = message.TypeName };

            var result = Send(dto);

            if (result.TypeName == null)
            {
                return new Message<TOut>();
            }

            var type = Type.GetType(result.TypeName);
            var instance = Activator.CreateInstance(type, result.Body, result.Header);

            return instance as Message<TOut>;
        }

        public virtual void SendAsync<TIn>(Message<TIn> message) 
            where TIn : class, ICommand
        {
            Message dto = new Message() { Header = message.Header, Body = message.Body, TypeName = message.TypeName };
            
            Queue.Enqueue(dto);
        }

        protected virtual void InitWorker(ConcurrentQueue<Message> queue, int workerCount)
        {
            if (workerCount < 1)
            {
                return;
            }

            for (int i = 1; i <= workerCount; i++)
            {
                var worker = new Thread(DoWork);
                worker.Name = "AsyncWorker" + i.ToString();
                worker.Start(queue);
            }
        }

        protected virtual void DoWork(object obj)
        {
            ConcurrentQueue<Message> queue = obj as ConcurrentQueue<Message>;

            if (queue == null)
            {
                Trace.WriteLine("obj is not ConcurrentQueue<Message>");
                return;
            }
            const int DEFAULT_WAIT = 200;
            
            var factory = CreateChannelFactory();

            while (true)
            {
                try
                {
                    Message msg = null;
                    if (!queue.TryDequeue(out msg))
                    {
                        Thread.Sleep(DEFAULT_WAIT);
                        continue;
                    }

                    if (msg == null)
                    {
                        Thread.Sleep(DEFAULT_WAIT);
                        continue;
                    }

                    Trace.Write(GetWorkerName() + " begin send: " + msg.Header.MessageKey);
                    DoSend(factory, msg);
                    Trace.Write(GetWorkerName() + " send: " + msg.Header.MessageKey + " complete");

                    Thread.Sleep(DEFAULT_WAIT);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(GetWorkerName() + " DoWork Exception:" + ex.Message);
                    Thread.Sleep(1000);
                }
            }
        }

        private string GetWorkerName()
        {
            return Thread.CurrentThread.Name;
        }
    }
}
