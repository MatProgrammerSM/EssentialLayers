using EssentialLayers.Helpers.Cache;
using EssentialLayers.Helpers.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EssentialLayers.Helpers.Http
{
	public class HttpWebHelper
	{
		private readonly HttpClient HttpClient = new(
			new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
			}
		);

		/**/

		public HttpWebHelperOption httpWebHelperOption = new();

		/**/


		public async Task<HttpWebResponse<TResult>> GetAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions options = null
		)
		{
			try
			{
				options ??= new RequestOptions();

				HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
					$"{httpWebHelperOption.AppName}/{httpWebHelperOption.AppVersion}"
				);

				foreach (KeyValuePair<string, string> header in options.Headers!)
				{
					HttpClient.DefaultRequestHeaders.Add(
						header.Key, header.Value
					);
				}

				if (options.IsCached)
				{
					string key = request.Serialize();

					HttpWebResponse<TResult> resultCache = await CacheHelper<TResult>.HttpWebResponseCache.GetOrCreate(
						key, async () => await SendAsync<TResult, TRequest>(
							request, url, HttpMethod.Get, options
						)
					);

					return resultCache;
				}
				else return await SendAsync<TResult, TRequest>(
					request, url, HttpMethod.Get, options
				);
			}
			catch (Exception e)
			{
				return HttpWebResponse<TResult>.Fail(
					e, HttpStatusCode.InternalServerError
				);
			}
		}

		public async Task<HttpWebResponse<TResult>> PostAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions options = null
		)
		{
			try
			{
				options ??= new RequestOptions();

				HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
					$"{httpWebHelperOption.AppName}/{httpWebHelperOption.AppVersion}"
				);

				foreach (KeyValuePair<string, string> header in options.Headers!)
				{
					HttpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
				}

				if (options.IsCached)
				{
					string key = request.Serialize();

					HttpWebResponse<TResult> resultCache = await CacheHelper<TResult>.HttpWebResponseCache.GetOrCreate(
						key, async () => await SendAsync<TResult, TRequest>(
							request, url, HttpMethod.Post, options
						)
					);

					return resultCache;
				}
				else return await SendAsync<TResult, TRequest>(
					request, url, HttpMethod.Post, options
				);
			}
			catch (Exception e)
			{
				return HttpWebResponse<TResult>.Fail(
					e, HttpStatusCode.InternalServerError
				);
			}
		}

		private async Task<HttpWebResponse<TResult>> SendAsync<TResult, TRequest>(
			TRequest request, string url, HttpMethod httpMethod,
			RequestOptions options
		)
		{
			string jsonRequest = request.Serialize();
			string bearerToken = httpWebHelperOption.BearerToken.NotEmpty() ? httpWebHelperOption.BearerToken : options.BearerToken;

			Debug.WriteLine($"HttpService -> Request Method [{httpMethod.Method}]: URL [ {url} ], JSON [ {jsonRequest} ] ");

			using HttpRequestMessage httpRequestMessage = new()
			{
				Content = new StringContent(jsonRequest, Encoding.UTF8),
				RequestUri = new Uri(url),
				Method = httpMethod
			};

			httpRequestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
			httpRequestMessage.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

			if (!string.IsNullOrEmpty(options.BearerToken))
			{
				httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(
					"Bearer", bearerToken
				);
			}

			HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(
				httpRequestMessage, options.CancellationToken
			);

			string response = await httpResponseMessage.Content.ReadAsStringAsync();
			string jsonResponse = response.Serialize();

			Debug.WriteLine($"HttpService -> Response Method [{httpMethod.Method}]: URL [ {url} ], JSON [ {jsonResponse} ] ");

			return ManageResponse<TResult>(httpResponseMessage.StatusCode, response);
		}

		private static HttpWebResponse<TResult> ManageResponse<TResult>(
			HttpStatusCode httpStatusCode, string response
		)
		{
			switch (httpStatusCode)
			{
				case HttpStatusCode.OK:
				case HttpStatusCode.BadRequest:

					TResult result = response.Deserialize<TResult>();

					return HttpWebResponse<TResult>.Success(result, httpStatusCode);

				case HttpStatusCode.NotFound:

					return HttpWebResponse<TResult>.Fail(
						"The request method not found", httpStatusCode
					);

				case HttpStatusCode.ServiceUnavailable:

					return HttpWebResponse<TResult>.Fail(
						"The server is unavailable", httpStatusCode
					);

				case HttpStatusCode.Forbidden:

					return HttpWebResponse<TResult>.Fail(
						"The request is forbidden", httpStatusCode
					);

				default:

					return HttpWebResponse<TResult>.Fail(response.Serialize(), httpStatusCode);
			}
		}
	}
}