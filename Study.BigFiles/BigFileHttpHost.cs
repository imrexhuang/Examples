﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;

namespace Study.BigFiles
{
    public class BigFileHttpHost : IDisposable
    {
        public const String HOST_CONFIG_SECTION = "bigfile.hosts";
        const String URI_PREFIX_FORMAT = "http://{0}:{1}/";
        const String API_NAME = "BigFileApi";
        const String TIME_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";

        public Int32 Port { get; private set; }
        public String FilePath { get; private set; }
        public Int64 FileSize { get; private set; }
        public Boolean IsDisposed { get; private set; }
        protected HttpListener Listerner { get; private set; }
        protected Thread ListernerThread { get; private set; }

        public BigFileHttpHost(Int32 port, String filePath, Int64 fileSize)
        {
            Port = port;
            FilePath = filePath;
            FileSize = fileSize;
        }

        private HttpListener CreateListener()
        {
            HttpListener listerner = new HttpListener();
            listerner.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily != AddressFamily.InterNetwork)
                {
                    continue;
                }
                listerner.Prefixes.Add(GetURI(ip.ToString()));
            }

            listerner.Prefixes.Add(GetURI("localhost"));
            listerner.Prefixes.Add(GetURI("127.0.0.1"));

            return listerner;
        }

        private String GetURI(String ip)
        {
            return String.Format(URI_PREFIX_FORMAT, ip, this.Port);
        }

        public void Run(HttpListener listerner)
        {
            while (listerner.IsListening)
            {
                Trace.WriteLine("waiting request...");
                HttpListenerContext ctx = listerner.GetContext();

                ThreadPool.QueueUserWorkItem(obj =>
                {
                    HttpListenerContext httpCtx = obj as HttpListenerContext;

                    try
                    {
                        DoHandler(httpCtx);
                    }
                    catch (Exception ex)
                    {
                        String exMsg = String.Format("Request Url:{0} Http Method:{1} Exception:{2}", ctx.Request.Url, ctx.Request.HttpMethod, ex.ToString());
                        Trace.WriteLine(exMsg);
                        using (StreamWriter writer = new StreamWriter(ctx.Response.OutputStream, Encoding.UTF8))
                        {
                            writer.WriteLine(exMsg);
                            writer.Close();
                        }

                        httpCtx.Response.StatusCode = (Int32)HttpStatusCode.InternalServerError;
                        httpCtx.Response.ContentType = "text/plain";
                        httpCtx.Response.ContentEncoding = Encoding.UTF8;
                        httpCtx.Response.Close();
                    }
                }, ctx);
            }
        }

        public void Start()
        {
            if (Listerner != null && Listerner.IsListening)
            {
                return;
            }

            Listerner = CreateListener();
            
            ListernerThread = new Thread(obj =>
            {
                HttpListener http = (HttpListener)obj;
                Run(http);
            });
            ListernerThread.Name = "HttpServer:" + this.Port;
            ListernerThread.IsBackground = true;
            
            Listerner.Start();
            ListernerThread.Start(this.Listerner);
            IsDisposed = false;
            Trace.WriteLine(ListernerThread.Name + " started.");
        }

        public void Stop()
        {
            if (Listerner == null || ListernerThread == null || IsDisposed)
            {
                return;
            }

            ListernerThread.Abort();
            Listerner.Stop();

            Trace.WriteLine(ListernerThread.Name + " stopped.");
        }

        public void Dispose()
        {
            try
            {
                if (ListernerThread != null)
                {
                    ListernerThread.Abort();

                    Trace.WriteLine(ListernerThread.Name + " disposed.");
                }

                if (Listerner != null)
                {
                    Listerner.Stop();
                    Listerner.Close();
                }

                IsDisposed = true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        protected void DoHandler(HttpListenerContext ctx)
        {
            String url = (ctx.Request.RawUrl ?? String.Empty).Trim('/');
            Trace.WriteLine(String.Format("Client:{0} {1} Request:{2}", ctx.Request.RemoteEndPoint.Address, ctx.Request.HttpMethod, ctx.Request.Url));

            if (String.Equals(url, API_NAME, StringComparison.OrdinalIgnoreCase) && ctx.Request.HttpMethod == "POST")
            {
                UploadFile(ctx);
                return;
            }

            if (url.StartsWith(API_NAME, StringComparison.OrdinalIgnoreCase) && ctx.Request.HttpMethod == "GET")
            {
                DownloadFile(ctx);
                return;
            }
            
            StartInfo(ctx);
        }

        private void UploadFile(HttpListenerContext ctx)
        {
            Int64 fileId = 0L;

            Byte[] buffer = GetFile(ctx.Request.ContentEncoding, GetBoundary(ctx.Request.ContentType), ctx.Request.InputStream);
            //File.WriteAllBytes(Guid.NewGuid().ToString("N") + ".jpg", buffer);
            using (BigFile bigFile = new BigFile(this.FilePath, this.FileSize))
            {
                fileId = bigFile.Write(buffer);
            }
            buffer = null;
            Debug.WriteLine(fileId);

            using (StreamWriter writer = new StreamWriter(ctx.Response.OutputStream))
            {
                writer.WriteLine(fileId);
                writer.Close();
            }

            IPEndPoint clientIp = ctx.Request.RemoteEndPoint;
            String fileUrl = fileId == 0L ? String.Empty : String.Format(URI_PREFIX_FORMAT, clientIp.Address, clientIp.Port) + API_NAME + "/" + fileId;

            ctx.Response.AppendHeader("File-Url", fileUrl);
            ctx.Response.StatusCode = (Int32)HttpStatusCode.OK;
            ctx.Response.ContentType = "text/plain";
            ctx.Response.Close();
        }
        
        private String GetBoundary(String ctype)
        {
            return "--" + ctype.Split(';')[1].Split('=')[1];
        }

        private Byte[] GetFile(Encoding enc, String boundary, Stream input)
        {
            if (input == Stream.Null)
            {
                return new Byte[0];
            }

            Byte[] boundaryBytes = enc.GetBytes(boundary);
            Int32 boundaryLen = boundaryBytes.Length;

            using (MemoryStream output = new MemoryStream())
            {
                Byte[] buffer = new Byte[1024];
                Int32 len = input.Read(buffer, 0, 1024);
                Int32 startPos = -1;

                // Find start boundary
                while (true)
                {
                    if (len == 0)
                    {
                        throw new Exception("Start Boundaray Not Found");
                    }

                    startPos = IndexOf(buffer, len, boundaryBytes);
                    if (startPos >= 0)
                    {
                        break;
                    }
                    else
                    {
                        Array.Copy(buffer, len - boundaryLen, buffer, 0, boundaryLen);
                        len = input.Read(buffer, boundaryLen, 1024 - boundaryLen);
                    }
                }

                // Skip four lines (Boundary, Content-Disposition, Content-Type, and a blank)
                for (Int32 i = 0; i < 4; i++)
                {
                    while (true)
                    {
                        if (len == 0)
                        {
                            throw new Exception("Preamble not Found.");
                        }

                        startPos = Array.IndexOf(buffer, enc.GetBytes("\n")[0], startPos);
                        if (startPos >= 0)
                        {
                            startPos++;
                            break;
                        }
                        else
                        {
                            len = input.Read(buffer, 0, 1024);
                        }
                    }
                }

                Array.Copy(buffer, startPos, buffer, 0, len - startPos);
                len = len - startPos;

                while (true)
                {
                    Int32 endPos = IndexOf(buffer, len, boundaryBytes);
                    if (endPos >= 0)
                    {
                        if (endPos > 0) output.Write(buffer, 0, endPos - 2);
                        break;
                    }
                    else if (len <= boundaryLen)
                    {
                        throw new Exception("End Boundaray Not Found");
                    }
                    else
                    {
                        output.Write(buffer, 0, len - boundaryLen);
                        Array.Copy(buffer, len - boundaryLen, buffer, 0, boundaryLen);
                        len = input.Read(buffer, boundaryLen, 1024 - boundaryLen) + boundaryLen;
                    }
                }

                return output.ToArray();
            }
        }

        private Int32 IndexOf(Byte[] buffer, Int32 len, Byte[] boundaryBytes)
        {
            for (Int32 i = 0; i <= len - boundaryBytes.Length; i++)
            {
                Boolean match = true;
                for (Int32 j = 0; j < boundaryBytes.Length && match; j++)
                {
                    match = buffer[i + j] == boundaryBytes[j];
                }

                if (match)
                {
                    return i;
                }
            }

            return -1;
        }

        private void DownloadFile(HttpListenerContext ctx)
        {
            String fileIdUrl = ctx.Request.RawUrl.Replace(API_NAME, String.Empty).Trim('/');
            Int64 fileId = 0L;
            Int64.TryParse(fileIdUrl, out fileId);
            DateTime uploadDate = DateTime.MinValue;

            using (BinaryWriter writer = new BinaryWriter(ctx.Response.OutputStream))
            {
                if (fileId == 0)
                {
                    writer.Write(new Byte[0], 0, 0);
                }
                else
                {
                    using (BigFile bigFile = new BigFile(this.FilePath, this.FileSize))
                    {
                        Byte[] buffer = bigFile.Read(fileId, out uploadDate) ?? new Byte[0];
                        writer.Write(buffer, 0, buffer.Length);
                    }
                }
            }

            ctx.Response.AppendHeader("File-Date", uploadDate.ToString(TIME_FORMAT));
            ctx.Response.StatusCode = (Int32)HttpStatusCode.OK;
            ctx.Response.Close();
        }

        private void StartInfo(HttpListenerContext ctx)
        {            
            const Int32 MB_SIZE = 1048576;
            Process process = Process.GetCurrentProcess();

            PerformanceCounter pcMemory = new PerformanceCounter("Process", "Working Set - Private", process.ProcessName);
            PerformanceCounter pcCpuLoad = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            pcCpuLoad.NextValue();

            CounterSample cs1 = pcCpuLoad.NextSample();
            Thread.Sleep(200);
            CounterSample cs2 = pcCpuLoad.NextSample();
            Single finalCpuCounter = CounterSample.Calculate(cs1, cs2);
            HostConfig section = ConfigurationManager.GetSection(HOST_CONFIG_SECTION) as HostConfig;

            using (StreamWriter writer = new StreamWriter(ctx.Response.OutputStream, Encoding.UTF8))
            {
                writer.WriteLine("Web文件传输服务已经启动!");
                writer.WriteLine("启动时间:" + process.StartTime.ToString(TIME_FORMAT));
                writer.WriteLine("系统时间:" + DateTime.Now.ToString(TIME_FORMAT));
                writer.WriteLine("请求地址:" + ctx.Request.RawUrl);
                writer.WriteLine("客户端IP:" + ctx.Request.RemoteEndPoint.Address);
                writer.WriteLine("线程数量:" + process.Threads.Count);
                writer.WriteLine("程序内存:" + Math.Round(pcMemory.NextValue() / MB_SIZE, 2) + "MB");
                writer.WriteLine("系统CPU:" + Math.Round(finalCpuCounter, 0) + "%");
                
                if (section != null && section.Hosts != null)
                {
                    writer.WriteLine();
                    foreach (HostElement setting in section.Hosts)
                    {
                        writer.WriteLine("文件路径:" + setting.FilePath + " 文件大小:" + setting.Size + " Http端口:" + setting.Port);

                        BigFile.Header header = null;
                        using (BigFile file = new BigFile(setting.FilePath, setting.FileSize))
                        {
                            header = file.GetHeader();
                        }

                        writer.WriteLine("剩余容量:" + Math.Round((Decimal)header.FreeStorage / MB_SIZE, 2) + "MB");
                        writer.WriteLine("文件个数:" + header.FileCount + " 上一周期文件个数:" + header.CycleTotalFileCount);
                        writer.WriteLine("文件覆盖次数:" + header.OverwriteCount);
                        writer.WriteLine("上一文件刻度:" + header.PrevOffset + " 上一文件存储时间:" + header.ActiveTime.ToString(TIME_FORMAT));
                        writer.WriteLine("末尾文件刻度:" + header.LastOffset + " 末尾文件存储时间:" + header.LastFileTime.ToString(TIME_FORMAT));
                        writer.WriteLine("当前文件刻度:" + header.CurrentOffset);
                        writer.WriteLine();
                    }
                }

                writer.Close();
            }

            ctx.Response.StatusCode = (Int32)HttpStatusCode.OK;
            ctx.Response.ContentType = "text/plain";
            ctx.Response.ContentEncoding = Encoding.UTF8;
            ctx.Response.Close();
        }
    }
}
