using System.Reflection;

namespace Dominio.Core.Extensions
{
    public static class ReflectionManager
    {
        /// <summary>
        /// Obtiene el valor de una propiedad de un objeto mediante reflexión
        /// y lo convierte al tipo especificado.
        /// </summary>
        /// <typeparam name="T">El tipo al que se convertirá el valor de la propiedad.</typeparam>
        /// <param name="obj">El objeto del cual se obtendrá la propiedad.</param>
        /// <param name="name">El nombre de la propiedad que se desea obtener.</param>
        /// <returns>
        /// El valor de la propiedad convertido a <typeparamref name="T"/>.
        /// Si la propiedad no existe o su valor es nulo, devuelve el valor por defecto de <typeparamref name="T"/>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// var persona = new { Nombre = "Ana", Edad = 30 };
        ///
        /// string nombre = persona.GetPropValue<string>("Nombre"); // "Ana"
        /// int edad = persona.GetPropValue<int>("Edad"); // 30
        /// int altura = persona.GetPropValue<int>("Altura"); // 0 (propiedad inexistente)
        ///
        /// Console.WriteLine($"Nombre: {nombre}, Edad: {edad}, Altura: {altura}");
        /// </code>
        /// </example>
        public static T GetPropValue<T>(this object obj, string name)
        {
            object retval = GetPropValue(obj, name);
            if (retval == null) { return default(T); }

            return (T)retval;
        }

        /// <summary>
        /// Establece el valor de una propiedad de un objeto mediante reflexión.
        /// </summary>
        /// <typeparam name="T">El tipo de la propiedad que se desea asignar.</typeparam>
        /// <param name="obj">El objeto en el cual se establecerá la propiedad.</param>
        /// <param name="name">
        /// El nombre de la propiedad que se desea establecer. 
        /// Se admite notación con puntos para propiedades anidadas.
        /// </param>
        /// <param name="value">El valor que se asignará a la propiedad.</param>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// var persona = new Persona { Nombre = "Ana", Edad = 25 };
        ///
        /// // Cambiar el valor de la propiedad "Nombre"
        /// persona.SetPropValue<string>("Nombre", "Luis");
        ///
        /// Console.WriteLine(persona.Nombre); // Luis
        /// </code>
        /// </example>
        public static void SetPropValue<T>(this object obj, string name, object value)
        {
            foreach (String part in name.Split('.'))
            {
                if (obj == null) { return; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return; }

                info.SetValue(obj, (T)value, null);
            }
        }

        /// <summary>
        /// Obtiene el valor de una propiedad de un objeto mediante reflexión.
        /// Soporta propiedades anidadas usando notación con puntos.
        /// </summary>
        /// <param name="obj">El objeto del cual se obtendrá la propiedad.</param>
        /// <param name="name">
        /// El nombre de la propiedad que se desea obtener. 
        /// Se admite notación con puntos para acceder a propiedades anidadas.
        /// </param>
        /// <returns>
        /// El valor de la propiedad especificada, o <c>null</c> si el objeto es nulo,
        /// la propiedad no existe, o su valor es nulo.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// var persona = new Persona 
        /// { 
        ///     Nombre = "Ana", 
        ///     Direccion = new Direccion { Ciudad = "Madrid" } 
        /// };
        ///
        /// object nombre = persona.GetPropValue("Nombre"); // "Ana"
        /// object ciudad = persona.GetPropValue("Direccion.Ciudad"); // "Madrid"
        /// object pais = persona.GetPropValue("Direccion.Pais"); // null (propiedad inexistente)
        ///
        /// Console.WriteLine($"Nombre: {nombre}, Ciudad: {ciudad}, País: {pais}");
        /// </code>
        /// </example>
        private static object GetPropValue(this object obj, string name)
        {
            foreach (string part in name.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }
    }
}
