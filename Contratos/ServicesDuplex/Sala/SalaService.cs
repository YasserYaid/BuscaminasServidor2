using Contratos.Properties.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Contratos
{
    public partial class BuscaminasServicio : ISalaService, ISalaServiceDuplex
    {
        private Dictionary<string, Sala> inventarioSalas = new Dictionary<string, Sala>();
        public Sala CrearNuevaSala()
        {
            try
            {
                string codigoSala;
                do
                {
                    codigoSala = GenerarCodigoDeSala();
                } while (inventarioSalas.ContainsKey(codigoSala));

                Sala sala = new Sala();
                sala.codigoSala = codigoSala;
                sala.turno = 1;
                sala.jugadoresSalaCallback = new Dictionary<string, ISalaServiceCallback>();
                sala.jugadoresChatCallback = new Dictionary<string, IChatServiceCallback>();
                sala.jugadoresJuegoCallback = new Dictionary<string, IJuegoServiceCallback>();
                sala.tablero = CrearTableroInicial(10, 10, 15);
                inventarioSalas.Add(codigoSala, sala);
                return sala;
            }
            catch (CommunicationObjectFaultedException e)
            {
                throw new FaultException(Recurso.Excepcion_MSJCONST + e);
            }
        }
        public Sala BuscarSala(string codigoSala)
        {
            try
            {
                Sala sala = null;
                inventarioSalas.TryGetValue(codigoSala, out sala);
                return sala;
            }
            catch (CommunicationObjectFaultedException e)
            {
                throw new FaultException(Recurso.Excepcion_MSJCONST + e);
            }
        }

        public void AgregarJugadorComoPrimerJugador(string codigoSala, string nombreUsario)
        {
            try
            {
                ISalaServiceCallback conexion = OperationContext.Current.GetCallbackChannel<ISalaServiceCallback>();
                inventarioSalas[codigoSala].jugadoresSalaCallback.Add(nombreUsario, conexion);
            }
            catch(KeyNotFoundException e)
            {
                Console.WriteLine(Recurso.Excepcion_MSJCONST + e);
            }
            catch (CommunicationObjectFaultedException e)
            {
                throw new FaultException(Recurso.Excepcion_MSJCONST + e);
            }
        }

        public void AgregarJugadorComoSegundoJugador(string codigoSala, string nombreUsuario)
        {
            try
            {
                ISalaServiceCallback conexion = OperationContext.Current.GetCallbackChannel<ISalaServiceCallback>();
                inventarioSalas[codigoSala].jugadoresSalaCallback.Add(nombreUsuario, conexion);
                Dictionary<string, ISalaServiceCallback>.KeyCollection jugadores = inventarioSalas[codigoSala].jugadoresSalaCallback.Keys;
                string primero = jugadores.First();
                string ultimo = jugadores.Last();
                inventarioSalas[codigoSala].jugadoresSalaCallback[primero].ActualizarNombresUsuario(primero, ultimo);
                inventarioSalas[codigoSala].jugadoresSalaCallback[ultimo].ActualizarNombresUsuario(primero, ultimo);
            }
            catch (CommunicationObjectFaultedException e)
            {
                throw new FaultException(Recurso.Excepcion_MSJCONST + e);
            }
        }

        public void ColocarJugadoresEnElJuego(string codigoSala)
        {
            try
            {
                Dictionary<string, ISalaServiceCallback>.KeyCollection jugadores = inventarioSalas[codigoSala].jugadoresSalaCallback.Keys;
                string primero = jugadores.First();
                string ultimo = jugadores.Last();
                inventarioSalas[codigoSala].jugadoresSalaCallback[primero].JugarMultigugador(codigoSala);
                inventarioSalas[codigoSala].jugadoresSalaCallback[ultimo].JugarMultigugador(codigoSala);
            }
            catch (CommunicationObjectFaultedException e)
            {
                throw new FaultException(Recurso.Excepcion_MSJCONST + e);
            }
        }

        private string GenerarCodigoDeSala()
        {
            try
            {
                string codigoGenerado;
                string abecedario = Recurso.AbecedarioAlfanumcerio_MSJCONST;
                char[] arregloCaracteres = new char[4];
                Random random = new Random();

                for (int posicion = 0; posicion < arregloCaracteres.Length; posicion++)
                {
                    arregloCaracteres[posicion] = abecedario[random.Next(abecedario.Length)];
                }

                codigoGenerado = new String(arregloCaracteres);

                return codigoGenerado.ToLower();
            }
            catch (CommunicationObjectFaultedException e)
            {
                throw new FaultException(Recurso.Excepcion_MSJCONST + e);
            }
        }
    }
}
