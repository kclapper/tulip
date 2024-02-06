namespace Gamification.UI.Models
{
	public class SapModel
	{
	}

	public class Data
	{
		public Metadata Metadata { get; set; }
		public int Id { get; set; }
		public string User { get; set; }
		public string Step1 { get; set; }
		public string Step2 { get; set; }
		public string Step3 { get; set; }
		public string Step4 { get; set; }
		public string Step5 { get; set; }
		public string Step6 { get; set; }
		public string Step7 { get; set; }
		public string Step8 { get; set; }
		public string Step9 { get; set; }
		public string Step10 { get; set; }
		public string Step11 { get; set; }
		public string Step12 { get; set; }
		public string Step13 { get; set; }
		public string FulfillmentMandatory { get; set; }
		public string FulfillmentAll { get; set; }
		public int Point { get; set; }
		public int Badge { get; set; }
		public ToDetail ToDetail { get; set; }
	}

	public class Deferred
	{
		public string Uri { get; set; }
	}

	public class Metadata
	{
		public string Id { get; set; }
		public string Uri { get; set; }
		public string Type { get; set; }
	}

	public class SapViewModel
	{
		public Data Data { get; set; }
	}

	public class ToDetail
	{
		public Deferred Deferred { get; set; }
	}

}
