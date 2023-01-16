using Logica.Properties.Recursos;
using Datos.BuscaminasModelos;
using System.Data.Entity.Infrastructure;
using System;
using System.Linq;
using Datos.Contexto;
using System.Security.Cryptography;
using System.Text;
using System.Data.Entity.Core;
using System.Net.Mail;
using System.Collections.Generic;

namespace Logica
{
    public class CuentaUsuarioServiceMgtLogica
    {
        public EstadoOperacion RegistrarUsuario(string nombreUsuario, string contraseniaUsuario, string correoUsuario, bool genero)
        {
            EstadoOperacion estadoRegistro = EstadoOperacion.FALLIDO;
            try
            {
                string contraseniaEncriptada = UsarCriptografiaHashSHA256(contraseniaUsuario);

                using (var contextoBaseDatos = new BuscaminasModeloContexto())
                {
                    Jugador nuevoRegistroJugador = new Jugador
                    {
                        nombreUsuario = nombreUsuario,
                        correo = correoUsuario,
                        contrasenia = contraseniaEncriptada,
                        genero = genero,
                        estado = 0,
                    };

                    contextoBaseDatos.Jugadores.Add(nuevoRegistroJugador);
                    int entradas = contextoBaseDatos.SaveChanges();
                    contextoBaseDatos.SaveChanges();

                    if (entradas > 0)
                    {
                        estadoRegistro = EstadoOperacion.EXITOSO;
                    }
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(Recurso.Excepcion_MSJCONST + e);
                estadoRegistro = EstadoOperacion.FALLIDO;
            }
            return estadoRegistro;
        }

        public EstadoOperacion ConsultarDisponibilidad(string usuario, string correo)
        {
            EstadoOperacion estadoDisponibilidad = EstadoOperacion.FALLIDO;
            try
            {
                using (var contextoBaseDatos = new BuscaminasModeloContexto())
                {
                    int jugadoresEncontrados = (from Jugador in contextoBaseDatos.Jugadores
                                                where Jugador.nombreUsuario == usuario || Jugador.correo == correo
                                                select Jugador).Count();
                    if (jugadoresEncontrados == 0)
                    {
                        estadoDisponibilidad = EstadoOperacion.EXITOSO;
                    }
                }
            }
            catch (EntitySqlException e)
            {
                Console.WriteLine(Recurso.Excepcion_MSJCONST + e);
                estadoDisponibilidad = EstadoOperacion.FALLIDO;
            }
            return estadoDisponibilidad;
        }

        public EstadoOperacion ComprobarCodigos(string codigoEnviado, string codigoRecibido)
        {
            EstadoOperacion estadoComprobacionCodigos = EstadoOperacion.FALLIDO;
            if (codigoEnviado == codigoRecibido)
            {
                estadoComprobacionCodigos = EstadoOperacion.EXITOSO;
            }
            return estadoComprobacionCodigos;
        }

        public string GenerarCodigo()
        {
            string caracteres = Recurso.AbecedarioAlfanumcerio_MSJCONST;
            char[] arregloCaracteres = new char[8];
            Random random = new Random();

            for (int iterador = 0; iterador < arregloCaracteres.Length; iterador++)
            {
                arregloCaracteres[iterador] = caracteres[random.Next(caracteres.Length)];
            }

            String resultadoString = new String(arregloCaracteres);

            return resultadoString;

        }

        public EstadoOperacion EnviarCorreo(string correoDestino, string asunto, string mensaje, string codigoCorreo)
        {
            EstadoOperacion estadoEnvioCorreo = EstadoOperacion.FALLIDO;
            string DIRECCION_CORREO_EMISOR = Recurso.DireccionCorreoEmisor_MSJCONST;
            string NOMBRE_CORREO_EMISOR = Recurso.NombreCorreoEmisor_MSJCONST;
            string CONTRASENIA_APLICACION_CORREO_EMISOR = Recurso.ContraseniaAplicacionCorreoEmisor_MSJCONST;
            MailAddress direccionCorreoEmisor;
            MailMessage mensajeAEnviar;
            SmtpClient clienteSmtp;

            try
            {
                direccionCorreoEmisor = new MailAddress(DIRECCION_CORREO_EMISOR, NOMBRE_CORREO_EMISOR);
                mensajeAEnviar = new MailMessage();
                clienteSmtp = new SmtpClient();

                mensajeAEnviar.From = direccionCorreoEmisor;
                mensajeAEnviar.To.Add(correoDestino);
                mensajeAEnviar.Subject = asunto;
                mensajeAEnviar.Body = mensaje + codigoCorreo;

                clienteSmtp.Host = Recurso.SmtpGmail_MSJCONST;
                clienteSmtp.Port = 587;//25 a veces da error
                clienteSmtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                clienteSmtp.EnableSsl = true;
                clienteSmtp.UseDefaultCredentials = false;
                clienteSmtp.Credentials = new System.Net.NetworkCredential(DIRECCION_CORREO_EMISOR, CONTRASENIA_APLICACION_CORREO_EMISOR);
                clienteSmtp.Send(mensajeAEnviar);

                estadoEnvioCorreo = EstadoOperacion.EXITOSO;
            }
            catch (EntitySqlException e)
            {
                Console.WriteLine(Recurso.Excepcion_MSJCONST + e);
                estadoEnvioCorreo = EstadoOperacion.FALLIDO;
            }
            catch (SmtpException e)
            {
                Console.WriteLine(Recurso.Excepcion_MSJCONST + e);
                estadoEnvioCorreo = EstadoOperacion.FALLIDO;
            }
            catch (FormatException e)
            {
                Console.WriteLine(Recurso.Excepcion_MSJCONST + e);
                estadoEnvioCorreo = EstadoOperacion.FALLIDO;
            }
            return estadoEnvioCorreo;
        }

        public EstadoOperacion ValidarCredenciales(string nombreUsuario, string contrasenia)
        {
            EstadoOperacion estadoValidacion = EstadoOperacion.FALLIDO;
            try
            {
                string contraseniaDesencriptada = UsarCriptografiaHashSHA256(contrasenia);

                using (var contexto = new BuscaminasModeloContexto())
                {
                    var jugadores = (from Jugador in contexto.Jugadores
                                     where Jugador.nombreUsuario == nombreUsuario && Jugador.contrasenia == contraseniaDesencriptada
                                     select Jugador).Count();
                    if (jugadores > 0)
                    {
                        estadoValidacion = EstadoOperacion.EXITOSO;
                    }
                }
            }
            catch (EntitySqlException e)
            {
                Console.WriteLine(Recurso.Excepcion_MSJCONST + e);
                estadoValidacion = EstadoOperacion.FALLIDO;
            }
            return estadoValidacion;
        }

        public EstadoOperacion VerificarEstadoJugador(string nombreUsuario)
        {
            EstadoOperacion estadoVerificacion = EstadoOperacion.EXITOSO;
            try
            {
                using (var contexto = new BuscaminasModeloContexto())
                {
                    List<Jugador> listaJugadores = contexto.Jugadores.Where(x => x.nombreUsuario == nombreUsuario).ToList();

                    if (listaJugadores.Count != 0)
                    {
                        if (listaJugadores[0].estado > 0)
                        {
                            estadoVerificacion = EstadoOperacion.FALLIDO;
                        }
                    }
                    else 
                    {
                        estadoVerificacion = EstadoOperacion.FALLIDO;
                    }
                }
            }
            catch (EntitySqlException e)
            {
                Console.WriteLine(Recurso.Error_MSJCONST + e);
                estadoVerificacion = EstadoOperacion.FALLIDO;
            }
            return estadoVerificacion;
        }

        public EstadoOperacion ActualizarEstado(string usuario, int estadoJugador)
        {
            EstadoOperacion estadoOperacionActualizacionEstado = EstadoOperacion.FALLIDO;
            try
            {
                using (var contexto = new BuscaminasModeloContexto())
                {
                    var jugadores = contexto.Jugadores.Where(x => x.nombreUsuario == usuario).ToList();

                    if (jugadores.Count > 0)
                    {
                        jugadores[0].estado = estadoJugador;
                        int entradas = contexto.SaveChanges();
                        contexto.SaveChanges();

                        if (entradas > 0)
                        {
                            estadoOperacionActualizacionEstado = EstadoOperacion.EXITOSO;
                        }
                    }
                }
            }
            catch (EntitySqlException e)
            {
                Console.WriteLine(Recurso.Error_MSJCONST + e);
                estadoOperacionActualizacionEstado = EstadoOperacion.FALLIDO;
            }
            return estadoOperacionActualizacionEstado;
        }

        public static int ObtenerIdJugador(string nombreUsuario)
        {
            int idJugador = 0;
            try
            {
                using (var contexto = new BuscaminasModeloContexto())
                {
                    var indexJugador = from Jugador in contexto.Jugadores
                                  where Jugador.nombreUsuario == nombreUsuario
                                  select Jugador.idJugador;

                    if (indexJugador.Count() > 0)
                    {
                        idJugador = indexJugador.First();
                    }
                    else
                    {
                        idJugador = -1;
                    }
                }
            }
            catch (EntitySqlException e)
            {
                Console.WriteLine(Recurso.Excepcion_MSJCONST + e);
                idJugador = -1;
            }
            return idJugador;
        }

        public static string ObtenerNombreUsuario(int idJugador)
        {
            string usuarioJugador = "";
            try
            {
                using (var contexto = new BuscaminasModeloContexto())
                {
                    var jugador = (from Jugador in contexto.Jugadores
                                   where Jugador.idJugador == idJugador
                                   select Jugador);

                    if (jugador.Count() > 0)
                    {
                        usuarioJugador = jugador.First().nombreUsuario;
                    }
                }
            }
            catch (EntitySqlException e)
            {
                Console.WriteLine(Recurso.Excepcion_MSJCONST + e);
                usuarioJugador = "";
            }
            return usuarioJugador;
        }

        public static string ObtenerCorreoJugador(string nombreUsuario)
        {
            string correoJugador = "";
            try
            {
                using (var contexto = new BuscaminasModeloContexto())
                {
                    var jugador = from Jugador in contexto.Jugadores
                                  where Jugador.nombreUsuario == nombreUsuario
                                  select Jugador.correo;

                    if (jugador.Count() > 0)
                    {
                        correoJugador = jugador.First();
                    }
                }
            }
            catch (EntitySqlException e)
            {
                Console.WriteLine(Recurso.Excepcion_MSJCONST + e);
                correoJugador = "";
            }
            return correoJugador;
        }

        public static int ObtenerEstadoJugador(int idJugador)
        {
            int estado = -1;

            try
            {
                using (var contextoBaseDatos = new BuscaminasModeloContexto())
                {
                    var jugador = from Jugador in contextoBaseDatos.Jugadores
                                  where Jugador.idJugador == idJugador
                                  select Jugador.estado;

                    if (jugador.Count() > 0)
                    {
                        estado = jugador.First();
                    }
                }
            }
            catch (EntitySqlException e)
            {
                Console.WriteLine(Recurso.Excepcion_MSJCONST + e);
                estado = -1;
            }
            return estado;
        }

        public static bool VerificarExistenciaJugador(string nombreUsuario)
        {
            bool esJugadorExistente = false;
            try
            {
                using (var contextoBaseDatos = new BuscaminasModeloContexto())
                {
                    var jugador = (from Jugador in contextoBaseDatos.Jugadores
                                   where Jugador.nombreUsuario == nombreUsuario
                                   select Jugador).Count();

                    if (jugador == 0)
                    {
                        esJugadorExistente = false;
                    }
                    else
                    {
                        esJugadorExistente = true;
                    }
                }
            }
            catch (EntitySqlException e)
            {
                Console.WriteLine(Recurso.Excepcion_MSJCONST + e);
                esJugadorExistente = false;
            }
            return esJugadorExistente;
        }

        private string UsarCriptografiaHashSHA256(string contrasenia)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding codificacion = new ASCIIEncoding();
            byte[] flujo = null;
            StringBuilder formadorDeCadena = new StringBuilder();
            flujo = sha256.ComputeHash(codificacion.GetBytes(contrasenia));
            for (int iterador = 0; iterador < flujo.Length; iterador++)
            {
                formadorDeCadena.AppendFormat(Recurso.AppendFormat_MSJCONST, flujo[iterador]);
            }
            return formadorDeCadena.ToString();
        }
    }
}
