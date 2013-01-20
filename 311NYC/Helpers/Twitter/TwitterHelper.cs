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
using System.IO.IsolatedStorage;
using Hammock.Authentication.OAuth;
using Hammock.Web;
using System.Collections.Generic;
using Microsoft.Phone.Controls;
using GalaSoft.MvvmLight.Threading;
using Hammock;
using System.Text;
using GalaSoft.MvvmLight.Messaging;

namespace RedBit.WindowsPhone.ExternalApi.Twitter
{
    public static class TwitterHelper
    {
        private static WebBrowser m_browser;

        public const string TWITTER_REQ_TOKEN_KEY = "TwitterReqTokenKey";
        public const string TWITTER_REQ_SECRET_KEY = "TwitterReqSecretKey";
        public const string TWITTER_ACCESS_TOKEN_KEY = "TwitterAccessTokenKey";
        public const string TWITTER_ACCESS_SECRET_KEY = "TwitterAccessSecretKey";

        public static bool HasAuthenticated 
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains(TWITTER_ACCESS_TOKEN_KEY);
            }
        }

        public static string AccessToken
        {
            get
            {
                if (HasAuthenticated)
                    return IsolatedStorageSettings.ApplicationSettings[TWITTER_ACCESS_TOKEN_KEY].ToString();
                else
                    return "";
            }
        }

        public static string AccessSecretToken
        {
            get
            {
                if (HasAuthenticated)
                    return IsolatedStorageSettings.ApplicationSettings[TWITTER_ACCESS_SECRET_KEY].ToString();
                else
                    return "";
            }
        }


        public static void Authenticate(WebBrowser wb)
        {
            m_browser = wb;
            var oauth = new OAuthWorkflow
            {
                ConsumerKey = TwitterSettings.ConsumerKey,
                ConsumerSecret = TwitterSettings.ConsumerKeySecret,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                RequestTokenUrl = TwitterSettings.RequestTokenUri,
                Version = TwitterSettings.OAuthVersion,
                CallbackUrl = TwitterSettings.CallbackUri
            };

            var info = oauth.BuildRequestTokenInfo(WebMethod.Get);
            var objOAuthWebQuery = new OAuthWebQuery(info);
            objOAuthWebQuery.HasElevatedPermissions = true;
            objOAuthWebQuery.SilverlightUserAgentHeader = "Hammock";
            objOAuthWebQuery.SilverlightMethodHeader = "GET";

            objOAuthWebQuery.QueryResponse += new EventHandler<WebQueryResponseEventArgs>(objOAuthWebQuery_QueryResponse);
            objOAuthWebQuery.RequestAsync(TwitterSettings.RequestTokenUri, null);
            
        }

        static void objOAuthWebQuery_QueryResponse(object sender, WebQueryResponseEventArgs e)
        {

            var parameters = GetQueryParameters(e.Response);
            IsolatedStorageSettings.ApplicationSettings[TWITTER_REQ_TOKEN_KEY] = parameters["oauth_token"];
            IsolatedStorageSettings.ApplicationSettings[TWITTER_REQ_SECRET_KEY] = parameters["oauth_token_secret"];

            IsolatedStorageSettings.ApplicationSettings.Save();

            var authorizeUrl = TwitterSettings.AuthorizeUri + "?oauth_token=" + IsolatedStorageSettings.ApplicationSettings[TWITTER_REQ_TOKEN_KEY];

            
            DispatcherHelper.UIDispatcher.BeginInvoke(() =>
            {
                m_browser.Visibility = Visibility.Visible;
                m_browser.Navigating += new EventHandler<NavigatingEventArgs>(m_browser_Navigating);
                m_browser.Navigate(new Uri(authorizeUrl));
            });



        }

        static void m_browser_Navigating(object sender, NavigatingEventArgs e)
        {
            if (e.Uri.ToString().StartsWith(TwitterSettings.CallbackUri))
            {
                var AuthorizeResult = GetQueryParameters(e.Uri.ToString());
                var VerifyPin = AuthorizeResult["oauth_verifier"];
                m_browser.Visibility = Visibility.Collapsed;

                //We now have the Verification pin
                //Using the request token and verification pin to request for Access tokens

                var AccessTokenQuery = GetAccessTokenQuery(
                                                                         IsolatedStorageSettings.ApplicationSettings[TWITTER_REQ_TOKEN_KEY].ToString(),     //The request Token
                                                                         IsolatedStorageSettings.ApplicationSettings[TWITTER_REQ_SECRET_KEY].ToString(),       //The request Token Secret
                                                                         VerifyPin         // Verification Pin
                                                                        );

                AccessTokenQuery.QueryResponse += new EventHandler<WebQueryResponseEventArgs>(AccessTokenQuery_QueryResponse);
                AccessTokenQuery.RequestAsync(TwitterSettings.AccessTokenUri, null);
            }
        }

        public static Dictionary<string, string> GetQueryParameters(string response)
        {
            Dictionary<string, string> nameValueCollection = new Dictionary<string, string>();
            string[] items = response.Split('&');

            foreach (string item in items)
            {
                if (item.Contains("="))
                {
                    string[] nameValue = item.Split('=');
                    if (nameValue[0].Contains("?"))
                        nameValue[0] = nameValue[0].Replace("?", "");
                    nameValueCollection.Add(nameValue[0], System.Net.HttpUtility.UrlDecode(nameValue[1]));
                }
            }
            return nameValueCollection;
        }

        internal static OAuthWebQuery GetAccessTokenQuery(string requestToken, string RequestTokenSecret, string oAuthVerificationPin)
        {
            var oauth = new OAuthWorkflow
            {
                AccessTokenUrl = TwitterSettings.AccessTokenUri,
                ConsumerKey = TwitterSettings.ConsumerKey,
                ConsumerSecret = TwitterSettings.ConsumerKeySecret,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                Token = requestToken,
                Verifier = oAuthVerificationPin,
                Version = TwitterSettings.OAuthVersion
            };

            var info = oauth.BuildAccessTokenInfo(WebMethod.Post);
            var objOAuthWebQuery = new OAuthWebQuery(info);
            objOAuthWebQuery.HasElevatedPermissions = true;
            objOAuthWebQuery.SilverlightUserAgentHeader = "Hammock";
            objOAuthWebQuery.SilverlightMethodHeader = "GET";
            return objOAuthWebQuery;
        }

        static void AccessTokenQuery_QueryResponse(object sender, WebQueryResponseEventArgs e)
        {
            
                var parameters =GetQueryParameters(e.Response);
                IsolatedStorageSettings.ApplicationSettings[TWITTER_ACCESS_TOKEN_KEY] = parameters["oauth_token"];
                IsolatedStorageSettings.ApplicationSettings[TWITTER_ACCESS_SECRET_KEY] = parameters["oauth_token_secret"];
                IsolatedStorageSettings.ApplicationSettings.Save();

                Messenger.Default.Send(TwitterMessages.TwitterLoginSuccess);
        }

        public static void Tweet(string tweet)
        {
            var credentials = new OAuthCredentials
            {
                Type = OAuthType.ProtectedResource,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                ConsumerKey = TwitterSettings.ConsumerKey,
                ConsumerSecret = TwitterSettings.ConsumerKeySecret,
                Token = IsolatedStorageSettings.ApplicationSettings[TWITTER_ACCESS_TOKEN_KEY].ToString(),     //The request Token
                TokenSecret = IsolatedStorageSettings.ApplicationSettings[TWITTER_ACCESS_SECRET_KEY].ToString(), 
                Version = "1.0"
            };

            var restClient = new RestClient
            {
                Authority = TwitterSettings.StatusUpdateUrl,
                HasElevatedPermissions = true,
                Credentials = credentials,
                Method = WebMethod.Post
            };

            restClient.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            // Create a Rest Request and fire it
            var restRequest = new RestRequest
            {
                Path = "1/statuses/update.xml?status=" +  tweet 
            };

            var ByteData = Encoding.UTF8.GetBytes(tweet);
            restRequest.AddPostContent(ByteData);
            restClient.BeginRequest(restRequest, new RestCallback(TweetCallback));

        }

        static void TweetCallback(RestRequest rq, RestResponse rs, object obj)
        {
            //#warning add error trapping in case the tweet is not successfull.  Could have been removed as authorised
            //DispatcherHelper.UIDispatcher.BeginInvoke(() => MessageBox.Show("Tweet Sent"));
            Messenger.Default.Send(TwitterMessages.TweetSentSuccess);
        }

        public static void Logout()
        {
            IsolatedStorageSettings.ApplicationSettings[TWITTER_ACCESS_TOKEN_KEY] = null;
            IsolatedStorageSettings.ApplicationSettings.Remove(TWITTER_ACCESS_TOKEN_KEY);
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

    }
}
