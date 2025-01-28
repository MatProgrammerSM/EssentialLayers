using AutoMapper;
using AutoMapper.Configuration;
using System.Collections.Generic;

namespace EssentialLayers.Services
{
	internal class MapperService(IMapper mapper) : IMapperService
	{
		private readonly IMapper Mapper = mapper;

		private readonly HashSet<MappingExpression<object, object>> MappingExpressions;

		/**/

		public void Add<Source, Destination>(Source source, Destination destination)
		{
			MappingExpressions.Add(null);
		}

		public Result Map<Source, Result>(
			Source source
		)
		{
			return Mapper.Map<Source, Result>(source);
		}

		public IList<Destination> MapList<Source, Destination>(
			IEnumerable<Source> source
		)
		{
			return Mapper.Map<IEnumerable<Source>, IList<Destination>>(source);
		}

		public Destination MapToExisting<Source, Destination>(
			Source source, Destination destination
		)
		{
			return Mapper.Map(source, destination);
		}

		public IList<Destination> MapListToExisting<Source, Destination>(
			IEnumerable<Source> source, IList<Destination> destination
		)
		{
			return Mapper.Map(source, destination);
		}
	}
}