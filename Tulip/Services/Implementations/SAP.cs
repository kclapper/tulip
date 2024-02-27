using Newtonsoft.Json.Linq;

namespace Tulip.Services.Implementations
{
    public class SAP: ISAP
    {
        private readonly ILogger<SAP> logger;

        private readonly Dictionary<string, int> PointsDictionary = new Dictionary<string, int>()
        {
            {"@08@10", 10},
            {"@09@7", 8},
            {"@0A@5", 7 },
            {"@0A@", 7 },
            {"0", 0}
        };

        private string fulfillment = "0";
        private Dictionary<string, bool> stepsCompletedDictionary = new Dictionary<string, bool>();
        private int point = 0;
        private int level = 0;
        private string badge = "";
        private List<int> Points = new List<int>();
        private List<string> Steps = new List<string>();

        public SAP(ILogger<SAP> logger, JToken responseBody)
        {
            this.logger = logger;

            var dictionaryList = new List<DictionaryModel>();
            foreach (var property in responseBody.Children<JProperty>())
            {
                dictionaryList.Add(new DictionaryModel {
                    Key = property.Name,
                    Value = property.Value.ToString()
                });
            }
 
            foreach (var item in dictionaryList)
            {
                if (item.Key.Contains("Step"))
                {
                    Points.Add(PointsDictionary[item.Value]);
                    Steps.Add(item.Key);

                    if (item.Value != "0")
                    {
                        fulfillment = "100"; 
                        stepsCompletedDictionary[item.Key] = true;
                    }
                    else
                    {
                        stepsCompletedDictionary[item.Key] = false;
                    }
                }

                if (item.Key.Equals("FulfillmentAll"))
                {
                    fulfillment = item.Value;
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

            if (stepsCompletedDictionary.All(entry => entry.Value))
            {
                fulfillment = "100";
            }

            //fulfillment, remove "%" if exists in the string.
            fulfillment = fulfillment.Contains("%") ? fulfillment.Remove(fulfillment.IndexOf('%')) : fulfillment;
        }
        public class DictionaryModel
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        public int GetFulfillment()
        {
            return int.Parse(fulfillment);
        }

        public string GetBadge()
        {
            return badge;
        }

        public int GetLevel()
        {
            return level;
        }

        public int GetPoint()
        {
            return point;
        }

        public List<int> GetPointsList()
        {
            return Points;
        }

        public List<string> GetStepsList()
        {
            return Steps;
        }
    }
}