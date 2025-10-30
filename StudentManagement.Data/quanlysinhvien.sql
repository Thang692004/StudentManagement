--
-- Database: `quanlysinhvien`
-- Khởi tạo database, nếu database chưa tồn tại
-- CREATE DATABASE IF NOT EXISTS `quanlysinhvien` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
-- USE `quanlysinhvien`;
--

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

-- --------------------------------------------------------
-- Table structure for table `khoa`
-- --------------------------------------------------------

CREATE TABLE `khoa` (
  `MaKhoa` varchar(10) NOT NULL,
  `TenKhoa` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `khoa`
--
INSERT INTO `khoa` (`MaKhoa`, `TenKhoa`) VALUES
('CNTT', 'Công nghệ Thông tin'),
('KT', 'Kinh tế'),
('DL', 'Du lịch'),
('XD', 'Xây dựng');

-- --------------------------------------------------------
-- Table structure for table `lop`
-- --------------------------------------------------------

CREATE TABLE `lop` (
  `MaLop` varchar(10) NOT NULL,
  `TenLop` varchar(100) NOT NULL,
  `MaKhoa` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `lop`
--
INSERT INTO `lop` (`MaLop`, `TenLop`, `MaKhoa`) VALUES
('CNTT01', 'Công nghệ thông tin 01', 'CNTT'),
('CNTT02', 'Công nghệ thông tin 02', 'CNTT'),
('KT01', 'Kế toán 01', 'KT'),
('KT02', 'Kinh doanh Quốc tế 02', 'KT'),
('DL01', 'Quản trị Du lịch 01', 'DL'),
('XD01', 'Kỹ thuật Xây dựng 01', 'XD');

-- --------------------------------------------------------
-- Table structure for table `nhat_ky_he_thong`
-- --------------------------------------------------------

CREATE TABLE `nhat_ky_he_thong` (
  `ID` int(11) NOT NULL,
  `TenDangNhap` varchar(50) NOT NULL,
  `ThoiGian` datetime NOT NULL DEFAULT current_timestamp(),
  `HanhDong` varchar(200) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------
-- Table structure for table `sinh_vien`
-- --------------------------------------------------------

CREATE TABLE `sinh_vien` (
  `MaSV` varchar(20) NOT NULL,
  `HoTen` varchar(100) NOT NULL,
  `NgaySinh` date DEFAULT NULL,
  `GioiTinh` varchar(10) DEFAULT NULL,
  `DiaChi` varchar(200) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `SoDienThoai` varchar(15) DEFAULT NULL,
  `MaKhoa` varchar(10) NOT NULL,
  `MaLop` varchar(10) NOT NULL,
  `TrangThai` tinyint(4) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `sinh_vien`
--
INSERT INTO `sinh_vien` (`MaSV`, `HoTen`, `NgaySinh`, `GioiTinh`, `DiaChi`, `Email`, `SoDienThoai`, `MaKhoa`, `MaLop`, `TrangThai`) VALUES
('SV01', 'Nguyễn Văn An', '2003-05-12', 'Nam', 'Hà Nội', 'sv01@mail.com', '0911111101', 'CNTT', 'CNTT01', 1),
('SV02', 'Trần Thị Bình', '2003-09-20', 'Nữ', 'Hải Phòng', 'sv02@mail.com', '0922222202', 'CNTT', 'CNTT01', 1),
('SV03', 'Lê Văn Chính', '2004-01-15', 'Nam', 'Đà Nẵng', 'sv03@mail.com', '0933333303', 'CNTT', 'CNTT02', 1),
('SV04', 'Phạm Thị Duyên', '2004-11-30', 'Nữ', 'Huế', 'sv04@mail.com', '0944444404', 'CNTT', 'CNTT02', 1),
('SV05', 'Hoàng Văn Huy', '2002-07-10', 'Nam', 'TP.HCM', 'sv05@mail.com', '0955555505', 'KT', 'KT01', 1),
('SV06', 'Đỗ Thị Kiều', '2002-03-25', 'Nữ', 'Nghệ An', 'sv06@mail.com', '0966666606', 'KT', 'KT01', 1),
('SV07', 'Bùi Văn Lâm', '2003-08-19', 'Nam', 'Hà Nam', 'sv07@mail.com', '0977777707', 'DL', 'DL01', 1),
('SV08', 'Ngô Thị Mai', '2004-12-02', 'Nữ', 'Nam Định', 'sv08@mail.com', '0988888808', 'DL', 'DL01', 1),
('SV09', 'Phan Văn Phong', '2003-04-05', 'Nam', 'Hà Nội', 'sv09@mail.com', '0999999909', 'KT', 'KT02', 1),
('SV10', 'Vũ Thị Thanh', '2004-06-14', 'Nữ', 'Quảng Ninh', 'sv10@mail.com', '0901010110', 'KT', 'KT02', 1),
('SV11', 'Trần Đình Trung', '2005-10-09', 'Nam', 'Hải Phòng', 'sv11@mail.com', '0912121211', 'CNTT', 'CNTT01', 1),
('SV12', 'Đặng Thị Lan', '2004-02-17', 'Nữ', 'Hải Dương', 'sv12@mail.com', '0923232312', 'CNTT', 'CNTT01', 1),
('SV13', 'Lý Văn Hải', '2003-05-22', 'Nam', 'Đà Nẵng', 'sv13@mail.com', '0934343413', 'XD', 'XD01', 1),
('SV14', 'Mai Thị Nguyệt', '2003-09-11', 'Nữ', 'Huế', 'sv14@mail.com', '0945454514', 'XD', 'XD01', 1),
('SV15', 'Nguyễn Minh Quân', '2002-01-29', 'Nam', 'TP.HCM', 'sv15@mail.com', '0956565615', 'KT', 'KT01', 1),
('SV16', 'Bùi Thị Tuyết', '2004-07-16', 'Nữ', 'Nghệ An', 'sv16@mail.com', '0967676716', 'KT', 'KT01', 1),
('SV17', 'Phạm Văn Hùng', '2003-11-04', 'Nam', 'Hà Nam', 'sv17@mail.com', '0978787817', 'DL', 'DL01', 1),
('SV18', 'Hoàng Thị Yến', '2002-03-13', 'Nữ', 'Nam Định', 'sv18@mail.com', '0989898918', 'DL', 'DL01', 1),
('SV19', 'Vũ Văn Khang', '2004-12-27', 'Nam', 'Hà Nội', 'sv19@mail.com', '0990909019', 'CNTT', 'CNTT02', 1),
('SV20', 'Nguyễn Thị Hương', '2003-08-08', 'Nữ', 'Quảng Ninh', 'sv20@mail.com', '0901111220', 'CNTT', 'CNTT02', 1);

-- --------------------------------------------------------
-- Table structure for table `tai_khoan`
-- --------------------------------------------------------

CREATE TABLE `tai_khoan` (
  `TenDangNhap` varchar(50) NOT NULL,
  `MatKhau` varchar(255) NOT NULL,
  `VaiTro` enum('Admin','Student') NOT NULL,
  `MaSV` varchar(20) DEFAULT NULL,
  `TrangThai` tinyint(4) DEFAULT 1,
  `LanSai` int(11) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tai_khoan` (Đã được hợp nhất từ bảng users cũ)
--
INSERT IGNORE INTO `tai_khoan` 
    (`TenDangNhap`, `MatKhau`, `VaiTro`, `MaSV`) 
VALUES
    ('admin', '123456', 'Admin', NULL), -- Admin không có MaSV
    ('sv01', '123456', 'Student', 'SV01'),
    ('sv02', '123456', 'Student', 'SV02'),
    ('sv03', '123456', 'Student', 'SV03'),
    ('sv04', '123456', 'Student', 'SV04'),
    ('sv05', '123456', 'Student', 'SV05'),
    ('sv06', '123456', 'Student', 'SV06'),
    ('sv07', '123456', 'Student', 'SV07'),
    ('sv08', '123456', 'Student', 'SV08'),
    ('sv09', '123456', 'Student', 'SV09'),
    ('sv10', '123456', 'Student', 'SV10'),
    ('sv11', '123456', 'Student', 'SV11'),
    ('sv12', '123456', 'Student', 'SV12'),
    ('sv13', '123456', 'Student', 'SV13'),
    ('sv14', '123456', 'Student', 'SV14'),
    ('sv15', '123456', 'Student', 'SV15'),
    ('sv16', '123456', 'Student', 'SV16'),
    ('sv17', '123456', 'Student', 'SV17'),
    ('sv18', '123456', 'Student', 'SV18'),
    ('sv19', '123456', 'Student', 'SV19'),
    ('sv20', '123456', 'Student', 'SV20');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `khoa`
--
ALTER TABLE `khoa`
  ADD PRIMARY KEY (`MaKhoa`);

--
-- Indexes for table `lop`
--
ALTER TABLE `lop`
  ADD PRIMARY KEY (`MaLop`),
  ADD KEY `fk_lop_khoa` (`MaKhoa`);

--
-- Indexes for table `nhat_ky_he_thong`
--
ALTER TABLE `nhat_ky_he_thong`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `fk_nhatky_taikhoan` (`TenDangNhap`);

--
-- Indexes for table `sinh_vien`
--
ALTER TABLE `sinh_vien`
  ADD PRIMARY KEY (`MaSV`),
  ADD UNIQUE KEY `Email` (`Email`),
  ADD KEY `fk_sv_khoa` (`MaKhoa`),
  ADD KEY `fk_sv_lop` (`MaLop`);

--
-- Indexes for table `tai_khoan`
--
ALTER TABLE `tai_khoan`
  ADD PRIMARY KEY (`TenDangNhap`),
  ADD KEY `fk_taikhoan_sinhvien` (`MaSV`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `nhat_ky_he_thong`
--
ALTER TABLE `nhat_ky_he_thong`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `lop`
--
ALTER TABLE `lop`
  ADD CONSTRAINT `fk_lop_khoa` FOREIGN KEY (`MaKhoa`) REFERENCES `khoa` (`MaKhoa`) ON UPDATE CASCADE;

--
-- Constraints for table `nhat_ky_he_thong`
--
ALTER TABLE `nhat_ky_he_thong`
  ADD CONSTRAINT `fk_nhatky_taikhoan` FOREIGN KEY (`TenDangNhap`) REFERENCES `tai_khoan` (`TenDangNhap`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `sinh_vien`
--
ALTER TABLE `sinh_vien`
  ADD CONSTRAINT `fk_sv_khoa` FOREIGN KEY (`MaKhoa`) REFERENCES `khoa` (`MaKhoa`) ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_sv_lop` FOREIGN KEY (`MaLop`) REFERENCES `lop` (`MaLop`) ON UPDATE CASCADE;

--
-- Constraints for table `tai_khoan`
--
ALTER TABLE `tai_khoan`
  ADD CONSTRAINT `fk_taikhoan_sinhvien` FOREIGN KEY (`MaSV`) REFERENCES `sinh_vien` (`MaSV`) ON DELETE SET NULL ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;