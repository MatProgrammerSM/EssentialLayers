using EssentialLayers.Helpers.Extension;
using EssentialLayers.Request.Helpers;
using EssentialLayers.Request.Services.Helpers;
using EssentialLayers.Request.Services.Http.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using RequestOptions = EssentialLayers.Request.Services.Http.Models.RequestOptions;

namespace EssentialLayers.Request.Services.Http
{
	internal class HttpService : IHttpService
	{
		private readonly HttpClient HttpClient = new(
			new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
			}
		);

		private HttpOption HttpOption = new()
		{
			AppName = "AppName",
			AppVersion = "1.0"
		};

		/**/

		public void SetOptions(HttpOption httpOption)
		{
			HttpOption = httpOption;
		}

		public async Task<HttpResponse<TResult>> GetAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions? options = null
		)
		{
			return await SendAsync<TResult, TRequest>(
				request, url, HttpMethod.Get, options
			);
		}

		public async Task<HttpResponse<TResult>> PostAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions? options = null
		)
		{
			return await SendAsync<TResult, TRequest>(
				request, url, HttpMethod.Post, options
			);
		}

		public async Task<HttpResponse<TResult>> PutAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions? options = null
		)
		{
			return await SendAsync<TResult, TRequest>(
				request, url, HttpMethod.Put, options
			);
		}

		public async Task<HttpResponse<TResult>> DeleteAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions? options = null
		)
		{
			return await SendAsync<TResult, TRequest>(
				request, url, HttpMethod.Delete, options
			);
		}

		private async Task<HttpResponse<TResult>> SendAsync<TResult, TRequest>(
			TRequest request, string url, HttpMethod httpMethod,
			RequestOptions? options
		)
		{
			try
			{
				options ??= new RequestOptions();

				HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
					$"{HttpOption.AppName}/{HttpOption.AppVersion}"
				);

				foreach (KeyValuePair<string, string> header in options.Headers!)
				{
					HttpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
				}

				string jsonRequest = request.Serialize();
				string bearerToken = options.BearerToken.NotEmpty() ? options.BearerToken : string.Empty;

				Debug.WriteLine(
					$"HttpService -> Request Method [{httpMethod.Method}]: URL [ {url} ], JSON [ {jsonRequest} ] "
				);

				using HttpRequestMessage httpRequestMessage = new()
				{
					Content = new StringContent(jsonRequest, Encoding.UTF8),
					RequestUri = new Uri(url),
					Method = httpMethod
				};

				httpRequestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
				httpRequestMessage.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

				if (bearerToken.NotEmpty())
				{
					httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(
						"Bearer", bearerToken
					);
				}

				HttpResponseMessage? httpResponseMessage = null;

				if (options.IsCached)
				{
					string key = request.Serialize();

					httpResponseMessage = await CacheHelper<TResult>.HttpResponseMessage.GetOrCreate(
						key, async () => await HttpClient.SendAsync(
							httpRequestMessage, options.CancellationToken
						)
					);
				}
				else
				{
					httpResponseMessage = await HttpClient.SendAsync(
						httpRequestMessage, options.CancellationToken
					);
				}

				string response = await httpResponseMessage.Content.ReadAsStringAsync();
				string jsonResponse = response.Serialize();

				Debug.WriteLine(
					$"HttpService -> Response Method [{httpMethod.Method}]: URL [ {url} ], JSON [ {jsonResponse} ] "
				);

				return HttpHelper.ManageResponse<TResult>(
					httpResponseMessage.StatusCode, response, HttpOption.CastResultAsResultHelper
				);
			}
			catch (Exception e)
			{
				return HttpResponse<TResult>.Fail(e, HttpStatusCode.InternalServerError);
			}
		}
	}
}