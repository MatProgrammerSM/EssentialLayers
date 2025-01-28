using System.Collections.Generic;

namespace EssentialLayers.Services
{
	public interface IMapperService
	{
		

		Result Map<Source, Result>(
			Source source
		);

		IList<Destination> MapList<Source, Destination>(
			IEnumerable<Source> source
		);

		Destination MapToExisting<Source, Destination>(
			Source source, Destination destination
		);

		IList<Destination> MapListToExisting<Source, Destination>(
			IEnumerable<Source> source, IList<Destination> destination
		);
	}
}