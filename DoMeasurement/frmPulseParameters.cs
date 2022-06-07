using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.IO;

namespace DoMeasurement
{
    public partial class frmPulseParameters : Form
    {
        string myProgramDataPath; //Base Program Data Dir
        string myPulseParamFile;
        PulseParameters myPulseParameters = new PulseParameters();

        public frmPulseParameters()
        {
            InitializeComponent();

            myProgramDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "NFPE"); //Base Program Data Dir
            myPulseParamFile = Path.Combine(myProgramDataPath, "PulseParameters.json");

            if (!Directory.Exists(myProgramDataPath))
            {
                Directory.CreateDirectory(myProgramDataPath);
            }

        }

        private void Pulse_Parameters_Load(object sender, EventArgs e)
        {
            if (!File.Exists(myPulseParamFile))
            {
                // Create default Parameter file and store default values
                SetPulseDefaults();
                myPulseParameters.ToJSONFile(myPulseParamFile);
            }
            else
            {
                // loading last updated Pulse Parameter values from file to display in text box
                myPulseParameters = myPulseParameters.FromJSONFile(myPulseParamFile);
            }
            PopulateGui();
        }

        private void SetPulseDefaults()
        {
            //myPulseParameters.PulseType = PulseType.Bilevel;

            myPulseParameters.Voltage1 = 15.0;
            myPulseParameters.Voltage2 = 10.0;
            myPulseParameters.Time1 = 0.5;
            myPulseParameters.Time2 = 2.5;
            myPulseParameters.PulsesInTrain = 100;
            myPulseParameters.PulseFrequency = 50;
            myPulseParameters.RestingTime = 1;
            myPulseParameters.NumberOfTrains = 1;
        }

        private void PopulateGui()
        {
            this.CmbPulseType.SelectedIndexChanged -= new System.EventHandler(this.CmbPulseType_SelectedIndexChanged); //disable event
            CmbPulseType.SelectedIndex = (int)myPulseParameters.PulseType;
            this.CmbPulseType.SelectedIndexChanged += new System.EventHandler(this.CmbPulseType_SelectedIndexChanged); //re-enable event

            nudVoltage1.Value = (decimal)myPulseParameters.Voltage1;
            nudVoltage2.Value = (decimal)myPulseParameters.Voltage2;
            nudTime1.Value = (decimal)myPulseParameters.Time1;
            nudTime2.Value = (decimal)myPulseParameters.Time2;
            nudPulsesInTrain.Value = (decimal)myPulseParameters.PulsesInTrain;
            nudPulseFrequency.Value = (decimal)myPulseParameters.PulseFrequency;
            nudRestingTime.Value = (decimal)myPulseParameters.RestingTime;
            nudNumberOfTrains.Value = (decimal)myPulseParameters.NumberOfTrains;

            if (myPulseParameters.PulseType != PulseType.Bilevel) // load default values for non-applicable parameters if the pulse is square or exponential
            {
                nudVoltage2.Value = 10.0M;
                nudTime2.Value = 2.5M;
            }

            // changing pulse parameter form display depending on pulse type selected by the user
            if (CmbPulseType.SelectedIndex == 0)
            {
                // the display is adjusted for the bilevel pulse
                pnlV2T2.Enabled = true;
            }
            else
            {
                // the display is adjusted for the exponential pulse
                pnlV2T2.Enabled = false;
            }
        }

        private void nudVoltage1_ValueChanged(object sender, EventArgs e)
        {
            myPulseParameters.Voltage1 = (double)nudVoltage1.Value;
        }

        private void nudVoltage2_ValueChanged(object sender, EventArgs e)
        {
            myPulseParameters.Voltage2 = (double)nudVoltage2.Value;
        }

        private void nudTime1_ValueChanged(object sender, EventArgs e)
        {
            myPulseParameters.Time1 = (double)nudTime1.Value;
        }

        private void nudTime2_ValueChanged(object sender, EventArgs e)
        {
            myPulseParameters.Time2 = (double)nudTime2.Value;
        }

        private void nudPulsesInTrain_ValueChanged(object sender, EventArgs e)
        {
            myPulseParameters.PulsesInTrain = (int)nudPulsesInTrain.Value;
        }

        private void nudPulseFrequency_ValueChanged(object sender, EventArgs e)
        {
            myPulseParameters.PulseFrequency = (int)nudPulseFrequency.Value;
        }

        private void nudRestingTime_ValueChanged(object sender, EventArgs e)
        {
            myPulseParameters.RestingTime = (double)nudRestingTime.Value;
        }

        private void nudNumberOfTrains_ValueChanged(object sender, EventArgs e)
        {
            myPulseParameters.NumberOfTrains = (int)nudNumberOfTrains.Value;
        }

        private void CmbPulseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            myPulseParameters.PulseType = (PulseType)CmbPulseType.SelectedIndex;
            SetPulseDefaults();
            PopulateGui();            

        }

        private void btnCancelPulse_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSetPulse_Click(object sender, EventArgs e)
        {
            myPulseParameters.ToJSONFile(myPulseParamFile);
        }

        private void btnDefaultParameters_Click(object sender, EventArgs e)
        {
            // Allows user to return to default set of Parameters
            myPulseParameters.PulseType = PulseType.Bilevel;
            SetPulseDefaults();
            PopulateGui();
        }

        private enum PulseType
        {
            Bilevel,
            Square,
            Exponential
        }

        private class PulseParameters
        {
            public PulseType PulseType { get; set; }
            public double Voltage1 { get; set; }
            public double Voltage2 { get; set; }
            public double Time1 { get; set; }
            public double Time2 { get; set; }
            public int PulsesInTrain { get; set; }
            public int PulseFrequency { get; set; }
            public double RestingTime { get; set; }
            public int NumberOfTrains { get; set; }

            public string ToJSON()
            {
                return JsonConvert.SerializeObject(this); //no indentation
            }
            public PulseParameters FromJSON(string myJSON)
            {
                return (PulseParameters)JsonConvert.DeserializeObject(myJSON, typeof(PulseParameters)); 
            }
            public void ToJSONFile(string myFilePath)
            {
                System.IO.File.WriteAllText(myFilePath, ToJSON());
            }
            public PulseParameters FromJSONFile(string myFilePath)
            {
                return FromJSON(System.IO.File.ReadAllText(myFilePath));
            }

        }
    }
}
