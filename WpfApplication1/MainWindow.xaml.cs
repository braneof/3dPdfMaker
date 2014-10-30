using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using System.IO;

namespace ThreeDPdfMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonDataDir_Click(object sender, RoutedEventArgs e)
        {
            // Run the tests
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Select the data directory";
            dlg.IsFolderPicker = true;
            //dlg.InitialDirectory = TextBoxDataDir.Text;  // uses default or recent directory if text is empty or invalid

            //dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = Environment.CurrentDirectory;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var folder = dlg.FileName;
                LabelDataDir.Content = folder;
            }
        }

        private void ButtonSTLFile_Click(object sender, RoutedEventArgs e)
        {
            // Run the tests
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Choose an .stl file";
            //dlg.IsFolderPicker = true;
            //dlg.InitialDirectory = TextBoxSessionFile.Text;  // uses default or recent directory if text is empty or invalid

            //dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = Environment.CurrentDirectory;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = true;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var stlFile = dlg.FileName;
                LabelSessionFile.Content = stlFile;
                /* debug */
                string outputFile = System.IO.Path.GetFileNameWithoutExtension(LabelSessionFile.Content.ToString()) + ".stl";
                //string outputDir = System.IO.Path.GetDirectoryName(LabelSessionFile.Content.ToString()) + "\\";
                SetDebugTextAndClipboard(outputFile);
                /* end debug */
            }
        }


        private void ButtonMakePDF_Click(object sender, RoutedEventArgs e)
        {
            //string cmd = ssprintf("\"%s/R:\OrthoVis Releases\OtherTools\WebSessionConverter_latest\SessionConverter.exe\" \"%s/data\" \"%s\" \"%s.websession.zip\"", appPath.c_str(), appPath.c_str(), sessionFile.c_str(), sessionNoExtension.c_str());
            string fullPath = LabelDataDir.Content.ToString() + "\\StlToU3d\\StlToU3d.exe";
            Process process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = System.IO.Path.GetFileName(fullPath);
            process.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(fullPath);
            process.StartInfo.Arguments = "\"" + LabelSessionFile.Content.ToString() + "\"";
            process.Start();
            process.WaitForExit();// Waits here for the process to exit.

            string outputU3d = System.IO.Path.GetFileNameWithoutExtension(LabelSessionFile.Content.ToString()) + ".u3d";
            string stlToU3dPath = LabelDataDir.Content.ToString() + "\\StlToU3d";
            string pdfDirPath = LabelDataDir.Content.ToString() + "\\pdf";
            File.Move(stlToU3dPath + "\\" + outputU3d, pdfDirPath + "\\" + "models.u3d");
            SetDebugTextAndClipboard("Moved " + stlToU3dPath + "\\" + outputU3d + " to " + pdfDirPath + "\\" + outputU3d);
            //fullPath = @"R:\OrthoVis Releases\OtherTools\StlToU3d_latest\StlToU3d.exe";
            process.StartInfo.FileName = "pdflatex.exe";
            process.StartInfo.WorkingDirectory = LabelDataDir.Content.ToString();
            //string outputZip = System.IO.Path.GetFileNameWithoutExtension(LabelSessionFile.Content.ToString()) + ".zip";
            //string outputDir = System.IO.Path.GetDirectoryName(LabelSessionFile.Content.ToString()) + "\\";
            string pdfDir = LabelDataDir.Content.ToString() + "\\pdf";
            process.StartInfo.Arguments = String.Format("-output-directory \"{0}\" \"{0}\\OrthoVis3dModelPlan.tex\"", pdfDir);
            process.Start();
            process.WaitForExit();// Waits here for the process to exit.
        }

        private void ButtonButtonWebToDesktop_Click(object sender, RoutedEventArgs e)
        {
            //string startPath = @"c:\example\start";
            //string zipPath = @"c:\example\result.zip";
            //string extractPath = @"c:\example\extract";

            //System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath);

            //string extractPath = System.IO.Path.GetDirectoryName(LabelSessionFile.Content.ToString()) + "\\" + System.IO.Path.GetFileNameWithoutExtension(LabelSessionFile.Content.ToString()) + "\\ExtractedZipFolder";

            //////////ZipFile.ExtractToDirectory(LabelDebug.Content.ToString(), extractPath);

            //SetDebugTextAndClipboard(System.IO.Path.GetDirectoryName(LabelSessionFile.Content.ToString()) + "\\" + System.IO.Path.GetFileNameWithoutExtension(LabelSessionFile.Content.ToString()) + "\\ExtractedZipFolder");

            //using (StreamReader reader = File.OpenText(extractPath + @"\session.json"))
            //{
            //    JObject data = (JObject)JToken.ReadFrom(new JsonTextReader(reader));

            //    int implantVersion = (int)data["implantVersion"];

            //    int implantInclination = (int)data["implantInclination"];

            //    int implantRoll = (int)data["implantRoll"];

            //    data["implantVersion"] = implantVersion + 5;
            //    data["implantInclination"] = implantInclination + 5;
            //    data["implantRoll"] = implantRoll + 5;

            //    JObject implantPosition = (JObject)data["implantPosition"];
            //    // ["Json.NET", "CodePlex"]

            //    float posX = (float)implantPosition["x"];
            //    float posY = (float)implantPosition["y"];
            //    float posZ = (float)implantPosition["z"];

            //    implantPosition["x"] = posX + 10.0f;
            //    implantPosition["y"] = posY + 10.0f;
            //    implantPosition["z"] = posZ + 10.0f;
            //    posX = (float)implantPosition["x"];

            //    SetDebugTextAndClipboard(String.Format("version: {0}, inclination: {1}, roll: {2}, posX: {3}, posY: {4}, posZ: {5}", implantVersion, implantInclination, implantRoll, posX, posY, posZ));

            //    string modifiedJson = @"c:\testSession.json";
            //    File.WriteAllText(modifiedJson, data.ToString());


            //    //string cmd = ssprintf("\"%s/R:\OrthoVis Releases\OtherTools\WebSessionConverter_latest\SessionConverter.exe\" \"%s/data\" \"%s\" \"%s.websession.zip\"", appPath.c_str(), appPath.c_str(), sessionFile.c_str(), sessionNoExtension.c_str());
            //    string fullPath = @"R:\OrthoVis Releases\OtherTools\WebSessionConverter_latest\SessionConverter.exe";
            //    ProcessStartInfo psi = new ProcessStartInfo();
            //    psi.FileName = System.IO.Path.GetFileName(fullPath);
            //    psi.WorkingDirectory = System.IO.Path.GetDirectoryName(fullPath);
            //    //string outputZip = System.IO.Path.GetFileNameWithoutExtension(LabelSessionFile.Content.ToString()) + ".zip";
            //    //string outputDir = System.IO.Path.GetDirectoryName(LabelSessionFile.Content.ToString()) + "\\";

            //    psi.Arguments = "\"" + LabelDataDir.Content.ToString() + "\" \"" + modifiedJson + "\" \"" + LabelSessionFile.Content.ToString() + "\"";
            //    //Debugging
            //    // end debug
            //    Process.Start(psi);
            //}


        }

        private void SetDebugTextAndClipboard(string text)
        {
            LabelDebug.Content = text;
            System.Windows.Clipboard.SetText(text);
        }

    }
}
