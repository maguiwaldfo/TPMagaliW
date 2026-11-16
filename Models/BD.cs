
using Microsoft.Data.SqlClient;
using Dapper;

namespace tpfinal.Models;

public class BD
{
    private static string conexion = "Server=LAPTOP-M8JB68MT\\SQLEXPRESS;Database=BD;Trusted_Connection=True;TrustServerCertificate=True;";

    public static SqlConnection ObtenerConexion()
    {
        return new SqlConnection(conexion);
    }


    public Usuario IniciarSesion(string nombreUsuario, string contrasena)
    {
        Usuario usuario;

        using (SqlConnection connection = ObtenerConexion())
        {
            usuario = connection.QueryFirstOrDefault<Usuario>(
                "SELECT * FROM Usuarios WHERE NombreUsuario = @nombreUsuario AND Contrasena = @contrasena",
                new { nombreUsuario = nombreUsuario, contrasena = contrasena }
            );
        }

        return usuario;
    }


    public void RegistrarUsuario(Usuario usuario)
    {
        using (SqlConnection connection = ObtenerConexion())
        {
            connection.Execute(
                "INSERT INTO Usuarios (NombreUsuario, Contrasena, Nombre, Apellido) VALUES (@NombreUsuario, @Contrasena, @Nombre, @Apellido)",
                usuario
            );
        }
    }

    public List<Espacio> ObtenerEspacios(int ultimoId)
    {
        List<Espacio> espacios;

        using (SqlConnection connection = ObtenerConexion())
        {
            espacios = connection.Query<Espacio>(
                "SELECT TOP 8 * FROM Espacios WHERE Activo = 1 AND Id > @ultimoId ORDER BY Id",
                new { ultimoId = ultimoId }
            ).ToList();
        }

        return espacios;
    }

    public Espacio ObtenerDetalleEspacio(int id)
    {
        Espacio espacio;

        using (SqlConnection connection = ObtenerConexion())
        {
            espacio = connection.QueryFirstOrDefault<Espacio>(
                "SELECT * FROM Espacios WHERE Id = @id AND Activo = 1",
                new { id = id }
            );
        }

        return espacio;
    }


    public List<Reserva> ObtenerReservasDelDia(int idEspacio, DateTime fecha)
    {
        List<Reserva> reservas;

        using (SqlConnection connection = ObtenerConexion())
        {
            reservas = connection.Query<Reserva>(
                "SELECT * FROM Reservas WHERE IdEspacio = @idEspacio AND CAST(FechaHoraInicio AS DATE) = @fecha AND Cancelado = 0",
                new { idEspacio = idEspacio, fecha = fecha }
            ).ToList();
        }

        return reservas;
    }

    public List<Reserva> ObtenerReservasUsuarioDelDia(int idUsuario, DateTime fecha)
    {
        List<Reserva> reservas;

        using (SqlConnection connection = ObtenerConexion())
        {
            reservas = connection.Query<Reserva>(
                "SELECT * FROM Reservas WHERE IdUsuario = @idUsuario AND CAST(FechaHoraInicio AS DATE) = @fecha AND Cancelado = 0",
                new { idUsuario = idUsuario, fecha = fecha }
            ).ToList();
        }

        return reservas;
    }


    public void RegistrarReserva(Reserva reserva)
    {
        using (SqlConnection connection = ObtenerConexion())
        {
            connection.Execute(
                "INSERT INTO Reservas (IdEspacio, IdUsuario, FechaHoraInicio, CantidadHoras, Cancelado, FechaCancelacion) VALUES (@IdEspacio, @IdUsuario, @FechaHoraInicio, @CantidadHoras, 0, NULL)",
                reserva
            );
        }
    }


    public List<Reserva> ObtenerReservasProximas(int idUsuario)
    {
        List<Reserva> reservas;

        using (SqlConnection connection = ObtenerConexion())
        {
            reservas = connection.Query<Reserva>(
                "SELECT * FROM Reservas WHERE IdUsuario = @idUsuario AND FechaHoraInicio >= GETDATE() AND Cancelado = 0 ORDER BY FechaHoraInicio",
                new { idUsuario = idUsuario }
            ).ToList();
        }

        return reservas;
    }

    public List<Reserva> ObtenerReservasPasadas(int idUsuario)
    {
        List<Reserva> reservas;

        using (SqlConnection connection = ObtenerConexion())
        {
            reservas = connection.Query<Reserva>(
                "SELECT * FROM Reservas WHERE IdUsuario = @idUsuario AND FechaHoraInicio < GETDATE() ORDER BY FechaHoraInicio DESC",
                new { idUsuario = idUsuario }
            ).ToList();
        }

        return reservas;
    }


    public bool CancelarReserva(int idReserva, int idUsuario)
    {
        bool resultado = false;

        using (SqlConnection connection = ObtenerConexion())
        {
            int filasModificadas = connection.Execute(
                "UPDATE Reservas SET Cancelado = 1, FechaCancelacion = GETDATE() WHERE Id = @idReserva AND IdUsuario = @idUsuario AND Cancelado = 0 AND FechaHoraInicio >= DATEADD(HOUR, 24, GETDATE())",
                new { idReserva = idReserva, idUsuario = idUsuario }
            );

            if (filasModificadas > 0)
            {
                resultado = true;
            }
        }

        return resultado;
    }
}
