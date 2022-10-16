using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterviewTask.Support;

namespace InterviewTask.Models
{
    public class Import
    {
        public IFormFile FormFile { get; set; }
        public IEnumerable<ConsignmentInformation> Records { get; set; }
    }
}
