namespace EssentialLayers.Helpers.Models
{
	public class TOptionModel<T>
	{
		public string Title { get; set; } = string.Empty;

		public T Value { get; set; } = default;

		/**/

		public TOptionModel() { }

		public TOptionModel(string title, T value)
		{
			Title = title;
			Value = value;
		}

		public static TOptionModel<T> Default => new(string.Empty, default);
	}
}