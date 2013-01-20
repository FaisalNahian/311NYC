using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace RedBit.WindowsPhone.ExternalApi.Twitter
{
    public struct TwitterSettings
    {
        public static string ConsumerKeySecret = "JtSWfl0Lr8bApmbtXUNLUdnEm0TP3cYtuI6BnHAFc";
        public static string ConsumerKey = "XIHzWvZouZvQyKcCPoBvg";
        public static string OAuthVersion = "1.0a";
        public static string RequestTokenUri = "http://twitter.com/oauth/request_token";
        public static string AccessTokenUri = "https://api.twitter.com/oauth/access_token";
        public static string CallbackUri = "http://vanguide.cloudapp.net/";
        public static string AuthorizeUri = "http://twitter.com/oauth/authorize";
        public static string StatusUpdateUrl = "http://api.twitter.com"; 


    }
}
