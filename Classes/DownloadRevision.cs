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
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Net;

namespace MadCow
{
    class DownloadRevision
    {
        public static void DownloadLatest()
        {
            try
            {
                WebClient client = new WebClient();
                Uri uri = new Uri("https://github.com/mooege/mooege/zipball/master");
                Console.WriteLine("Downloading latest Mooege revision [" + Program.lastRevision + "]...");
                client.DownloadFile(uri, Program.programPath + "/Mooege.zip");
                Console.WriteLine("Downloading latest Mooege revision [" + Program.lastRevision + "] Complete");         
            }

            catch (WebException webEx)
            {
                Console.WriteLine(webEx.ToString());
                if (webEx.Status == WebExceptionStatus.ConnectFailure)
                {
                    Console.WriteLine("Error: Connection Failure!");
                }
            }
        }
    }
}
