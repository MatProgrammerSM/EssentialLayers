using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using EssentialLayers.Request.Helpers;
using EssentialLayers.Request.Helpers.Estension;
using EssentialLayers.Request.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RequestOptions = EssentialLayers.Request.Models.RequestOptions;

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

		public async Task<HttpResponse<TResult>> DeleteAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions? options = null
		)
		{
			return await SendAsync<TResult, TRequest>(
				request, url, HttpMethod.Delete, options
			);
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

		public void SetOptions(HttpOption httpOption)
		{
			HttpOption = httpOption;
		}

		private async Task<HttpResponse<TResult>> SendAsync<TResult, TRequest>(
			TRequest request, string url, HttpMethod httpMethod,
			RequestOptions? options
		)
		{
			try
			{
				options ??= new RequestOptions();
				bool insensitiveMapping = true;

				HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
					$"{HttpOption.AppName}/{HttpOption.AppVersion}"
				);

				foreach (KeyValuePair<string, string> header in options.Headers!)
				{
					HttpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
				}

				if (options.BaseUri.NotEmpty()) HttpClient.BaseAddress = new Uri(options.BaseUri);

				if (false.IsAny(options.InsensitiveMapping, HttpOption.InsensitiveMapping)) insensitiveMapping = false;

				string jsonRequest = request.Serialize();
				string bearerToken = options.BearerToken.NotEmpty() ? options.BearerToken : string.Empty;
				ResultHelper<HttpContent> contentResult = request.ToHttpContent(options.ContentType);

				if (contentResult.Ok.False()) return HttpResponse<TResult>.Fail(
					contentResult.Message, HttpStatusCode.InternalServerError
				);

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

				if (httpResponseMessage.IsSuccessStatusCode.False()) return HttpResponse<TResult>.Fail(
					response, httpResponseMessage.StatusCode
				);

				string jsonResponse = response.Serialize();

				Debug.WriteLine(
					$"HttpService -> Response Method [{httpMethod.Method}]: URL [ {url} ], JSON [ {jsonResponse} ] "
				);

				return HttpHelper.ManageResponse<TResult>(
					httpResponseMessage.StatusCode, response, HttpOption.CastResultAsResultHelper, insensitiveMapping
				);
			}
			catch (Exception e)
			{
				return HttpResponse<TResult>.Fail(e, HttpStatusCode.InternalServerError);
			}
		}
	}
}