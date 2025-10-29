-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Sep 23, 2025 at 11:36 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `quanlysinhvien`
--

-- --------------------------------------------------------

--
-- Table structure for table `classes`
--

CREATE TABLE `classes` (
  `class_code` varchar(20) NOT NULL,
  `class_name` varchar(100) NOT NULL,
  `faculty` varchar(100) NOT NULL,
  `year` year(4) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `classes`
--

INSERT INTO `classes` (`class_code`, `class_name`, `faculty`, `year`) VALUES
('CNTT1', 'Công nghệ thông tin 01', 'Khoa CNTT', '2021'),
('CNTT2', 'Công nghệ thông tin 02', 'Khoa CNTT', '2022'),
('DL1', 'Du lịch 01', 'Khoa Du lịch', '2022'),
('KT1', 'Kế toán 01', 'Khoa Kinh tế', '2021'),
('QTKD1', 'Quản trị kinh doanh 01', 'Khoa Kinh tế', '2020');

-- --------------------------------------------------------

--
-- Table structure for table `logs`
--

CREATE TABLE `khoa` (
  `MaKhoa` varchar(20) NOT NULL,
  `TenKhoa` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

ALTER TABLE `khoa`
  ADD PRIMARY KEY (`MaKhoa`);

INSERT INTO `khoa` (`MaKhoa`, `TenKhoa`) VALUES
('CNTT', 'Công nghệ thông tin'),
('KT', 'Kinh tế - Kế toán'),
('DL', 'Du lịch');

CREATE TABLE `logs` (
  `log_id` int(11) NOT NULL,
  `user_id` int(11) DEFAULT NULL,
  `action` enum('login','logout') DEFAULT NULL,
  `timestamp` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `logs`
--

INSERT INTO `logs` (`log_id`, `user_id`, `action`, `timestamp`) VALUES
(1, 2, 'login', '2025-09-20 08:30:00'),
(2, 2, 'logout', '2025-09-20 10:00:00'),
(3, 3, 'login', '2025-09-21 09:15:00'),
(4, 3, 'logout', '2025-09-21 11:30:00'),
(5, 4, 'login', '2025-09-22 14:00:00'),
(6, 4, 'logout', '2025-09-22 16:00:00'),
(7, 5, 'login', '2025-09-23 07:45:00'),
(8, 5, 'logout', '2025-09-23 09:15:00'),
(9, 6, 'login', '2025-09-23 08:10:00'),
(10, 6, 'logout', '2025-09-23 12:00:00');

-- --------------------------------------------------------

--
-- Table structure for table `students`
--

CREATE TABLE `students` (
  `student_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `full_name` varchar(100) NOT NULL,
  `date_of_birth` date DEFAULT NULL,
  `gender` enum('Nam','Nữ','Khác') DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `phone` varchar(20) DEFAULT NULL,
  `address` varchar(255) DEFAULT NULL,
  `major` varchar(100) DEFAULT NULL,
  `enrollment_year` year(4) DEFAULT NULL,
  `class_code` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `students`
--

INSERT INTO `students` (`student_id`, `user_id`, `full_name`, `date_of_birth`, `gender`, `email`, `phone`, `address`, `major`, `enrollment_year`, `class_code`) VALUES
(1, 2, 'Nguyen Van A', '2003-05-12', 'Nam', 'sva1@example.com', '0911111111', 'Hà Nội', 'CNTT', '2021', 'CNTT1'),
(2, 3, 'Tran Thi B', '2002-09-20', 'Nữ', 'svb2@example.com', '0922222222', 'Hải Phòng', 'CNTT', '2021', 'CNTT1'),
(3, 4, 'Le Van C', '2003-01-15', 'Nam', 'svc3@example.com', '0933333333', 'Đà Nẵng', 'CNTT', '2022', 'CNTT2'),
(4, 5, 'Pham Thi D', '2002-11-30', 'Nữ', 'svd4@example.com', '0944444444', 'Huế', 'CNTT', '2022', 'CNTT2'),
(5, 6, 'Hoang Van E', '2001-07-10', 'Nam', 'sve5@example.com', '0955555555', 'TP.HCM', 'QTKD', '2020', 'QTKD1'),
(6, 7, 'Do Thi F', '2002-03-25', 'Nữ', 'svf6@example.com', '0966666666', 'Nghệ An', 'QTKD', '2020', 'QTKD1'),
(7, 8, 'Bui Van G', '2003-08-19', 'Nam', 'svg7@example.com', '0977777777', 'Hà Nam', 'Kế toán', '2021', 'KT1'),
(8, 9, 'Ngo Thi H', '2001-12-02', 'Nữ', 'svh8@example.com', '0988888888', 'Nam Định', 'Kế toán', '2021', 'KT1'),
(9, 10, 'Phan Van I', '2002-04-05', 'Nam', 'svi9@example.com', '0999999999', 'Hà Nội', 'Du lịch', '2022', 'DL1'),
(10, 11, 'Vu Thi J', '2003-06-14', 'Nữ', 'svj10@example.com', '0901010101', 'Quảng Ninh', 'Du lịch', '2022', 'DL1'),
(11, 12, 'Nguyen Van K', '2001-10-09', 'Nam', 'svk11@example.com', '0912121212', 'Hà Nội', 'CNTT', '2021', 'CNTT1'),
(12, 13, 'Tran Thi L', '2002-02-17', 'Nữ', 'svl12@example.com', '0923232323', 'Hải Dương', 'CNTT', '2021', 'CNTT1'),
(13, 14, 'Le Van M', '2003-05-22', 'Nam', 'svm13@example.com', '0934343434', 'Đà Nẵng', 'CNTT', '2022', 'CNTT2'),
(14, 15, 'Pham Thi N', '2002-09-11', 'Nữ', 'svn14@example.com', '0945454545', 'Huế', 'CNTT', '2022', 'CNTT2'),
(15, 16, 'Hoang Van O', '2001-01-29', 'Nam', 'svo15@example.com', '0956565656', 'TP.HCM', 'QTKD', '2020', 'QTKD1'),
(16, 17, 'Do Thi P', '2003-07-16', 'Nữ', 'svp16@example.com', '0967676767', 'Nghệ An', 'QTKD', '2020', 'QTKD1'),
(17, 18, 'Bui Van Q', '2002-11-04', 'Nam', 'svq17@example.com', '0978787878', 'Hà Nam', 'Kế toán', '2021', 'KT1'),
(18, 19, 'Ngo Thi R', '2001-03-13', 'Nữ', 'svr18@example.com', '0989898989', 'Nam Định', 'Kế toán', '2021', 'KT1'),
(19, 20, 'Phan Van S', '2003-12-27', 'Nam', 'svs19@example.com', '0990909090', 'Hà Nội', 'Du lịch', '2022', 'DL1'),
(20, 21, 'Vu Thi T', '2002-08-08', 'Nữ', 'svt20@example.com', '0901111222', 'Quảng Ninh', 'Du lịch', '2022', 'DL1');

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `user_id` int(11) NOT NULL,
  `username` varchar(50) NOT NULL,
  `password` varchar(255) NOT NULL,
  `role` enum('Admin','Student') DEFAULT 'Student',
  `created_at` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`user_id`, `username`, `password`, `role`, `created_at`) VALUES
(1, 'admin', '123456', 'Admin', '2025-09-23 16:36:17'),
(2, 'sv01', '123456', 'Student', '2025-09-23 16:36:17'),
(3, 'sv02', '123456', 'Student', '2025-09-23 16:36:17'),
(4, 'sv03', '123456', 'Student', '2025-09-23 16:36:17'),
(5, 'sv04', '123456', 'Student', '2025-09-23 16:36:17'),
(6, 'sv05', '123456', 'Student', '2025-09-23 16:36:17'),
(7, 'sv06', '123456', 'Student', '2025-09-23 16:36:17'),
(8, 'sv07', '123456', 'Student', '2025-09-23 16:36:17'),
(9, 'sv08', '123456', 'Student', '2025-09-23 16:36:17'),
(10, 'sv09', '123456', 'Student', '2025-09-23 16:36:17'),
(11, 'sv10', '123456', 'Student', '2025-09-23 16:36:17'),
(12, 'sv11', '123456', 'Student', '2025-09-23 16:36:17'),
(13, 'sv12', '123456', 'Student', '2025-09-23 16:36:17'),
(14, 'sv13', '123456', 'Student', '2025-09-23 16:36:17'),
(15, 'sv14', '123456', 'Student', '2025-09-23 16:36:17'),
(16, 'sv15', '123456', 'Student', '2025-09-23 16:36:17'),
(17, 'sv16', '123456', 'Student', '2025-09-23 16:36:17'),
(18, 'sv17', '123456', 'Student', '2025-09-23 16:36:17'),
(19, 'sv18', '123456', 'Student', '2025-09-23 16:36:17'),
(20, 'sv19', '123456', 'Student', '2025-09-23 16:36:17'),
(21, 'sv20', '123456', 'Student', '2025-09-23 16:36:17');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `classes`
--
ALTER TABLE `classes`
  ADD PRIMARY KEY (`class_code`);

--
-- Indexes for table `logs`
--
ALTER TABLE `logs`
  ADD PRIMARY KEY (`log_id`),
  ADD KEY `user_id` (`user_id`);

--
-- Indexes for table `students`
--
ALTER TABLE `students`
  ADD PRIMARY KEY (`student_id`),
  ADD KEY `user_id` (`user_id`),
  ADD KEY `class_code` (`class_code`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`user_id`),
  ADD UNIQUE KEY `username` (`username`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `logs`
--
ALTER TABLE `logs`
  MODIFY `log_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `students`
--
ALTER TABLE `students`
  MODIFY `student_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `logs`
--
ALTER TABLE `logs`
  ADD CONSTRAINT `logs_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`);

--
-- Constraints for table `students`
--
ALTER TABLE `students`
  ADD CONSTRAINT `students_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`),
  ADD CONSTRAINT `students_ibfk_2` FOREIGN KEY (`class_code`) REFERENCES `classes` (`class_code`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
