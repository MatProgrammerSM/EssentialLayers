using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace EssentialLayers.Request.Helpers.Estension
{
	public static class HttpExtension
	{
		public static ResultHelper<HttpContent> ToHttpContent<T>(this T request, string contentType)
		{
			try
			{
				switch (contentType)
				{
					case "application/x-www-form-urlencoded":

						IDictionary<string, string>? dicitionary = request as IDictionary<string, string>;

						return ResultHelper<HttpContent>.Success(new FormUrlEncodedContent(dicitionary));

					default:

						string jsonRequest = request.Serialize();

						return ResultHelper<HttpContent>.Success(new StringContent(jsonRequest, Encoding.UTF8));
				}
			}
			catch (Exception e)
			{
				return ResultHelper<HttpContent>.Fail(e);
			}
		}
	}
}