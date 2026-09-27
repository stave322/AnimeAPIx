using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_EventAttendance02
{
    public static class AttendanceDB
    {
        private const string ConnectionString =
          "Server=localhost;Database=eventattendance_db;Uid=root;Pwd=1234567890;SslMode=Disabled;Allow Zero Datetime=True;Convert Zero Datetime=True;";

        public static int InsertCheckIn(int studentId, int eventId, DateTime checkInTime)
        {
            using var conn = DbHelper.GetConnection();
            const string sql = @"
                INSERT INTO attendance (student_id, event_id, check_in_time, status)
                VALUES (@sid, @eid, @cin, 'Check In');
                SELECT LAST_INSERT_ID();";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@sid", studentId);
            cmd.Parameters.AddWithValue("@eid", eventId);
            cmd.Parameters.AddWithValue("@cin", checkInTime);
            conn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static int InsertCheckOut(int studentId, int eventId, DateTime checkOutTime)
        {
            using var conn = DbHelper.GetConnection();
            const string sql = @"
                INSERT INTO attendance (student_id, event_id, check_out_time, status)
                VALUES (@sid, @eid, @cout, 'Check Out');
                SELECT LAST_INSERT_ID();";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@sid", studentId);
            cmd.Parameters.AddWithValue("@eid", eventId);
            cmd.Parameters.AddWithValue("@cout", checkOutTime);
            conn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // Update check-out only if there is no existing check-out time (prevents overwriting)
        public static void UpdateCheckOut(int attendanceId, DateTime checkOutTime)
        {
            using var conn = DbHelper.GetConnection();
            const string sql = @"
                UPDATE attendance
                  SET check_out_time = @cout, status = 'Completed'
                  WHERE attendance_id = @aid
                    AND check_out_time IS NULL";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@cout", checkOutTime);
            cmd.Parameters.AddWithValue("@aid", attendanceId);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // Update check-in only if there is no existing check-in time (prevents overwriting)
        public static void UpdateCheckIn(int attendanceId, DateTime checkInTime)
        {
            using var conn = DbHelper.GetConnection();
            const string sql = @"
                UPDATE attendance 
                SET check_in_time = @cin, status = 'Check In' 
                WHERE attendance_id = @aid
                  AND (check_in_time IS NULL OR check_in_time = '0000-00-00 00:00:00')";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@cin", checkInTime);
            cmd.Parameters.AddWithValue("@aid", attendanceId);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public static List<Attendance> GetAll()
        {
            var list = new List<Attendance>();
            using var conn = DbHelper.GetConnection();

            const string sql = @"
                SELECT a.attendance_id,
                       a.check_in_time,
                       a.check_out_time,
                       a.status,
                       s.student_code,
                       s.student_name,
                       s.course,
                       s.year_level,
                       e.event_name,
                       e.semester
                FROM attendance a
                JOIN students s ON a.student_id = s.student_id
                JOIN events   e ON a.event_id   = e.event_id
                ORDER BY COALESCE(a.check_in_time, a.check_out_time) DESC,
                         a.attendance_id DESC";

            using var cmd = new MySqlCommand(sql, conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                DateTime checkInTime = default;
                if (reader["check_in_time"] != DBNull.Value)
                    checkInTime = Convert.ToDateTime(reader["check_in_time"]);

                DateTime? checkOutTime = null;
                if (reader["check_out_time"] != DBNull.Value)
                    checkOutTime = Convert.ToDateTime(reader["check_out_time"]);

                string rawStatus = reader["status"]?.ToString() ?? "";
                string status = rawStatus.Trim() switch
                {
                    "Check In" or "Check-in" or "check in" => "Check In",
                    "Check Out" or "Check-out" or "check out" => "Check Out",
                    "Completed" or "completed" => "Completed",
                    _ => rawStatus
                };

                list.Add(new Attendance
                {
                    AttendanceID = Convert.ToInt32(reader["attendance_id"]),
                    StudentID = reader["student_code"].ToString() ?? "",
                    StudentName = reader["student_name"].ToString() ?? "",
                    Course = reader["course"].ToString() ?? "",
                    Year = reader["year_level"].ToString() ?? "",
                    EventName = reader["event_name"].ToString() ?? "",
                    Semester = reader["semester"].ToString() ?? "",
                    CheckInTime = checkInTime,
                    CheckOutTime = checkOutTime,
                    Status = status
                });
            }

            return list;
        }
    }
}
