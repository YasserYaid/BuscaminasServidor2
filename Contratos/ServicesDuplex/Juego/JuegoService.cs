using Contratos.Properties.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Contratos
{
    public partial class BuscaminasServicio : IJuegoServiceDuplex
    {
        public void AgregarJugadorJuegoCallback(string codigoSala, string nombreUsuario)
        {
            try
            {
                IJuegoServiceCallback conexion = OperationContext.Current.GetCallbackChannel<IJuegoServiceCallback>();
                inventarioSalas[codigoSala].jugadoresJuegoCallback.Add(nombreUsuario, conexion);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(Recurso.Excepcion_MSJCONST + e);
            }
            catch (CommunicationObjectFaultedException e)
            {
                throw new FaultException(Recurso.Excepcion_MSJCONST + e);
            }
        }

        public void PasarTurno(string codigoSala, string nombreUsuarioClic, int turnoUsuarioActual, Celda celda)
        {
            try
            {
                Dictionary<string, IJuegoServiceCallback>.KeyCollection jugadores = inventarioSalas[codigoSala].jugadoresJuegoCallback.Keys;
                string primero = jugadores.First();
                string ultimo = jugadores.Last();
                inventarioSalas[codigoSala].jugadoresJuegoCallback[primero].RecibirTurno(nombreUsuarioClic, turnoUsuarioActual, celda);
                inventarioSalas[codigoSala].jugadoresJuegoCallback[ultimo].RecibirTurno(nombreUsuarioClic, turnoUsuarioActual, celda);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(Recurso.Excepcion_MSJCONST + e);
            }
            catch (CommunicationObjectFaultedException e)
            {
                throw new FaultException(Recurso.Excepcion_MSJCONST + e);
            }
        }

        public void EnviarConclusion(string codigoSala, string nombreUsuario)
        {
            try
            {
                Dictionary<string, IJuegoServiceCallback>.KeyCollection jugadores = inventarioSalas[codigoSala].jugadoresJuegoCallback.Keys;
                string primero = jugadores.First();
                string ultimo = jugadores.Last();
                inventarioSalas[codigoSala].jugadoresJuegoCallback[primero].RecibirConclusion(nombreUsuario);
                inventarioSalas[codigoSala].jugadoresJuegoCallback[ultimo].RecibirConclusion(nombreUsuario);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(Recurso.Excepcion_MSJCONST + e);
            }
            catch (CommunicationObjectFaultedException e)
            {
                throw new FaultException(Recurso.Excepcion_MSJCONST + e);
            }
        }

        public Tablero CrearTableroInicial(int ancho, int alto, int numeroMinas)
        {
            try
            {
                Tablero tablero = new Tablero();
                List<Celda> bombas = new List<Celda>();
                tablero.bombas = bombas;
                Celda[,] matrizCeldas = new Celda[ancho, alto];

                for (int fila = 0; fila < ancho; fila++)
                {
                    for (int columna = 0; columna < alto; columna++)
                    {
                        Celda celda = new Celda();
                        celda.filaPosicion = fila;
                        celda.columnaPosicion = columna;
                        celda.esMinada = false;
                        matrizCeldas[fila, columna] = celda;
                    }
                }

                int contadorMinas = 0;
                Random posicionMinas = new Random();

                while (contadorMinas < numeroMinas)
                {
                    int filas = posicionMinas.Next(ancho);
                    int columnas = posicionMinas.Next(alto);

                    Celda celda = matrizCeldas[filas, columnas];

                    if (!celda.esMinada)
                    {
                        celda.esMinada = true;
                        contadorMinas++;
                    }
                    tablero.bombas.Add(celda);
                }

                return tablero;
            }
            catch (CommunicationObjectFaultedException e)
            {
                throw new FaultException(Recurso.Excepcion_MSJCONST + e);
            }
        }

        public void CrearNuevoTablero(string codigoSala, int ancho, int alto, int numeroMinas)
        {
            Tablero tablero = CrearTableroInicial(ancho, alto, numeroMinas);
            inventarioSalas[codigoSala].tablero = tablero;
        }
    }
}
