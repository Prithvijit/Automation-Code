using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DoMeasurement
{

    class IpParameters
    {
        public double maxFeret { get; set; }
        public double minArea { get; set; }
        public double maxFeret2 { get; set; }
        public double minArea2 { get; set; }
        public double maxArea { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this); //no indentation
        }
        public IpParameters FromJSON(string myJSON)
        {
            return (IpParameters)JsonConvert.DeserializeObject(myJSON, typeof(IpParameters));
        }
        public void ToJSONFile(string myFilePath)
        {
            System.IO.File.WriteAllText(myFilePath, ToJSON());
        }
        public IpParameters FromJSONFile(string myFilePath)
        {
            return FromJSON(System.IO.File.ReadAllText(myFilePath));
        }
        public void SetIpDefaults()
        {
            //Setting default values for the IP analysis
            maxFeret = 50.0;
            minArea = 30.0;
            maxFeret2 = 1000.0;
            minArea2 = 500.0;
            maxArea = 15000.0;
        }
    }
}
