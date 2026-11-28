# Cuộn Phim

Bài tập cá nhân Phát triển ứng dụng TMĐT của Trần Hữu Tuấn Bằng, MSSV 31241026329, lớp 26C1INF50902702.

Cuộn Phim mô phỏng một cửa hàng phim. Khách có thể lọc phim, xem chi tiết, tạo tài khoản, cập nhật giỏ và đặt hàng thử. Quản trị viên quản lý phim, thể loại và poster. Checkout ghi đơn, chi tiết và địa chỉ giao hàng vào SQL Server. Bài thực hành chỉ dùng dữ liệu mẫu và không thu tiền.

## Công nghệ và thư mục

- ASP.NET Core MVC .NET 10, Razor, ASP.NET Core Identity, EF Core 10 và Microsoft SQL Server.
- `SourceCode/31241026329_TranHuuTuanBang` chứa project, các controller, model, view, dịch vụ và migration. Tên namespace có dấu gạch dưới đầu để hợp lệ trong C#.
- `Report` chứa báo cáo và minh chứng. `SamplePosters` chứa poster minh họa để tải lên qua giao diện.
- Migration theo thứ tự `InitialCreate`, `MoviesAndGenres`, `HoaDon` và `ShippingDetails`.

## Chạy local

Cài SDK .NET 10, VS Code và C# Dev Kit. Có thể dùng SSMS để xem dữ liệu. Tạo cơ sở dữ liệu SQL Server rỗng trên Somee hoặc SQL Server của máy thực hành.

```powershell
cd ASP-CORE/SourceCode/31241026329_TranHuuTuanBang
dotnet --list-sdks
dotnet tool restore
dotnet restore
dotnet dev-certs https --trust
```

Trong PowerShell, lưu chuỗi kết nối trên máy qua biến môi trường của phiên. Khi kết nối Somee, thêm `TrustServerCertificate=True` vào chuỗi nếu chưa có, theo hướng dẫn bài tập. Thiết lập tài khoản quản trị bằng email dùng thử và mật khẩu mạnh trước lần chạy đầu tiên.

```powershell
$env:ConnectionStrings__DefaultConnection = Read-Host "Connection string SQL Server"
$env:Admin__Email = Read-Host "Email quan tri thu"
$adminSecret = Read-Host "Mat khau quan tri" -AsSecureString
$env:Admin__Password = (New-Object System.Net.NetworkCredential("", $adminSecret)).Password
dotnet ef database update
dotnet run --launch-profile https
```

Có thể thay biến môi trường bằng ASP.NET Core User Secrets trên máy phát triển. Khóa tương ứng là `ConnectionStrings:DefaultConnection`, `Admin:Email`, `Admin:Password`. Đóng cửa sổ PowerShell sau khi thực hành để kết thúc các biến của phiên. Tài khoản quản trị đã tạo vẫn nằm trong cơ sở dữ liệu. Có thể bỏ cấu hình bootstrap sau lần chạy thành công đầu tiên.

Tại địa chỉ HTTPS trong dòng `Now listening on`, đăng nhập quản trị. Mở **Thể loại** để tạo ít nhất ba mục, rồi dùng **Thêm phim** để nhập ít nhất năm phim và upload poster. Chọn ngày phát hành từ hôm nay trở đi. Thử thêm phim vào giỏ, cập nhật số lượng, xóa một dòng, đặt thử bằng dữ liệu giả và kiểm tra ba bảng đơn hàng trong SSMS.

Nếu cần minh chứng bước xác nhận đăng ký ở môi trường local, có thể đặt `Identity__RequireConfirmedAccount=true` trước khi chạy. Identity mặc định của dự án cho phép đăng ký và đăng nhập trực tiếp khi khóa này bỏ trống. Quản trị phim và thể loại luôn cần vai trò Admin.

Email xác nhận qua Gmail SMTP nằm trong quy trình checkout. Để thử, đặt `Email__Address`, `Email__AppPassword` và `Email__TestRecipient` bằng cấu hình riêng. Checkout chỉ gửi đến địa chỉ thử đã cấu hình. Đơn hàng được lưu trước khi gửi thư và vẫn có trang xác nhận nếu không gửi được email.

## Triển khai Somee

```powershell
dotnet publish -c Release -r win-x64 --self-contained false -p:UseAppHost=false -o ./publish
dotnet ef migrations script --idempotent -o migrations.sql
```

Áp dụng migration đến cơ sở dữ liệu đã tạo, dùng `dotnet ef database update` với đúng chuỗi kết nối hoặc chạy tập lệnh SQL bằng công cụ SQL có hỗ trợ phân tách batch `GO`. Nén nội dung thư mục `publish` với `web.config` ở gốc. Tải bản nén lên File manager của Somee và giải nén. Cấu hình bí mật bên hosting bằng biến môi trường hoặc file `appsettings.Production.json` chỉ nằm ở hosting. Các khóa mẫu có trong `appsettings.example.json`. Nếu dùng trình quản lý tệp Somee, có thể lưu riêng chuỗi kết nối một dòng trong `App_Data/connection.txt`. Thư mục này nằm ngoài `wwwroot` của ASP.NET Core và được IIS bảo vệ. Cấu hình triển khai chạy Kestrel phía sau IIS (out-of-process).

Poster upload được giữ trong `App_Data/posters` và ứng dụng phục vụ qua `/posters`. Giữ thư mục đó khi cập nhật ứng dụng và sao lưu cùng cơ sở dữ liệu. Sau khi cấu hình HTTPS và chạy site, kiểm tra trên cửa sổ ẩn danh, mở kho phim, thử giỏ hàng và checkout trước khi dán URL vào LMS.

Website triển khai: https://cuonphim31241026329.somee.com/
