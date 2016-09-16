using System;
using System.IO;

namespace GeometryTester
{
	/// <summary>
	/// Summary description for DataLog.
	/// </summary>
	public class DataLog
	{
		protected string fileName = "DataLog.txt";
		public DataLog()
		{
			if(File.Exists(fileName))
			{
			}
			else
			{
				File.Create(fileName);
			}
		}
		public void LogData(string data)
		{
			StreamWriter w = File.AppendText(fileName);
			Log (data, w);
			// Close the writer and underlying file.
			w.Close();
		}
		private static void Log(string logMessage, TextWriter w)
		{
			w.Write("{0} {1}", DateTime.Now.ToShortDateString(),
				DateTime.Now.ToShortTimeString());
			w.WriteLine("    {0}", logMessage);
			// Update the underlying file.
			w.Flush(); 
		}

	}
}
