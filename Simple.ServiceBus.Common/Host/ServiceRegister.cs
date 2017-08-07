﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Simple.ServiceBus.Host
{
    public class ServiceRegister
    {
        private static ConcurrentDictionary<string, List<CustomPair<IPublishService, RunInfo>>> Cache = new ConcurrentDictionary<string, List<CustomPair<IPublishService, RunInfo>>>();

        public static ServiceRegister GlobalRegister = new ServiceRegister();
        private static object _syncObj = new object();

        public void Register(string requestKey, IPublishService subscriber, string subscriberId)
        {
            lock (_syncObj)
            {
                if (Cache.ContainsKey(requestKey))
                {
                    if (Cache[requestKey].All(m => m.Key != subscriber))
                    {
                        Cache[requestKey].Add(new CustomPair<IPublishService, RunInfo>(subscriber, new RunInfo(subscriberId)));
                    }
                }
                else
                {
                    var list = new List<CustomPair<IPublishService, RunInfo>>();
                    list.Add(new CustomPair<IPublishService, RunInfo>(subscriber, new RunInfo(subscriberId)));
                    Cache.AddOrUpdate(requestKey, list, (k, l) => l);
                }
            }
            
        }

        public void UnRegister(string requestKey, IPublishService subscriber)
        {
            if (! Cache.ContainsKey(requestKey))
            {
                return;
            }

            lock (_syncObj)
            {
                var item = Cache[requestKey].FirstOrDefault(m => m.Key == subscriber);

                if (item == null || item.Key == null)
                {
                    return;
                }

                Cache[requestKey].Remove(item);
            }
        }

        public List<CustomPair<IPublishService, RunInfo>> GetHandler(string requestKey)
        {
            if (Cache.ContainsKey(requestKey))
            {
                return Cache[requestKey];
            }

            return new List<CustomPair<IPublishService, RunInfo>>();
        }

        public Dictionary<string, List<RunInfo>> GetStats()
        {
            return Cache.ToDictionary(m => m.Key, m => m.Value.Select(t => t.Value).ToList());
        }

        public class RunInfo
        {
            public string Id { get; private set; }

            public long Count { get; set; }

            public DateTime Time { get; set; }

            public RunInfo() : this("unknown")
            {
                
            }

            public RunInfo(string id)
            {
                Id = id;
                Time = DateTime.Now;
            }

            public void Up()
            {
                Thread.MemoryBarrier();
                this.Count++;
                Thread.MemoryBarrier();

                Thread.MemoryBarrier();
                this.Time = DateTime.Now;
                Thread.MemoryBarrier();
            }

            public void Active()
            {
                Thread.MemoryBarrier();
                this.Time = DateTime.Now;
                Thread.MemoryBarrier();
            }

            public override string ToString()
            {
                return Id + "=" + Count + "(" + Time.ToString("yyyyMMddHHmmss") + ")";
            }
        }

        public class CustomPair<TKey, TValue>
        {
            public TKey Key { get; set; }

            public TValue Value { get; set; }

            public CustomPair(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }
    }
}
