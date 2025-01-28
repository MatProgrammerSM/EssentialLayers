using AutoMapper;
using System.Collections.Generic;

namespace EssentialLayers.Helpers.Mapper
{
	public class MapperHelper(IMapper mapper) : Profile
	{
		private readonly IMapper Mapper = mapper;

		/**/

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