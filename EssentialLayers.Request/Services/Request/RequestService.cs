using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using EssentialLayers.Request.Helpers;
using EssentialLayers.Request.Helpers.Estension;
using EssentialLayers.Request.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EssentialLayers.Request.Services.Request
{
    internal class RequestService : IRequestService
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

		public async Task<HttpResponseMessage> DeleteAsync<TRequest>(
			TRequest request, string url, RequestOptions? options = null
		)
		{
			return await SendAsync(
				request, url, HttpMethod.Delete, options
			);
		}

		public async Task<HttpResponseMessage> GetAsync<TRequest>(
			TRequest request, string url, RequestOptions? options = null
		)
		{
			return await SendAsync(
				request, url, HttpMethod.Get, options
			);
		}

		public async Task<HttpResponseMessage> PostAsync<TRequest>(
			TRequest request, string url, RequestOptions? options = null
		)
		{
			return await SendAsync(
				request, url, HttpMethod.Post, options
			);
		}

		public async Task<HttpResponseMessage> PutAsync<TRequest>(
			TRequest request, string url, RequestOptions? options = null
		)
		{
			return await SendAsync(
				request, url, HttpMethod.Put, options
			);
		}

		private async Task<HttpResponseMessage> SendAsync<TRequest>(
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
				ResultHelper<HttpContent> contentResult = request.ToHttpContent(options.ContentType);

				Debug.WriteLine(
					$"HttpService -> Request Method [{httpMethod.Method}]: URL [ {url} ], JSON [ {jsonRequest} ] "
				);

				using HttpRequestMessage httpRequestMessage = new()
				{
					Content = contentResult.Data,
					RequestUri = new Uri(url),
					Method = httpMethod
				};

				httpRequestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(options.ContentType);
				httpRequestMessage.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(options.ContentType));

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

					httpResponseMessage = await CacheHelper<HttpResponseMessage>.HttpResponseMessage.GetOrCreate(
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

				return httpResponseMessage;
			}
			catch (Exception e)
			{
				Debug.WriteLine(
					$"Error -> Response Method [{httpMethod.Method}]: URL [ {url} ], JSON [ {e.Message} ] "
				);

				return null!;
			}
		}

		public void SetOptions(HttpOption httpOption)
		{
			HttpOption = httpOption;
		}
	}
}