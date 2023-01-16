using Contratos.Properties.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Contratos
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, InstanceContextMode = InstanceContextMode.Single)]
    public partial class BuscaminasServicio : IChatServiceDuplex
    {
        public void AgregarJugadorChatCallback(string codigoSala, string nombreUsuario)
        {
            try
            {
                IChatServiceCallback conexion = OperationContext.Current.GetCallbackChannel<IChatServiceCallback>();
                inventarioSalas[codigoSala].jugadoresChatCallback.Add(nombreUsuario, conexion);
            }
            catch(ArgumentNullException e)
            {
                Console.WriteLine(Recurso.Excepcion_MSJCONST + e);
            }
            catch (CommunicationObjectFaultedException e)
            {
                throw new FaultException(Recurso.Excepcion_MSJCONST + e);
            }
        }

        public void EnviarMensajeChat(string codigoSala, string nombreUsuario, string mensaje)
        {
            try 
            {
                Dictionary<string, IChatServiceCallback>.KeyCollection jugadores = inventarioSalas[codigoSala].jugadoresChatCallback.Keys;
                string primero = jugadores.First();
                string ultimo = jugadores.Last();
                inventarioSalas[codigoSala].jugadoresChatCallback[primero].RecibirMensaje(nombreUsuario, mensaje);
                inventarioSalas[codigoSala].jugadoresChatCallback[ultimo].RecibirMensaje(nombreUsuario, mensaje);
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
    }
}
