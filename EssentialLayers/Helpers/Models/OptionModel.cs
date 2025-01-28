namespace EssentialLayers.Helpers.Models
{
	public class OptionModel
	{
		public string Title { get; set; } = string.Empty;

		public string Value { get; set; } = string.Empty;

		/**/

		public OptionModel() { }

		public OptionModel(string title, string value)
		{
			Title = title;
			Value = value;
		}

		public static OptionModel Default => new(string.Empty, string.Empty);
	}
}