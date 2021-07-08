using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Imgur.API.Authentication;
using Imgur.API.Endpoints;
using Imgur.API.Models;

using Newtonsoft.Json;

namespace LineBotFaceRecognition.Controllers
{
    public class LineBotWebHookController : isRock.LineBot.LineWebHookControllerBase
    {

        [Route("api/LineFaceRec")]
        [HttpPost]
        public IHttpActionResult POST()
        {
            //取得Web.config中的 app settings
            var token = "2XzeoNcOOPVo9e0jDGa3QwT1RA+eZNN4AgIICLXor+RQ/8w8tj2l9mViRkt1yaYE0mFtTFDe+2QR3xwZrDasVH2JvAy5n2L72OXbCUE7wNgZXANLuTujZb5h5GxZPTj/91l1pEVYQnjmS3lVWa060wdB04t89/1O/w1cDnyilFU=";
            const string AdminUserId = "Ua3d3e1675bca2f5e468a6c80bf49f332";

            isRock.LineBot.Event LineEvent = null;
            try
            {
                //設定ChannelAccessToken(或抓取Web.Config)
                this.ChannelAccessToken = token;
                //取得Line Event(本例是範例，因此只取第一個)
                LineEvent = this.ReceivedMessage.events.FirstOrDefault();
                //配合Line verify 
                if (LineEvent.replyToken == "00000000000000000000000000000000") return Ok();
                //回覆訊息
                if (LineEvent.type == "message")
                {
                    if (LineEvent.message.type == "image") //收到圖片
                    {
                        //辨識與繪製圖片
                        var Messages = ProcessImageAsync(LineEvent, token);
                        var userid = LineEvent.source.userId;
                        this.PushMessage(userid, Messages);
                    }
                    else
                    {
                        this.ReplyMessage(LineEvent.replyToken, "這是展示人臉辨識的LINE Bot，請拍一張有人的照片給我唷...");
                    }

                }
                //response OK
                return Ok();
            }
            catch (Exception ex)
            {
                //如果發生錯誤，傳訊息給Admin
                this.PushMessage(AdminUserId, "發生錯誤:\n" + ex.Message);
                //response OK
                return Ok();
            }
        }

        /// <summary>
        /// 處理照片
        /// </summary>
        /// <param name="LineEvent"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private List<isRock.LineBot.MessageBase> ProcessImageAsync(isRock.LineBot.Event LineEvent, string token)
        {
            string Msg = "";
            //取得照片從LineEvent取得用戶上傳的圖檔bytes
            var byteArray = isRock.LineBot.Utility.GetUserUploadedContent(LineEvent.message.id, token);
            //取得圖片檔案FileStream, 分別作為繪圖與分析用
            Stream MemStream1 = new MemoryStream(byteArray);
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(MemStream1);
            string ImgurURL = "";
            using (MemoryStream m = new MemoryStream())
            {
                bmp.Save(m, System.Drawing.Imaging.ImageFormat.Png);
                ImgurURL = UploadImage2Imgur(m.ToArray());
            }
            Msg = $"{ImgurURL}";
            string dect = TryFunction(ImgurURL);
            string FaceData = formatFaceApi(dect);//結果是要放入資料庫的
            Msg = Msg + "\n" + FaceData;
            //建立文字訊息
            isRock.LineBot.TextMessage TextMsg = new isRock.LineBot.TextMessage(Msg);
            //建立圖形訊息(用上傳後的網址)
            isRock.LineBot.ImageMessage imageMsg = new isRock.LineBot.ImageMessage(new Uri(ImgurURL), new Uri(ImgurURL));
            //建立集合
            var Messages = new List<isRock.LineBot.MessageBase>();
            Messages.Add(TextMsg);
            Messages.Add(imageMsg);

            //一次把集合中的多則訊息回覆給用戶
            return Messages;
        }

        //Upload Image to Imgur
        private string UploadImage2Imgur(byte[] bytes)
        {
            var Imgur_CLIENT_ID = "50140947a620cb3";
            var Imgur_CLIENT_SECRET = "579e3c98ced5660c5628fb426436ae474b3a928d";

            //建立 ImgurClient準備上傳圖片
            var client = new ApiClient(Imgur_CLIENT_ID, Imgur_CLIENT_SECRET);
            var httpClient = new HttpClient();

            var endpoint = new ImageEndpoint(client, httpClient);
            IImage image;
            //上傳Imgur
            image = endpoint.UploadImageAsync(new MemoryStream(bytes)).GetAwaiter().GetResult();

            return image.Link;
        }
        static string TryFunction(string URL)
        {
            string host = "https://react-native-face-test.cognitiveservices.azure.com/face/v1.0/detect?returnFaceAttributes=age,emotion";
            string subscriptionKey = "541a8e95880049fabb04e5944019974d";
            var body = new { url = URL };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(host);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                var response = client.SendAsync(request).Result;
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                return jsonResponse;
            }
        }
        static string formatFaceApi(string FaceData)
        {
            string result ="";
            string[] FaceDataArray = FaceData.Split(new char[7] { '[', '{', '"', ':', ',', '}', ']' });
            FaceDataArray = FaceDataArray.Where(s => !string.IsNullOrEmpty(s)).ToArray();
            foreach (var item in FaceDataArray)
                result += item + "\n";
            return result;
            /* 用來處理當出現兩個人臉時
            var searchData = Array.FindAll(sArray, (v) => { return v.StartsWith("faceId"); });
           if (searchData.Length == 1)
           {
        Console.WriteLine("只有一個face ID");
           }
           */
        }
    }
}
