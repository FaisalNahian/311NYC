using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Phone.Controls.Maps;
using _311NYC.Model;
using _311NYC.Helpers;
using RedBit.WindowsPhone.ExternalApi.Twitter;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json.Linq;

namespace _311NYC.DataLoader
{
    public static class DataUploaderProxy
    {
        private static string currentUserId = "";

        private static string pointUri = "http://vanguide.cloudapp.net/Summaries/Add.json";
        private static string tagUri = "http://vanguide.cloudapp.net/Summaries/AddTag.json/";
        private static string commentUri = "http://vanguide.cloudapp.net/Comments/Add.json/";
        private static string ratingUri = "http://vanguide.cloudapp.net/Summaries/AddRating.json/";
        private static string loginUri = "http://vanguide.cloudapp.net/User/Authenticate.json?appId=526701A4-D0E9-4755-ACC5-43C55428594C&oauth_token={0}&oauth_token_secret={1}";

        private static CookieContainer cc = new CookieContainer();

        #region Login Support
        public static void Login()
        {
            string uri = string.Format(loginUri, TwitterHelper.AccessToken, TwitterHelper.AccessSecretToken);
            HttpWebRequest req = WebRequest.CreateHttp(uri);
            req.CookieContainer = cc;
            req.BeginGetResponse(LoginRequest, req);
        }

        public static void LoginRequest(IAsyncResult asyncResult)
        {
            WebRequest request = (WebRequest)asyncResult.AsyncState;
            WebResponse response = null;

            try
            {

                // Get the response stream.
                response = request.EndGetResponse(asyncResult);

                string resultString = string.Empty;
                Stream responseStream = response.GetResponseStream();
                using (StreamReader sd = new StreamReader(response.GetResponseStream()))
                    resultString = sd.ReadToEnd();

                JObject o = JObject.Parse(resultString);

                currentUserId = o.SelectToken("user_id").ToString();

                DispatcherHelper.UIDispatcher.BeginInvoke(() => { MessageBox.Show("Logged in", "Success", MessageBoxButton.OK); });

            }
            catch (Exception e)
            {
                DispatcherHelper.UIDispatcher.BeginInvoke(() => { MessageBox.Show("Error contacting service. " + e.Message); });
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
        }
        #endregion Login Support

        #region Rating Support

        public static void AddRating(string id, string guid, int rating)
        {
            rating = rating * 20;

            
            HttpWebRequest req = WebRequest.CreateHttp(ratingUri + id);
            req.CookieContainer = cc;
            req.Headers["RequestGuid"] = guid;
            req.Headers["rating"] = rating.ToString();
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.BeginGetRequestStream(AddRatingPostRequest, req);
        }



        public static void AddRatingPostRequest(IAsyncResult asyncResult)
        {
            WebRequest req = (WebRequest)asyncResult.AsyncState;
            Stream reqStream = req.EndGetRequestStream(asyncResult);
            StreamWriter writer = new StreamWriter(reqStream);

            string guid = req.Headers["RequestGuid"];

                writer.Write("&rating=" + req.Headers["rating"]);
   
                writer.Close();
                reqStream.Close();
                req.BeginGetResponse(OnAddRating, req);
        }

        public static void OnAddRating(IAsyncResult asyncResult)
        {
            WebRequest request = (WebRequest)asyncResult.AsyncState;
            WebResponse response = null;
            
            try
            {

                // Get the response stream.
                response = request.EndGetResponse(asyncResult);
                Stream responseStream = response.GetResponseStream();

                DispatcherHelper.UIDispatcher.BeginInvoke(() => { MessageBox.Show("Rating Added", "Success", MessageBoxButton.OK); });
                Messenger.Default.Send(true, MessageTokens.RatingAdded);

            }
            catch(Exception e)
            {
                DispatcherHelper.UIDispatcher.BeginInvoke(() => { MessageBox.Show("Error contacting service. " + e.Message); });
                Messenger.Default.Send(false, MessageTokens.RatingAdded);
            }
            finally
            {
                if( response != null )
                    response.Close();
            }
        }

        #endregion Rating Support

        #region Tag Support

        public static void AddTag(string id, string guid,  string tag)
        {
            HttpWebRequest req = WebRequest.CreateHttp(tagUri + id);
            req.CookieContainer = cc;
            req.Headers["RequestGuid"] = guid;
            req.Headers["UserTag"] = tag;
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.BeginGetRequestStream(AddTagPostRequest, req);
        }

        static void AddTagPostRequest(IAsyncResult asyncResult)
        {
            WebRequest req = (WebRequest)asyncResult.AsyncState;
            Stream reqStream = req.EndGetRequestStream(asyncResult);
            StreamWriter writer = new StreamWriter(reqStream);

            string guid = req.Headers["RequestGuid"];
            

                writer.Write("&tag=" + req.Headers["UserTag"]);

                writer.Close();
                reqStream.Close();
                req.BeginGetResponse(OnAddTag, req);
           
        }

        static void OnAddTag(IAsyncResult asyncResult)
        {
            WebRequest request = (WebRequest)asyncResult.AsyncState;

            // Get the response stream.
            WebResponse response = null;
            
            
            try
            {
                response = request.EndGetResponse(asyncResult);
                Stream responseStream = response.GetResponseStream();
                DispatcherHelper.UIDispatcher.BeginInvoke(() => { MessageBox.Show("You succesfully added a tag!", "Success", MessageBoxButton.OK); });
                Messenger.Default.Send(true, MessageTokens.TagAdded);
            }
            catch (Exception e)
            {
                DispatcherHelper.UIDispatcher.BeginInvoke(() => { MessageBox.Show("Error contacting service. " + e.Message, "Error", MessageBoxButton.OK); });
                Messenger.Default.Send(false, MessageTokens.TagAdded);
            }
            finally
            {
                if( response != null )
                    response.Close();
            }
        }

        #endregion Tag Support

        #region Comment Support

        public static void AddComment(string id, string guid, string comment)
        {
            HttpWebRequest req = WebRequest.CreateHttp(commentUri + id);
            req.CookieContainer = cc;
            req.Headers["RequestGuid"] = guid;
            req.Headers["AutoTweet"] = "false";
            req.Headers["RequestCommentBody"] = HttpUtility.UrlEncode(comment);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.BeginGetRequestStream(AddCommentPostRequest, req);
        }

        private static void AddCommentPostRequest(IAsyncResult asyncResult)
        {
            WebRequest req = (WebRequest)asyncResult.AsyncState;
            Stream reqStream = req.EndGetRequestStream(asyncResult);
            StreamWriter writer = new StreamWriter(reqStream);

            string guid = req.Headers["RequestGuid"];
            
                writer.Write("Text=" + req.Headers["RequestCommentBody"]);

                writer.Close();
                reqStream.Close();
                req.BeginGetResponse(OnAddComment, req);
            
        }

        private static void OnAddComment(IAsyncResult asyncResult)
        {
            WebRequest request = (WebRequest)asyncResult.AsyncState;

            // Get the response stream.
            WebResponse response = null;

            try
            {
                response = request.EndGetResponse(asyncResult);
                Stream responseStream = response.GetResponseStream();


                DispatcherHelper.UIDispatcher.BeginInvoke(() => { MessageBox.Show("Comment Added", "Success", MessageBoxButton.OK); });
                Messenger.Default.Send(true, MessageTokens.CommentAdded);

            }
            catch (Exception e)
            {
                DispatcherHelper.UIDispatcher.BeginInvoke(() => { MessageBox.Show("Error contacting service. " + e.Message); });
                Messenger.Default.Send(false, MessageTokens.CommentAdded);
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
        }

        #endregion Comment Support

        #region Shared Landmark Support 
        private class AddNewSharedLandmarkState
        {
            public DataUploadCallback Callback { get; set; }
            public HttpWebRequest WebRequest { get; set; }
            public string PostText { get; set; }
        }

        public static void AddNewSharedLandmark(SharedLandmark landmark, DataUploadCallback callback)
        {

            try
            {
                string guid = Guid.NewGuid().ToString();
                StringWriter writer = new StringWriter();
                writer.Write("Description=" + HttpUtility.UrlEncode(landmark.Description));
                writer.Write("&LayerId=" + currentUserId );
                writer.Write("&Latitude=" + HttpUtility.UrlEncode(landmark.Location.Latitude.ToString()));
                writer.Write("&Longitude=" + HttpUtility.UrlEncode(landmark.Location.Longitude.ToString()));
                writer.Write("&Guid=" + guid);
                writer.Write("&Name=" + landmark.Name);
                //writer.Write("&Tag=" + currentSummary.Tag);

                string postText = writer.ToString();
                writer.Close();

                HttpWebRequest req = WebRequest.CreateHttp(pointUri);
                req.CookieContainer = cc;
                req.Headers["RequestGuid"] = guid;
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                //create a state object to pass around
                AddNewSharedLandmarkState state = new AddNewSharedLandmarkState()
                {
                    WebRequest = req,
                    Callback = callback,
                    PostText = postText
                };

                req.BeginGetRequestStream(CreatePlaceMarkSummaryPostRequest, state);
            }
            catch (Exception e)
            {
                if (callback != null)
                    callback(new DataUploadResultsArgs(e));
            }
        }

        static void CreatePlaceMarkSummaryPostRequest(IAsyncResult asyncResult)
        {
            AddNewSharedLandmarkState state = asyncResult.AsyncState as AddNewSharedLandmarkState;

            try
            {
                if (state != null)
                {
                    HttpWebRequest req = state.WebRequest;
                    Stream reqStream = req.EndGetRequestStream(asyncResult);
                    StreamWriter writer = new StreamWriter(reqStream);

                    // "Description", "LayerId", "Latitude", "Longitude", "Tag", "Guid"
                    string guid = req.Headers["RequestGuid"];
                    writer.Write(state.PostText);

                    writer.Close();
                    reqStream.Close();
                    req.BeginGetResponse(onCreatedPlaceMarkSummary, state);
                }
            }
            catch (Exception e)
            {
                if (state.Callback != null)
                    state.Callback(new DataUploadResultsArgs(e));
            }
        }

        static void onCreatedPlaceMarkSummary(IAsyncResult asyncResult)
        {
            AddNewSharedLandmarkState state = asyncResult.AsyncState as AddNewSharedLandmarkState;
            if (state != null)
            {
                WebRequest request = state.WebRequest;

                // Get the response stream.
                WebResponse response = null;

                try
                {
                    response = request.EndGetResponse(asyncResult);
                    Stream responseStream = response.GetResponseStream();
                    //notify via the callback that we are good to go
                    if (state.Callback != null)
                        state.Callback(new DataUploadResultsArgs(true));
                    //DispatcherHelper.UIDispatcher.BeginInvoke(() => { MessageBox.Show("Landmark Added", "Success", MessageBoxButton.OK); });
                    Messenger.Default.Send(true, MessageTokens.SocialPointAdded);
                }
                catch (WebException we)
                {
                    try
                    {
                        Stream responseStream = response.GetResponseStream();
                        
                    }
                    catch (Exception ignored) { }
                    if (state.Callback != null)
                        state.Callback(new DataUploadResultsArgs(we));
                    Messenger.Default.Send(false, MessageTokens.SocialPointAdded);
                }
                catch (Exception ex)
                {
                    if (state.Callback != null)
                        state.Callback(new DataUploadResultsArgs(ex));
                    //       DispatcherHelper.UIDispatcher.BeginInvoke(() => { MessageBox.Show("Error contacting service. " + e.Message); });
                    Messenger.Default.Send(false, MessageTokens.SocialPointAdded);
                }
                finally
                {
                    if (response != null)
                        response.Close();
                }
            }
        }
        #endregion Shared Landmark Support
    }
}
