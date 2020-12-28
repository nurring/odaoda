using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using Python.Runtime;
using System.Diagnostics;
using OpenCvSharp;
using Model;
using log4net;

namespace ODA_Viewer
{
    public partial class VSearch : Form
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Stopwatch stopWatch;
        public VSearch()
        {
            InitializeComponent();
            stopWatch = new Stopwatch();
            InitializePythonNet();
            log.Debug("Initialized");
        }

        private List<CHANNEL> channelOrder = new List<CHANNEL>{CHANNEL.R, CHANNEL.R, CHANNEL.R, CHANNEL.R, CHANNEL.R, 
                                                                CHANNEL.W, CHANNEL.W, CHANNEL.W, CHANNEL.R, CHANNEL.W, 
                                                                CHANNEL.B, CHANNEL.B, CHANNEL.B, CHANNEL.B, CHANNEL.B,
                                                                CHANNEL.G, CHANNEL.G, CHANNEL.G, CHANNEL.G, CHANNEL.G,};
        private List<CHANNEL> channelOrderOrigin = new List<CHANNEL>{CHANNEL.R, CHANNEL.R, CHANNEL.R, CHANNEL.R,
                                                                    CHANNEL.W, CHANNEL.W, CHANNEL.W, CHANNEL.R,
                                                                    CHANNEL.B, CHANNEL.B, CHANNEL.B, CHANNEL.B,
                                                                    CHANNEL.G, CHANNEL.G, CHANNEL.G, CHANNEL.G,};
        private List<ALGORITHM_TYPE> algoOrder = new List<ALGORITHM_TYPE> { ALGORITHM_TYPE.LH, ALGORITHM_TYPE.HL, ALGORITHM_TYPE.HH, ALGORITHM_TYPE.LL, ALGORITHM_TYPE.BLEND,
                                                                            ALGORITHM_TYPE.LH, ALGORITHM_TYPE.HL, ALGORITHM_TYPE.HH, ALGORITHM_TYPE.LL, ALGORITHM_TYPE.BLEND,
                                                                            ALGORITHM_TYPE.LH, ALGORITHM_TYPE.HL, ALGORITHM_TYPE.HH, ALGORITHM_TYPE.LL, ALGORITHM_TYPE.BLEND,
                                                                            ALGORITHM_TYPE.LH, ALGORITHM_TYPE.HL, ALGORITHM_TYPE.HH, ALGORITHM_TYPE.LL, ALGORITHM_TYPE.BLEND,};
        private List<ALGORITHM_TYPE> algoOrderOrigin = new List<ALGORITHM_TYPE> { ALGORITHM_TYPE.MIN, ALGORITHM_TYPE.MAX, ALGORITHM_TYPE.AVG, ALGORITHM_TYPE.STD,
                                                                                 ALGORITHM_TYPE.MIN, ALGORITHM_TYPE.MAX, ALGORITHM_TYPE.AVG, ALGORITHM_TYPE.STD,
                                                                                 ALGORITHM_TYPE.MIN, ALGORITHM_TYPE.MAX, ALGORITHM_TYPE.AVG, ALGORITHM_TYPE.STD,
                                                                                 ALGORITHM_TYPE.MIN, ALGORITHM_TYPE.MAX, ALGORITHM_TYPE.AVG, ALGORITHM_TYPE.STD,};
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 20; i++)
            {
                string filePath = @"C:\Users\Div Na\Desktop\new.jpg";
                string[] words = filePath.Split('\\');
                string fileName = words[words.Length - 1];
                Mat temp = Cv2.ImRead(filePath, ImreadModes.Grayscale);
                Thumbnail newTn = new Thumbnail();
                newTn.Image = temp;
                newTn.FileName = fileName;
                newTn.Channel = channelOrder[i];
                newTn.AlgorithmType = algoOrder[i];
                ThumbnailCollection.Instance.AddThumbnail(newTn);
                log.Debug(ThumbnailCollection.Instance.GetSize());
            }
            for (int i = 0; i < 16; i++)
            {
                string filePath = @"C:\Users\Div Na\Desktop\origin.jpg";
                string[] words = filePath.Split('\\');
                string fileName = words[words.Length - 1];
                Mat temp = Cv2.ImRead(filePath, ImreadModes.Grayscale);
                Thumbnail newTn = new Thumbnail();
                newTn.Image = temp;
                newTn.FileName = fileName;
                newTn.Channel = channelOrderOrigin[i];
                newTn.AlgorithmType = algoOrderOrigin[i];
                ThumbnailCollection.Instance.AddThumbnail(newTn);
                log.Debug(ThumbnailCollection.Instance.GetSize());

            }
            ThumbnailViewer viewer = new ThumbnailViewer();
            viewer.ShowDialog();
            //RunThumbnailGenerater();
            //SearchCondition searchCondition = new SearchCondition();

            //List<string> asdf = new List<string>();
            ////asdf = WebHdfsGetFileContent("https://www.briandunning.com/sample-data/us-500.zip");
            ////asdf = WebHdfsGetFileContent("https://vcanus-my.sharepoint.com/:u:/p/divna/EevZmJ68T7xFmaQHXPsNxccBnMSziqIifkcmIIKlSJL-Kw?download=1");
            ////Console.WriteLine(asdf[0]);
            //dynamic result = RunThumbnailGenerater();
            //Thumbnail tn = new Thumbnail();
            //tn.CompType = COMP_TYPE.SEN_FS;
            //ThumbnailCollection.Instance.AddThumbnail(tn);
            ////ThumbnailCollection tc = new ThumbnailCollection();
            ////tc.AddThumbnail(tn);
        }

        private void RunSequenceForOne(string url)
        {
            List<string> csv = new List<string>();
            csv = WebHdfsGetFileContent(url);
            dynamic images = RunThumbnailGenerater();
            foreach (var img in images)
            {
                Thumbnail tn = new Thumbnail();
                Mat thumbnailImage = new Mat(68, 120, MatType.CV_8SC1, img);
                tn.Image = thumbnailImage;
                ThumbnailCollection.Instance.AddThumbnail(tn);
            }


        }
        private void RunSequenceForAll(List<string> urlList)
        {
            foreach (var url in urlList)
            {
                RunSequenceForOne(url);
            }
        }

        private List<string> WebHdfsGetFileContent(string hdfsUrl)
        {
            var url = hdfsUrl;
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            List<string> odaImage = new List<string>();
            using (Stream input = response.GetResponseStream())
            {
                odaImage = UnzipFile(input);
            }
            return odaImage;
        }

        private List<string> UnzipFile(Stream fsSource)
        {
            MemoryStream ms = new MemoryStream();
            byte[] chunk = new byte[4096];
            int bytesRead;
            while ((bytesRead = fsSource.Read(chunk, 0, chunk.Length)) > 0)
            {
                ms.Write(chunk, 0, bytesRead);
            }
            Stream unzippedEntryStream = null;

            ZipArchive archive = new ZipArchive(ms);
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                if (entry.FullName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    unzippedEntryStream = entry.Open();
                }
            }

            List<string> csv = new List<string>();
            using (StreamReader reader = new StreamReader(unzippedEntryStream))
            {
                String csvStr = reader.ReadToEnd();

                csv = csvStr.Split(',').ToList();

                Console.WriteLine(csv);
                //string[] lines = System.IO.File.ReadAllLines(@"food.txt");

                ////Create the list
                //List<string[]> grid = new List<string[]>();

                ////Populate the list
                //foreach (var line in lines) grid.Add(line.Split(','));
            }
            return csv;
        }

        private void InitializePythonNet()
        {
            string pythonScripts = @"C:\Users\Div Na\Workspace\py\lg_oda";
            var pathToVirtualEnv = @"C:\ProgramData\Anaconda3\envs\lgenv";

            Environment.SetEnvironmentVariable("PATH", pathToVirtualEnv, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", pathToVirtualEnv, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONPATH", $"{pathToVirtualEnv}\\Lib\\site-packages;", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONPATH", pythonScripts, EnvironmentVariableTarget.Process);
            PythonEngine.PythonHome = pathToVirtualEnv;
            PythonEngine.Initialize();
            PythonEngine.PythonPath = Environment.GetEnvironmentVariable("PYTHONPATH", EnvironmentVariableTarget.Process);

        }

        //private void RunThumbnailGenerater()
        private dynamic RunThumbnailGenerater()
        {
            dynamic result;
            dynamic result2;
            using (Py.GIL())
            {
                dynamic np = Py.Import("numpy");
                dynamic tgFile = Py.Import("tg"); // tg.py
                //dynamic thumbnailGenerator = tgFile.thumbnail_generator("log", "haar", 3); //클래스
                //Console.WriteLine(thumbnailGenerator.Length);

                dynamic tester = tgFile.tester(3); //클래스
                //result = tester.test(odaCsv);
                //dynamic numpyOda = np.array(odaCsv, dtype: np.int32);
                //result2 = tester.test2(numpyOda);

                dynamic a = np.array(new List<string> { "6", "5", "4" }, dtype: np.int32);
                dynamic b = np.array(new List<int> { 3, 2, 1 }, dtype: np.int32);
                dynamic c = np.array(new List<dynamic> { a, b });
                dynamic d = np.array(new List<dynamic> { b, a });
                dynamic e = np.array(new List<dynamic> { c, d });
                result = tester.test(e);
                
            }
            int[][][] mylist = (int[][][])result;
            for (int i = 0; i < mylist.Length; i++)
            {
                for (int j = 0; j < mylist[i].Length; j++)
                {
                    for (int k = 0; k < mylist[i][j].Length; k++)
                    {
                        Console.WriteLine(mylist[i][j][k]);

                    }
                }
            }
            //List<int> mylist = ((int[])result).ToList<int>();
            //foreach (var item in mylist)
            //{
            //    Console.WriteLine(item);
            //}

            return result;
        }

    }
}
