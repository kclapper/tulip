using Tulip.Data;
using Tulip.Models;
using Tulip.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
//using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Tulip.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITasksServices _tasksServices;
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private string _badge { get; set; }
        public string _caseStudy { get; set; }

        public HomeController(ILogger<HomeController> logger, ITasksServices tasksServices, HttpClient client,
            IConfiguration configuration, ApplicationDbContext db)
        {
            _tasksServices = tasksServices;
            _client = client;
            _configuration = configuration;
            _db = db;
            //_caseStudy = caseStudy;
            _logger = logger;
        }
        /*
                public String GetUrl(int clientId, String userId, String applicationServer)
                {
                    _caseStudy = "MM";
                    return $"https://{applicationServer.Trim()}/sap/opu/odata/sap/ZUCC_GBM_GM_SRV/MM_FSet(Id=2,User='{userId.ToUpper().Trim()}')?$format=json&sap-client={clientId}";
                    //return $"https://{applicationServer.Trim()}/sap/opu/odata/sap/ZUCC_GBM_SRV/MM_FSet(Id=2,User='{userId.ToUpper().Trim()}')?$format=json&sap-client={clientId}";
                }
                */
        public String GetUrl(int clientId = 111, string userId = "LEARN-30", string applicationServer = "e45z.4.ucc.md", string caseStudy = "MM")
        {
            _logger.LogInformation($"GetUrl(clientId = {clientId}, userId={userId}, applicationServer={applicationServer}, caseStudy={caseStudy})");
            _caseStudy = caseStudy;
            return $"http://{applicationServer.Trim()}/sap/opu/odata/sap/ZUCC_GBM_GM_SRV/{caseStudy}_FSet(Id=2,User='{userId.ToUpper().Trim()}')?$format=json&sap-client={clientId}";
            //return $"https://{applicationServer.Trim()}/sap/opu/odata/sap/ZUCC_GBM_SRV/MM_FSet(Id=2,User='{userId.ToUpper().Trim()}')?$format=json&sap-client={clientId}";
        }

        public ActionResult Index()
        {
            return View();
        }

        private ApplicationUser getCurrentUser()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _db.ApplicationUsers.Find(userId);
        }

        public async Task<ActionResult> Dashboard(string caseStudy = "MM")
        {
            try
            {
                var userInfo = getCurrentUser();

                _logger.LogInformation($"Dashboard for user: {userInfo.UserName}\n" 
                    + $"\tUserId: {userInfo.UserId}\n"
                    + $"\tClientId: {userInfo.ClientId}\n"
                    + $"\tApplicationServer: {userInfo.ApplicationServer}\n"
                    + $"\tcaseStudy: {caseStudy}\n"
                );

                // TODO: Hardcoded credentials have to go.
                var userName = "TEACH-003";
                var passwd = "Naqiya99";

                var url = GetUrl(userInfo.ClientId, userInfo.UserId, userInfo.ApplicationServer, caseStudy);

                _logger.LogInformation($"SAP URL: {url}");

                // Use this handler to allow untrusted SSL Certificates
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, certChain, policyErrors) =>
                    {
                        return true;
                    };

                using var client = new HttpClient(handler);

                var authToken = Encoding.ASCII.GetBytes($"{userName}:{passwd}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(authToken));

                var result = await client.GetAsync(url);

                _logger.LogInformation($"SAP query result: {result}");

                var content = await result.Content.ReadAsStringAsync();

                var dictionaryList = new List<DictionaryModel>();
                var jObject = JObject.Parse(content);
                var jToken = jObject["d"];
        /*  Logic change on calculating StepsCount progress, 
            if atleast one step is accomplished from 
            StepCount out of total Steps, then ViewBag.StepCount on Dashboard
            Score GrowthChart and Congratulation Card text.
            will consider it as 100% 
        */
                var PointsDictionary = new Dictionary<string, int>()
        {
            {"@08@10", 10},
            {"@09@7", 8},
            {"@0A@5", 7 },
            {"@0A@", 7 },
            {"0", 0}
        };

                // Initialize fulfill to "0" by default
                string fulfill = "0";

                // Dictionary to track whether at least one step has been completed for each StepsCount
                Dictionary<string, bool> stepsCompletedDictionary = new Dictionary<string, bool>();

                foreach (var property in jToken.Children<JProperty>())
                {
                    var dictionaryModel = new DictionaryModel
                    {
                        Key = property.Name,
                        Value = property.Value.ToString()
                    };
                    dictionaryList.Add(dictionaryModel);
                }

                // Get points for that specific learn id
                int point = 0;
                int level = 0;
                string badge = "";
                List<int> Points = new List<int>();
                List<string> Steps = new List<string>();
                foreach (var item in dictionaryList)
                {
                    if (item.Key.Contains("Step"))
                    {
                        Points.Add(PointsDictionary[item.Value]);
                        Steps.Add(item.Key);

                        // Check if the step value is not "0"
                        if (item.Value != "0")
                        {
                            fulfill = "100"; //if step value not 0, update progress on congrats card as 100%
                            stepsCompletedDictionary[item.Key] = true;
                        }
                        else
                        {
                            stepsCompletedDictionary[item.Key] = false;
                        }
                    }

                    if (item.Key.Equals("FulfillmentAll"))
                    {
                        fulfill = item.Value;
                    }

                    if (item.Key.Equals("Points"))
                    {
                        point = int.Parse(item.Value);
                    }
                    if (item.Key.Equals("Level"))
                    {
                        level = int.Parse(item.Value);
                    }
                    if (item.Key.Equals("Badge"))
                    {
                        badge = item.Value;
                    }
                }

                // Check if at least one step has been completed for each StepsCount
                if (stepsCompletedDictionary.All(entry => entry.Value))
                {
                    fulfill = "100";
                }

                //ful, remove "%" if exists in the string.
                string aa = fulfill.Contains("%") ? fulfill.Remove(fulfill.IndexOf('%')) : fulfill;
                ViewBag.Fulfillment = int.Parse(aa);

                ViewBag.Point = point;
                ViewBag.Levels = level;
                ViewBag.Badge = badge;
                _badge = badge;
                ViewBag.PointsList = Points;
                ViewBag.StepsList = Steps;
                ViewBag.StepsCount = Steps.Count;

                var existingRecord = _db.LeaderBoaders.SingleOrDefault(m => m.Username == userInfo.UserId && m.CaseStudy == _caseStudy);

                if (existingRecord != null)
                {
                    // if record exists, update the points
                    existingRecord.Point = point;
                    _db.SaveChanges();
                }
                else
                {
                    // If the record doesn't exist, create it
                    var records = new LeaderBoader()
                    {
                        CaseStudy = _caseStudy,
                        Username = userInfo.UserId,
                        Point = point
                    };

                    // add the record to the database
                    _db.LeaderBoaders.Add(records);
                    _db.SaveChanges();
                }

                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ActionResult> CaseStudy(string caseStudy)
        {
            try
            {
                var userInfo = new ApplicationUser();

                var userList = _db.ApplicationUsers.ToList().Where(q => q.UserId == HttpContext.User.Identity.Name.ToUpper());
                foreach (var data in userList)
                {
                    userInfo = new ApplicationUser()
                    {
                        ApplicationServer = data.ApplicationServer,
                        ClientId = data.ClientId,
                        UserId = data.UserId
                    };
                }
                var userName = "TEACH-003";
                var passwd = "Naqiya99";

                var url = GetUrl(userInfo.ClientId, userInfo.UserId, userInfo.ApplicationServer, caseStudy);

                // use this handler to allow untrusted SSL Certificates
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };

                using var client = new HttpClient(handler);

                var authToken = Encoding.ASCII.GetBytes($"{userName}:{passwd}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(authToken));

                var result = await client.GetAsync(url);
                var content = await result.Content.ReadAsStringAsync();

                var dictionaryList = new List<DictionaryModel>();
                var jObject = JObject.Parse(content);
                var jToken = jObject["d"];


                var PiontsDictionary = new Dictionary<string, int>()
            {
                {"@08@10", 10},
                {"@09@7", 8},
                {"@0A@5", 7 },
                {"@0A@", 7 },
                {"0", 0}
            };

                foreach (var property in jToken.Children<JProperty>())
                {
                    var dictionaryModel = new DictionaryModel
                    {
                        Key = property.Name,
                        Value = property.Value.ToString()
                    };
                    dictionaryList.Add(dictionaryModel);
                }

                // Get points for that specific learn id
                int point = 0;
                int level = 0;
                string fulfill = "";
                string badge = "";
                List<int> Points = new List<int>();
                List<string> Steps = new List<string>();
                foreach (var item in dictionaryList)
                {
                    if (item.Key.Contains("Step"))
                    {
                        Points.Add(PiontsDictionary[item.Value]);
                        Steps.Add(item.Key);
                    }

                    if (item.Key.Equals("FulfillmentAll"))
                    {
                        fulfill = item.Value;
                    }

                    if (item.Key.Equals("Points"))
                    {
                        point = int.Parse(item.Value);
                    }
                    if (item.Key.Equals("Level"))
                    {
                        level = int.Parse(item.Value);
                    }
                    if (item.Key.Equals("Badge"))
                    {
                        badge = item.Value;
                    }
                }

                // ful
                string aa = fulfill.Remove(fulfill.IndexOf('%'));
                ViewBag.Fulfillment = int.Parse(aa);
                //ViewBag.Point = Points.Sum();
                ViewBag.Point = point;
                ViewBag.Levels = level;
                ViewBag.PointsList = Points;
                ViewBag.StepsList = Steps;
                ViewBag.StepsCount = Steps.Count;
                ViewBag.Badge = badge;

                var existingRecord = _db.LeaderBoaders.SingleOrDefault(m => m.Username == userInfo.UserId && m.CaseStudy == _caseStudy);

                if (existingRecord != null)
                {
                    // if record exists, update the points
                    existingRecord.Point = point;
                    _db.SaveChanges();
                }
                else
                {
                    // If the record doesn't exist, add a new record to the database
                    var records = new LeaderBoader()
                    {
                        CaseStudy = _caseStudy,
                        Username = userInfo.UserId,
                        Point = point
                    };

                    _db.LeaderBoaders.Add(records);
                    _db.SaveChanges();
                }
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public static List<DictionaryModel> ParseJson(string jsonString)
        {
            var dictionaryList = new List<DictionaryModel>();
            var jObject = JObject.Parse(jsonString);
            var jToken = jObject["d"];

            foreach (var property in jToken.Children<JProperty>())
            {
                var dictionaryModel = new DictionaryModel
                {
                    Key = property.Name,
                    Value = property.Value.ToString()
                };
                dictionaryList.Add(dictionaryModel);
            }

            return dictionaryList;
        }
        public class DictionaryModel
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
        /*
                [HttpPost]
                public async Task<IActionResult> Index(string username)
                {

                    var p = 0;
                    var b = 0;
                    var c = 0;

                    var points = 0;
                    var data = await _tasksServices.GetResponsePoint(username);
                    foreach (var item in data)
                    {
                        points += item.Score;
                    }
                    if (points > 30)
                    {
                        p = points;
                        b = 2;
                        c = 1;
                    }

                    else if (points >= 15 && points < 30)
                    {
                        p = points;
                        b = 1;
                    }
                    else
                    {
                        p = points;
                    }

                    var dataa = new
                    {
                        p,
                        b,
                        c
                    };

                    return Ok(dataa);
                }
        */
        public async Task<IActionResult> LeaderBoard(string caseStudy = "MM")
        {
            
            switch (caseStudy)
            {
                case "FI":
                    ViewBag.Header = "FI - Accounts Payable";
                    break;
                case "FI_AR":
                    ViewBag.Header = "FI - Accounts Receivable";
                    break;
                case "MM":
                    ViewBag.Header = "Material Management";
                    break;
                case "SD":
                    ViewBag.Header = "Sales and Distribution";
                    break;
                case "PP":
                    ViewBag.Header = "Production Planning";
                    break;
                default:
                    break;
            }


            var data = await _tasksServices.GetLeaders(caseStudy);
            return View(data);
        }

        public async Task<IActionResult> Tasks()
        {
            var data = await _tasksServices.GetTasks();
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> TaskResponse(TasksResponse taskResponse)
        {
            var data = await _tasksServices.CreateResponse(taskResponse);
            //await Index(taskResponse.RespondantName);
            return Ok(data);
        }

        public IActionResult TaskResponse()
        {

            return View();
        }

        /**
        @brief Retrieves and displays badges for a specified case study.*
        This method takes a case study identifier as a parameter and retrieves corresponding badge data.
        The retrieved data is used to generate a list of badges, and the result is passed to the view for display.*
        @param caseStudy The case study identifier (default is "MM").*
        @return Task<IActionResult> representing the result of the operation, displaying the badges view.
        */

        public async Task<IActionResult> Badges(string caseStudy = "MM")
        {
            switch (caseStudy)
            {
                case "FI":
                    ViewBag.Header = "FI - Accounts Payable";
                    break;
                case "FI_AR":
                    ViewBag.Header = "FI - Accounts Receivable";
                    break;
                case "MM":
                    ViewBag.Header = "Material Management";
                    break;
                case "SD":
                    ViewBag.Header = "Sales and Distribution";
                    break;
                case "PP":
                    ViewBag.Header = "Production Planning";
                    break;
                default:
                    break;
            }
            var data = await getBatch(caseStudy);
            var badge = new List<Badges>();
            switch (caseStudy)
            {
                case "PP":
                    if (data.Contains("Login"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                    }
                    else if (data.Contains("Master"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Master"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                    }
                    else if (data.Contains("SOP"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Master"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "SOP"
                        });
                    }
                    else if (data.Contains("MRP"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Master"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "SOP"
                        });
                        badge.Add(new Badges() { Badge = "MRP" });
                    }
                    else if (data.Contains("Production"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Master"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "SOP"
                        });
                        badge.Add(new Badges() { Badge = "MRP" });
                        badge.Add(new Badges() { Badge = "Production" });
                    }
                    break;

                case "SD":
                    if (data.Contains("Login"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                    }
                    else if (data.Contains("Master"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Master"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                    }
                    else if (data.Contains("Quotation"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Master"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Quotations"
                        });
                    }
                    else if (data.Contains("Sales"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Master"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Quotations"
                        });
                        badge.Add(new Badges() { Badge = "Sales" });
                    }
                    else if (data.Contains("Biling"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Master"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Quotations"
                        });
                        badge.Add(new Badges() { Badge = "Sales" });
                        badge.Add(new Badges() { Badge = "Biling" });
                    }
                    break;
                case "FI_AR":
                    if (data.Contains("Login"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                    }
                    else if (data.Contains("Master"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Master"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                    }
                    else if (data.Contains("Invoice"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Master"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Invoice"
                        });
                    }
                    else if (data.Contains("Payment"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Master"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Invoice"
                        });
                        badge.Add(new Badges() { Badge = "Payment" });
                    }
                    else if (data.Contains("Scenario"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Master"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Invoice"
                        });
                        badge.Add(new Badges() { Badge = "Payment" });
                        badge.Add(new Badges() { Badge = "Scenario" });
                    }
                    break;
                case "FI":
                    if (data.Contains("Login"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                    }
                    else if (data.Contains("Master"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Master"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                    }
                    else if (data.Contains("Funds"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Master"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Funds"
                        });
                    }
                    else if (data.Contains("Invoice"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Master"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Funds"
                        });
                        badge.Add(new Badges() { Badge = "Invoice" });
                    }
                    else if (data.Contains("Payment"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Master"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Funds"
                        });
                        badge.Add(new Badges() { Badge = "Invoice" });
                        badge.Add(new Badges() { Badge = "Payment" });
                    }
                    break;
                default:
                    if (data.Contains("Login"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                    }
                    else if (data.Contains("Master"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Master"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                    }
                    else if (data.Contains("RFQ"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Master"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "RFQ"
                        });
                    }
                    else if (data.Contains("PO"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Master"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "RFQ"
                        });
                        badge.Add(new Badges() { Badge = "PO" });
                    }
                    else if (data.Contains("FI"))
                    {
                        badge.Add(new Badges()
                        {
                            Badge = "Master"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "Login"
                        });
                        badge.Add(new Badges()
                        {
                            Badge = "RFQ"
                        });
                        badge.Add(new Badges() { Badge = "PO" });
                        badge.Add(new Badges() { Badge = "FI" });
                    }
                    break;
            }

            return View(badge);
        }


        public async Task<string> getBatch(string caseStudy)
        {
            try
            {
                var userInfo = getCurrentUser();

                var userName = "TEACH-003";
                var passwd = "Naqiya99";

                var url = GetUrl(userInfo.ClientId, userInfo.UserId, userInfo.ApplicationServer, caseStudy);

                // use this handler to allow untrusted SSL Certificates
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };

                using var client = new HttpClient(handler);

                var authToken = Encoding.ASCII.GetBytes($"{userName}:{passwd}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(authToken));

                var result = await client.GetAsync(url);
                var content = await result.Content.ReadAsStringAsync();


                var dictionaryList = new List<DictionaryModel>();
                var jObject = JObject.Parse(content);
                var jToken = jObject["d"];


                var PiontsDictionary = new Dictionary<string, int>()
            {
                {"@08@10", 10},
                {"@09@7", 8},
                {"@0A@5", 7 },
                {"@0A@", 7 },
                {"0", 0}
            };

                foreach (var property in jToken.Children<JProperty>())
                {
                    var dictionaryModel = new DictionaryModel
                    {
                        Key = property.Name,
                        Value = property.Value.ToString()
                    };
                    dictionaryList.Add(dictionaryModel);
                }

                string badge = "";
                foreach (var item in dictionaryList)
                {

                    if (item.Key.Equals("Badge"))
                    {
                        badge = item.Value;
                    }
                }
                _badge = badge;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return _badge;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
