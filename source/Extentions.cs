using System;
using System.Windows.Forms;

namespace DDTN
{
    public static class Extentions
    {
        public static void ZeroIndex(this ComboBox cb) 
        {
            cb.SelectedIndex = cb.Items.Count != 0 ? 0 : -1; 
        }

        public static int Int(this string str) 
        { 
            try
            {
                return Convert.ToInt32(str);
            }
            catch{ return 0;}
        }

        public static double Double(this string str)
        {
            try
            {
                return Convert.ToDouble(str);
            }
            catch { return 0.0; }
        }
    }
}
