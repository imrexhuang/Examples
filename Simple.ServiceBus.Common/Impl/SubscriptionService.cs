﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Simple.ServiceBus.Common.Impl
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SubscriptionService : ISubscription
    {
        protected OperationContext Context
        {
            get
            {
                return OperationContext.Current;
            }
        }

        public void Subscribe(string requestKey)
        {
            IPublishing subscriber = Context.GetCallbackChannel<IPublishing>();
            
            Trace.WriteLine(Context.GetClientAddress() +  " Subscribed");
            
            ServiceRegister.GlobalRegister.Register(requestKey, subscriber);
        }

        public void UnSubscribe(string requestKey)
        {
            IPublishing subscriber = Context.GetCallbackChannel<IPublishing>();
            
            Trace.WriteLine(Context.GetClientAddress() + " UnSubscribed");

            ServiceRegister.GlobalRegister.UnRegister(requestKey, subscriber);
        }


        public string Ping()
        {
            var result = Context.GetClientAddress() + " ping current host at:" + DateTime.Now.ToString();
            Trace.WriteLine(result);

            return result;
        }
    }
}
