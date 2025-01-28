using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using EssentialLayers.Request.Helpers.Estension;
using EssentialLayers.Request.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EssentialLayers.Request.Helpers.Request
{
	public class RequestHelper
	{
		private readonly HttpClient HttpClient;

		private readonly HttpOption HttpOption;

		/**/

		public RequestHelper()
		{
			HttpClient = new(
				new HttpClientHandler
				{
					ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
				}
			);

			HttpOption = new()
			{
				AppName = "AppName",
				AppVersion = "1.0"
			};
		}

		public async Task<HttpResponseMessage> SendAsync<TRequest>(
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

				GlobalFunctions.Info(url, httpMethod.Method, jsonRequest);

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

				GlobalFunctions.Info(url, httpMethod.Method, response);

				return httpResponseMessage;
			}
			catch (Exception e)
			{
				GlobalFunctions.Error(e);

				return null!;
			}
		}

		public void SetOptions(HttpOption httpOption)
		{
			HttpOption.AppName = httpOption.AppName;
			HttpOption.AppVersion = httpOption.AppVersion;
			HttpOption.BaseUri = httpOption.BaseUri;
			HttpOption.BearerToken = httpOption.BearerToken;
			HttpOption.CastResultAsResultHelper = httpOption.CastResultAsResultHelper;
			HttpOption.InsensitiveMapping = httpOption.InsensitiveMapping;

			HttpClient.BaseAddress = new Uri(httpOption.BaseUri);
			HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
				"Bearer", httpOption.BearerToken
			);
		}
	}
}