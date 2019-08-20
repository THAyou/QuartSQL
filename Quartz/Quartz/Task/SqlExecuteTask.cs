using Quartz;
using QuartzSql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzSQL.Task
{
    public class SqlExecuteTask : IJob
    {
        private FileTool LogTool = null;
        public void Execute(IJobExecutionContext context)
        {

            //生成日志工具
            var LogPath = AppDomain.CurrentDomain.BaseDirectory + "/TaskLog";
            var FileName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            LogTool = new FileTool(LogPath);
            LogTool.FileExists(FileName);
            try
            {
                LogTool.AppendFile($"[{DateTime.Now.ToString()}]开始执行Sql{Environment.NewLine}");
                var SQLFilePath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "/SQLFile";
                var SQLFileTool = new FileTool(SQLFilePath);
                var FileList = SQLFileTool.FileDetailsList;
                FileList.ForEach(m => {
                    try
                    {
                        LogTool.AppendFile($"[{DateTime.Now.ToString()}]开始执行Sql文件[{m.FileName}]{Environment.NewLine}");
                        var sql = SQLFileTool.GetFileContent(SQLFilePath, m.FileName);
                        SqlHelp.ExSql(sql, LogTool);
                        LogTool.AppendFile($"[{DateTime.Now.ToString()}]Sql文件[{m.FileName}]执行成功{Environment.NewLine}");
                    }
                    catch (Exception ex)
                    {
                        LogTool.AppendFile($"[{DateTime.Now.ToString()}]Sql文件[{m.FileName}]执行失败,ex:{ex.Message}{Environment.NewLine}");
                    }
                });
            }
            catch (Exception ex)
            {
                LogTool.AppendFile($"[{DateTime.Now.ToString()}]任务执行失败,ex:{ex.Message}{Environment.NewLine}");
            }


        }
    }
}
