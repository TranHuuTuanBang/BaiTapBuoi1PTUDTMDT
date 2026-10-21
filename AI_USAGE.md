# AI Usage Log

## 1. Công cụ AI

- **Tên công cụ:** ChatGPT
- **Mục đích:** Hỗ trợ phân tích giao diện website, đề xuất cải tiến và hướng dẫn xây dựng giao diện bằng HTML/CSS.

## 2. Mục đích sử dụng

AI được sử dụng để:
- Phân tích giao diện thẻ sản phẩm trên trang chủ Thế Giới Di Động.
- Xác định các vấn đề về bố cục, cách hiển thị giá bán, giá gốc và thông tin khuyến mãi.
- Đề xuất bố cục thẻ sản phẩm thống nhất, dễ quan sát.
- Giải thích và hỗ trợ viết mã HTML/CSS.
- Hướng dẫn kiểm tra giao diện trên trình duyệt và điều chỉnh cho màn hình máy tính, điện thoại.

## 3. Prompt đã sử dụng

Các prompt chính đã sử dụng trong quá trình thực hiện:

**Prompt 1 – Phân tích giao diện:**

“Phân tích giao diện website Thế Giới Di Động, tập trung vào khu vực hiển thị danh sách sản phẩm trên trang chủ. Xác định vấn đề về bố cục, cách hiển thị giá bán, giá gốc và thông tin khuyến mãi, sau đó đề xuất hướng cải tiến.”

**Prompt 2 – Đề xuất thiết kế:**

“Đề xuất cải tiến giao diện thẻ sản phẩm bằng HTML/CSS, bảo đảm các thẻ có bố cục thống nhất, giá bán nổi bật, thông tin giảm giá dễ nhận biết và tên sản phẩm không làm lệch bố cục.”

**Prompt 3 – Xây dựng giao diện:**

“Hướng dẫn xây dựng trang HTML/CSS hiển thị 4 thẻ sản phẩm, sử dụng CSS Grid và Flexbox để căn chỉnh hình ảnh, tên sản phẩm, giá bán, giá gốc, nhãn khuyến mãi và nút Mua ngay.”

**Prompt 4 – Kiểm tra giao diện:**

“Hướng dẫn điều chỉnh giao diện thẻ sản phẩm để hiển thị phù hợp trên máy tính và điện thoại, đồng thời giữ kích thước các thẻ và cách trình bày thông tin nhất quán.”
## 4. Kết quả AI đề xuất

AI đề xuất các giải pháp chính:
- Chuẩn hóa cấu trúc và kích thước thẻ sản phẩm.
- Sử dụng CSS Grid để sắp xếp danh sách sản phẩm.
- Sử dụng Flexbox để căn chỉnh các thành phần bên trong thẻ.
- Làm nổi bật giá bán; trình bày giá gốc bằng chữ gạch ngang và bổ sung phần trăm giảm giá.
- Giới hạn tên sản phẩm tối đa hai dòng.
- Bổ sung nhãn khuyến mãi, thông tin đánh giá và nút Mua ngay.
- Sử dụng Media Queries để điều chỉnh số cột theo kích thước màn hình.

## 5. Kiểm tra và điều chỉnh

Sau khi nhận hướng dẫn, tôi đã:
- Tạo và chỉnh sửa hai file `index.html` và `style.css`.
- Mở trang HTML bằng trình duyệt để kiểm tra kết quả hiển thị.
- Điều chỉnh chiều cao khu vực tên sản phẩm để hạn chế chênh lệch giữa các thẻ.
- Điều chỉnh chiều cao hình ảnh để bố cục sản phẩm đồng đều hơn.
- Bổ sung các kiểu hiển thị cho nhãn khuyến mãi, giá gốc, phần trăm giảm giá và thông tin đánh giá.
- Kiểm tra bố cục trên các kích thước màn hình khác nhau.

Các thông tin giá bán, đánh giá và số lượng đã bán được sử dụng để minh họa, không đại diện cho dữ liệu bán hàng thực tế của website.

## 6. Phần tôi hiểu và có thể giải thích

- **HTML5:** Xây dựng cấu trúc trang bằng các thẻ như `header`, `main`, `section`, `article`, `h2`, `h3`, `img` và `button`.
- **CSS Grid:** Sắp xếp các thẻ sản phẩm thành nhiều cột.
- **Flexbox:** Sắp xếp các thành phần bên trong thẻ sản phẩm theo chiều dọc và hỗ trợ đẩy nút Mua ngay xuống cuối thẻ.
- **Media Queries:** Thay đổi bố cục khi kích thước màn hình thay đổi.
- **`object-fit: contain`:** Giúp hình ảnh sản phẩm nằm gọn trong khung mà không bị cắt theo cách thông thường.
- **`line-clamp`:** Giới hạn số dòng hiển thị của tên sản phẩm.
- **`text-decoration: line-through`:** Tạo đường gạch ngang cho giá gốc.
- **`margin-top: auto`:** Hỗ trợ đẩy nút xuống cuối thẻ khi thẻ sử dụng Flexbox theo chiều dọc.

AI đóng vai trò hỗ trợ trong quá trình thực hiện. Tôi cần hiểu chức năng của các thành phần mã nguồn, kiểm tra kết quả thực tế và chịu trách nhiệm về sản phẩm cuối cùng.