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
            if (reader["FechaNacimiento"] != null && reader["FechaNacimiento"].ToString() != "") fechaNaci = DateTime.Parse(reader["FechaNacimiento"].ToString());
            int? codigoClub = null;
            if (reader["CodigoClub"] != null && reader["CodigoClub"].ToString() != "") codigoClub = Int32.Parse(reader["CodigoClub"].ToString());

            Atleta atleta = new Atleta();
            atleta.IdAtleta = Int32.Parse(reader["IdAtleta"].ToString());
            atleta.NombreCompleto = reader["NombreCompleto"] != null && reader["NombreCompleto"].ToString() != "" ? reader["NombreCompleto"].ToString() : null;
            atleta.FechaNacimiento = fechaNaci ;
            atleta.Genero = reader["Genero"] != null && reader["Genero"].ToString() != "" ? reader["Genero"].ToString() : null;
            atleta.Pais = reader["Pais"] != null && reader["Pais"].ToString() != "" ? reader["Pais"].ToString() : null ;
            atleta.Licencia = reader["Licencia"] != null && reader["Licencia"].ToString() != "" ? reader["Licencia"].ToString() : null;
            atleta.CodigoClub = codigoClub;
            atleta.NombreCompletoClub = reader["NombreCompletoClub"] != null && reader["NombreCompletoClub"].ToString() != "" ? reader["NombreCompletoClub"].ToString() : null;
            atleta.NombreCortoClub = reader["NombreCortoClub"] != null && reader["NombreCortoClub"].ToString() != "" ? reader["NombreCortoClub"].ToString() : null;
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
        /// Rellena los parámetros del SQL command 
        /// con los valores de los atributos del objeto
        /// </summary>
        /// <param name="competicion">La Competicion que se desea insertar/actualizar</param>
        /// <param name="command">SQL Command que realizará la consulta</param>
        /// <returns>El comando sql con los parametros rellenados</returns>
        internal SqlCommand CompeticionASqlCommandParams(Competicion competicion, SqlCommand command)
        {
            command.Parameters.AddWithValue("@NombreCompeticion", competicion.NombreCompeticion ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@FechaCompeticion", competicion.FechaCompeticion);
            command.Parameters.AddWithValue("@Pais", competicion.Pais ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Ciudad", competicion.Ciudad ?? (object)DBNull.Value);
            //command.Parameters.AddWithValue("@Status", competicion.Status ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@LongitudPiscina", competicion.LongitudPiscina);
            command.Parameters.AddWithValue("@NumSesion", competicion.NumSesion);
            command.Parameters.AddWithValue("@NombreSesion", competicion.NombreSesion ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@CategoriaGenero", competicion.CategoriaGenero ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@RondaEvento", competicion.RondaEvento ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@CantidadRelevosNado", competicion.CantidadRelevosNado);
            return command;
        }

        /// <summary>
        /// Crea un objeto Marca según lo devuelto por el reader
        /// </summary>
        /// <param name="reader">SqlReader que proporciona los datos</param>
        /// <returns>La competicion correspondiente a esa iteración del reader</returns>
        internal Marca DbReaderAMarca(SqlDataReader reader)
        {
            int? DistanciaSplit = null;
            if (reader["DistanciaSplit"] != null && reader["DistanciaSplit"].ToString() != "") DistanciaSplit = Int32.Parse(reader["DistanciaSplit"].ToString());
            int? IdCategoriaEdad = null;
            //if (reader["IdCategoriaEdad"] != null && reader["IdCategoriaEdad"].ToString() != "") IdCategoriaEdad = Int32.Parse(reader["IdCategoriaEdad"].ToString());
            int? IdEvento = null;
            if (reader["IdEvento"] != null && reader["IdEvento"].ToString() != "") IdEvento = Int32.Parse(reader["IdEvento"].ToString());

            Marca marca = new Marca();
            marca.IdMarca = Int32.Parse(reader["IdMarca"].ToString());
            marca.FechaMarca = DateTime.Parse(reader["FechaMarca"].ToString());
            marca.TiempoNado = reader["TiempoNado"] != null ? reader["TiempoNado"].ToString() : null;
            marca.Puntos = Int32.Parse(reader["Puntos"].ToString());
            marca.Comentario = reader["Comentario"].ToString();
            marca.RecorridoNado = reader["RecorridoNado"] != null ? reader["RecorridoNado"].ToString() : null;
            marca.DistanciaNado = Int32.Parse(reader["DistanciaNado"].ToString());
            marca.DistanciaSplit = DistanciaSplit;
            marca.EstiloNado = reader["EstiloNado"] != null ? reader["EstiloNado"].ToString() : null;
            marca.IdCategoriaEdad = IdCategoriaEdad;
            marca.IdEvento = IdEvento;
            marca.IdAtleta = Int32.Parse(reader["IdAtleta"].ToString());
            return marca;
        }

        /// <summary>
        /// Rellena los parámetros del SQL command 
        /// con los valores de los atributos del objeto
        /// </summary>
        /// <param name="marca">La Marca que se desea insertar/actualizar</param>
        /// <param name="command">SQL Command que realizará la consulta</param>
        /// <returns>El comando sql con los parametros rellenados</returns>
        internal SqlCommand MarcaASqlCommandParams(Marca marca, SqlCommand command)
        {
            command.Parameters.AddWithValue("@FechaMarca", marca.FechaMarca);
            command.Parameters.AddWithValue("@TiempoNado", marca.TiempoNado);
            command.Parameters.AddWithValue("@Puntos", marca.Puntos);
            command.Parameters.AddWithValue("@Comentario", marca.Comentario ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@RecorridoNado", marca.RecorridoNado);
            command.Parameters.AddWithValue("@DistanciaNado", marca.DistanciaNado);
            command.Parameters.AddWithValue("@DistanciaSplit", marca.DistanciaSplit ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@EstiloNado", marca.EstiloNado);
            command.Parameters.AddWithValue("@IdCategoriaEdad", marca.IdCategoriaEdad ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IdAtleta", marca.IdAtleta);
            command.Parameters.AddWithValue("@IdEvento", marca.IdEvento ?? (object)DBNull.Value);
            return command;
        }

        /// <summary>
        /// Crea un objeto Record según lo devuelto por el reader
        /// </summary>
        /// <param name="reader">SqlReader que proporciona los datos</param>
        /// <returns>La competicion correspondiente a esa iteración del reader</returns>
        internal RecordESP DbReaderARecord(SqlDataReader reader)
        {
            int? DistanciaSplit = null;
            if (reader["DistanciaSplit"] != null && reader["DistanciaSplit"].ToString() != "") DistanciaSplit = Int32.Parse(reader["DistanciaSplit"].ToString());
            int? IdCategoriaEdad = null;
            if (reader["IdCategoriaEdad"] != null && reader["IdCategoriaEdad"].ToString() != "") IdCategoriaEdad = Int32.Parse(reader["IdCategoriaEdad"].ToString());
            int? EdadAtleta = null;
            if (reader["EdadAtleta"] != null && reader["EdadAtleta"].ToString() != "") EdadAtleta = Int32.Parse(reader["EdadAtleta"].ToString());
            
            RecordESP record = new RecordESP();
            record.IdRecord = Int32.Parse(reader["IdRecord"].ToString());
            record.IdMarca = Int32.Parse(reader["IdMarca"].ToString());
            record.FechaRecord = DateTime.Parse(reader["FechaRecord"].ToString());
            record.TiempoNado = reader["TiempoNado"] != null ? reader["TiempoNado"].ToString() : null;
            record.Puntos = Int32.Parse(reader["Puntos"].ToString());
            record.RecorridoNado = reader["RecorridoNado"] != null ? reader["RecorridoNado"].ToString() : null;
            record.DistanciaNado = Int32.Parse(reader["DistanciaNado"].ToString());
            record.DistanciaSplit = DistanciaSplit;
            record.EstiloNado = reader["EstiloNado"] != null ? reader["EstiloNado"].ToString() : null;
            record.EdadAtleta = EdadAtleta;
            record.IdCategoriaEdad = IdCategoriaEdad;
            record.IdAtleta = Int32.Parse(reader["IdAtleta"].ToString());
            return record;
        }

        /// <summary>
        /// Rellena los parámetros del SQL command 
        /// con los valores de los atributos del objeto
        /// </summary>
        /// <param name="record">La Record que se desea insertar/actualizar</param>
        /// <param name="command">SQL Command que realizará la consulta</param>
        /// <returns>El comando sql con los parametros rellenados</returns>
        internal SqlCommand RecordASqlCommandParams(RecordESP record, SqlCommand command)
        {
            command.Parameters.AddWithValue("@FechaRecord", record.FechaRecord);
            command.Parameters.AddWithValue("@TiempoNado", record.TiempoNado);
            command.Parameters.AddWithValue("@Puntos", record.Puntos);
            command.Parameters.AddWithValue("@RecorridoNado", record.RecorridoNado);
            command.Parameters.AddWithValue("@DistanciaNado", record.DistanciaNado);
            command.Parameters.AddWithValue("@DistanciaSplit", record.DistanciaSplit ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@EstiloNado", record.EstiloNado);
            command.Parameters.AddWithValue("@EdadAtleta", record.EdadAtleta ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IdCategoriaEdad", record.IdCategoriaEdad ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IdAtleta", record.IdAtleta);
            command.Parameters.AddWithValue("@IdMarca", record.IdMarca ?? (object)DBNull.Value);
            return command;
        }
    }
}
