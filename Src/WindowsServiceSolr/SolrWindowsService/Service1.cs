using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Net;
using System.IO;


namespace SolrWindowsService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
            if (!System.Diagnostics.EventLog.SourceExists("Solr_Service"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "Solr_Service", "Application");
            }
            eventLog1.Source = "Solr_Service";
            eventLog1.Log = "Application";
        }

        public void time_elapsed(object sender, ElapsedEventArgs e)
        {
            string sURL;
            sURL = "http://localhost:8983/solr/dataimport?command=full-import";

            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);

            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();
            
            eventLog1.WriteEntry("Mail Sending on " + DateTime.Now.ToString());

            //SendEmail("Muhilan@maa.com.my", "tmuhilan@gmail.com", "Automatic Mail sending", "Successfully working contact tmuhilan@gmail.com");

        }
        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("In OnStart");
            System.Timers.Timer time = new System.Timers.Timer();
            time.Start();
            time.Interval = 300000;
            time.Elapsed += time_elapsed;
            
        }
        protected override void OnStop()
        {
            eventLog1.WriteEntry("In onStop.");
        }
    }
}