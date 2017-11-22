﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Simple.ServiceBus.Utility;

namespace Simple.ServiceBus.Messages
{
    [DataContract, Serializable]
    //[KnownType("GetKnownTypes")]
    public class Message 
    {
        private Object _body;

        [DataMember]
        protected string BodyWrapper { get; set; }

        [DataMember]
        public virtual MessageHeader Header { get; set; }

        public virtual Object Body
        {
            get 
            {
                if (_body != null)
                {
                    return _body;
                }

                var type = GetType(BodyType);
                _body = JsonUtils.Deserialize(BodyWrapper, type);

                return _body; 
            }
            set
            {
                if (value != null)
                {
                    BodyType = value.GetType().FullName;
                }

                _body = value;

                BodyWrapper = JsonUtils.Serialize(value);
            }
        }

        [DataMember]
        public virtual string BodyType { get; set; }

        [DataMember]
        public virtual string TypeName { get; set; }

        [DataMember]
        public Message Next { get; protected set; }

        public void SetNext(Message next)
        {
            if (Next == null)
            {
                Next = next;

                return;
            }

            Next.SetNext(next);
        }

        protected static Type[] GetKnownTypes()
        {
            return MessageTypeHelper.GetCommandTypes();
        }

        protected static Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null) return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }

    }

    public class Message<T> where T : class, ICommand
    {
        public virtual MessageHeader Header { get; set; }

        public virtual T Body { get; set; }
        
        public virtual string BodyType { get; set; }

        public virtual string TypeName { get; set; }

        public virtual FailCommand Fail { get; set; }

        public virtual bool IsSuccess
        {
            get
            {
                return Fail == null;
            }
        }
        
        public Message() : this(null, null)
        {
            
        }

        public Message(T data) : this(data, null)
        {

        }

        public Message(T data, MessageHeader header)
        {
            Body = data;
            Header = header;

            BodyType = typeof(T).FullName;
            TypeName = this.GetType().FullName;
        }
        
        public Message ToMessage()
        {
            var msg = new Message();
            msg.Header = this.Header;
            msg.Body = this.Body;
            msg.TypeName = this.TypeName;

            return msg;
        }
    }

    internal class MessageTypeHelper
    {
        static Type[] _types;
        
        public static Type[] GetCommandTypes()
        {
            if (_types == null)
            {
                List<Assembly> allAssemblies = new List<Assembly>();
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                foreach (string dll in Directory.GetFiles(path, "*.dll"))
                    allAssemblies.Add(Assembly.LoadFile(dll));

                var result = new List<Type>();

                Type thisType = typeof(ICommand);

                allAssemblies.ForEach(m =>
                {
                    result.AddRange(m.GetTypes().Where(t => t.IsSubclassOf(thisType) || t.GetInterface(thisType.Name) == thisType));
                });

                _types = result.ToArray();
            }

            return _types;
        }
    }
}
