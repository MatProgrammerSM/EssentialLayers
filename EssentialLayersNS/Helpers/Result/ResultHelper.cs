using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace EssentialLayersNS.Helpers.Result
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

			if (ErrorMessages.Messages.TryGetValue(type, out string userMessage))
			{
				Debug.WriteLine(
					$" ResultHelper - File: {file} | Member: {member} | Line Number: {lineNumber} - [{e.Message}]"
				);

				if (userMessage != null) return new ResultHelper<T>(false, userMessage, default!);
			}

			return new ResultHelper<T>(
				false, "Ocurrio un error no controlado, contactar al administrador", default!
			);
		}
	}
}