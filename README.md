# StudentManagement

## Mục lục

- [Giới thiệu](#giới-thiệu)
- [Tính năng chính](#tính-năng-chính)
- [Kiến trúc hệ thống](#kiến-trúc-hệ-thống)
- [Cấu trúc dự án](#cấu-trúc-dự-án)
- [Công nghệ sử dụng](#công-nghệ-sử-dụng)
- [Yêu cầu hệ thống](#yêu-cầu-hệ-thống)
- [Cách cài đặt và chạy](#cách-cài-đặt-và-chạy)
- [Ví dụ sử dụng](#ví-dụ-sử-dụng)
- [Kiểm thử](#kiểm-thử)
- [Đóng góp](#đóng-góp)
- [Giấy phép](#giấy-phép)
- [Liên hệ](#liên-hệ)

## Giới thiệu

Dự án **StudentManagement** là một ứng dụng quản lý sinh viên toàn diện được phát triển bằng ngôn ngữ lập trình C#. Ứng dụng được thiết kế để hỗ trợ ba loại người dùng chính: Quản trị viên (Admin), Giảng viên (Lecturer) và Sinh viên (Student). Mỗi vai trò được phân quyền rõ ràng để đảm bảo an ninh và hiệu quả trong việc quản lý hệ thống giáo dục.

Ứng dụng giúp tự động hóa các quy trình như quản lý tài khoản, môn học, phân công giảng dạy, đăng ký học phần, và chấm điểm, giảm thiểu sai sót thủ công và tăng cường trải nghiệm người dùng.

## Tính năng chính

### Cho Quản trị viên (Admin):
- Quản lý tài khoản người dùng (thêm, sửa, xóa Admin, Lecturer, Student).
- Quản lý thông tin môn học (tạo, cập nhật, xóa môn học).
- Phân công giảng viên cho các môn học.
- Giám sát hoạt động hệ thống và báo cáo tổng quan.

### Cho Giảng viên (Lecturer):
- Xem danh sách môn học được phân công.
- Nhập và cập nhật điểm số cho sinh viên.
- Xem hồ sơ sinh viên trong các lớp học.
- Báo cáo tiến độ học tập.

### Cho Sinh viên (Student):
- Đăng ký học phần theo lịch trình.
- Xem điểm số và tiến độ học tập.
- Cập nhật thông tin cá nhân.
- Tra cứu thông tin môn học và giảng viên.

### Tính năng chung:
- Hệ thống xác thực bảo mật (đăng nhập, đăng xuất).
- Giao diện thân thiện, dễ sử dụng.
- Tìm kiếm và lọc thông tin nhanh chóng.
- Xuất báo cáo (PDF, Excel) cho các dữ liệu quan trọng.

## Kiến trúc hệ thống

Ứng dụng được xây dựng theo mô hình kiến trúc phân lớp (Layered Architecture), bao gồm:

- **Presentation Layer (UI)**: Xử lý giao diện người dùng và tương tác.
- **Business Logic Layer (Core)**: Chứa logic nghiệp vụ và quy tắc xử lý.
- **Data Access Layer (Data)**: Quản lý truy cập và thao tác với cơ sở dữ liệu.

Điều này đảm bảo tính tách biệt, dễ bảo trì và mở rộng.

## Cấu trúc dự án

Dự án được tổ chức theo kiến trúc phân lớp để dễ bảo trì và mở rộng:

- **StudentManagement.Core/**: Chứa logic cốt lõi của ứng dụng, bao gồm các lớp xử lý nghiệp vụ và truy vấn dữ liệu (ví dụ: `ReadDatabase/ReadUsers.cs`).
- **StudentManagement.Data/**: Lớp truy cập dữ liệu, chịu trách nhiệm tương tác với cơ sở dữ liệu, bao gồm các entity models và repository patterns.
- **StudentManagement.UI/**: Giao diện người dùng, chứa các form, control và xử lý event người dùng. Có thể sử dụng Windows Forms hoặc WPF.
- **StudentManagement.Tests/**: Chứa các unit test và integration test để đảm bảo chất lượng code.
- **StudentManagement.sln**: File solution của Visual Studio để quản lý toàn bộ dự án và dependencies.

## Công nghệ sử dụng

- **Ngôn ngữ lập trình**: C# (.NET Standard/Core)
- **Framework**: .NET Framework 4.8 hoặc .NET Core 3.1+ (tùy phiên bản)
- **Cơ sở dữ liệu**: SQL Server hoặc SQLite (có thể cấu hình trong code)
- **ORM**: Entity Framework hoặc ADO.NET cho truy cập dữ liệu
- **UI Framework**: Windows Forms hoặc WPF
- **Testing Framework**: NUnit hoặc MSTest
- **IDE khuyến nghị**: Visual Studio 2019/2022
- **Version Control**: Git

## Yêu cầu hệ thống

- **Hệ điều hành**: Windows 10/11 (khuyến nghị), hoặc Linux/Mac với .NET Core.
- **Bộ nhớ**: Ít nhất 4GB RAM.
- **Đĩa cứng**: 1GB dung lượng trống.
- **Phần mềm**: .NET Framework/Core SDK, SQL Server hoặc tương tự.
- **Khác**: Git để clone repository.

## Cách cài đặt và chạy

1. **Clone repository**:
   ```
   git clone https://github.com/Thang692004/StudentManagement.git
   cd StudentManagement
   ```

2. **Mở dự án**:
   - Mở file `StudentManagement.sln` bằng Visual Studio.

3. **Cài đặt dependencies**:
   - NuGet packages sẽ được khôi phục tự động khi build.
   - Nếu cần, chạy `nuget restore` trong Package Manager Console.

4. **Cấu hình cơ sở dữ liệu**:
   - Tạo database mới trong SQL Server.
   - Cập nhật connection string trong file cấu hình (app.config hoặc appsettings.json).

5. **Build dự án**:
   - Chọn Build > Build Solution trong Visual Studio.

6. **Chạy ứng dụng**:
   - Nhấn F5 hoặc chọn Start để chạy.
   - Đăng nhập với tài khoản Admin mặc định (nếu có).

### Lưu ý:
- Đảm bảo SQL Server đang chạy nếu sử dụng local database.
- Nếu gặp lỗi, kiểm tra log trong Output window của Visual Studio.

## Ví dụ sử dụng

1. **Đăng nhập**:
   - Chọn vai trò (Admin/Lecturer/Student) và nhập credentials.

2. **Quản lý sinh viên (Admin)**:
   - Điều hướng đến menu "Quản lí sinh viên".
   - Thêm sinh viên mới bằng cách nhập tên, mã sv, email, sdt ...

3. **Xem thông tin lớp học, khoa (Student)**:
   - Vào "Danh sách Khoa/Danh sách Lớp".


## Kiểm thử

Dự án bao gồm unit tests trong thư mục `StudentManagement.Tests/`. Để chạy tests:

1. Mở Test Explorer trong Visual Studio.
2. Chọn Run All Tests.

Các tests bao gồm:
- Tests cho logic nghiệp vụ.
- Tests cho data access.
- Integration tests cho UI.

## Đóng góp

Chúng tôi chào đón mọi đóng góp từ cộng đồng! Nếu bạn muốn cải thiện dự án, vui lòng:

- Tạo issue để báo cáo lỗi hoặc đề xuất tính năng mới.
- Fork repository và tạo pull request với các thay đổi.
- Tuân thủ coding standards (sử dụng StyleCop hoặc tương tự).
- Viết unit tests cho mọi thay đổi logic.
- Cập nhật documentation nếu cần.


Cảm ơn bạn đã quan tâm đến dự án StudentManagement!
