using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using QuartzSql;
using QuartzSQL.Task;
using System;
using System.Collections.Generic;
using System.IO;

namespace QuartzSQL
{
    public class MainService
    {
        private IScheduler scheduler = null;
        private static readonly string ServiceName = "Quartz";
        private FileTool LogTool = null;
        private static XmlConfigTool XmlTool = null;

        public MainService()
        {
            try
            {
                var Path = AppDomain.CurrentDomain.BaseDirectory + "/ServiceLog";
                var FileName = "ServiceLog.txt";
                LogTool = new FileTool(Path);
                LogTool.FileExists(FileName);
                scheduler = StdSchedulerFactory.GetDefaultScheduler();

                LogTool.AppendFile($"[{DateTime.Now.ToString()}]定时任务初始化成功:{ServiceName}{Environment.NewLine}");

                Console.WriteLine("MainService构造成功");
            }
            catch (Exception ex)
            {
                LogTool.AppendFile($"[{DateTime.Now.ToString()}]定时任务初始化失败:{ServiceName},ex:{ex.Message}{Environment.NewLine}");
            }
        }

        public void OnStart()
        {
            try
            {
                LogTool.AppendFile($"[{DateTime.Now.ToString()}]{string.Format("Windows服务 {0} 正在启动...", ServiceName)}{Environment.NewLine}");
                scheduler.Start();
                try
                {
                    IJobDetail job1 = JobBuilder.Create<SqlExecuteTask>().WithIdentity("Job1", "group1").Build();
                    XmlTool = new XmlConfigTool("SqlServerConfig.xml"); 
                    var Cron = XmlTool.GetValue("Cron");
                    ITrigger trigger1 = TriggerBuilder.Create().WithIdentity("mytrigger", "group1").StartNow().WithCronSchedule(Cron).Build();


                    scheduler.ScheduleJob(job1, trigger1);      //把作业，触发器加入调度器。

                }
                catch (Exception ex) { }
                LogTool.AppendFile($"[{DateTime.Now.ToString()}]{string.Format("Windows服务 {0} 已经启动", ServiceName)}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                LogTool.AppendFile($"[{DateTime.Now.ToString()}]windows服务启动失败:{ServiceName},ex:{ex.Message}{Environment.NewLine}");
            }
        }

        public void OnStop()
        {
            try
            {
                LogTool.AppendFile($"[{DateTime.Now.ToString()}]{string.Format("Windows服务 {0} 正在停止...", ServiceName)}{Environment.NewLine}");
                scheduler.Shutdown(false);
                LogTool.AppendFile($"[{DateTime.Now.ToString()}]{string.Format("Windows服务 {0} 已经停止", ServiceName)}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                LogTool.AppendFile($"[{DateTime.Now.ToString()}]{string.Format("Windows服务 {0} 停止失败,ex:" + ex.Message, ServiceName)}{Environment.NewLine}");
            }
        }

        public void OnPause()
        {
            try
            {
                LogTool.AppendFile($"[{DateTime.Now.ToString()}]{string.Format("Windows服务 {0} 正在暂停...", ServiceName)}{Environment.NewLine}");
                scheduler.PauseAll();
                LogTool.AppendFile($"[{DateTime.Now.ToString()}]{string.Format("Windows服务 {0} 已经暂停", ServiceName)}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                LogTool.AppendFile($"[{DateTime.Now.ToString()}]{string.Format("Windows服务 {0} 暂停失败,ex:" + ex.Message, ServiceName)}{Environment.NewLine}");
            }
        }

        public void OnContinue()
        {
            try
            {
                LogTool.AppendFile($"[{DateTime.Now.ToString()}]{string.Format("Windows服务 {0} 准备继续执行...", ServiceName)}{Environment.NewLine}");
                scheduler.ResumeAll();
                LogTool.AppendFile($"[{DateTime.Now.ToString()}]{string.Format("Windows服务 {0} 继续执行", ServiceName)}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                LogTool.AppendFile($"[{DateTime.Now.ToString()}]{string.Format("Windows服务 {0} 继续执行失败,ex" + ex.Message, ServiceName)}{Environment.NewLine}");
            }
        }
    }
}
