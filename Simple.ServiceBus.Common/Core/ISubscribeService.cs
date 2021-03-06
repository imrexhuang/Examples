﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Simple.ServiceBus
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IPublishService))]
    public interface ISubscribeService
    {
        [OperationContract]
        void Subscribe(string requestKey);

        [OperationContract]
        void UnSubscribe(string requestKey);

        [OperationContract]
        string Ping(string[] keys);
    }
}
