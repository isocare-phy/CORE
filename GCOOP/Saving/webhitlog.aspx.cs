using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.IO;
using System.Text;
using System.Diagnostics;
using EncryptDecryptEngine;
using DataLibrary;

namespace Saving
{
    public partial class webhitlog : System.Web.UI.Page
    {
        public string coreURI = WebUtil.coreURI;
        public string currentURI = WebUtil.GetVirtualDirectory();
        public string prefix = "/Applications/temp/";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack)
            {
                var hgws = new Decryption().DecryptStrBase64( Request.Form["hgws"]);
                var hgcallback = Request.Form["hgcallback"];
                var hgcururl = Request.Form["hgcururl"];
                var hgfilename = Request.Form["hgfilename"];
                var hghit_date = Request.Form["hghit_date"];
                var hghtmldata = Request.Form["hghtmldata"];
                if (hgcallback.Length > 0 && hgcallback.ToLower().IndexOf("save")>=0)
                {
                    hghtmldata = hghtmldata.Replace("</body>", "<font color=red ><hr/>**** ข้อมูล ณ กดตอนกด ปุ่มบันทึก (Save) ****</hr/></font><br/></body>");
                }
                hghtmldata = hghtmldata.Replace("</body>", "<font color=blue >[" + hghit_date + "]" + hgcururl + "</font></body>");
                var hgsession_id = Request.Form["hgsession_id"];
                var hgusername = Request.Form["hgusername"];
                if (hgfilename != null && hgfilename.Length > 0)
                {
                    XmlConfigService xmlconfig = new XmlConfigService(WebUtil.GetGcoopPath());
                    prefix = xmlconfig.CentLogSaveURI;
                    String rootPath = xmlconfig.CentLogSavePath.Length <= 0 ? Server.MapPath("~/") : xmlconfig.CentLogSavePath;
                    //File.WriteAllText(Server.MapPath("~/" + hgfilename + ".html"), hghtmldata);
                    String curURI = Request.Url.AbsoluteUri;
                    curURI = curURI.Substring(0, curURI.LastIndexOf("/"));
                    String filepathtemp = rootPath+ prefix;
                    String fileurl = curURI + prefix + hgfilename + ".html";
                    String filepath = rootPath+("" + prefix + hgfilename + ".html");
                    String fileimgpath = rootPath + ("" + prefix + hgfilename + ".jpg");
                    TextWriter tw = new StreamWriter(filepath, false, Encoding.UTF8);
                    tw.WriteLine(hghtmldata);
                    tw.Close();

                    LaunchCommandApp(rootPath, fileurl, fileimgpath);

                    String htmlfilename=hgfilename + ".html";
                    String htmlurl=curURI+ prefix+htmlfilename;
                    String htmlfilepath = rootPath + ("" + prefix + hgfilename + ".html");
                    String imgfilename=hgfilename + ".jpg";
                    String imgurl = curURI + prefix + imgfilename;
                    String imgfilepath = rootPath + ("" + prefix + hgfilename + ".jpg");
                    saveHitLog(hgws,hgcallback, hghit_date, hgsession_id, htmlfilename, htmlurl, htmlfilepath, imgfilename, imgurl,  imgfilepath);
                    WebUtil.cleanupHitLogs(filepathtemp, hgws);
                }
                if (hgcallback.Length > 0)
                {
                    var callback = "parent." + hgcallback + ";";
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallBackScript", callback, true);
                }
            }
        }

        public void saveHitLog(String SsConnectionString,String hgcallback, String hghit_date, String hgsession_id, String htmlfilename, String htmlurl, String htmlfilepath, String imgfilename, String imgurl, String imgfilepath)
        {
            try
            {
                Sta ta = new Sta(SsConnectionString);
                try
                {
                    ta.Exe("alter table cmhitlog add(htmlfilename varchar2(150),htmlurl varchar2(150),htmlfilepath varchar2(250))");
                    ta.Exe("alter table cmhitlog add(imgfilename varchar2(150),imgurl varchar2(150),imgfilepath varchar2(250)) ");
                }
                catch { }
                try
                {
                    String serverMassage="";
                    if (hgcallback.Length > 0 && hgcallback.ToLower().IndexOf("save") >= 0)
                    {
                        String nowStr = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        serverMassage = "ข้อมูล ณ กดตอนกด ปุ่มบันทึก (Save)";
                        try
                        {
                            String sqlCentLog = @"
                                    insert into cmhitlog
                                    (
                                        hit_date,               server_ip,              client_ip,
                                        application,            coop_id,                coop_control,
                                        current_page,           current_pagedesc,       message_type,
                                        username,               url_absolute,           methode, 
                                        jspostback,             webservice,             webservice_ram,
                                        webservicereport,       webservicereport_ram,   server_message,
                                        load_time,              session_id,             external_user,
                                        htmlfilename,           htmlurl,                htmlfilepath,
                                        imgfilename,            imgurl,                  imgfilepath
                                    )
                                    (select * from ( select 
                                        to_timestamp('" + nowStr + @"','yyyymmddHH24missff3') as hit_date,                 
                                        server_ip,              client_ip,
                                        application,            coop_id,                coop_control,
                                        current_page,           current_pagedesc,       message_type,
                                        username,               url_absolute,           methode, 
                                        jspostback,             webservice,             webservice_ram,
                                        webservicereport,       webservicereport_ram,   '" + serverMassage + @"' as server_message,
                                        load_time,          '" + hgsession_id + @"' as session_id ,          external_user,
                                        '" + htmlfilename + @"' as htmlfilename,'" + htmlurl + @"' as htmlurl,'" + htmlfilepath + @"' as htmlfilepath,
                                        '" + imgfilename + @"' as imgfilename,'" + imgurl + @"' as imgurl,'" + imgfilepath + @"'  as imgfilepath
                                     from cmhitlog where session_id='" + hgsession_id + @"' order by hit_date desc )
                                     where rownum=1
                                    )";
                            //ta.Exe(sqlCentLog);
                        }
                        catch { }
                    }
                    else
                    {

                        String sql = @"update cmhitlog set 
                                    htmlfilename='" + htmlfilename + @"',
                                    htmlurl='" + htmlurl + @"',
                                    htmlfilepath='" + htmlfilepath + @"',
                                    imgfilename='" + imgfilename + @"',
                                    imgurl='" + imgurl + @"',
                                    imgfilepath='" + imgfilepath + @"' 
                                   where hit_date =cast(to_timestamp('" + hghit_date + @"','yyyymmddHH24missff3') as date) 
                                    and session_id='" + hgsession_id + @"'";
                        ta.Exe(sql);
                    }
                }
                catch { }
                finally
                {
                    ta.Close();
                }
            }
            catch { }
        }

        public void LaunchCommandApp(String rootPath,String url,String saveFile)
        {
            // For the example
            string ex1 = rootPath;
            string ex2 = "/wkhtmltox/bin/";

            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = ex1+ex2+"wkhtmltoimage.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = " " + url + " " + saveFile;

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch
            {
                // Log error.
            }
        }
    }
}