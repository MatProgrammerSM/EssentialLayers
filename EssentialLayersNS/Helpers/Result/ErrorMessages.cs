using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EssentialLayersNS.Helpers.Result
{
	public sealed class ErrorMessages
	{
		public static readonly IDictionary<Type, string> Messages = new Dictionary<Type, string>
		{
			{typeof(NullReferenceException), $"Hay una referencia de objeto que esta nula"},
			{typeof(ArgumentNullException), $"Hay un argumento que esta llegando vacio"},
			{typeof(ArgumentOutOfRangeException), $"Esta tratando de acceder a un elemento de una lista que no existe"},
			{typeof(ArithmeticException), $"Hay un error aritmetico que no deja continuar el proceso"},
			{typeof(InsufficientMemoryException), $"No hay sufuciente memoria en el dispositivo"},
			{typeof(InvalidCastException), $"Hay un error al momento de realizar la conversión de un tipo a otro"},
			{typeof(OperationCanceledException), $"Error al realizar la cancelación"},
			{typeof(TimeoutException), $"El tiempo de espera para el consumo del recurso se ha agotado"},
			{typeof(UriFormatException), $"Hay una url no válida"},
			{typeof(KeyNotFoundException), $"Hay una llave que no esta presente en un diccionario"},
			{typeof(NotSupportedException), $"Hay una error con la lectura o escritura del archivo"},
			{typeof(UnauthorizedAccessException), $"No hay permisos de escritura en archivos"},
			{typeof(TaskCanceledException), $"La operación no pudo ser completada"},
			{typeof(InvalidOperationException), $"La llave no se encontró en el diccionario"},
			{typeof(HttpRequestException), $"La llave no se encontró en el diccionario"}
		};
	}
}