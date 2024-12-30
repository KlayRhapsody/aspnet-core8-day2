# aspnet-core8-day2

ASP.NET Core 8 開發實戰：新手上路篇

### **檢視 API 預設回傳錯誤訊息的格式，所導致的問題**
回應的內容為純文字的 HTML 內容，即使 Header 中有 `Accept: application/json`，也不會回傳 JSON 格式的錯誤訊息，這樣的結果對於前端開發者來說，是不太友善的

```bash
curl --location 'http://localhost:5010/WeatherForecast/exception' \
--header 'Accept: application/json'
```

錯誤內容
![alt text](./docs/images/ErrorMessage.png)


### **啟用 Https Middleware**

在 Program.cs 中定義 `app.UseHttpsRedirection()` 來啟用 Https Middleware，若在創建專案時未啟用除了新增 Middleware 外，還需要在 launchSettings.json 中設定 `applicationUrl` 來啟用 Https

```json
"https": {
    "commandName": "Project",
    "dotnetRunMessages": true,
    "launchBrowser": true,
    "launchUrl": "swagger",
    "applicationUrl": "https://localhost:7268;http://localhost:5010",
    "environmentVariables": {
    "ASPNETCORE_ENVIRONMENT": "Development"
    }
}
```