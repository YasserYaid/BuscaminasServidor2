using System.ServiceModel;

namespace Contratos
{
    [ServiceContract(CallbackContract = typeof(IJuegoServiceCallback))]
    public interface IJuegoServiceDuplex
    {
        [OperationContract(IsOneWay = true)]
        void AgregarJugadorJuegoCallback(string codigoSala, string nombreUsuario);
        [OperationContract(IsOneWay = true)]
        void PasarTurno(string codigoSala, string nombreUsuarioClic, int turnoUsuarioClic, Celda celda);
        [OperationContract(IsOneWay = true)]
        void EnviarConclusion(string codigoSala, string nombreUsuario);
        [OperationContract(IsOneWay = true)]
        void CrearNuevoTablero(string codigoSala, int ancho, int alto, int numeroMinas);
    }
}
