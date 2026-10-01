# 🛒 TechMart Product Manager - Windows Forms Application

Dự án phần mềm quản lý danh mục sản phẩm dành cho hệ thống cửa hàng bán lẻ TechMart, được xây dựng trên nền tảng **Windows Forms (C# / .NET)** với giao diện động (Dynamic UI) linh hoạt và tự động co giãn (Responsive).

---

## 👤 Thông Tin Sinh Viên
* **Họ và tên:** Nguyễn Vũ Nhật Minh
* **Mã sinh viên:** 24810320314
* **Tài khoản GitHub:** minhnvn24810320314
* **Tên Repository:** NGUYEN-VU-NHAT-MINH_24810320314_WINFORMS-GUI
* **Học phần:** Thực hành WinForms GUI

---

## 🛠 Nền Tảng & Công Nghệ (Tech Stack)
* **Ngôn ngữ lập trình:** C# (.NET 8.0 / .NET 10.0)
* **Thư viện UI:** Windows Forms (WinForms Desktop)
* **Kiến trúc UI:** 
  * `TableLayoutPanel` & `FlowLayoutPanel`: Tự động căn chỉnh Responsive, co giãn linh hoạt theo kích thước cửa sổ ứng dụng.
  * `BindingSource` & `BindingList<T>`: Đồng bộ dữ liệu hai chiều (Data Binding) trực tiếp giữa danh sách và giao diện.
* **Quản lý phiên bản:** Git & GitHub

---

## ✨ Các Tính Năng Chính (Features)

1. **Quản lý sản phẩm (CRUD):**
   * Thêm sản phẩm mới đầy đủ thuộc tính: *Mã SP, Tên SP, Danh mục (Điện thoại, Laptop, Phụ kiện), Đơn giá, Số lượng và Hình ảnh đại diện*.
   * Xóa sản phẩm khỏi danh sách có xác nhận qua hộp thoại `MessageBox`.
2. **Kiểm tra dữ liệu đầu vào (Data Validation):**
   * Tích hợp `ErrorProvider` cảnh báo biểu tượng đỏ trực quan khi nhập thiếu Tên SP, Đơn giá `<= 0` hoặc Số lượng không hợp lệ.
3. **Hiển thị & Tương tác dữ liệu (Data Binding):**
   * Hiển thị danh sách sản phẩm dạng bảng bằng `DataGridView`.
   * Nhấp chọn dòng trên bảng để hiển thị ngược dữ liệu và xem ảnh sản phẩm trên ô `PictureBox`.
4. **Tìm kiếm thời gian thực (Real-time Search):**
   * Lọc danh sách sản phẩm tức thì theo Tên SP ngay khi người dùng gõ từ khóa.
5. **Xuất báo cáo File CSV (Export File):**
   * Hỗ trợ xuất danh sách sản phẩm ra file `.csv` qua thanh Menu `File -> Export CSV` (hoặc phím tắt `Ctrl + E`).
6. **Thanh trạng thái (Status Strip):**
   * Hiển thị và tự động cập nhật tổng số lượng sản phẩm hiện có ở góc dưới màn hình.

---

## 🚀 Hướng Dẫn Cài Đặt & Chạy Dự Án

### Yêu cầu môi trường:
* **Visual Studio 2022** (hoặc mới hơn) đã cài đặt Workload **.NET desktop development**.
* **.NET SDK 8.0 / .NET 10.0**.

### Các bước thực hiện:
1. Clone Repository này về máy:
   ```bash
   git clone [https://github.com/minhnvn24810320314/NGUYEN-VU-NHAT-MINH_24810320314_WINFORMS-GUI.git](https://github.com/minhnvn24810320314/NGUYEN-VU-NHAT-MINH_24810320314_WINFORMS-GUI.git)
