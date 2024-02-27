using Tulip.Data;
using Tulip.Models;
using Tulip.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Tulip.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITasksServices _tasksServices;
        private readonly ApplicationDbContext _db;
        private string _badge { get; set; }
        public string _caseStudy { get; set; }
        private ISAPBuilder _sapBuilder;

        public HomeController(ILogger<HomeController> logger, ITasksServices tasksServices, 
             ApplicationDbContext db, ISAPBuilder sapBuilder)
        {
            _tasksServices = tasksServices;
            _db = db;
            _logger = logger;
            _sapBuilder = sapBuilder;
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
                    + $"\tcaseStudy: {caseStudy}"
                );

                ISAP sap = await _sapBuilder
                    .SetUsername(userInfo.UserId)
                    .SetClientId(userInfo.ClientId)
                    .SetApplicationServer(userInfo.ApplicationServer)
                    .SetCaseStudy(caseStudy)
                    .Build();


                ViewBag.Fulfillment = sap.GetFulfillment();

                ViewBag.Point = sap.GetPoint();
                ViewBag.Levels = sap.GetLevel();
                ViewBag.Badge = sap.GetBadge();
                _badge = sap.GetBadge();
                ViewBag.PointsList = sap.GetPointsList();
                ViewBag.StepsList = sap.GetStepsList();
                ViewBag.StepsCount = sap.GetStepsList().Count;
                
                var existingRecord = _db.LeaderBoaders.SingleOrDefault(m => m.Username == userInfo.UserId && m.CaseStudy == _caseStudy);
                var leaders = _db.LeaderBoaders.Where(lb => lb.CaseStudy == _caseStudy);
                var sorted = leaders.OrderByDescending(lb => lb.Point);
                ViewBag.TopThree = sorted.Take(3);
                ViewBag.LeaderTotal = sorted.Count();

                LeaderBoardText(_caseStudy);//get correct casestudy text for dash leaderboard
  
                if (existingRecord != null)
                {
                    // if record exists, update the points
                    existingRecord.Point = sap.GetPoint();
                    _db.SaveChanges();
                }
                else
                {
                    // If the record doesn't exist, create it
                    var records = new LeaderBoader()
                    {
                        CaseStudy = _caseStudy,
                        Username = userInfo.UserId,
                        Point = sap.GetPoint()
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
        private void LeaderBoardText(string caseStudy){
            switch (caseStudy)
            {
                case "FI":
                    ViewBag.LeaderBoardText = "FI - Accounts Payable";
                    break;
                case "FI_AR":
                    ViewBag.LeaderBoardText = "FI - Accounts Receivable";
                    break;
                case "MM":
                    ViewBag.LeaderBoardText = "Material Management";
                    break;
                case "SD":
                    ViewBag.LeaderBoardText = "Sales and Distribution";
                    break;
                case "PP":
                    ViewBag.LeaderBoardText = "Production Planning";
                    break;
                default:
                    break;
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

                ISAP sap = await _sapBuilder
                    .SetUsername(userInfo.UserId)
                    .SetClientId(userInfo.ClientId)
                    .SetApplicationServer(userInfo.ApplicationServer)
                    .SetCaseStudy(caseStudy)
                    .Build();

                ViewBag.Fulfillment = sap.GetFulfillment();
                ViewBag.Point = sap.GetPoint();
                ViewBag.Levels = sap.GetLevel();
                ViewBag.PointsList = sap.GetPointsList();
                ViewBag.StepsList = sap.GetStepsList();
                ViewBag.StepsCount = sap.GetStepsList().Count;
                ViewBag.Badge = sap.GetBadge();


                var existingRecord = _db.LeaderBoaders.SingleOrDefault(m => m.Username == userInfo.UserId && m.CaseStudy == _caseStudy);

                if (existingRecord != null)
                {
                    // if record exists, update the points
                    existingRecord.Point = sap.GetPoint();
                    _db.SaveChanges();
                }
                else
                {
                    // If the record doesn't exist, add a new record to the database
                    var records = new LeaderBoader()
                    {
                        CaseStudy = _caseStudy,
                        Username = userInfo.UserId,
                        Point = sap.GetPoint()
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

                ISAP sap = await _sapBuilder
                    .SetUsername(userInfo.UserId)
                    .SetClientId(userInfo.ClientId)
                    .SetApplicationServer(userInfo.ApplicationServer)
                    .SetCaseStudy(caseStudy)
                    .Build();

                _badge = sap.GetBadge();
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
