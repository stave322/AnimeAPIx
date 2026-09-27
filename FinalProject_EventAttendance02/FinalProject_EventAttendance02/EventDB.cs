using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_EventAttendance02
{
    public static class EventDB
    {

        private static int GetNextAvailableId(MySqlConnection conn)
        {
            const string sql = @"
                SELECT COALESCE(MIN(t1.event_id + 1), 1) as next_id
                FROM events t1
                LEFT JOIN events t2 ON t1.event_id + 1 = t2.event_id
                WHERE t2.event_id IS NULL";

            using var cmd = new MySqlCommand(sql, conn);
            var result = cmd.ExecuteScalar();
            return result != DBNull.Value ? Convert.ToInt32(result) : 1;
        }




        public static int Insert(Event ev)
        {
            using var conn = DbHelper.GetConnection();
            conn.Open();


            using var transaction = conn.BeginTransaction();

            try
            {

                int nextId = GetNextAvailableId(conn);

                const string sql = @"
                    INSERT INTO events (event_id, event_name, event_date, check_in_start, check_in_end,
                                        location, semester, status)
                    VALUES (@id, @name, @date, @start, @end, @loc, @sem, @status)";

                using var cmd = new MySqlCommand(sql, conn, transaction);
                cmd.Parameters.AddWithValue("@id", nextId);
                cmd.Parameters.AddWithValue("@name", ev.EventName);
                cmd.Parameters.AddWithValue("@date", ev.Date.Date);
                cmd.Parameters.AddWithValue("@start", ev.CheckInStart.ToString(@"hh\:mm\:ss"));
                cmd.Parameters.AddWithValue("@end", ev.CheckInEnd.ToString(@"hh\:mm\:ss"));
                cmd.Parameters.AddWithValue("@loc", ev.Location);
                cmd.Parameters.AddWithValue("@sem", ev.Semester);
                cmd.Parameters.AddWithValue("@status", ev.Status);

                cmd.ExecuteNonQuery();
                transaction.Commit();

                return nextId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public static List<Event> GetAll()
        {
            var list = new List<Event>();
            using var conn = DbHelper.GetConnection();
            const string sql = "SELECT * FROM events ORDER BY event_date, check_in_start";
            using var cmd = new MySqlCommand(sql, conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var ev = new Event(
                    reader["event_name"].ToString()!,
                    reader["location"].ToString()!,
                    Convert.ToDateTime(reader["event_date"]),
                    reader["semester"].ToString()!,
                    TimeSpan.Parse(reader["check_in_start"].ToString()!),
                    TimeSpan.Parse(reader["check_in_end"].ToString()!)
                );
                ev.EventID = Convert.ToInt32(reader["event_id"]);
                ev.Status = reader["status"].ToString()!;
                if (reader["check_out_start"] != DBNull.Value)
                    ev.CheckOutStart = TimeSpan.Parse(reader["check_out_start"].ToString()!);
                if (reader["check_out_end"] != DBNull.Value)
                    ev.CheckOutEnd = TimeSpan.Parse(reader["check_out_end"].ToString()!);
                list.Add(ev);
            }
            return list;
        }

        // update
        public static int Update(Event ev)
        {
            using var conn = DbHelper.GetConnection();
            const string sql = @"
                UPDATE events
                SET event_name      = @name,
                    event_date      = @date,
                    check_in_start  = @start,
                    check_in_end    = @end,
                    check_out_start = @cout_s,
                    check_out_end   = @cout_e,
                    location        = @loc,
                    semester        = @sem,
                    status          = @status
                WHERE event_id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", ev.EventID);
            cmd.Parameters.AddWithValue("@name", ev.EventName);
            cmd.Parameters.AddWithValue("@date", ev.Date.Date);
            cmd.Parameters.AddWithValue("@start", ev.CheckInStart.ToString(@"hh\:mm\:ss"));
            cmd.Parameters.AddWithValue("@end", ev.CheckInEnd.ToString(@"hh\:mm\:ss"));
            cmd.Parameters.AddWithValue("@cout_s", ev.CheckOutStart.HasValue
                                                    ? ev.CheckOutStart.Value.ToString(@"hh\:mm\:ss")
                                                    : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@cout_e", ev.CheckOutEnd.HasValue
                                                    ? ev.CheckOutEnd.Value.ToString(@"hh\:mm\:ss")
                                                    : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@loc", ev.Location);
            cmd.Parameters.AddWithValue("@sem", ev.Semester);
            cmd.Parameters.AddWithValue("@status", ev.Status);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        // Delete
        public static int Delete(int eventId)
        {
            using var conn = DbHelper.GetConnection();
            const string sql = "DELETE FROM events WHERE event_id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", eventId);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }
    }
}