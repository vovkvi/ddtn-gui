using System.Collections.Generic;
using System.Xml.Linq;

namespace DDTN
{
    public static class cfg
    {
        private static string xmlfile = "config.xml";

        public static double CosF = 0;
        public static double UPerc = 0;
        public static Dictionary<int, double> IK = new Dictionary<int, double>();
        public static Dictionary<int, double> TK = new Dictionary<int, double>();
        public static List<VL> VLS = new List<VL>();

        static cfg() 
        {
            XDocument doc = XDocument.Load(xmlfile);
            CosF = doc.Root.Attribute("COS").Value.Double();
            UPerc = doc.Root.Attribute("U").Value.Double();
            foreach (XElement e in doc.Root.Element("TK").Elements()) 
                TK.Add(e.Attribute("T").Value.Int(), e.Attribute("K").Value.Double());
            foreach (XAttribute e in doc.Root.Attributes()) 
            {
                string name = e.Name.ToString();
                if (name.Contains("IK"))
                    IK.Add(name.Replace("IK", string.Empty).Int(), e.Value.Double());
            }
            foreach (XElement e in doc.Root.Elements("VL")) 
            {
                foreach(XElement vle in e.Elements())
                {
                    VLS.Add(new VL() 
                    {
                        M = e.Attribute("M").Value,
                        S = vle.Attribute("S").Value,
                        I = vle.Attribute("I").Value.Int()
                    });
                }
            }
        }
    }

    public class VL 
    {
        public string M { get; set; }
        public string S { get; set; }
        public int I { get; set; }
    }
}
