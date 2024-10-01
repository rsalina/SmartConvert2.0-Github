using System;

namespace TCPSmart.Conexion
{
    public class Bitacora
    {
        private DateTime _StartTime = DateTime.Now.ToUniversalTime();
        private long _ReceivedBytes = 0;
        private long _SentBytes = 0;
        /// <summary>
        /// Hora y Fecha en la que empezo la conexion cliente o servidor.
        /// </summary>
        public DateTime StartTime
        {
            get { return _StartTime; }
        }
        /// <summary>
        /// Duracion de la conexion.
        /// </summary>
        public TimeSpan UpTime
        {
            get { return DateTime.Now.ToUniversalTime() - _StartTime; }
        }

        public long BytesRecibidos
        {
            get { return _ReceivedBytes; }
            internal set { _ReceivedBytes = value; }
        }

        public long BytesEnviados
        {
            get { return _SentBytes; }
            internal set { _SentBytes = value; }
        }

        public Bitacora()
        {

        }
        /// <summary>
        /// TODO: Aqui Sincronizar la BD 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string ret =
                "-- B I T A C O R A --" + Environment.NewLine +
                "    Inicio          : " + _StartTime.ToString() + Environment.NewLine +
                "    Duracion        : " + UpTime.ToString() + Environment.NewLine +
                "    Bytes Recibidos : " + BytesRecibidos + Environment.NewLine +
                "    Bytes Enviados  : " + BytesEnviados + Environment.NewLine;
            return ret;
        }

        public void Reset()
        {
            _ReceivedBytes = 0;
            _SentBytes = 0;
        }

    }
}
