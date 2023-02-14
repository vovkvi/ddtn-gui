using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DDTN
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Text += " v." + Application.ProductVersion;
            tbCosF.Text = cfg.CosF.ToString();
            tbCosF.TextChanged += (o, e) => { cfg.CosF = tbCosF.Text.Double(); Calculate(); };
            tbUperc.Text = cfg.UPerc.ToString();
            tbUperc.TextChanged += (o, e) => { cfg.UPerc = tbUperc.Text.Double(); Calculate(); };
            foreach (var e in cfg.VLS)
            {
                if (cbM.Items.Contains(e.M))
                    continue;
                else
                    cbM.Items.Add(e.M);
            }
            cbM.ZeroIndex();
        }

        private void Calculate() 
        {
            double vli = tbI.Text.Double();
            int vlrasch = tbRasch.Text.Int();
            // ДДТН (Iном * температурный коэф.)
            foreach (Label lbI in tlp.Controls.OfType<Label>().Where(x => x.Name.Contains("I"))) 
            {
                int temp = 0;
                if (lbI.Name.Contains("M"))
                    temp = 0 - (lbI.Name.Replace("IM", string.Empty).Int());
                else
                    temp = lbI.Name.Replace("IP", string.Empty).Int();
                lbI.Text = Math.Round(vli * cfg.TK[temp] * vlrasch).ToString();
            }
            // АДТН (Iном * температурный коэф. * 20 %)
            foreach (Label lbI in tlp.Controls.OfType<Label>().Where(x => x.Name.Contains("A")))
            {
                int temp = 0;
                if (lbI.Name.Contains("M"))
                    temp = 0 - (lbI.Name.Replace("AM", string.Empty).Int());
                else
                    temp = lbI.Name.Replace("AP", string.Empty).Int();
                lbI.Text = Math.Round(vli * cfg.TK[temp] * 1.2 * vlrasch).ToString();
            }
            // P при U кВ
            foreach (Label lbI in tlp.Controls.OfType<Label>().Where(x => x.Name.Contains("U")))
            {
                int temp = 0;
                double volt = 0;
                var mass = new string[] {};
                if (lbI.Name.Contains("M"))
                {
                    mass = lbI.Name.Split('M');
                    temp = (0 - mass[1].Int());
                }
                else
                {
                    mass = lbI.Name.Split('P');
                    temp = mass[1].Int();
                }
                volt = mass[0].Replace("U", string.Empty).Int();
                volt = volt + ((volt / 100) * cfg.UPerc);
                double result = Math.Round((vli * cfg.TK[temp] * volt * 1.73 * cfg.CosF ) / 1000);
                lbI.Text = Math.Round((result * vlrasch)).ToString();
            }
        }

        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != '-')
                e.Handled = true;
            if (e.KeyChar == 44 || e.KeyChar == 46)
            {
                (sender as TextBox).Text = ((sender as TextBox).Text.Contains(',') | (sender as TextBox).Text.Contains('.')) ? (sender as TextBox).Text : (sender as TextBox).Text + ",";
                (sender as TextBox).SelectionStart = (sender as TextBox).Text.Length;
            }
        }

        private void cbM_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbS.Items.Clear();
            List<VL> vls = cfg.VLS.FindAll(x => x.M == cbM.Text);
            foreach (var vl in vls)
                cbS.Items.Add(vl.S);
            cbS.ZeroIndex();
            lblVL.Text = chbNoVL.Checked ? "Не задано. Iном = " + tbI.Text + " А" : cbM.Text + "-" + cbS.Text + " (Iном = " + tbI.Text + " А)";
            Calculate();
        }

        private void cbS_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbI.Text = cfg.VLS.First(x => x.M == cbM.Text && x.S == cbS.Text).I.ToString();
            lblVL.Text = chbNoVL.Checked ? "Не задано. Iном = " + tbI.Text + " А" : cbM.Text + "-" + cbS.Text + " (Iном = " + tbI.Text + " А)";
            Calculate();
        }

        private void chbNoVL_CheckedChanged(object sender, EventArgs e)
        {
            tbI.Enabled = chbNoVL.Checked ? true : false;
            cbM.Enabled = chbNoVL.Checked ? false : true;
            cbS.Enabled = chbNoVL.Checked ? false : true;
            lblVL.Text = chbNoVL.Checked ? "Не задано. Iном = " + tbI.Text + " А" : cbM.Text + "-" + cbS.Text + " (Iном = " + tbI.Text + " А)";
        }

        private void tb_TextChanged(object sender, EventArgs e)
        {
            lblVL.Text = chbNoVL.Checked ? "Не задано. Iном = " + tbI.Text + " А" : cbM.Text + "-" + cbS.Text + " (Iном = " + tbI.Text + " А)";
            Calculate();
        }

        private void tbUperc_TextChanged(object sender, EventArgs e)
        {
            cfg.UPerc = tbUperc.Text.Double();
            double u35 = 35.0;
            double u110 = 110.0;
            double u220 = 220.0;
            double u330 = 330.0;
            double u500 = 500.0;
            double p = 100;
            lbl35.Text = Math.Round(35 + ((u35 / p) * cfg.UPerc)).ToString();
            lbl110.Text = Math.Round(110 + ((u110 / p) * cfg.UPerc)).ToString();
            lbl220.Text = Math.Round(220 + ((u220 / p) * cfg.UPerc)).ToString();
            lbl330.Text = Math.Round(330 + ((u330 / p) * cfg.UPerc)).ToString();
            lbl500.Text = Math.Round(500 + ((u500 / p) * cfg.UPerc)).ToString();
            Calculate();
        }

        private void tbRasch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8)
                e.Handled = true;
        }
    }
}
