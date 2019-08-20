using Quartz;
using Quartz.Xml.JobSchedulingData20;
using QuartzSQL.Task;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Topshelf;

namespace QuartzSQL
{
    public class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.RunAsLocalSystem();

                x.SetServiceName("Quartz");
                x.SetDisplayName("SQL定时调度服务");

                x.Service<MainService>(sc =>
                {
                    sc.ConstructUsing(() => new MainService());
                    sc.WhenStarted(a => a.OnStart());
                    sc.WhenStopped(a => a.OnStop());
                    sc.WhenContinued(a => a.OnContinue());
                    sc.WhenPaused(a => a.OnPause());
                });
            });
        }
    }
}
