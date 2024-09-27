namespace Invoices.Models
{
	public class Invoice
	{
		public string InvoiceNumber { get; set; }
		public DateTime IssueDate { get; set; }
		public string Seller { get; set; }
		public string Buyer { get; set; }
		public List<InvoiceItem> Items { get; set; }
		public decimal TotalAmount { get; set; }
	}
}
