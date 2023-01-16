using System.ServiceModel;

namespace Contratos
{
    [ServiceContract]
    public interface IJuegoServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void RecibirTurno(string nombreUsuarioClic, int turnoUsuarioClic, Celda celda);
        [OperationContract(IsOneWay = true)]
        void RecibirConclusion(string nombreUsuario);
    }
}
