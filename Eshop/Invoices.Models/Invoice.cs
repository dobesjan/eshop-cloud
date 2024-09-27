namespace Invoices.Models
{

	// TODO: Add rest of fields
	public class Invoice
	{
		public int InvoiceNumber { get; set; }
		public DateTime IssueDate { get; set; }
		public string Seller { get; set; }
		public string Buyer { get; set; }
		public List<InvoiceItem> Items { get; set; }
		public double TotalAmount { get; set; }
	}
}
