# Result Helper
It is an abstraction layer that through its properties (Ok, Message, Data) and methods (Fail, Success), return a standardized result, to perform better exception handling.

### Definition
#### Properties

| Property      | Description                           |
| :------------ | :------------------------------------ |
| Ok            | Display the result of the operation   |
| Message       | Display the message of the operation  |
| Data          | Display the type of data\<T>          |

#### Methods
| Property                      | Description                                           |
| :---------------------------- | :---------------------------------------------------- |
| Success(T data)               | set the type of data\<T> and the Ok property in true  |
| Fail(string message)          | set the Message property and the Ok property in false |


### Examples
#### Code
```
int division = Divide(10);

public int Divide(divisor, dividend){
    try {
        int result = divisor / dividend;
        
        return result;
    } catch (Exception e){
        thrown e;
        
        return 0;
    }
}

```

#### With ResultHelper

```
ResultHelper<int> result = Divide(10);

if (!result.Ok) Debug.WriteLine(result.Message);

int division = result.Data;

public ResultHelper<int> Divide(divisor, dividend){
    try {
        int result = divisor / dividend;
        
        return ResultHelper<T>.Success(result);
    } catch (Exception e){
        ResultHelper<T>.Fail(e);
    }
}

```
___

# Dapper Service
It is an abstraction layer that provide a serie of functions to communicate the back-end with the database trought of store procedures.

### Examples of usage
#### Configure in your Program.cs
```
app.Services.ConfigureDatabase(connectionString);
```
##### Use
```
using EssentialLayers.Services.Database;

ResultHelper<IEnumerable<UserDto.Result>> result = await DatabaseService.ExecuteAll<UserDto.Result, UserDto.Request>(
    request, "spQueryUsers"
);

if (result.Ok.False()) Debug.WriteLine(result.Message);

IEnumerable<T> users = result.Data;
```

### Definition
#### Methods
| Execute                                                                        |
| :----------------------------------------------------------------------------- |
| ResultHelper\<U> Execute<T, U>(T request, string storedProcedure);             |
| Task<ResultHelper<\<U>> ExecuteAsync<T, U>(T request, string storedProcedure); |

| Execute                                                                        |
| :----------------------------------------------------------------------------- |
| ResultHelper\<U> Execute<T, U>(T request, string storedProcedure);             |
| Task<ResultHelper<\<U>> ExecuteAsync<T, U>(T request, string storedProcedure); |

| ExecuteAll                                                                                     |
| :--------------------------------------------------------------------------------------------- |
| ResultHelper\<IEnumerable\<U>> ExecuteAll<T, U>(T request, string storedProcedure);            |
| Task<ResultHelper\<IEnumerable\<U>>> ExecuteAllAsync<T, U>(T request, string storedProcedure); |

| ExecuteComplex                                                                        |
| :------------------------------------------------------------------------------------ |
| ResultHelper\<U> ExecuteComplex<T, U>(T request, string storedProcedure);             |
| Task<ResultHelper<\<U>> ExecuteComplexAsync<T, U>(T request, string storedProcedure); |

| ExecuteComplexAll                                                                                     |
| :---------------------------------------------------------------------------------------------------- |
| ResultHelper\<U> ExecuteComplexAll<T, U>(T request, string storedProcedure);                          |
| Task<ResultHelper\<IEnumerable\<U>>> ExecuteComplexAllAsync<T, U>(T request, string storedProcedure); |

| QueryMultiple                                                                                                        |
| :------------------------------------------------------------------------------------------------------------------- |
| ResultHelper<IEnumerable<IEnumerable\<dynamic>>> QueryMultiple<T, U>(T request, string storedProcedure);             |
| Task<ResultHelper<IEnumerable<IEnumerable\<dynamic>>>> QueryMultipleAsync<T, U>(T request, string storedProcedure);  |
___

# Cache Helper
It is an abstraction layer that provide a method with two overloads to add cache in any place. 

### Definition
#### Methods
| GetOrCreate                                             |
| :------------------------------------------------------ |
| T GetOrCreate(object key, T item);                      |
| Task<T> GetOrCreate(object key, Func<Task<T>> action);  |


### Examples of usage
```
string message = SimpleMemoryCache<string>.GetOrCreate("message", "Hello Essential Layers!");

Debug.WriteLine(message);

```
```
IList<City> cities = SimpleMemoryCache<City>.GetOrCreate("cities", async() => GetCitiesAsync());

Debug.WriteLine(cities);

```
