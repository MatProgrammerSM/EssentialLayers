﻿using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using System.Net;

namespace EssentialLayers.Request.Helpers
{
	internal static class HttpHelper
	{
		public static HttpResponse<TResult> ManageResponse<TResult>(
			HttpStatusCode httpStatusCode, string response, bool castAsResultHelper, bool insensitiveMapping
		)
		{
			switch (httpStatusCode)
			{
				case HttpStatusCode.OK:
				case HttpStatusCode.BadRequest:

					if (castAsResultHelper)
					{
						ResultHelper<TResult> resultHelper = response.Deserialize<ResultHelper<TResult>>(insensitive: insensitiveMapping);

						if (resultHelper.Ok.False()) return HttpResponse<TResult>.Fail(
							resultHelper.Message, httpStatusCode
						);

						return HttpResponse<TResult>.Success(resultHelper.Data, httpStatusCode);
					}
					else
					{
						TResult? result = response.Deserialize<TResult>(insensitive: insensitiveMapping);

						return HttpResponse<TResult>.Success(result, httpStatusCode);
					}

				case HttpStatusCode.InternalServerError:

					return HttpResponse<TResult>.Fail(
						$"There server has returned the next error {response}", httpStatusCode
					);

				case HttpStatusCode.NotFound:

					return HttpResponse<TResult>.Fail(
						"The request method not found", httpStatusCode
					);

				case HttpStatusCode.ServiceUnavailable:

					return HttpResponse<TResult>.Fail(
						"The server is unavailable", httpStatusCode
					);

				case HttpStatusCode.Forbidden:

					return HttpResponse<TResult>.Fail(
						"The request is forbidden", httpStatusCode
					);

				default:

					return HttpResponse<TResult>.Fail(response.Serialize(), httpStatusCode);
			}
		}
	}
}