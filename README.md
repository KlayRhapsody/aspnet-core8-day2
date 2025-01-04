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


### **Razor 相關規則**

透過繼承 PageModel 來實現 Razor Page 的後端邏輯，並透過 Razor Page 的檔案結構來實現前端頁面的設計，Razor Page 的後端邏輯與前端頁面設計是放在同一個檔案中，例如 `Privacy.cshtml.cs` 與 `Privacy.cshtml`，檔名為路由的一部分，例如 `Privacy.cshtml` 對應的路由為 `/Privacy`，頁面內容則為 `Privacy.cshtml` 的內容


### **ASP.NET Core 的預設路由無大小寫區分**

ASP.NET Core 的預設路由系統基於 Windows 檔案系統的大小寫不敏感性，因此路由也被設計為大小寫不敏感的。如果應用程式部署在大小寫敏感的檔案系統（例如 Linux 上的 ext4 文件系統）上，這個行為仍然是由框架本身的設定所控制，與檔案系統無關


### **Razor vs. Blazor 的選擇建議**

| **比較項目**       | **Razor**                                            | **Blazor**                                                 |
|---------------------|-----------------------------------------------------|-----------------------------------------------------------|
| **運行模式**        | 伺服器端渲染（SSR）                                   | 客戶端（WebAssembly）或伺服器模式                         |
| **互動性**          | 需要結合 JavaScript                                   | 使用 C# 實現互動性，適合 SPA 開發                         |
| **開發者友好性**    | 更適合熟悉 HTML/C# 的傳統開發者                       | 適合 .NET 全棧開發者                                      |
| **SEO 支援**        | 強，伺服器端生成完整 HTML                              | WebAssembly 的 SEO 支援較弱，Blazor Server 支援較強       |
| **性能**           | 依賴伺服器資源，適合大規模請求                        | 初次加載慢（WebAssembly），高互動性應用適合               |
| **使用場景**        | 內容為主的動態網頁                                    | 互動性強的前端應用或單頁應用                              |
| **技術限制**        | 缺乏前端互動能力，需額外依賴 JS                       | WebAssembly 加載慢，伺服器模式依賴 SignalR                |


### **Blazor 的模式**
1. **Blazor Server**
   - 應用程式邏輯執行在伺服器上，透過 SignalR 即時同步到客戶端。
   - 適合需要即時更新、對 SEO 友好並且延遲要求不高的應用。

2. **Blazor WebAssembly (WASM)**
   - 應用程式邏輯執行在客戶端（瀏覽器端），以 WebAssembly 運行 .NET 程式。
   - 適合對互動性能要求高、能減少伺服器依賴的應用。

3. **Auto 模式**
   - **自動選擇模式**：應用程式會根據執行環境或用戶端條件動態選擇 Blazor Server 或 Blazor WebAssembly 模式運行。

```csharp
// Auto          - 下載 WebAssembly 資產時使用伺服器，然後使用 WebAssembly
// None          - 沒有互動功能 (只有靜態伺服器轉譯)
// Server        - 在伺服器上執行
// WebAssembly   - 使用 WebAssembly 在瀏覽器中執行
dotnet new blazor -n Blazor8 --interactivity Server
```


### **Blazor Server 元件傳遞機制**
1. **父元件向子元件傳遞參數**
   - 在子元件中定義參數，並在父元件中使用子元件時，透過 `@` 符號來傳遞參數
2. **子元件向父元件傳遞事件**
   - 在子元件中定義事件，並在父元件中使用子元件時，透過 `EventCallback` 來傳遞事件


### **新增 Minimal API 專案**

```bash
// -controllers, --use-controllers
// -minimal, --use-minimal-apis
// 預設不指定參數則創建 Minimal API
dotnet new webapi -n MiniApi8 --use-minimal-apis -f net8.0
```

### **在 Minimal API 中創建 Endpoints**

1. **創建資料模型**
   - 在 Models 資料夾中新增 Student.cs 檔案，並定義 Student 資料模型和命名空間
2. **安裝 Microsoft.VisualStudio.Web.CodeGeneration.Design package**
```bash
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
```
3. **Scaffold Endpoints**
```bash
// -e 表示 Endpoints 名稱
// -o 表示 允許覆蓋現有文件
// -m 表示 指定模型
// -outDir 表示 輸出目錄
dotnet aspnet-codegenerator minimalapi -e StudentEndpoints -o -m MiniApi8.Models.Student -outDir ./Endpoints
```


### **在 Minimal API 中使用 Entity Framework**

1. **安裝並啟動 MS SQL Server**
```bash
docker pull mcr.microsoft.com/mssql/server:2022-latest

docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=xxxxxx" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```
2. **新增資料庫**
   - [ContosoUniversity](https://gist.github.com/doggy8088/2a2f7075d49b3814d19513426ede3549)
3. **使用 EF Core Power Tools 反向工程**
- 安裝 Sql Server package
```xml
<ItemGroup>
  <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" />
</ItemGroup>
```
- 註冊 DbContext 服務
```csharp
builder.Services.AddDbContext<ContosoUniversityContext>(
   options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```
- 設定 ConnectionStrings (連線字串需自行設定帳密)
```json
{
   "ConnectionStrings": {
      "DefaultConnection": "data source=localhost;initial catalog=ContosoUniversity;user id=sa;password=xxxxxx;encrypt=True;trust server certificate=True;command timeout=300"
   }
}
```
- 設定 `efcpt-config.json` 避免產生額外 View、Store Procedure Code 以及覆蓋現有設定檔
```json
{
   "refresh-object-lists": false,
   "views": [],
   "store-procedures": [],
}
```
- 安裝 EF Core Power Tools package
```xml
<ItemGroup>
   <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.0" />
   <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.0" />
</ItemGroup>
```
- 產生 CourseEndpoints
```bash
dotnet aspnet-codegenerator minimalapi -dc MiniApi8.Models.ContosoUniversityContext -e CourseEndpoints -o -m MiniApi8.Models.Course -outDir Endpoints
```





### **MS SQL 預設表格**

| 資料庫名稱 | 主要用途                                      | 特點                                                |
|------------|----------------------------------------------|---------------------------------------------------|
| **master** | 存儲伺服器層級的配置和元數據                  | SQL Server 的核心資料庫，對伺服器正常運行至關重要  |
| **model**  | 為新建的資料庫提供模板                       | 可自定義來影響所有新資料庫的默認設置              |
| **msdb**   | 支持代理服務、自動化和備份還原的元數據存儲   | 包含作業、計劃任務和歷史記錄                      |
| **tempdb** | 用於臨時數據和中間結果                       | 每次伺服器啟動後會重新創建                        |

SQL Server 和 MySQL 都有一些系統資料庫，但它們在功能和設計機制上有所不同。以下是對比 Microsoft SQL Server 的四個預設資料庫（`master`、`model`、`msdb`、`tempdb`）與 MySQL 系統資料庫的解析：


### **SQL Server 和 MySQL 機制的比較**

| 功能/用途              | **SQL Server 預設資料庫**               | **MySQL 系統資料庫**                   |
|-----------------------|---------------------------------------|---------------------------------------|
| **伺服器元數據管理**     | `master`                              | `mysql`                              |
| **新建資料庫模板**       | `model`                               | 無直接對應，可用全域參數模擬          |
| **備份與作業管理**       | `msdb`                                | 無直接對應，使用外部工具（如 cron）   |
| **臨時數據與結果**       | `tempdb`                              | 使用 `CREATE TEMPORARY TABLE`        |
| **元數據和結構查詢**     | 使用 `sys` 視圖                      | `information_schema` 和 `performance_schema` |
| **性能監控與診斷**       | 動態管理視圖（DMVs）、XEvents          | `performance_schema` 和 `sys`       |


### **新增空的 Web 專案**

空專案是一個最簡單的 ASP.NET Core 專案，不包含任何預設的 Middleware 或服務

```bash
dotnet new web -n EmptyWeb8 -f net8.0
```

### **Middleware 的執行順序**

當收到 HTTP Request 時，請求會由 Middleware 逐步上到下執行，因此 Middleware 的註冊順序會影響執行順序
   - 當 Middleware 執行後可透過 `next()` 方法將請求傳遞到下一個 Middleware
   - 當 Terminal Middleware 執行後，請求將不會再傳遞到下一個 Middleware，而是由下到上逐步返回並執行 `next()` 下面的邏輯
   - 因此第一個 Middleware 會是最後一個執行的 Middleware
   - 要透過 Middleware 執行 Try-Catch 錯誤處理，需將 Try-Catch 放在第一個註冊的 Middleware 中，確保所有邏輯都能被 Try-Catch 包裹
   - 可以透過靜態類別擴充方法的方式來定義 Middleware，這樣可以讓 Middleware 的註冊更加簡潔
   - Fuction 名稱可以使用中文，只要檔案編碼為 UTF-8 即可
   - Middleware 無法取得終端架構的資訊，像是路由資訊，因此無法取得 Controller 與 Action 的資訊


### **Middleware 的使用方式**

1. **使用 app.Run() 來自定義 Terminal Middleware**
```csharp
app.Run(async context =>
{
   await context.Response.WriteAsync("Hello, World!");
});
```
2. **使用 app.Use() 來自定義 Middleware**
```csharp
app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("1");

    await next();

    await context.Response.WriteAsync("2");
});
```

### **Middleware Hot Reload 執行流程範例**

1. 使用者發送一個帶有 `?hotreload` 的 HTTP 請求，例如：
   ```
   https://example.com/page?hotreload
   ```
2. 伺服器收到請求，進入這段程式碼。
3. `context.Request.Query.ContainsKey("hotreload")` 返回 `true`。
4. 伺服器在回應中插入：
   ```html
   <script src="/_framework/aspnetcore-browser-refresh.js"></script>
   ```
5. 瀏覽器收到回應，執行插入的 JavaScript 腳本，啟用 Hot Reload 功能。


### **Middleware 的擺放順序**

當要使用 Middleware 時，應該要注意 Middleware 的擺放順序，但此順序並無標準答案，很多時候文件也不一定有寫說要放在哪
   - 若未自行指定 `app.UseRouting()` 則 `app.UseRouting()` 會在 Middleware 的第一個來執行
   - Map 開頭的 Middleware 無順序性
   - 以下為 MVC 的 Middleware 順序
```csharp
app.UseExceptionHandler("/Home/Error");
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute();
```

### **`app.UseRouting()`**

Routing 會去處理網址上的路由結構，找到這個網址要去對到誰去執行
- URL Read/Write 肯定要放在 Routing 後面
- StaticFile 跟 Routing 並沒有相依性
- 將傳入的 HTTP 請求與路由表進行匹配，匹配路由


### **Response Security Http Header Middleware 加在哪**

`MapControllerRoute` 前較合適 (沒標準答案)

延伸問題：放置 `next()` 上面還是下面？ 

上面，Header 要先送才能送 Body

- Http response 有 output buffer，若後續的 Middleware 輸出的 response body 超過 buffer 就會先將內容回傳出去，而 Header 就來不及處理
- 在處理 HTTP 回應時，標頭必須在主體發送之前發出。因為一旦主體開始發送，ASP.NET Core 就會鎖定（flush）標頭，這時你就不能再更改標頭了。如果標頭處理放在 `next()` 之後，但 `next()` 可能會在其後的中介軟體中導致主體開始發送（比如當內容超過 buffer 大小時），這樣就來不及修改或添加標頭了

