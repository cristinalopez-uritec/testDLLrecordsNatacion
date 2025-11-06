using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testDLLrecordsNatacion.Model.Entities;
using testDLLrecordsNatacion.Model;
using NLog;
using System.Configuration;

namespace testDLLrecordsNatacion.Model
{
    /// <summary>
    /// Donde se realizan todas las operaciones 
    /// que comunican directamente con la BD.
    /// </summary>
    internal class OperacionesBD
    {
        //Access connection string stored in WebConfig file of the project containing the DLL
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private readonly Parseador parseador = new Parseador();

        #region Athlete 
        /// <summary>
        /// Busca un atleta por su nombre completo.
        /// </summary>
        /// <param name="NombreCompletoAtleta">Nombre completo del atleta</param>
        /// <returns>el atleta o null si no se encuentra en la BD</returns>
        public Atleta BuscarAtletaPorNombre(string NombreCompletoAtleta)
        {
            string query = "SELECT * FROM RecordsNatacionAtleta WHERE NombreCompleto = @NombreCompleto";
            Atleta atleta = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NombreCompleto", NombreCompletoAtleta);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        atleta = parseador.DbReaderAAtleta(reader);
                    }
                }
                catch (Exception ex)
                {
                    Log.Instance.Fatal("BuscarAtletaPorNombre failed", $"{ex.Message}\n\n{ex.StackTrace}");
                }
                finally
                {
                    reader.Close();
                }
            }
            return atleta;
        }

        /// <summary>
        /// Busca el atleta por su nombre completo.
        /// Si lo encuentra, devuelve su Id y (si lo indicas) actualiza sus campos vacíos.
        /// Si no lo encuentra, lo inserta en la BD y devuelve su Id.
        /// </summary>
        /// <param name="atletaBuscar">Nombre completo del atleta a buscar</param>
        /// <param name="revisarActualizarCampos"> Si queremos actualizar los campos vacíos del atleta si existe.</param>
        /// <returns>Id del atleta, -1 si falla</returns>
        public int BuscarEInsertarAtleta(Atleta atletaBuscar, bool revisarActualizarCampos)
        {
            int idAtleta = -1;
            //TODO: open db connection
            Atleta atletaExiste = BuscarAtletaPorNombre(atletaBuscar.NombreCompleto);

            //Insert the athlete if it doesn't exist
            if (atletaExiste == null)
            {
                idAtleta = InsertarAtleta(atletaBuscar);
            }
            else
            {
                idAtleta = atletaExiste.IdAtleta;
                if (revisarActualizarCampos)
                {
                    //TODO: check if any fields can be updated (only for LenexXML importation)
                }
            }
            //TODO: close db connection
            return idAtleta;
        }

        /// <summary>
        /// Obtiene la información de un atleta buscándolo por su ID 
        /// </summary>
        /// <param name="idAtleta">Id del atleta</param>
        /// <returns>el atleta o null si no lo encuentra en la BD</returns>
        public Atleta BuscarAtletaPorId(int idAtleta)
        {
            string query = "SELECT * FROM RecordsNatacionAtleta WHERE IdAtleta = @IdAtleta";
            Atleta atleta = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdAtleta", idAtleta);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        atleta = parseador.DbReaderAAtleta(reader);
                    }
                }
                catch (Exception ex)
                {
                    Log.Instance.Fatal("BuscarAtletaPorId failed", $"{ex.Message}\n\n{ex.StackTrace}");
                }
                finally
                {
                    reader.Close();
                }
            }
            return atleta; 
        }

        /// <summary>
        /// Obtiene todos los atletas de la BD
        /// </summary>
        /// <returns>Lista de todos los atletas existentes</returns>
        public List<Atleta> ObtenerAtletas()
        {
            string query = "SELECT * FROM RecordsNatacionAtleta";
            List<Atleta> atletas = new List<Atleta>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        atletas.Add(parseador.DbReaderAAtleta(reader));
                    }
                }
                catch(Exception ex)
                {
                    Log.Instance.Fatal("ObtenerAtletas failed", $"{ex.Message}\n\n{ex.StackTrace}");
                }
                finally
                {
                    reader.Close();
                }
            };
            return atletas;
        }

        /// <summary>
        /// Insertar Atleta en BD.
        /// NOTA: esta función no comprueba si dicho atleta ya existe antes de añadirlo.
        /// </summary>
        /// <param name="atleta">el atleta a insertar</param>
        /// <returns>Id del atleta insertado, -1 si ha fallado</returns>
        public int InsertarAtleta(Atleta atleta)
        {
            int newAthleteId = -1;
            string query = "INSERT INTO RecordsNatacionAtleta (NombreCompleto,FechaNacimiento,Genero,Pais,Licencia,CodigoClub,NombreCompletoClub,NombreCortoClub) " +
                            "VALUES (@NombreCompleto,@FechaNacimiento,@Genero,@Pais,@Licencia,@CodigoClub,@NombreCompletoClub,@NombreCortoClub); " +
                            "SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    parseador.AtletaASqlCommandParams(atleta,command);

                    connection.Open();
                    newAthleteId = Convert.ToInt32(command.ExecuteScalar()); //devuelve el id del atleta insertado

                    //Check Error
                    if (newAthleteId < 0)
                    {
                        Log.Instance.Fatal("InsertarAtleta failed");
                        return -1;
                    }   
                }
            }

            return newAthleteId;
        }

        //TODO: update athlete (when already existed on BD due to Excel import but is lacking data, update it with the new data provided by XML)
        #endregion

        #region Event
        /// <summary>
        /// Insertar Competicion en BD.
        /// NOTA: esta función no comprueba si dicha competicion ya existe antes de añadirla.
        /// </summary>
        /// <param name="competicion">la competicion a insertar</param>
        /// <returns>Id de la competicion insertada, -1 si ha fallado</returns>
        public int InsertarCompeticion(Competicion competicion)
        {
            int newEventId = -1;
            string query = "INSERT INTO RecordsNatacionCompeticion (NombreCompeticion,FechaCompeticion,Pais,Ciudad,LongitudPiscina,NumSesion,NombreSesion,CategoriaGenero,RondaEvento,CantidadRelevosNado) " +
                            "VALUES (@NombreCompeticion,@FechaCompeticion,@Pais,@Ciudad,@LongitudPiscina,@NumSesion,@NombreSesion,@CategoriaGenero,@RondaEvento,@CantidadRelevosNado); " + 
                            "SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    parseador.CompeticionASqlCommandParams(competicion,command);
                    
                    connection.Open();
                    newEventId = Convert.ToInt32(command.ExecuteScalar()); //return the Id of the record inserted

                    // Check Error
                    if (newEventId < 0)
                    {
                        Log.Instance.Fatal("InsertarCompeticion failed");
                        return -1;
                    }
                }
            }

            return newEventId;
        }

        /// <summary>
        /// Obtiene todas las competiciones de la BD
        /// </summary>
        /// <returns>Lista de todas las competiciones existentes</returns>
        public List<Competicion> ObtenerCompeticiones()
        {
            string query = "SELECT * FROM RecordsNatacionCompeticion";
            List<Competicion> competicion = new List<Competicion>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        competicion.Add(parseador.DbReaderACompeticion(reader));
                    }
                }
                catch (Exception ex)
                {
                    Log.Instance.Fatal("ObtenerCompeticiones failed", $"{ex.Message}\n\n{ex.StackTrace}");
                }
                finally
                {
                    reader.Close();
                }
            }

            return competicion;
        }
        #endregion


        #region Results
        /// <summary>
        /// Insertar Marca en BD.
        /// NOTA: esta función no comprueba si dicha marca ya existe antes de añadirla.
        /// </summary>
        /// <param name="marca">la marca a insertar</param>
        /// <returns>Id de la marca insertada, -1 si ha fallado</returns>
        public int InsertarMarca(Marca marca)
        {
            int newResultId = -1;
            string query = "INSERT INTO RecordsNatacionMarca (FechaMarca,EdadMaxGrupoEdad,EdadMinGrupoEdad,DistanciaSplit,TiempoNado,RecorridoNado,DistanciaNado,EstiloNado,Puntos,EsPuntuacionFina,IdAtleta,Comentario,TiempoDeEntrada,IdEvento) " +
                            $"VALUES (@FechaMarca,@EdadMaxGrupoEdad,@EdadMinGrupoEdad,@DistanciaSplit,@TiempoNado,@RecorridoNado,@DistanciaNado,@EstiloNado,@Puntos,@EsPuntuacionFina,@IdAtleta,@Comentario,@TiempoDeEntrada,@IdEvento); " +
                            "SELECT SCOPE_IDENTITY();";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    parseador.MarcaASqlCommandParams(marca, command);
                    connection.Open();
                    newResultId = Convert.ToInt32(command.ExecuteScalar()); //devuelve el ID de la Marca insertada

                    // Check Error
                    if (newResultId < 0)
                    {
                        Log.Instance.Fatal("InsertarMarca failed");
                        return -1;
                    }
                }
            }

            return newResultId;
        }

        /// <summary>
        /// Obtiene todas las marcas de la BD
        /// </summary>
        /// <returns>Lista de todas las marcas existentes</returns>
        public List<Marca> ObtenerMarcas()
        {
            string query = "SELECT * FROM RecordsNatacionMarca";
            List<Marca> marcas = new List<Marca>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        marcas.Add(parseador.DbReaderAMarca(reader));
                    }
                }
                catch (Exception ex)
                {
                    Log.Instance.Fatal("SelectAllMarcas failed", $"{ex.Message}\n\n{ex.StackTrace}");
                }
                finally
                {
                    reader.Close();
                }
            }

            return marcas;
        }
        #endregion

        #region Records
        /// <summary>
        /// Insertar Record en BD.
        /// NOTA: esta función no comprueba si dicho record ya existe antes de añadirla.
        /// </summary>
        /// <param name="record">el record a insertar</param>
        /// <returns>Id del record insertado, -1 si ha fallado</returns>
        public int InsertarRecord(RecordESP record)
        {
            int newRecordId = -1;
            string query = "INSERT INTO RecordsNatacionMarca (FechaMarca,NombreGrupoEdad,TiempoNado,RecorridoNado,DistanciaNado,EstiloNado,Puntos,IdAtleta) " +
                            $"VALUES (@FechaMarca,@NombreGrupoEdad,@TiempoNado,@RecorridoNado,@DistanciaNado,@EstiloNado,@Puntos,@IdAtleta); " +
                            "SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    parseador.RecordASqlCommandParams(record, command);

                    connection.Open();
                    newRecordId = Convert.ToInt32(command.ExecuteScalar()); //devuelve el Id del record insertado

                    // Check Error
                    if (newRecordId < 0)
                    {
                        Log.Instance.Fatal("InsertarRecord failed");
                        return -1;
                    }
                }
            }

            return newRecordId;
        }

        /// <summary>
        /// Obtiene todos los records existentes en la BD
        /// </summary>
        /// <returns>Lista de todos los records existentes</returns>
        public List<RecordESP> ObtenerRecords()
        {
            string query = "SELECT * FROM RecordsNatacionRecord";
            List<RecordESP> records = new List<RecordESP>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        records.Add(parseador.DbReaderARecord(reader));
                    }
                }
                catch (Exception ex)
                {
                    Log.Instance.Fatal("SelectAllRecords failed", $"{ex.Message}\n\n{ex.StackTrace}");
                }
                finally
                {
                    reader.Close();
                }
            }

            return records;
        }
        #endregion




        /// <summary>
        /// Obtiene las mejores marcas de un atleta en concreto
        /// para poder insertarlas en records a modo de records personales
        /// </summary>
        /// <param name="IdAtleta">Atleta del que extraer las marcas</param>
        /// <returns>Lista de mejores marcas del atleta</returns>
        public List<Marca> ObtenerRecordsPersonalesMarcas(int IdAtleta)
        {
            string query = @"select DISTINCT og.*
                            from RecordsNatacionMarca og
                            inner join (
	                            SELECT DISTINCT MAX(PUNTOS) AS 'PuntosMax',EstiloNado,DistanciaNado,RecorridoNado,NombreGrupoEdad 
	                            FROM RecordsNatacionMarca 
	                            where IdAtleta = " + IdAtleta + @" and NombreGrupoEdad IS NOT NULL
	                            GROUP BY EstiloNado,DistanciaNado,RecorridoNado,NombreGrupoEdad
	                            HAVING (MAX(Puntos) > 0)
                            ) as grouped on grouped.PuntosMax = og.Puntos
	                            and grouped.DistanciaNado = og.DistanciaNado 
	                            and grouped.EstiloNado = og.EstiloNado
	                            and grouped.RecorridoNado = og.RecorridoNado
	                            and grouped.NombreGrupoEdad = og.NombreGrupoEdad
                            where og.IdAtleta = " + IdAtleta +
                            " Order by og.EstiloNado,og.DistanciaNado,og.RecorridoNado,og.NombreGrupoEdad;";

            List<Marca> marcas = new List<Marca>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        marcas.Add(parseador.DbReaderAMarca(reader));
                    }
                }
                catch (Exception ex)
                {
                    Log.Instance.Fatal("ObtenerRecordsPersonalesMarcas failed", $"{ex.Message}\n\n{ex.StackTrace}");
                }
                finally
                {
                    reader.Close();
                }
            }

            return marcas;
        }

        /// <summary>
        /// Inserta las mejores marcas de todos los atletas existentes
        /// en la tabla de records a modo de records personales
        /// </summary>
        public void InsertarRecordsPersonalesMarcas()
        {
            List<Atleta> atletas = ObtenerAtletas();
            List<Marca> recordsPersonales = new List<Marca>();
            foreach (Atleta atleta in atletas)
            {
                recordsPersonales.AddRange(ObtenerRecordsPersonalesMarcas(atleta.IdAtleta));
            }

            List<RecordESP> records = new List<RecordESP>();
            foreach(Marca marca in recordsPersonales)
            {
                int newRecordId = -1;
                string query = "INSERT INTO RecordsNatacionRecord (FechaRecord,TiempoNado,RecorridoNado,DistanciaNado,DistanciaSplit,EstiloNado,Puntos,IdAtleta,IdMarca) " +
                                $"VALUES (@RecordDate,@SwimTime,@SwimCourse,@SwimDistance,@DistanciaSplit,@SwimStroke,@Points,@AthleteId,@IdMarca); ";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RecordDate", marca.FechaMarca);
                        command.Parameters.AddWithValue("@AgeCategory", (object)DBNull.Value);
                        command.Parameters.AddWithValue("@SwimTime", marca.TiempoNado ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@DistanciaSplit", marca.DistanciaSplit ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@SwimDistance", marca.DistanciaNado);
                        command.Parameters.AddWithValue("@SwimCourse", marca.RecorridoNado);
                        command.Parameters.AddWithValue("@SwimStroke", marca.EstiloNado);
                        command.Parameters.AddWithValue("@Points", marca.Puntos);
                        command.Parameters.AddWithValue("@AthleteId", marca.IdAtleta);
                        command.Parameters.AddWithValue("@IdMarca", marca.IdMarca);
                        command.Parameters.AddWithValue("@EventId", marca.IdEvento);


                        connection.Open();
                        newRecordId = Convert.ToInt32(command.ExecuteScalar()); //return the Id of the record inserted

                        // Check Error
                        if (newRecordId < 0)
                        {
                            Console.WriteLine("Error inserting data into Database!");
                            return;
                        }
                    }
                }

            }
        }
    }
}
