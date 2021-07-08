using isRock.LineBot;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LineBotFaceRecognition
{
    public partial class _default : System.Web.UI.Page
    {
        const string channelAccessToken = "2XzeoNcOOPVo9e0jDGa3QwT1RA+eZNN4AgIICLXor+RQ/8w8tj2l9mViRkt1yaYE0mFtTFDe+2QR3xwZrDasVH2JvAy5n2L72OXbCUE7wNgZXANLuTujZb5h5GxZPTj/91l1pEVYQnjmS3lVWa060wdB04t89/1O/w1cDnyilFU=";
        const string AdminUserId= "Ua3d3e1675bca2f5e468a6c80bf49f332";
        const string FaceapiKey = "541a8e95880049fabb04e5944019974d";
        const string FaceapiEndpoint = "https://react-native-face-test.cognitiveservices.azure.com/face/v1.0";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var bot = new Bot(channelAccessToken);
            bot.PushMessage(AdminUserId, $"測試 {DateTime.Now.ToString()} ! ");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            var bot = new Bot(channelAccessToken);
            bot.PushMessage(AdminUserId, 1,2);
        }
        protected  void Button3_Click(object sender, EventArgs e)
        {
            var flex = @"
[

{
  ""type"": ""template"",
  ""altText"": ""this is an image carousel template"",
  ""template"": {
                ""type"": ""image_carousel"",
    ""columns"": [
      {
                    ""imageUrl"": ""https://i.imgur.com/6C96oJv.png"",
        ""action"": {
                        ""type"": ""uri"",
          ""label"": ""蘋果"",
          ""uri"": ""https://www.youtube.com/watch?v=ADCArgJeQZQ""
        }
                },
      {
                    ""imageUrl"": ""https://i.imgur.com/T9Llt8B.png"",
        ""action"": {
                        ""type"": ""uri"",
          ""label"": ""鳳梨"",
          ""uri"": ""https://www.youtube.com/watch?v=YlU9O-TYkQU""
        }
                },
      {
                    ""imageUrl"": ""https://i.imgur.com/pJmvVKp.png"",
        ""action"": {
                        ""type"": ""uri"",
          ""label"": ""西瓜"",
          ""uri"": ""https://www.youtube.com/watch?v=Vz55uDiFmEY""
        }
                }
    ]
  }
        }

    
]
";
            isRock.LineBot.Bot bot = new isRock.LineBot.Bot("2XzeoNcOOPVo9e0jDGa3QwT1RA+eZNN4AgIICLXor+RQ/8w8tj2l9mViRkt1yaYE0mFtTFDe+2QR3xwZrDasVH2JvAy5n2L72OXbCUE7wNgZXANLuTujZb5h5GxZPTj/91l1pEVYQnjmS3lVWa060wdB04t89/1O/w1cDnyilFU=");
            bot.PushMessageWithJSON("Ua3d3e1675bca2f5e468a6c80bf49f332", flex);
        }
        
    }
}