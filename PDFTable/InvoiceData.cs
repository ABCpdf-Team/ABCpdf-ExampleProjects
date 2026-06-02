// ===========================================================================
//	©2013-2025 WebSupergoo. All rights reserved.
//
//	This source code is for use exclusively with the ABCpdf product with
//	which it is distributed, under the terms of the license for that
//	product. Details can be found at
//
//		http://www.websupergoo.com/
//
//	This copyright notice must not be deleted and must be reproduced alongside
//	any sections of code extracted from this module.
// ===========================================================================

using System;
using System.Xml;
using System.Globalization;


namespace PDFTableExamples {

	/// <summary>
	/// InvoiceData class is used to represent invoice information stored in xml file
	/// </summary>
	public class InvoiceData	{
		public InvoiceData()	{
		}
		
		/// <summary>
		/// Load invoice data from xml file
		/// </summary>
		/// <param name="inFile">Xml file path</param>
		public void LoadFromFile(string inFile)	{
			try {
				XmlTextReader xmlReader = new XmlTextReader(inFile);
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(xmlReader);

				XmlNode invoice = xmlDoc["Invoice"];

				XmlNode companyInfo = invoice["CompanyInfo"];
				if (companyInfo.Attributes["Name"] != null)
					Company.Name = companyInfo.Attributes["Name"].Value;
				if (companyInfo.Attributes["Address"] != null)
					Company.Address = companyInfo.Attributes["Address"].Value;
				if (companyInfo.Attributes["State"] != null)
					Company.State = companyInfo.Attributes["State"].Value;
				if (companyInfo.Attributes["Phone"] != null)
					Company.Phone = companyInfo.Attributes["Phone"].Value;
				if (companyInfo.Attributes["Zip"] != null)
					Company.Zip = companyInfo.Attributes["Zip"].Value;
				if (companyInfo.Attributes["City"] != null)
					Company.City  = companyInfo.Attributes["City"].Value;
				if (companyInfo.Attributes["logo"] != null)
					Company.logo = companyInfo.Attributes["logo"].Value;

				XmlNode customerInfo = invoice["Customer"];
				if (customerInfo.Attributes["Name"] != null)
					Customer.Name = customerInfo.Attributes["Name"].Value;
				if (customerInfo.Attributes["Address"] != null)
					Customer.Address = customerInfo.Attributes["Address"].Value;
				if (customerInfo.Attributes["State"] != null)
					Customer.State = customerInfo.Attributes["State"].Value;
				if (customerInfo.Attributes["Phone"] != null)
					Customer.Phone = customerInfo.Attributes["Phone"].Value;
				if (customerInfo.Attributes["Zip"] != null)
					Customer.Zip = customerInfo.Attributes["Zip"].Value;
				if (customerInfo.Attributes["City"] != null)
					Customer.City = customerInfo.Attributes["City"].Value;


				XmlNode paymentDetails = invoice["PaymentDetails"];

				if (paymentDetails != null)	{
					if (paymentDetails.Attributes["Payment"] != null)
						PaymentDetails.PaymentType = paymentDetails.Attributes["Payment"].Value;
					if (paymentDetails.Attributes["Name"] != null)
						PaymentDetails.Name = paymentDetails.Attributes["Name"].Value;
					if (paymentDetails.Attributes["CC"] != null)
						PaymentDetails.CC = paymentDetails.Attributes["CC"].Value;
					if (paymentDetails.Attributes["Expires"] != null)
						PaymentDetails.Expires = paymentDetails.Attributes["Expires"].Value;
				}

				XmlNode order = invoice["Order"];

				CultureInfo cultureInfo = new CultureInfo("en-US", false);
				NumberFormatInfo formatInfo = cultureInfo.NumberFormat;
				formatInfo.NumberDecimalDigits = 2;

				double taxRate = 0;
				double shippingCharge = 0;

				if (order != null) {
					if (order.Attributes["Date"] != null)
						OrderInfo.Date = order.Attributes["Date"].Value;
					if (order.Attributes["OrderNo"] != null)
						OrderInfo.OrderNo = order.Attributes["OrderNo"].Value;
					if (order.Attributes["FOB"] != null)
						OrderInfo.FOB = order.Attributes["FOB"].Value;
					if (order.Attributes["Rep"] != null)
						OrderInfo.Rep = order.Attributes["Rep"].Value;
					if (order.Attributes["TaxRate"] != null)
					{
						taxRate = Double.Parse(order.Attributes["TaxRate"].Value, cultureInfo);
						OrderInfo.TaxRate = "<StyleRun hpos = 1> " + taxRate.ToString("N", formatInfo) + "</StyleRun>";;
					}
					if (order.Attributes["ShippingCharge"] != null)
					{
						shippingCharge = Double.Parse(order.Attributes["ShippingCharge"].Value, cultureInfo);
						OrderInfo.ShippingCharge = "<StyleRun hpos = 1> " + shippingCharge.ToString("N", formatInfo) + "</StyleRun>";;
					}

					OrderItems = new OrderItemData[order.ChildNodes.Count];

					double Total = 0;
					for ( int i = 0; i < order.ChildNodes.Count; i++) {
						XmlNode item = order.ChildNodes.Item(i);
						OrderItems[i] = new OrderItemData();

						double unitPrice = Double.Parse(item.Attributes["UnitPrice"].Value, cultureInfo);
						int qty = Int32.Parse(item.Attributes["Qty"].Value);

						if (item.Attributes["Desc"] != null)
							OrderItems[i].Description = item.Attributes["Desc"].Value;
						if (item.Attributes["Qty"] != null)
							OrderItems[i].Qty = qty.ToString();
						if (item.Attributes["UnitPrice"] != null)
							OrderItems[i].UnitPrice = "<StyleRun hpos = 1> " + unitPrice.ToString("N", formatInfo) + "</StyleRun>";

						double SubTotal = qty * unitPrice;
						OrderItems[i].SubTotal = "<StyleRun hpos = 1> " + SubTotal.ToString("N", formatInfo) + "</StyleRun>";
						Total += SubTotal;
					}
					OrderInfo.Subtotal = "<StyleRun hpos = 1> " + Total.ToString("N", formatInfo) + "</StyleRun>";

					Total = Total + Total* taxRate + shippingCharge;
					OrderInfo.Total = "<StyleRun hpos = 1> " + Total.ToString("N", formatInfo) + "</StyleRun>";
				}
			}
			catch {
				//Invalid xml file
			}
		}

		/// <summary>
		/// Company information
		/// </summary>
		public CompanyInfoData Company	{get {return mCompany;}	}
		/// <summary>
		/// Customer information
		/// </summary>
		public CompanyInfoData Customer { get {return mCustomer;}}
		/// <summary>
		/// Order information
		/// </summary>
		public OrderInfoData OrderInfo	 {get {return mOrderInfo;}}
		/// <summary>
		/// Payment details information
		/// </summary>
		public PaymentDetailsData PaymentDetails	 {get {return mPaymentDetails;}}

		/// <summary>
		/// Returns formatted data for the header table
		/// </summary>
		/// <returns>Array of strings to put into the table cells</returns>
		public string[] GetHeaderTableData() {
			string[] textInfo = new string[4]{ "<StyleRun fontsize = 50>" + Company.Name + " </StyleRun>",
												 Company.Address, Company.City + ", " + Company.State + ", " + Company.Zip,
												Company.Phone};
			return textInfo;
		}

		/// <summary>
		/// Returns formatted data for the customer table
		/// </summary>
		/// <returns>Array of strings to put into the table cells</returns>
		public string[][] GetCustomerTableData() 
		{
			string[][] textInfo = new string[2][];
			textInfo[0] = new string[4] {"Name", "Address", "", "Phone"};

			textInfo[1] = new string[4] {Customer.Name, Customer.Address,
										(Customer.City + ", " + Customer.State + ", " + Customer.Zip),
										Customer.Phone};
			return textInfo;

		}

		/// <summary>
		/// Returns formatted data for the order table
		/// </summary>
		/// <returns>Array of strings to put into the table cells</returns>
		public string[][] GetOrderTableData() 
		{
			string[][] textInfo = new string[2][];
			textInfo[0] = new string[4] {"Date", "Order No", "Rep", "FOB"};
			textInfo[1] = new string[4] {OrderInfo.Date, OrderInfo.OrderNo,
											OrderInfo.Rep, OrderInfo.FOB};
			return textInfo;
		}

		/// <summary>
		/// Returns formatted data for the order items table
		/// </summary>
		/// <returns>Array of strings to put into the table cells</returns>
		public string[][] GetOrderItemsTableData() 
		{
			string[][] textInfo = new string[4][];

			for (int i = 0; i < 4; i++) {
				textInfo[0] = new string[1+OrderItems.Length];
				textInfo[1] = new string[1+OrderItems.Length];
				textInfo[2] = new string[1+OrderItems.Length];
				textInfo[3] = new string[1+OrderItems.Length];
			}

			textInfo[0][0] = "Qty";
			textInfo[1][0] = "Description";
			textInfo[2][0] = "<StyleRun hpos = 1 > Unit Price </StyleRun>";
			textInfo[3][0] = "<StyleRun hpos = 1 > Total </StyleRun>";

			for (int i = 0; i < OrderItems.Length; i++)	{
				textInfo[0][i+1] = OrderItems[i].Qty;
				textInfo[1][i+1] = OrderItems[i].Description;
				textInfo[2][i+1] = OrderItems[i].UnitPrice;
				textInfo[3][i+1] = OrderItems[i].SubTotal;
			}

			return textInfo;
		}

		/// <summary>
		/// Returns formatted data for the payment table
		/// </summary>
		/// <returns>Array of strings to put into the table cells</returns>
		public string[][] GetPaymentTableData()	
		{
			string[][] textInfo = new string[2][];

			textInfo[0] = new string[4] { "Payment type", "Name", "CC#", "Expires"};
			textInfo[1] = new string[4] { PaymentDetails.PaymentType,
										PaymentDetails.Name,
										PaymentDetails.CC,
										PaymentDetails.Expires};
			return textInfo;

		}

		/// <summary>
		/// Returns formatted data for the total table
		/// </summary>
		/// <returns>Array of strings to put into the table cells</returns>
		public string[][] GetTotalTableData() 
		{
			string[][] textInfo = new string[2][];
			textInfo[0] = new string[4] { "SubTotal", "Shipping", "Taxes", "Total"};
			textInfo[1] = new string[4] { OrderInfo.Subtotal,
										OrderInfo.ShippingCharge,
										OrderInfo.TaxRate,
										OrderInfo.Total};
			return textInfo;

		}

		/// <summary>
		/// Represents Company information
		/// </summary>
		public class CompanyInfoData {
			public string Name = "";
			public string Address = "";
			public string City = "";
			public string State = "";
			public string Phone = "";
			public string Zip = "";
			public string logo = "";
		};
		/// <summary>
		/// Represents general Order information
		/// </summary>
		public class OrderInfoData	
		{
			public string Date = "";
			public string OrderNo = "";
			public string Rep = "";
			public string FOB = "";

			public string ShippingCharge = "0";
			public string TaxRate = "0";
			public string Total = "";
			public string Subtotal = "";
		};
		/// <summary>
		/// Describes Order Item
		/// </summary>
		public class OrderItemData	
		{
			public string Qty = "";
			public string Description = "";
			public string UnitPrice = "";
			public string SubTotal = "";
		};

		/// <summary>
		/// Represents Payment details
		/// </summary>
		public class PaymentDetailsData 
		{
			public string PaymentType = "";
			public string Name = "";
			public string CC = "";
			public string Expires = "";
		};

		private CompanyInfoData mCompany = new CompanyInfoData();
		private CompanyInfoData mCustomer = new CompanyInfoData();
		private OrderInfoData mOrderInfo = new OrderInfoData();
		private PaymentDetailsData mPaymentDetails = new PaymentDetailsData();

		public OrderItemData[] OrderItems;
	}
}
