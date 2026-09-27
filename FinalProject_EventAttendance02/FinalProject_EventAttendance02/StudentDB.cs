using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_EventAttendance02
{
    public static class StudentDB
    {
        
        public static int Insert(generateQR qr)
        {
            using var conn = DbHelper.GetConnection();
            const string sql = @"
                INSERT INTO students (student_code, student_name, course, year_level,
                                      photo_path, qr_code_data)
                VALUES (@code, @name, @course, @year, @photo, @qr)
                ON DUPLICATE KEY UPDATE
                    student_name = @name,
                    course = @course,
                    year_level = @year,
                    photo_path = @photo,
                    qr_code_data = @qr;";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@code", qr.StudentID);
            cmd.Parameters.AddWithValue("@name", qr.StudentName);
            cmd.Parameters.AddWithValue("@course", qr.Course);
            cmd.Parameters.AddWithValue("@year", qr.Year);
            cmd.Parameters.AddWithValue("@photo", qr.PhotoPath ?? "");
            cmd.Parameters.AddWithValue("@qr", qr.QRCodeData ?? "");
            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        // select all
        public static List<generateQR> GetAll()
        {
            var list = new List<generateQR>();
            using var conn = DbHelper.GetConnection();
            const string sql = "SELECT * FROM students ORDER BY student_name";
            using var cmd = new MySqlCommand(sql, conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new generateQR
                {
                    StudentID = reader["student_code"]?.ToString() ?? string.Empty,
                    StudentName = reader["student_name"]?.ToString() ?? string.Empty,
                    Course = reader["course"]?.ToString() ?? string.Empty,
                    Year = reader["year_level"]?.ToString() ?? string.Empty,
                    PhotoPath = reader["photo_path"]?.ToString() ?? string.Empty,
                    QRCodeData = reader["qr_code_data"]?.ToString() ?? string.Empty
                });
            }
            return list;
        }

        // delete
        public static int Delete(string studentCode, string yearLevel)
        {
            using var conn = DbHelper.GetConnection();
            const string sql = @"
                DELETE FROM students
                WHERE student_code = @code AND year_level = @year";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@code", studentCode);
            cmd.Parameters.AddWithValue("@year", yearLevel);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        // exists
        public static bool Exists(string studentCode, string yearLevel)
        {
            using var conn = DbHelper.GetConnection();
            const string sql = @"
                SELECT COUNT(*) FROM students
                WHERE student_code = @code AND year_level = @year";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@code", studentCode);
            cmd.Parameters.AddWithValue("@year", yearLevel);
            conn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public static generateQR? GetByStudentCode(string studentCode)
        {
            using var conn = DbHelper.GetConnection();
            const string sql = @"
        SELECT student_code, student_name, course, year_level, photo_path, qr_code_data 
        FROM students 
        WHERE student_code = @code 
        LIMIT 1";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@code", studentCode.Trim());
            conn.Open();

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new generateQR
                {
                    StudentID = reader["student_code"]?.ToString() ?? "",
                    StudentName = reader["student_name"]?.ToString() ?? "",
                    Course = reader["course"]?.ToString() ?? "",
                    Year = reader["year_level"]?.ToString() ?? "",
                    PhotoPath = reader["photo_path"]?.ToString() ?? "",
                    QRCodeData = reader["qr_code_data"]?.ToString() ?? ""
                };
            }
            return null;
        }

        public static void Update(generateQR qr)
        {
            using var conn = DbHelper.GetConnection();
            conn.Open();
            const string query = @"UPDATE students 
                 SET student_name = @name, course = @course, photo_path = @photo, qr_code_data = @qrdata
                 WHERE student_code = @id AND year_level = @year";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", qr.StudentName);
            cmd.Parameters.AddWithValue("@course", qr.Course);
            cmd.Parameters.AddWithValue("@photo", qr.PhotoPath);
            cmd.Parameters.AddWithValue("@qrdata", qr.QRCodeData);
            cmd.Parameters.AddWithValue("@id", qr.StudentID);
            cmd.Parameters.AddWithValue("@year", qr.Year);
            cmd.ExecuteNonQuery();
        }
    }
}
