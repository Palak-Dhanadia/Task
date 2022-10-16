using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InterviewTask.Handle
{
    internal static class ValidateControl
    {
        internal static bool isOrderNumberValid(string orderNumber, out List<string> Errors)
        {
            Errors = new List<string>();
            var regex = new Regex(@"^ORD.\d+$");
            if (string.IsNullOrEmpty(orderNumber))
                Errors.Add($"Order Number is empty");
            if (!regex.IsMatch(orderNumber))
                Errors.Add($"Order Number is not valid. It should begin with 'ORD' and end with numbers");
            if (Errors.Count > 0)
                return false;
            return true;
        }

        internal static bool isConsignmentNumberValid(string consignmentNumber, out List<string> Errors)
        {
            Errors = new List<string>();
            var regex = new Regex(@"^CON.\d+$");
            if (string.IsNullOrEmpty(consignmentNumber))
                Errors.Add($"Consignment Number is missing");
            if (!regex.IsMatch(consignmentNumber))
                Errors.Add($"Consignment Number is not valid. It should begin with 'CON' and end with numbers");
            if (Errors.Count > 0)
                return false;
            return true;
        }

        internal static bool isParcelCodeValid(string parcelCode, out List<string> Errors)
        {
            Errors = new List<string>();
            var regex = new Regex(@"^PARC.\d+$");
            if (string.IsNullOrEmpty(parcelCode))
                Errors.Add($"Parcel Code is missing");
            if (!regex.IsMatch(parcelCode))
                Errors.Add($"Parcel Code is not valid. It should begin with 'PARC' and end with numbers");
            if (Errors.Count > 0)
                return false;
            return true;
        }

        internal static bool isConsigneeNameValid(string consigneeName, out List<string> Errors)
        {
            Errors = new List<string>();
            if (string.IsNullOrEmpty(consigneeName))
                Errors.Add($"Consignee Name is missing");
            if (Errors.Count > 0)
                return false;
            return true;
        }

        internal static bool isAddressValid(string address, out List<string> Errors)
        {
            Errors = new List<string>();
            if (string.IsNullOrEmpty(address))
                Errors.Add($"Address One is missing");
            if (Errors.Count > 0)
                return false;
            return true;
        }

        internal static bool isCountryCodeValid(string countryCode, out List<string> Errors)
        {
            Errors = new List<string>();
            if (string.IsNullOrEmpty(countryCode))
                Errors.Add($"Country Code is missing");
            if (Errors.Count > 0)
                return false;
            return true;
        }

        internal static bool isItemQuantityValid(string itemQuantity, out List<string> Errors)
        {
            Errors = new List<string>();
            var regex = new Regex(@"\d+");
            if (!regex.IsMatch(itemQuantity))
                Errors.Add($"Item Quantity need to be a number");
            if (Errors.Count > 0)
                return false;
            return true;
        }

        internal static bool isItemValueValid(string itemValue, out List<string> Errors)
        {
            Errors = new List<string>();
            var regex = new Regex(@"^\d*\.?\d*$");
            if (!regex.IsMatch(itemValue))
                Errors.Add($"Item Value need to be a number");
            if (Errors.Count > 0)
                return false;
            return true;
        }

        internal static bool isItemWeightValid(string itemWeight, out List<string> Errors)
        {
            Errors = new List<string>();
            var regex = new Regex(@"^\d*\.?\d*$");
            if (!regex.IsMatch(itemWeight))
                Errors.Add($"Item Weight need to be a number");
            if (Errors.Count > 0)
                return false;
            return true;
        }

        internal static bool isItemDescriptionValid(string itemDescription, out List<string> Errors)
        {
            Errors = new List<string>();
            if (string.IsNullOrEmpty(itemDescription))
                Errors.Add($"Item Description required");
            if (Errors.Count > 0)
                return false;
            return true;
        }
    }
}
