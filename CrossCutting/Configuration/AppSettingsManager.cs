using Dominio.Core.Extensions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossCutting.Configuration
{
    /// <summary>
    /// Administrador centralizado de configuraciones de la aplicación.
    /// Actúa como singleton lógico mediante clase estática.
    ///
    /// Estructura del índice en memoria:
    ///   Dictionary&lt;ConfiguracionId, Dictionary&lt;Atributo, Valor&gt;&gt;
    ///
    /// Esto permite búsquedas O(1) tanto por grupo como por atributo individual.
    /// </summary>
    public static class AppSettingsManager
    {
        // ── Estado interno ───────────────────────────────────────────────────

        /// <summary>
        /// Índice principal: ConfiguracionId → (Atributo → Valor)
        /// </summary>
        private static Dictionary<string, Dictionary<string, string>> _index
            = new(StringComparer.OrdinalIgnoreCase);

        private static string _connectionString = string.Empty;
        private static bool _isLoaded = false;
        private static readonly object _lock = new();

        // ── Inicialización ───────────────────────────────────────────────────

        /// <summary>
        /// Inicializa el manager con la cadena de conexión y carga la tabla.
        /// Debe llamarse una sola vez al arranque de la aplicación (Program.cs).
        /// Las llamadas posteriores son ignoradas salvo que <paramref name="forceReload"/> sea true.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión a SQL Server.</param>
        /// <param name="forceReload">Si es true fuerza la recarga aunque ya esté inicializado.</param>
        public static void Initialize(string connectionString, bool forceReload = false)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString),
                    "La cadena de conexión no puede estar vacía.");

            lock (_lock)
            {
                if (_isLoaded && !forceReload)
                    return; // Salida temprana — patrón singleton lógico

                _connectionString = connectionString;
                LoadSettings();
            }
        }

        /// <summary>
        /// Recarga las configuraciones desde la base de datos en tiempo de ejecución
        /// sin reiniciar la aplicación (recarga en caliente).
        /// </summary>
        public static void Reload()
        {
            lock (_lock)
            {
                if (string.IsNullOrWhiteSpace(_connectionString))
                    throw new InvalidOperationException(
                        "AppSettingsManager no ha sido inicializado. Llame a Initialize() primero.");

                LoadSettings();
            }
        }

        // ── Carga interna ────────────────────────────────────────────────────

        /// <summary>
        /// Ejecuta la lectura con ADO.NET puro para mantener el desacoplamiento
        /// de capas de Data/Infrastructure. Construye el índice en memoria.
        /// El swap final es atómico: si la carga falla, el índice anterior se preserva.
        /// </summary>
        private static void LoadSettings()
        {
            // Diccionario temporal — se asigna al campo solo si la carga es exitosa
            var tempIndex = new Dictionary<string, Dictionary<string, string>>(
                StringComparer.OrdinalIgnoreCase);

            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                const string query = @"
                    SELECT [ConfiguracionId],
                           [Atributo],
                           [Valor]
                    FROM   [Comunes].[ConfiguracionesDetalle]
                    WHERE  [ConfiguracionId] IS NOT NULL
                      AND  [Atributo]        IS NOT NULL";

                using var command = new SqlCommand(query, connection);
                using var reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                while (reader.Read())
                {
                    string configId = reader["ConfiguracionId"]?.ToStringValue() ?? string.Empty;
                    string atributo = reader["Atributo"]?.ToStringValue() ?? string.Empty;
                    string valor = reader["Valor"]?.ToStringValue() ?? string.Empty;

                    if (string.IsNullOrEmpty(configId) || string.IsNullOrEmpty(atributo))
                        continue;

                    // Crea el grupo si aún no existe
                    if (!tempIndex.ContainsKey(configId))
                        tempIndex[configId] = new Dictionary<string, string>(
                            StringComparer.OrdinalIgnoreCase);

                    tempIndex[configId][atributo] = valor; // última fila gana si hay duplicados
                }

                // ✅ Swap atómico — solo reemplaza si la carga fue exitosa
                _index = tempIndex;
                _isLoaded = true;
            }
            catch (SqlException ex)
            {
                throw new AppSettingsException(
                    "Error al cargar configuraciones desde la base de datos.", ex);
            }
            catch (Exception ex)
            {
                throw new AppSettingsException(
                    "Error inesperado al cargar configuraciones.", ex);
            }
        }

        // ── Métodos de acceso públicos ───────────────────────────────────────

        /// <summary>
        /// Retorna todos los atributos y valores que pertenecen a un ConfiguracionId.
        /// </summary>
        /// <param name="configurationId">Identificador del grupo de configuraciones.</param>
        /// <returns>
        /// Diccionario de solo lectura Atributo → Valor,
        /// o un diccionario vacío si el grupo no existe.
        /// </returns>
        public static IReadOnlyDictionary<string, string> GetConfigurationById(
            string configurationId)
        {
            EnsureLoaded();
            ValidateParam(configurationId, nameof(configurationId));

            return _index.TryGetValue(configurationId, out var group)
                ? group
                : new Dictionary<string, string>();
        }

        /// <summary>
        /// Retorna el valor de un atributo específico dentro de un grupo.
        /// </summary>
        /// <param name="configurationId">Identificador del grupo.</param>
        /// <param name="attribute">Nombre del atributo.</param>
        /// <returns>
        /// El valor como string, o <see cref="string.Empty"/> si no se encuentra.
        /// </returns>
        public static string GetConfigurationByIdAndAttribute(
            string configurationId,
            string attribute)
        {
            EnsureLoaded();
            ValidateParam(configurationId, nameof(configurationId));
            ValidateParam(attribute, nameof(attribute));

            if (_index.TryGetValue(configurationId, out var group) &&
                group.TryGetValue(attribute, out string? value))
                return value;

            return string.Empty;
        }

        /// <summary>
        /// Evalúa si una configuración está "activa" (encendida).
        /// Acepta como verdadero: "true", "1", "yes", "si", "sí", "on", "activo".
        /// </summary>
        /// <param name="configurationId">Identificador del grupo.</param>
        /// <param name="attributeId">Nombre del atributo bandera.</param>
        /// <returns>
        /// <c>true</c> si el valor se interpreta como activo, <c>false</c> en cualquier otro caso.
        /// </returns>
        public static bool IsConfigurationOn(
            string configurationId,
            string attributeId)
        {
            string valor = GetConfigurationByIdAndAttribute(configurationId, attributeId);

            return valor.ToLowerInvariant() switch
            {
                "true" or "1" or "yes" or "si" or "sí" or "on" or "activo" => true,
                _ => false
            };
        }

        // ── Métodos auxiliares de conversión tipada ──────────────────────────

        /// <summary>
        /// Obtiene un atributo y lo convierte a int.
        /// Retorna <paramref name="defaultValue"/> si no existe o la conversión falla.
        /// </summary>
        public static int GetInt(string configurationId, string attribute, int defaultValue = 0)
        {
            string raw = GetConfigurationByIdAndAttribute(configurationId, attribute);
            return int.TryParse(raw, out int result) ? result : defaultValue;
        }

        /// <summary>
        /// Obtiene un atributo y lo convierte a decimal.
        /// </summary>
        public static decimal GetDecimal(string configurationId, string attribute, decimal defaultValue = 0m)
        {
            string raw = GetConfigurationByIdAndAttribute(configurationId, attribute);
            return decimal.TryParse(raw,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out decimal result) ? result : defaultValue;
        }

        /// <summary>
        /// Retorna una instantánea (snapshot) de todo el índice.
        /// Útil para diagnóstico o endpoints de administración.
        /// </summary>
        public static IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> GetSnapshot()
        {
            EnsureLoaded();
            var snapshot = new Dictionary<string, IReadOnlyDictionary<string, string>>(
                StringComparer.OrdinalIgnoreCase);

            foreach (var (key, group) in _index)
                snapshot[key] = group;

            return snapshot;
        }

        // ── Guardianes internos ──────────────────────────────────────────────

        private static void EnsureLoaded()
        {
            if (!_isLoaded)
                throw new InvalidOperationException(
                    "AppSettingsManager no ha sido inicializado. " +
                    "Llame a AppSettingsManager.Initialize(connectionString) al inicio de la aplicación.");
        }

        private static void ValidateParam(string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(paramName,
                    $"El parámetro '{paramName}' no puede ser nulo o vacío.");
        }
    }

}
