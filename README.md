# LineChatBot-with-Azure-Face-API
![markdown](https://csharpcorner-mindcrackerinc.netdna-ssl.com/article/using-azure-face-api-with-asp-net-mvc/Images/image001.png)

**這是Line Chat Bot 透過傳入照片進行臉部偵測並且做情緒辯識**

這一部分的程式碼為專題-菓然有料-情緒辨識之水果茶飲推薦系統其中一小功能的程式碼，提供給大家的參考。

* 建議使用Ngrok進行測試 <br/> 
* LINE Bot後台的WebHook設定，其位置為 Http://你的domain/api/LineFaceRec

## 系統流程介紹
當使用者傳入照片後，透過從Line後端獲取使用者傳的照片，
此型態為*Stream*，透過將此*Stream*型態轉成*byte*
再將照片上傳至Imgur，並回傳照片網址回來，透過*Http Client* 進行 *Post* 傳輸，
將結果回傳回來並進行處理。
不使用原先Face API呼叫方法是因為不知為啥無法連上Azure，暫時改用*Http Client* 進行Call 這個 API。


## 參考資源 
LineBotSDK：https://www.nuget.org/packages/LineBotSDK
<br/>線上課程：https://www.udemy.com/line-bot/
<br/>更多內容，請參考電子書：https://www.pubu.com.tw/ebook/103305



