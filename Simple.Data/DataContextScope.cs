﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace Simple.Data
{
    public class DataContextScope : IReleasable, IDisposable
    {
        private const string KeyPrefix = "Simple.DataContextScope.";
        private const string DefaultConnectionName = "DefaultConnectionString";

        internal DataContext DataContext { get; private set; }

        public static DataContextScope Current
        {
            get
            {
                DataContextScope ctx = GetCurrent();
                return ctx;
            }
        }

        public static DataContextScope GetCurrent(string connectionName = null)
        {
            if (string.IsNullOrEmpty(connectionName))
            {
                connectionName = DefaultConnectionName;
            }
            var key = KeyPrefix + connectionName;
            
            DataContextScope ctx = null;
            if (HttpContext.Current != null)
            {
                ctx = HttpContext.Current.Items[key] as DataContextScope;

                if (ctx == null)
                {
                    ctx = new DataContextScope(connectionName);
                    HttpContext.Current.Items[key] = ctx;
                }
            }
            else
            {
                LocalDataStoreSlot slot = Thread.GetNamedDataSlot(key);
                ctx = Thread.GetData(slot) as DataContextScope;

                if (ctx == null)
                {
                    ctx = new DataContextScope(connectionName);
                    Thread.SetData(slot, ctx);
                }
            }

            return ctx;
        }

        private DataContextScope() : this(DefaultConnectionName)
        {
            
        }

        private DataContextScope(string connectionName)
        {
            DataContext = new DataContext(connectionName);
            
            //if (HttpContext.Current != null && HttpContext.Current.ApplicationInstance != null)
            //{
            //    //未起作用
            //    HttpContext.Current.ApplicationInstance.EndRequest += (sender, e) => DataContext.DoDispose(true);
            //}
        }

        public DataContextScope Begin()
        {
            DataContext.OpenConnection();

            return this;
        }

        public DataContextScope Begin(bool useTran)
        {
            DataContext.OpenConnection();
            if (useTran)
            {
                DataContext.BeginTransaction();
            }
            
            return this;
        }

        public DataContextScope Begin(IsolationLevel isolationLevel)
        {
            DataContext.OpenConnection();
            DataContext.BeginTransaction(isolationLevel);

            return this;
        }
        
        public void Commit()
        {
            if (DataContext.IsTransactionOpened)
            {
                DataContext.CommitTransaction();
            }
        }

        public void Rollback()
        {
            if (DataContext.IsTransactionOpened)
            {
                DataContext.RollbackTransaction();
            }
        }

        public void Dispose()
        {
            DataContext.Dispose();
        }

        void IReleasable.Dispose()
        {
            DataContext.DoDispose(true);
        }
    }
}
