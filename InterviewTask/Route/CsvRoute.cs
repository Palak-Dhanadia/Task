using CsvHelper.Configuration;
using System.Globalization;
using InterviewTask.Support;

namespace InterviewTask.Route
{
    public class CsvRoute : ClassMap<ConsignmentInformation>
    {
        public CsvRoute()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.OrderNumber).Name("Order No", "Order Number", "Order Id"); 
            Map(m => m.ConsignmentNumber).Name("Consignment No", "Consignment Number", "Consignment Id");
            Map(m => m.ParcelCode).Name("Parcel Code", "ParcelCode", "Parcel Id"); 
            Map(m => m.ConsigneeName).Name("Consignee Name", "ConsigneeName");
            Map(m => m.AddressOne).Name("Address 1", "AddressOne", "Address One");
            Map(m => m.AddressTwo).Name("Address 2", "AddressTwo", "Address Two"); 
            Map(m => m.City).Name("City"); 
            Map(m => m.CountryCode).Name("Country Code", "CountryCode"); 
            Map(m => m.ItemQuantity).Name("Item Quantity", "ItemQuantity");
            Map(m => m.ItemValue).Name("Item Value", "ItemValue"); 
            Map(m => m.ItemWeight).Name("Item Weight", "ItemWeight"); 
            Map(m => m.ItemDesciption).Name("Item Description", "ItemDescription"); 
            Map(m => m.ItemCurrency).Name("Item Currency", "ItemCurrency"); 
        }
    }
}
