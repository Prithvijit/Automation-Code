using _3Dconnexion; //see Siapp.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoMeasurement
{
    public partial class frmMain : Form
    {
        #region Variables

        private const string appName = "NFPE_Automation"; //appname is arbitrary
        private IntPtr devHdl = IntPtr.Zero;
        private SiApp.SpwRetVal res;

        #endregion Variables

        protected override void WndProc(ref Message msg)
        {
            //Check for Windows messages from the SpaceMouse:
            if (SiApp.IsSpaceMouseMessage(msg.Msg))
                TrackMouseEvents(msg);
            //Pass on messages to the base class:
            base.WndProc(ref msg);
        }

        public bool InitializeSiApp(IntPtr hWnd)
        {
            res = SiApp.SiInitialize();
            if (res != SiApp.SpwRetVal.SPW_NO_ERROR)
            {
                MessageBox.Show("Initialize function failed");
                return false;
            }

            Log("SiInitialize", res.ToString());

            SiApp.SiOpenData openData = new SiApp.SiOpenData();
            SiApp.SiOpenWinInit(openData, hWnd);
            if (openData.hWnd == IntPtr.Zero)
                MessageBox.Show("Handle is empty");
            Log("SiOpenWinInit", openData.hWnd + "(window handle)");

            devHdl = SiApp.SiOpen(appName, SiApp.SI_ANY_DEVICE, IntPtr.Zero, SiApp.SI_EVENT, openData);
            if (devHdl == IntPtr.Zero)
                MessageBox.Show("Open returns empty device handle");
            Log("SiOpen", devHdl + "(device handle)");

            return (devHdl != IntPtr.Zero);
        }

        private void TrackMouseEvents(Message msg)
        {
            //if (!SiApp.IsSpaceMouseMessage(msg.Msg))
            //    return;

            SiApp.SiGetEventData eventData = new SiApp.SiGetEventData();
            SiApp.SiGetEventWinInit(eventData, msg.Msg, msg.WParam, msg.LParam);

            SiApp.SiSpwEvent spwEvent = new SiApp.SiSpwEvent();
            SiApp.SpwRetVal val = SiApp.SiGetEvent(devHdl, SiApp.SI_AVERAGE_EVENTS, eventData, spwEvent);

            if (val == SiApp.SpwRetVal.SI_IS_EVENT)
            {
                Log("SiGetEventWinInit", eventData.msg.ToString());

                switch (spwEvent.type)
                {
                    case 0:
                        break;

                    case SiApp.SiEventType.SI_MOTION_EVENT:
                        {
                            double X = (double)spwEvent.spwData.mData[0];
                            double Y = (double)spwEvent.spwData.mData[2];
                            double Z = (double)spwEvent.spwData.mData[4];

                            if (X != 0 || Y != 0 || Z != 0)
                            {
                                myStageController.JogWithVelocity(-Y, -X, -Z);

                            }
                        }
                        break;

                    case SiApp.SiEventType.SI_ZERO_EVENT:
                        //Print("Zero event");
                        break;

                    case SiApp.SiEventType.SI_CMD_EVENT:
                        //Print("V3DCMD event: V3DCMD = {0}, pressed = {1}", spwEvent.cmdEventData.functionNumber, spwEvent.cmdEventData.pressed > 0);
                        break;

                    case SiApp.SiEventType.SI_APP_EVENT:
                        //Print("App event: appCmdID = \"{0}\", pressed = {1}", spwEvent.appCommandData.id.appCmdID, spwEvent.appCommandData.pressed > 0);
                        break;

                    default:
                        //Print("Event: type = \"{0}\"", spwEvent.type);
                        break;
                }
            }
        }

        public void CloseSiApp()
        {
            if (devHdl != IntPtr.Zero)
            {
                SiApp.SpwRetVal res = SiApp.SiClose(devHdl);
                Log("SiClose", res.ToString());
                int r = SiApp.SiTerminate();
                Log("SiTerminate", r.ToString());
            }
        }

        private string Get3DxWareHomeDirectory()
        {
            string softwareKeyName = string.Empty;
            string homeDirectory = string.Empty;

            if (IntPtr.Size == 8)
            {
                softwareKeyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\3Dconnexion\3DxWare";
            }
            else
            {
                softwareKeyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\3Dconnexion\3DxWare";
            }

            object regValue = Microsoft.Win32.Registry.GetValue(softwareKeyName, "Home Directory", null);
            if (regValue != null)
            {
                homeDirectory = regValue.ToString();
            }

            return homeDirectory;
        }

        private T PtrToStructure<T>(IntPtr ptr) where T : struct
        {
            return (T)System.Runtime.InteropServices.Marshal.PtrToStructure(ptr, typeof(T));
        }

        private void Print(string format, params object[] args)
        {
            Print(string.Format(format, args));
        }

        private void Print(string message)
        {
            //txtJoystickData.AppendText(string.Format("{0}{1}", message, Environment.NewLine));
        }

        private void Log(string functionName, string result)
        {
            //we may wish to log the data for debug purposes
        }


    }
}
