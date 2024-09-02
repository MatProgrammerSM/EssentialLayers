using EssentialLayers.Helpers.Logger;
using System;
using System.Runtime.CompilerServices;

namespace EssentialLayers.Helpers.Result
{
	public class ResultHelper<T>(bool ok, string message, T data)
	{
		public bool Ok { get; set; } = ok;

		public string Message { get; set; } = message;

		public T Data { get; set; } = data;

		public static ResultHelper<T> Success(T data) => new(true, string.Empty, data);

		public static ResultHelper<T> Fail(string message) => new(false, message, default!);

		public static ResultHelper<T> Fail(
			Exception e,
			[CallerFilePath] string file = null,
			[CallerMemberName] string member = null,
			[CallerLineNumber] int lineNumber = 0
		)
		{
			Type type = e.GetType();

			if (ErrorMessages.Messages.TryGetValue(type, out string message))
			{
				LoggerHelper.Error(
					e, $" ResultHelper - File: {file} | Member: {member} | Line Number: {lineNumber} - [{e.Message}]"
				);

				if (message != null) return new ResultHelper<T>(false, message, default!);
			}

			return new ResultHelper<T>(
				false, "Ocurrio un error no controlado, contactar al administrador", default!
			);
		}
	}
}