using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using InterviewTask.Support;
using InterviewTask.Handle;
using InterviewTask.Models;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using InterviewTask.Support;

namespace InterviewTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Import(Import model)
        {
            if (model.FormFile == null || !ModelState.IsValid)
            {
                return View("Error");
            }

            var filePath = FileControl.Save(model.FormFile, "/Loads/CSV");

            var isValidExtension = FileControl.isFileExtensionValid(model.FormFile, out var errors);
            if (!isValidExtension)
            {
                foreach (var error in errors)
                    ModelState.AddModelError("", error);
                System.IO.File.Delete(filePath);
                return View(model);
            }

            var fieldsToPrint = FileControl.FileFieldsValidation(filePath, out errors);
            if (errors.Count > 0)
            {
                foreach (var error in errors)
                    ModelState.AddModelError("", error);
                System.IO.File.Delete(filePath);
                return View(model);
            }
            model.Records = fieldsToPrint;
            return View("Index", model);
        }

        [HttpPost]
        public IActionResult ExportXML(Import model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return RedirectToAction("Error");
            }

            var directory = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Loads\\csv\\");
            var filePath = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First().ToString();
            var source = System.IO.File.ReadAllLines(filePath).ToList();
            XElement ord = new XElement("Root",
                from str in source
                let fields = str.Split(',')
                group str by fields[0] into groupedOrders
                select new XElement("Orders", new XAttribute("OrderID", groupedOrders.Key))
            );
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SendOrder([FromBody] IEnumerable<ConsignmentInformation> consignmentInformation)
        {
            if (consignmentInformation.Count() <= 0 || !ModelState.IsValid)
            {
                return RedirectToAction("Error");
            }
            var httpClient = new HttpClient();
            var baseUrl = "https://devaccountsnv.serveftp.net:65001/api";
            var apiHandler = new CallControl(consignmentInformation, httpClient, baseUrl);
            var token = await apiHandler.GetToken("/Jwt");
            var responseIds = await apiHandler.PostOrder(token, "/PurchaseOrder/Save");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
