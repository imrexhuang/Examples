﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Simple.Common.Extensions
{
    public static class HttpContextBaseExtension
    {
        public static string GetClientIp(this HttpRequestBase request)
        {
            var ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ipAddress))
            {
                var addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return request.ServerVariables["REMOTE_ADDR"];
        }

        public static string GetClientIp(this HttpRequest request)
        {
            return GetClientIp(request.RequestContext.HttpContext.Request);
        }

        public static bool IsIE(this HttpRequestBase request)
        {
            var userAgent = request.UserAgent ?? string.Empty;
            var browser = request.Browser.Browser;
            if (string.Equals(browser, "IE", StringComparison.OrdinalIgnoreCase)
                || string.Equals(browser, "InternetExplorer", StringComparison.OrdinalIgnoreCase)
                || userAgent.IndexOf("msie", StringComparison.OrdinalIgnoreCase) > -1
                || userAgent.IndexOf("rv:11.0", StringComparison.OrdinalIgnoreCase) > -1)
            {
                return true;
            }

            return false;
        }
    }
}
