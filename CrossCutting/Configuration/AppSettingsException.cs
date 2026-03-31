namespace CrossCutting.Configuration
{
    /// <summary>
    /// Excepción neutra de dominio que encapsula errores de configuración
    /// sin filtrar detalles de infraestructura hacia las capas superiores.
    /// </summary>
    public sealed class AppSettingsException : Exception
    {
        public AppSettingsException(string message) : base(message) { }
        public AppSettingsException(string message, Exception inner) : base(message, inner) { }
    }
}
