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
    /// Para parsear objetos de reader de BD en entidades del modelo
    /// o parsear de entidad del modelo a los parametros de un sqlCommand
    /// </summary>
    internal class Parseador
    {
        /// <summary>
        /// Crea un objeto Atleta según lo devuelto por el reader
        /// </summary>
        /// <param name="reader">SqlReader que proporciona los datos</param>
        /// <returns>El atleta correspondiente a esa iteración del reader</returns>
        internal Atleta DbReaderAAtleta(SqlDataReader reader)
        {
            DateTime? fechaNaci = null;
            if (reader["FechaNacimiento"] != null) fechaNaci = DateTime.Parse(reader["FechaNacimiento"].ToString());
            
            Atleta atleta = new Atleta();
            atleta.IdAtleta = Int32.Parse(reader["IdAtleta"].ToString());
            atleta.NombreCompleto = reader["NombreCompleto"] != null ? reader["NombreCompleto"].ToString() : null;
            atleta.FechaNacimiento = fechaNaci ;
            atleta.Genero = reader["Genero"] != null ? reader["Genero"].ToString() : null;
            atleta.Pais = reader["Pais"] != null ? reader["Pais"].ToString() : null ;
            atleta.Licencia = reader["Licencia"] != null ? reader["Licencia"].ToString() : null;
            atleta.CodigoClub = Int32.Parse(reader["CodigoClub"].ToString());
            atleta.NombreCompletoClub = reader["NombreCompletoClub"] != null ? reader["NombreCompletoClub"].ToString() : null;
            atleta.NombreCortoClub = reader["NombreCortoClub"] != null ? reader["NombreCortoClub"].ToString() : null;
            return atleta;
        }

        /// <summary>
        /// Rellena los parámetros del SQL command 
        /// con los valores de los atributos del objeto
        /// </summary>
        /// <param name="atleta">El atleta que se desea insertar/actualizar</param>
        /// <param name="command">SQL Command que realizará la consulta</param>
        /// <returns>El comando sql con los parametros rellenados</returns>
        internal SqlCommand AtletaASqlCommandParams(Atleta atleta, SqlCommand command)
        {
            command.Parameters.AddWithValue("@NombreCompleto", atleta.NombreCompleto);
            command.Parameters.AddWithValue("@FechaNacimiento", atleta.FechaNacimiento ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Genero", atleta.Genero ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Pais", atleta.Pais ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Licencia", atleta.Licencia ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@CodigoClub", atleta.CodigoClub);
            command.Parameters.AddWithValue("@NombreCompletoClub", atleta.NombreCompletoClub ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@NombreCortoClub", atleta.NombreCortoClub ?? (object)DBNull.Value);
            return command;
        }

        /// <summary>
        /// Crea un objeto Competicion según lo devuelto por el reader
        /// </summary>
        /// <param name="reader">SqlReader que proporciona los datos</param>
        /// <returns>La competicion correspondiente a esa iteración del reader</returns>
        internal Competicion DbReaderACompeticion(SqlDataReader reader)
        {
            Competicion competicion = new Competicion();
            competicion.IdCompeticion = Int32.Parse(reader["IdCompeticion"].ToString());
            competicion.NombreCompeticion = reader["NombreCompeticion"] != null ? reader["NombreCompleto"].ToString() : null;
            competicion.FechaCompeticion = DateTime.Parse(reader["FechaCompeticion"].ToString());
            competicion.Pais = reader["Pais"] != null ? reader["Pais"].ToString() : null;
            competicion.Ciudad = reader["Ciudad"] != null ? reader["Ciudad"].ToString() : null;
            competicion.LongitudPiscina = Int32.Parse(reader["PoolLength"].ToString());
            competicion.NumSesion = Int32.Parse(reader["NumSesion"].ToString());
            competicion.NombreSesion = reader["NombreSesion"] != null ? reader["NombreSesion"].ToString() : null;
            competicion.CategoriaGenero = reader["CategoriaGenero"] != null ? reader["CategoriaGenero"].ToString() : null; ;
            competicion.RondaEvento = reader["RondaEvento"] != null ? reader["RondaEvento"].ToString() : null;
            competicion.CantidadRelevosNado = Int32.Parse(reader["CantidadRelevosNado"].ToString());
            return competicion;
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
