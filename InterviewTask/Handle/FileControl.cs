using CsvHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InterviewTask.Support;
using System.Text.RegularExpressions;
using InterviewTask.Route;

namespace InterviewTask.Handle
{
    public static class FileControl
    {
        internal static bool isFileExtensionValid(IFormFile file, out List<string> errors)
        {
            errors = new List<string>();
            if (file == null)
            {
                errors.Add("Please provide a file to load");
                return false;
            }
            if (!file.FileName.EndsWith(".csv"))
            {
                errors.Add("Please provide a CSV file");
                return false;
            }
            return true;
        }

        internal static string Save(IFormFile formFile, string v)
        {
            if (formFile == null)
                throw new ArgumentNullException("The file is missing");

            if (string.IsNullOrEmpty(v))
                throw new ArgumentNullException("The file path is required for validating file content");

            var dirs = v.Split("/");
            var currentDir = Directory.GetCurrentDirectory();
            foreach (var dir in dirs)
            {
                currentDir = Path.Combine(currentDir, dir);
                DirectoryInfo dirInfo = Directory.CreateDirectory(currentDir);
            }
            currentDir = Path.Combine(currentDir, DateTime.Now.ToString("yyyyMMdd_HHmmss") + "Z" + $"_{formFile.FileName}");
            using (var fs = File.Create(currentDir))
            {
                formFile.CopyTo(fs);
            }
            return currentDir;

        }

        internal static IEnumerable<ConsignmentInformation> FileFieldsValidation(string filePath, out List<string> allErrors)
        {
            using (var reader = new StreamReader(filePath))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var returnRecords = new List<ConsignmentInformation>();
                    allErrors = new List<string>();
                    csv.Context.RegisterClassMap<CsvRoute>();
                    var records = csv.GetRecords<ConsignmentInformation>();
                    var fieldsToPrint = new List<ConsignmentInformation>();
                    var rowCounter = 1;
                    try
                    {
                        foreach (var record in records)
                        {
                            var errors = new List<string>();
                            var errorsPerRow = new List<string>();
                            if (!ValidateControl.isOrderNumberValid(record.OrderNumber, out errors))
                                errorsPerRow.AddRange(errors);
                            if (!ValidateControl.isConsignmentNumberValid(record.ConsignmentNumber, out errors))
                                errorsPerRow.AddRange(errors);
                            if (!ValidateControl.isParcelCodeValid(record.ParcelCode, out errors))
                                errorsPerRow.AddRange(errors);
                            if (!ValidateControl.isConsigneeNameValid(record.ConsigneeName, out errors))
                                errorsPerRow.AddRange(errors);
                            if (!ValidateControl.isAddressValid(record.AddressOne, out errors))
                                errorsPerRow.AddRange(errors);
                            if (!ValidateControl.isAddressValid(record.AddressTwo, out errors))
                                errorsPerRow.AddRange(errors);
                            if (!ValidateControl.isCountryCodeValid(record.CountryCode, out errors))
                                errorsPerRow.AddRange(errors);
                            if (!ValidateControl.isItemQuantityValid(record.ItemQuantity, out errors))
                                errorsPerRow.AddRange(errors);
                            if (!ValidateControl.isItemValueValid(record.ItemValue, out errors))
                                errorsPerRow.AddRange(errors);
                            if (!ValidateControl.isItemWeightValid(record.ItemWeight, out errors))
                                errorsPerRow.AddRange(errors);
                            if (!ValidateControl.isItemDescriptionValid(record.ItemDesciption, out errors))
                                errorsPerRow.AddRange(errors);
                            if (errorsPerRow.Count > 0)
                            {
                                errorsPerRow.Insert(0, $"{errorsPerRow.Count} errors found {rowCounter}");
                                allErrors.AddRange(errorsPerRow);
                            }
                            else
                            {
                                fieldsToPrint.Add(record);
                            }
                            rowCounter++;
                        }
                    }
                    catch (CsvHelper.MissingFieldException ex)
                    {
                        var regex = new Regex(@"'([0-9])'");
                        var match = regex.Match(ex.Message);
                        var index = match.Groups[1].ToString();
                        var intIndex = Convert.ToInt32(index);
                        allErrors.Add($"Error: Missing column {++intIndex} for row {rowCounter}");
                    }
                    if (allErrors.Count == 0)
                    {
                        returnRecords = fieldsToPrint;
                    }
                    return returnRecords;
                }
            }
        }
    }
}
