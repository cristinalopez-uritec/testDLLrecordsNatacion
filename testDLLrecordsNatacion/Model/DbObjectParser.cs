using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testDLLrecordsNatacion.Model.Entities;

namespace testDLLrecordsNatacion.Model
{
    /// <summary>
    /// Parse DB reader objects into model objets or
    /// parse a model object into the parameters of a a sqlCommand
    /// </summary>
    internal class Parser
    {
        /// <summary>
        /// Creates an Athlete Object based on 
        /// the returned values of the Sql Reader
        /// </summary>
        /// <param name="reader">SqlReader providing the data returned by the query</param>
        /// <returns>The athlete object corresponding to that reader iteration</returns>
        internal Athlete DbReaderToAthlete(SqlDataReader reader)
        {
            Athlete atleta = new Athlete();
            atleta.Id = Int32.Parse(reader["Id"].ToString());
            atleta.FullName = reader["FullName"].ToString();
            atleta.Birthdate = reader["Birthdate"].ToString() != "" ? reader["Birthdate"].ToString() : null;
            atleta.Gender = reader["Gender"].ToString() != "" ? reader["Gender"].ToString() : null;
            atleta.Nation = reader["Nation"].ToString() != "" ? reader["Nation"].ToString() : null;
            atleta.License = reader["License"].ToString() != "" ? reader["License"].ToString() : null;
            atleta.ClubName = reader["ClubName"].ToString() != "" ? reader["ClubName"].ToString() : null;
            atleta.ClubShortName = reader["ClubShortName"].ToString() != "" ? reader["ClubShortName"].ToString() : null;
            int? clubCode = null;
            if (reader["ClubCode"] != null && reader["ClubCode"].ToString() != "") clubCode = Int32.Parse(reader["ClubCode"].ToString());
            atleta.ClubCode = clubCode;
            return atleta;
        }

        /// <summary>
        /// Fills the parameters of the sql command with 
        /// the values of the attributes of the object
        /// </summary>
        /// <param name="athlete">The Athlete object we want to insert/update</param>
        /// <returns>The SQL command with the parameters</returns>
        internal SqlCommand AthleteToSqlCommandParams(Athlete athlete, SqlCommand command)
        {
            command.Parameters.AddWithValue("@fullName", athlete.FullName);
            command.Parameters.AddWithValue("@birthdate", athlete.Birthdate ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@gender", athlete.Gender ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@nation", athlete.Nation ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@license", athlete.License ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@clubCode", athlete.ClubCode ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@clubName", athlete.ClubName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@clubShortName", athlete.ClubShortName ?? (object)DBNull.Value);
            return command;
        }

        /// <summary>
        /// Creates an Event Object based on 
        /// the returned values of the Sql Reader
        /// </summary>
        /// <param name="reader">SqlReader providing the data returned by the query</param>
        /// <returns>The Event object corresponding to that reader iteration</returns>
        internal Event DbReaderToEvent(SqlDataReader reader)
        {
            Event evento = new Event();
            evento.Id = Int32.Parse(reader["Id"].ToString());
            evento.MeetName = reader["MeetName"].ToString();
            evento.MeetDate = DateTime.Parse(reader["MeetDate"].ToString());
            evento.Nation = reader["Nation"].ToString();
            evento.City = reader["City"].ToString();
            evento.Status = reader["Status"].ToString();
            evento.PoolLength = (int)reader["PoolLength"];
            evento.SessionNum = (int)reader["SessionNum"];
            evento.SessionName = reader["SessionName"].ToString();
            evento.GenderCategory = reader["GenderCategory"].ToString();
            evento.EventRound = reader["EventRound"].ToString();
            evento.EventCourse = reader["EventCourse"].ToString();
            evento.SwimDistance = Int32.Parse(reader["SwimDistance"].ToString());
            evento.SwimStroke = reader["SwimStroke"].ToString();
            evento.SwimRelayCount = Int32.Parse(reader["SwimRelayCount"].ToString());
            return evento;
        }

        /// <summary>
        /// Fills the parameters of the sql command with 
        /// the values of the attributes of the object
        /// </summary>
        /// <param name="evento">The Event object we want to insert/update</param>
        /// <returns>The SQL command with the parameters</returns>
        internal SqlCommand EventToSqlCommandParams(Event evento, SqlCommand command)
        {
            command.Parameters.AddWithValue("@MeetName", evento.MeetName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@MeetDate", evento.MeetDate);
            command.Parameters.AddWithValue("@Nation", evento.Nation ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@City", evento.City ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Status", evento.Status ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@PoolLength", evento.PoolLength);
            command.Parameters.AddWithValue("@SessionNum", evento.SessionNum);
            command.Parameters.AddWithValue("@SessionName", evento.SessionName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@GenderCategory", evento.GenderCategory ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@EventRound", evento.EventRound ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@EventCourse", evento.EventCourse ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@SwimDistance", evento.SwimDistance);
            command.Parameters.AddWithValue("@SwimStroke", evento.SwimStroke ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@SwimRelayCount", evento.SwimRelayCount);
            return command;
        }

        /// <summary>
        /// Creates a Result Object based on 
        /// the returned values of the Sql Reader
        /// </summary>
        /// <param name="reader">SqlReader providing the data returned by the query</param>
        /// <returns>The Result object corresponding to that reader iteration</returns>
        internal Result DbReaderToResult(SqlDataReader reader)
        {
            Result result = new Result();
            result.Id = Int32.Parse(reader["Id"].ToString());
            result.SplitDistance = (int)reader["SplitDistance"];
            result.SwimTime = reader["SwimTime"].ToString();
            result.Points = Int32.Parse(reader["Points"].ToString());
            result.IsWaScoring = Int32.Parse(reader["IsWaScoring"].ToString());
            result.EntryTime = reader["EntryTime"].ToString();
            result.Comment = reader["Comment"].ToString();
            result.AgeGroupMaxAge = Int32.Parse(reader["AgeGroupMaxAge"].ToString());
            result.AgeGroupMinAge = Int32.Parse(reader["AgeGroupMinAge"].ToString());
            result.EventId = Int32.Parse(reader["EventId"].ToString());
            result.AthleteId = Int32.Parse(reader["AthleteId"].ToString());
            return result;
        }

        /// <summary>
        /// Fills the parameters of the sql command with 
        /// the values of the attributes of the object
        /// </summary>
        /// <param name="result">The Result object we want to insert/update</param>
        /// <returns>The SQL command with the parameters</returns>
        internal SqlCommand ResultToSqlCommandParams(Result result, SqlCommand command)
        {
            command.Parameters.AddWithValue("@SplitDistance", result.SplitDistance);
            command.Parameters.AddWithValue("@SwimTime", result.SwimTime ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Points", result.Points);
            command.Parameters.AddWithValue("@IsWaScoring", result.IsWaScoring);
            command.Parameters.AddWithValue("@EntryTime", result.EntryTime ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Comment", result.Comment ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@AgeGroupMaxAge", result.AgeGroupMaxAge);
            command.Parameters.AddWithValue("@AgeGroupMinAge", result.AgeGroupMinAge);
            command.Parameters.AddWithValue("@EventId", result.EventId);
            command.Parameters.AddWithValue("@AthleteId", result.AthleteId);
            return command;
        }

        /// <summary>
        /// Creates a Record Object based on 
        /// the returned values of the Sql Reader
        /// </summary>
        /// <param name="reader">SqlReader providing the data returned by the query</param>
        /// <returns>The Record object corresponding to that reader iteration</returns>
        internal Record DbReaderToRecord(SqlDataReader reader)
        {
            Record record = new Record();
            record.Id = Int32.Parse(reader["Id"].ToString());
            record.RecordDate = DateTime.Parse(reader["RecordDate"].ToString());
            record.Position = Int32.Parse(reader["Position"].ToString());
            record.MeetStatus = reader["MeetStatus"].ToString();
            record.RecordType = reader["RecordType"].ToString();
            record.AgeCategory = reader["AgeCategory"].ToString();
            record.SwimTime = reader["SwimTime"].ToString();
            record.SwimDistance = (int)reader["SwimDistance"];
            record.SwimCourse = reader["SwimCourse"].ToString();
            record.SwimStroke = reader["SwimStroke"].ToString();
            record.Points = Int32.Parse(reader["Points"].ToString());
            record.ResultId = Int32.Parse(reader["ResultId"].ToString());
            record.AthleteId = Int32.Parse(reader["AthleteId"].ToString());
            return record;
        }

        /// <summary>
        /// Fills the parameters of the sql command with 
        /// the values of the attributes of the object
        /// </summary>
        /// <param name="record">The Record object we want to insert/update</param>
        /// <returns>The SQL command with the parameters</returns>
        internal SqlCommand RecordToSqlCommandParams(Record record, SqlCommand command)
        {
            command.Parameters.AddWithValue("@Position", record.Position);
            command.Parameters.AddWithValue("@MeetStatus", record.MeetStatus ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@RecordType", record.RecordType ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@RecordDate", record.RecordDate);
            command.Parameters.AddWithValue("@AgeCategory", record.AgeCategory ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@SwimTime", record.SwimTime ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@SwimDistance", record.SwimDistance);
            command.Parameters.AddWithValue("@SwimCourse", record.SwimCourse);
            command.Parameters.AddWithValue("@SwimStroke", record.SwimStroke);
            command.Parameters.AddWithValue("@Points", record.Points);
            command.Parameters.AddWithValue("@ResultId", record.ResultId);
            command.Parameters.AddWithValue("@AthleteId", record.AthleteId);
            return command;
        }
    }
}
