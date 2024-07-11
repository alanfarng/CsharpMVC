using CsharpMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CsharpMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        static public JArray getJson(string uri)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri); //request�ШD
            req.Timeout = 10000; //request�O�ɮɶ�
            req.Method = "GET"; //request�覡
            HttpWebResponse respone = (HttpWebResponse)req.GetResponse(); //����respone
            StreamReader streamReader = new StreamReader(respone.GetResponseStream(), Encoding.UTF8); //Ū��respone���
            string result = streamReader.ReadToEnd(); //Ū����̫�@��
            respone.Close();
            streamReader.Close();
            JObject jsondata = JsonConvert.DeserializeObject<JObject>(result); //�N����ରjson����
            return (JArray)jsondata["cwaopendata"]["dataset"]["Station"]; //�^��json�}�C
        }

        public IActionResult Index()
        {
            string uri = "https://opendata.cwa.gov.tw/fileapi/v1/opendataapi/O-A0091-001?Authorization=rdec-key-123-45678-011121314&format=JSON";
            JArray jsondata = getJson(uri);
            List<string> station = new List<string>();
            foreach (JObject obj in jsondata)
            {
                station.Add(obj["StationName"].ToString());
            }
            ViewData["StationList"] = station;
            return View();
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
