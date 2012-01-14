﻿// Copyright (C) 2011 Iker Ruiz Arnauda (Wesko)
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using System.IO;
using Nini.Config;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MadCow
{
    class Compile
    {
        //This paths may change depending on which repository ur trying to retrieve, they are set over ParseRevision.cs
        public static String currentMooegeExePath = "";
        public static String currentMooegeDebugFolderPath = "";
        public static String mooegeINI = "";
        public static String compileArgs = "";
        //This paths dont change.
        public static String madcowINI = Program.programPath + @"\Tools\\Settings.ini";
        public static String msbuildPath = RuntimeEnvironment.GetRuntimeDirectory() + @"\msbuild.exe";

        public static void ExecuteCommandSync(String command)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInformation =
                new System.Diagnostics.ProcessStartInfo(command);
            
                //procStartInformation.RedirectStandardOutput = true;
                //procStartInformation.UseShellExecute = false;
                procStartInformation.CreateNoWindow = true;
                procStartInformation.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInformation;
                Console.WriteLine("Compiling newest [" + ParseRevision.developerName + "] Mooege source...");
                proc.Start();
                proc.WaitForExit();
                Console.WriteLine("Compiling newest [" + ParseRevision.developerName + "] Mooege source Complete");
                if (File.Exists(Program.programPath + "\\Tools\\" + "madcow.ini"))
                {
                    IConfigSource source = new IniConfigSource(Program.programPath + @"\Tools\madcow.ini");
                    String Src = source.Configs["Balloons"].Get("ShowBalloons");

                    if (Src.Contains("1"))
                    {
                        Form1.GlobalAccess.notifyIcon1.ShowBalloonTip(1000, "MadCow", "Compiling Complete!", ToolTipIcon.Info);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void ModifyMooegeINI()
        {
            try
            {
                //First we modify the Mooege INI storage path.
                IConfigSource source = new IniConfigSource(Compile.mooegeINI);
                string fileName = source.Configs["Storage"].Get("MPQRoot");
                if (fileName.Contains("${Root}"))
                {
                    Console.WriteLine("Modifying Mooege MPQ storage folder...");
                    IConfig config = source.Configs["Storage"];
                    config.Set("MPQRoot", Program.programPath + "\\MPQ");
                    source.Save();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Modifying Mooege MPQ storage folder Complete");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Could not modify Mooege INI FILE");
                Console.WriteLine(e);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void CreateBatchCompileFile() 
        //This build up the compile batch file, this let us compile multiple repostiories by writing repositories source path.
        {
            String val = "CompileBatch.bat";
            if (File.Exists(Program.programPath + "\\Tools\\" + val))
            {
                File.Delete(Program.programPath + "\\Tools\\" + val);
                Console.WriteLine("Deleting Batch File..");
                FileInfo fi = new FileInfo(Program.programPath + "\\Tools\\" + val);
                StreamWriter sw = fi.CreateText();
                sw.WriteLine(@"cd C:\Windows\Microsoft.NET\Framework\v4.0.30319\");
                sw.WriteLine("MSBUILD MODIFY");
                sw.Close();
                Console.WriteLine("Created Batch File.");
            }
            else
            {
                FileInfo fi = new FileInfo(Program.programPath + "\\Tools\\" + val);
                StreamWriter sw = fi.CreateText();
                sw.WriteLine(@"cd C:\Windows\Microsoft.NET\Framework\v4.0.30319\");
                sw.WriteLine("MSBUILD MODIFY");
                sw.Close();
                Console.WriteLine("Created Batch File.");
            }
        }

        public static void WriteCompileBatch()
        //This modifieds the batch to the respective repository selected by the user.
        {
            String CompileBatch = (Program.programPath + "\\Tools\\CompileBatch.bat");
            StreamReader reader = new StreamReader(CompileBatch);
            string content = reader.ReadToEnd();
            reader.Close();

            content = Regex.Replace(content, "MODIFY", Compile.compileArgs);
            StreamWriter writer = new StreamWriter(CompileBatch);
            writer.Write(content);
            writer.Close();
        }

        /*static public void WriteVbsPath()
        {
            IConfigSource source = new IniConfigSource(Program.programPath + @"\Tools\madcow.ini");
            String Src = source.Configs["ShortCut"].Get("Shortcut");
            if (Src.Contains("1"))
            {
                File.Copy(Program.programPath + "\\Resources\\ShortcutCreator.vbs", Program.programPath + "\\Tools\\ShortcutCreator.vbs", true);

                String vbsPath = (Program.programPath + "\\Tools\\ShortcutCreator.vbs");
                StreamReader reader = new StreamReader(vbsPath);
                string content = reader.ReadToEnd();
                reader.Close();


                content = Regex.Replace(content, "MODIFY", Program.programPath + @"\MadCow2011.exe");
                content = Regex.Replace(content, "WESKO", Program.programPath);
                StreamWriter writer = new StreamWriter(vbsPath);
                writer.Write(content);
                writer.Close();

                //Creates shortcut
                if (File.Exists(Program.desktopPath + "\\MadCow.lnk"))
                {
                    File.Delete(Program.desktopPath + "\\MadCow.lnk");
                    System.Diagnostics.Process.Start(Program.programPath + "\\Tools\\ShortcutCreator.vbs");
                }
                else
                    System.Diagnostics.Process.Start(Program.programPath + "\\Tools\\ShortcutCreator.vbs");
            }
            else
            {
                //don't create a shortcut
            }
        }*/
    }
}
