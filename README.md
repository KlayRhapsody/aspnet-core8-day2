# aspnet-core8-day2

ASP.NET Core 8 開發實戰：新手上路篇

### **檢視 API 預設回傳錯誤訊息的格式，所導致的問題**
回應的內容為純文字的 HTML 內容，即使 Header 中有 `Accept: application/json`，也不會回傳 JSON 格式的錯誤訊息，這樣的結果對於前端開發者來說，是不太友善的

```bash
curl --location 'http://localhost:xxxx/WeatherForecast/exception' \
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

### **新增靜態檔案瀏覽功能**

在 Program.cs 中定義 `app.UseStaticFiles()` 來啟用靜態檔案瀏覽功能，這樣就可以直接透過瀏覽器來存取靜態檔案

靜態檔案放置於 wwwroot 資料夾中，透過瀏覽器存取 `http://localhost:xxxx/index.html` 即可存取到 wwwroot 資料夾中的 index.html 檔案


### **Razor 使用 Postman 請求 Post API 出現 400 錯誤**

ASP.NET Core Razor Pages（或 MVC）開啟了 防跨網站要求偽造（CSRF） 的防護機制時，會要求：

表單中（或請求中）帶有 __RequestVerificationToken。
同時也要帶對應的 Cookie（通常是 .AspNetCore.AntiForgery.xxxxx 開頭）。

當瀏覽器使用一般的 HTML 表單提交時，Razor 會自動幫你在 <form> 內插入這個 Token（隱藏欄位），並且瀏覽器會附帶相關 Cookie。因此在「一般使用者操作瀏覽器」的情況下，Token 與 Cookie 會自動配對，一切順利。
但在 Postman 測試時，若你沒有人為地把 Token 與 Cookie 帶上，伺服器就會判斷「CSRF 驗證失敗」，直接回傳 HTTP 400

```bash
curl --location 'http://localhost:5023/Privacy' \
--header 'Content-Type: application/x-www-form-urlencoded' \
--header 'Cookie: .AspNetCore.Antiforgery.iyZPTcKEqNY=THISISTOKEN' \
--data-urlencode '__RequestVerificationToken=THISISTOKEN'
```

### **ASP.NET Core 的預設路由無大小寫區分**

ASP.NET Core 的預設路由系統基於 Windows 檔案系統的大小寫不敏感性，因此路由也被設計為大小寫不敏感的。如果應用程式部署在大小寫敏感的檔案系統（例如 Linux 上的 ext4 文件系統）上，這個行為仍然是由框架本身的設定所控制，與檔案系統無關