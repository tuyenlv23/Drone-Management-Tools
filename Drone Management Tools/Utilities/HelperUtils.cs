using System;
using System.IO;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Drone_Management_Tools.Utilities
{
    public static class HelperUtils
    {
        delegate void DelegateShowValue(Control ctrl, string value);
        delegate void DelegateGetEventLogs(ListView ctrl, string log);

        public static string GetFilePath()
        {
            string filePath;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All Files (*.*)|*.*|Excel Files (*.xls;*.xlsx;*.csv)|*.xls;*.xlsx;*.csv|Xml Files (*.xml)|*.xml|Json Files (*.json)|*.json";
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.Title = "Select";

            if (ofd.ShowDialog() == DialogResult.OK)
                filePath = ofd.FileName;
            else
                return null;

            return filePath;
        }

        public static string GetFilePath(string title)
        {
            string filePath;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All Files (*.*)|*.*|Excel Files (*.xls;*.xlsx;*.csv)|*.xls;*.xlsx;*.csv|Xml Files (*.xml)|*.xml|Json Files (*.json)|*.json";
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.Title = title;

            if (ofd.ShowDialog() == DialogResult.OK)
                filePath = ofd.FileName;
            else
                return null;

            return filePath;
        }

        public static string GetFilePath(string title, string filter)
        {
            string filePath;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = filter;
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.Title = title;

            if (ofd.ShowDialog() == DialogResult.OK)
                filePath = ofd.FileName;
            else
                return null;

            return filePath;
        }

        public static string GetMultiFilePath()
        {
            string filePath;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All Files (*.*)|*.*|Excel Files (*.xls;*.xlsx;*.csv)|*.xls;*.xlsx;*.csv|Xml Files (*.xml)|*.xml|Json Files (*.json)|*.json";
            ofd.Multiselect = true;
            ofd.RestoreDirectory = true;
            ofd.Title = "Select";
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            if (ofd.ShowDialog() == DialogResult.OK)
                filePath = ofd.FileName;
            else
                return null;

            return filePath;
        }

        public static string GetMultiFilePath(string title)
        {
            string filePath;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All Files (*.*)|*.*|Excel Files (*.xls;*.xlsx;*.csv)|*.xls;*.xlsx;*.csv|Xml Files (*.xml)|*.xml|Json Files (*.json)|*.json";
            ofd.Multiselect = true;
            ofd.RestoreDirectory = true;
            ofd.Title = title;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            if (ofd.ShowDialog() == DialogResult.OK)
                filePath = ofd.FileName;
            else
                return null;

            return filePath;
        }

        public static string GetCSVFilePath()
        {
            string filePath;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV Files (*.csv)|*.csv";
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.Title = "Select";

            if (ofd.ShowDialog() == DialogResult.OK)
                filePath = ofd.FileName;
            else
                return null;

            return filePath;
        }

        public static string GetCSVFilePath(string title)
        {
            string filePath;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV Files (*.csv)|*.csv";
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.Title = title;

            if (ofd.ShowDialog() == DialogResult.OK)
                filePath = ofd.FileName;
            else
                return null;

            return filePath;
        }

        public static string GetFolderPath()
        {
            string folderPath;

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
                folderPath = fbd.SelectedPath;
            else
                return null;

            return folderPath;
        }

        public static string GetSavePath()
        {
            string savePath;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save";
            sfd.Filter = "All Files (*.*)|*.*";
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
                savePath = sfd.FileName;
            else
                return null;

            return savePath;
        }

        public static string GetSavePath(string title)
        {
            string savePath;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = title;
            sfd.Filter = "All Files (*.*)|*.*";
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
                savePath = sfd.FileName;
            else
                return null;

            return savePath;
        }

        public static string GetSavePath(string title, string filter, string fileName)
        {
            string savePath;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = title;
            sfd.Filter = filter;
            sfd.FileName = fileName;
            sfd.ValidateNames = true;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
                savePath = sfd.FileName;
            else
                return null;

            return savePath;
        }

        public static List<string> GetFileFromFolder(string folderPath)
        {
            List<string> result = new List<string>();

            if (Directory.Exists(folderPath))
            {
                string[] filePaths = Directory.GetFiles(folderPath);
                if (filePaths.Length > 0)
                {
                    foreach (var filePath in filePaths)
                    {
                        if (!result.Contains(filePath))
                            result.Add(filePath);
                    }
                }
            }

            return result;
        }

        public static bool CheckFileAvailable(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
                return false;
            }
        }

        public static bool ValidateFileInFolder(string sourceFile, string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                var _fileName = Path.GetFileName(sourceFile);
                var _destFile = Path.Combine(folderPath, _fileName);
                if (File.Exists(_destFile))
                    File.Delete(_destFile);

                File.Copy(sourceFile, _destFile);

                return true;
            }

            return false;
        }

        public static bool CreateFilePath(string filePath)
        {
            try
            {
                string directoryName = Path.GetDirectoryName(filePath);
                bool isFileExisted = false;
                do
                {
                    if (Directory.Exists(directoryName))
                    {
                        if (!File.Exists(filePath))
                            File.Create(filePath).Close();
                        else
                            isFileExisted = true;
                    }
                    else
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                }
                while (!isFileExisted);

                return isFileExisted;
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
                return false;
            }
        }

        public static bool CreateDirectory(string dirPath)
        {
            try
            {
                bool isDirExisted = false;
                do
                {
                    if (Directory.Exists(dirPath))
                        isDirExisted = true;
                    else
                        Directory.CreateDirectory(dirPath);
                }
                while (!isDirExisted);

                return isDirExisted;
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
                return false;
            }
        }

        public static bool TransferFile(string sourceFile, string destFile)
        {
            try
            {
                if (File.Exists(sourceFile))
                {
                    File.Copy(sourceFile, destFile, true);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
                return false;
            }
        }

        public static void SaveJsonFile(object objData, string savePath)
        {
            try
            {
                string result;

                if (!string.IsNullOrEmpty(savePath))
                {
                    // serialize JSON directly to a file
                    using (StreamWriter sw = File.CreateText(savePath))
                    {
                        var settings = new JsonSerializerSettings
                        {
                            DateFormatString = "dd-MMM-yyyy HH:mm:ss",
                            DateTimeZoneHandling = DateTimeZoneHandling.Local,
                        };

                        result = JsonConvert.SerializeObject(objData, Formatting.Indented, new StringEnumConverter());
                        sw.WriteLine(result);
                    }
                }
                else
                    LogUtils.AddLog("Save location is not selected", LogTypes.Info);
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
            }
        }

        public static bool IsSavedJsonFile(object objData, string savePath)
        {
            string result;

            if (!string.IsNullOrEmpty(savePath))
            {
                // serialize JSON directly to a file
                using (StreamWriter file = File.CreateText(savePath))
                {
                    result = JsonConvert.SerializeObject(objData, Formatting.Indented);
                    file.WriteLine(result);
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public static XDocument InitXDocument(string rootName, string projectName)
        {
            XDocument xDoc = new XDocument();
            xDoc.Declaration = new XDeclaration("1.0", "utf-8", null);

            XElement rootEle = new XElement(rootName);

            XElement prELe = new XElement(projectName);
            prELe.SetAttributeValue("Name", "Project Template");
            prELe.SetAttributeValue("Description", "Project Template");
            rootEle.Add(prELe);

            xDoc.Add(rootEle);

            return xDoc;
        }

        public static XElement AddGroupElement(ref XElement ele, string grName)
        {
            XElement grEle = new XElement("Group");
            grEle.SetAttributeValue("Name", grName);
            grEle.SetAttributeValue("Description", "");
            grEle.SetAttributeValue("Enabled", "true");

            XElement pEle = new XElement("Points");
            grEle.Add(pEle);
            ele.Add(grEle);

            return grEle;
        }

        public static void ShowValue(Control ctrl, string value)
        {
            if (ctrl.InvokeRequired)
            {
                DelegateShowValue d = new DelegateShowValue(ShowValue);
                ctrl.Invoke(d, new object[] { ctrl, value });
            }
            else
            {
                ctrl.Text = value;
            }
        }

        public static void ShowEventLogs(ListView ctrl, string log)
        {
            if (ctrl.InvokeRequired)
            {
                DelegateGetEventLogs d = new DelegateGetEventLogs(ShowEventLogs);
                ctrl.Invoke(d, new object[] { ctrl, log });
            }
            else
            {
                ctrl.Items.Add($"<{string.Format("{0:dd-MMM-yyyy HH:mm:ss.fff}", DateTime.Now)}> | {log}");
                ctrl.Items[ctrl.Items.Count - 1].EnsureVisible();
            }
        }

        public static string GetNumberElement(string strVal)
        {
            try
            {
                if (!string.IsNullOrEmpty(strVal))
                {
                    if (Regex.Match(strVal, @"\A[\d.,]+") != null)
                        return Regex.Match(strVal, @"\A[\d.,]+").ToString();
                }

                return null;
            }    
            catch
            {
                return null;
            }
        }

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117
        }


        public static float GetScalingFactor()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

            float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;

            return ScreenScalingFactor; // 1.25 = 125%
        }
    }

    public static class PATH_MANAGER
    {
        private const string DEVICE_LIBRARY_PATH = @"Configs\DeviceLibrary.json";
        private const string DRONE_CONFIG_PATH = @"Configs\DroneConfig.json";
        private const string VOLTAGE_CONFIG_PATH = @"Configs\VoltageConfig.json";
        private const string DEVICE_SCAN_PATH = @"Configs\DeviceScan.json";
        private const string DEVICE_CONFIG_PATH = @"Models\";
        private const string DATA_PATH = @"\Easy Revolution\Drone Management Tools\";

        private static string MS_APP_DATA_PATH = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + DATA_PATH;
        private static string MS_APP_IMAGE_PATH = Application.StartupPath + @"\Images\";

        public static string deviceLibraryPath = MS_APP_DATA_PATH + DEVICE_LIBRARY_PATH;
        public static string droneConfigPath = MS_APP_DATA_PATH + DRONE_CONFIG_PATH;
        public static string voltageConfigPath = MS_APP_DATA_PATH + VOLTAGE_CONFIG_PATH;
        public static string deviceScanPath = MS_APP_DATA_PATH + DEVICE_SCAN_PATH;
        public static string deviceConfigDir = MS_APP_DATA_PATH + DEVICE_CONFIG_PATH;
        public static string activeConfigDir = MS_APP_DATA_PATH;
        public static string appImageDir = MS_APP_IMAGE_PATH;
    }    
}
