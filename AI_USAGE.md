# AI Usage Log – Buổi 2

## 1. Công cụ AI
ChatGPT được sử dụng để đọc yêu cầu bài tập, rà soát mã nguồn Buổi 1, giải thích và hỗ trợ chỉnh sửa HTML/CSS.

## 2. Mục đích sử dụng
- Kiểm tra cấu trúc HTML hiện có và đề xuất phần tử HTML5 có ngữ nghĩa.
- Hướng dẫn thực hành CSS selector, Box Model, Flexbox, Grid, hover và focus-visible.
- Hỗ trợ cập nhật README và soạn nội dung báo cáo.
- Nhắc kiểm tra thủ công trước khi xác nhận giao diện hoạt động.

## 3. Prompt tiêu biểu
1. “Đọc mã HTML/CSS hiện tại của website Thế Giới Di Động từ Buổi 1. Chỉ ra cấu trúc HTML5 đã có, phần tử còn thiếu và các điểm có thể cải thiện. Không tạo website mới.”
2. “Tiếp tục phát triển trang hiện tại bằng HTML5 có ngữ nghĩa; giữ lại bốn sản phẩm và đường dẫn ảnh hiện có; bổ sung nav, main, section, article, alt và điều hướng nội trang phù hợp.”
3. “Thực hành CSS selector, Box Model, Flexbox, Grid, hover và focus-visible trên giao diện hiện có. Giữ CSS trong file riêng và không sử dụng framework.”
4. “Kiểm tra README và AI_USAGE để mô tả đúng các thay đổi đã thực hiện, không ghi nhận những kiểm tra chưa thực sự chạy.”

## 4. Kết quả hỗ trợ
- Tổ chức lại HTML theo header, nav, main, section, article và footer.
- Bổ sung aria-labelledby cho từng article và mô tả alt cho ảnh.
- Dùng Flexbox cho header, menu và nội dung thẻ sản phẩm.
- Dùng Grid và media query cho danh sách sản phẩm.
- Bổ sung hover, transition, focus-visible và prefers-reduced-motion.

## 5. Kiểm tra và điều chỉnh
Mã nguồn HTML và CSS đã được cập nhật trên repository. Việc mở website trên trình duyệt ở các kích thước khác nhau, thử Tab/Enter, kiểm tra ảnh và chụp ảnh trước/sau vẫn cần người học trực tiếp thực hiện. Không coi thao tác cập nhật mã từ xa là bằng chứng đã kiểm thử trên thiết bị của sinh viên.

## 6. Phần kiến thức cần giải thích
- header/nav/main/section/article/footer: phân vùng nội dung theo ngữ nghĩa.
- alt: mô tả thay thế cho hình ảnh.
- margin/padding/border: các phần của Box Model.
- display:flex, justify-content, align-items, gap: căn chỉnh theo trục.
- display:grid, grid-template-columns, repeat, minmax: bố trí hàng/cột.
- :hover và :focus-visible: phản hồi chuột và bàn phím.

Giá, đánh giá và số lượng bán hiển thị trong giao diện là dữ liệu minh họa, không phải dữ liệu bán hàng thực tế.
