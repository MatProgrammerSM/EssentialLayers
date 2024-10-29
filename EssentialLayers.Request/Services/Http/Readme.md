# Essential Layers
#### [EssentialLayers.Request](/EssentialLayers.Dapper/Readme.md)
It is a layer that performs the handling of the http service with typical methods, using dependency injection, plus consists of an implementation of results to manage the response.
### Configure

| Method  | Definition |
| :--------- | :------------ |
| GET | GetAsync<TResult, TRequest>(TRequest request, string url, RequestOptions? options = null) |
| POST | Task<HttpResponse<TResult>> PostAsync<TResult, TRequest>(TRequest request, string url, RequestOptions? options = null); |
| PUT | Task<HttpResponse<TResult>> PutAsync<TResult, TRequest>(TRequest request, string url, RequestOptions? options = null); |
| DELETE | Task<HttpResponse<TResult>> DeleteAsync<TResult, TRequest>(TRequest request, string url, RequestOptions? options = null); |

Created by [Mario Soto Moreno](https://github.com/MatProgrammerSM)