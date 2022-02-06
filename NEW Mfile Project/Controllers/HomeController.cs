using MFaaP.MFWSClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization.Json;
using System.Net;
using NEW_Mfile_Project.Models;
using System.Threading.Tasks;
using System.Net.Http;
using System.Dynamic;
using NEW_Mfile_Project.App_Start;
using Microsoft.Net.Http.Headers;

namespace NEW_Mfile_Project.Controllers
{
    public class HomeController : Controller
    {

        public static string domain = "http://127.0.0.1/";

        public static dynamic comments = null;

        ObjectProps obj = new ObjectProps();
        public ActionResult Comments()
        {
            List<ObjectProps> objectprop = new List<ObjectProps>();
            //objectprop = obj.ReadobjectProps();
            //commentsCount = objectprop.Count();

            return View(objectprop);
        }

        

        public ActionResult AddComment(string ID, string name)
        {
            //ViewBag.HtmlData = "<p style=\"font-size: 16px; line-height: 1.8em; color: rgb(51, 51, 51); font-family: &quot;Open Sans&quot;, sans-serif;\"><b>WYSIWYG Editor</b>&nbsp;is very useful when you want to allow the user to insert the formatted text content in textarea input field. Generally, the WYSIWYG editor is driven by JavaScript, converts formatted text to HTML before submitting the web form. The user can insert HTML content in the textarea and change the format directly in the text editor. When the editor content is submitted, the exact text format is posted as HTML to the server-side.</p><p style=\"font-size: 16px; line-height: 1.8em; color: rgb(51, 51, 51); font-family: &quot;Open Sans&quot;, sans-serif;\">There are many WYSIWYG editor plugins are available to&nbsp;<a href=\"https://www.codexworld.com/add-wysiwyg-html-editor-to-textarea-website/\" style=\"background-image: initial; background-position: initial; background-size: initial; background-repeat: initial; background-attachment: initial; background-origin: initial; background-clip: initial; color: rgb(66, 139, 202); border-bottom: 1px solid rgb(255, 115, 97);\">add text editor to textarea using jQuery</a>. CKEditor is the one of them to add a rich text editor to textarea. CKEditor is a WYSIWYG editor plugin that allows converting textarea to the fully-featured HTML editor. In this tutorial, we will show how can you&nbsp;<b>add CKEditor to textarea</b>&nbsp;in minutes.</p><p style=\"font-size: 16px; line-height: 1.8em; color: rgb(51, 51, 51); font-family: &quot;Open Sans&quot;, sans-serif;\">Before getting started, download the&nbsp;<a href=\"https://ckeditor.com/ckeditor-4/download/\" target=\"_blank\" style=\"background-image: initial; background-position: initial; background-size: initial; background-repeat: initial; background-attachment: initial; background-origin: initial; background-clip: initial; color: rgb(66, 139, 202); border-bottom: 1px solid rgb(255, 115, 97);\">latest version of CKEditor</a>&nbsp;plugin. Extract the downloaded archive and place it in the root of your web application directory. You don’t need to download the CKEditor separately, all the required files are included in our source code.</p><h2 style=\"font-family: &quot;Open Sans&quot;, sans-serif; font-weight: bold; line-height: 1.2em; color: rgb(51, 51, 51); margin: 25px 0px 15px; font-size: 20px;\">Add CKEditor to Textarea</h2>";

            if (ID == null || name == null)
            {
                return View("UploadComment");
            }

            if (Session["HtmlData"] != null)
            {
                TempData["HtmlData"] = Session["HtmlData"] as string;
            }

            if (TempData["HtmlData"] != null)
            {
                ViewBag.HtmlData = TempData["HtmlData"] as string;
            }

            TempData["ID"] = ID;
            TempData["name"] = name;

            comments = obj.ReadCombineComments(int.Parse(ID));
            if (comments != null)
            {
                TempData["paracount"] = comments.ParaCount;
            }
            return View("UploadComment");
        }

        public ActionResult UploadComment()
        {
            //ViewBag.HtmlData = "<p style=\"font-size: 16px; line-height: 1.8em; color: rgb(51, 51, 51); font-family: &quot;Open Sans&quot;, sans-serif;\"><b>WYSIWYG Editor</b>&nbsp;is very useful when you want to allow the user to insert the formatted text content in textarea input field. Generally, the WYSIWYG editor is driven by JavaScript, converts formatted text to HTML before submitting the web form. The user can insert HTML content in the textarea and change the format directly in the text editor. When the editor content is submitted, the exact text format is posted as HTML to the server-side.</p><p style=\"font-size: 16px; line-height: 1.8em; color: rgb(51, 51, 51); font-family: &quot;Open Sans&quot;, sans-serif;\">There are many WYSIWYG editor plugins are available to&nbsp;<a href=\"https://www.codexworld.com/add-wysiwyg-html-editor-to-textarea-website/\" style=\"background-image: initial; background-position: initial; background-size: initial; background-repeat: initial; background-attachment: initial; background-origin: initial; background-clip: initial; color: rgb(66, 139, 202); border-bottom: 1px solid rgb(255, 115, 97);\">add text editor to textarea using jQuery</a>. CKEditor is the one of them to add a rich text editor to textarea. CKEditor is a WYSIWYG editor plugin that allows converting textarea to the fully-featured HTML editor. In this tutorial, we will show how can you&nbsp;<b>add CKEditor to textarea</b>&nbsp;in minutes.</p><p style=\"font-size: 16px; line-height: 1.8em; color: rgb(51, 51, 51); font-family: &quot;Open Sans&quot;, sans-serif;\">Before getting started, download the&nbsp;<a href=\"https://ckeditor.com/ckeditor-4/download/\" target=\"_blank\" style=\"background-image: initial; background-position: initial; background-size: initial; background-repeat: initial; background-attachment: initial; background-origin: initial; background-clip: initial; color: rgb(66, 139, 202); border-bottom: 1px solid rgb(255, 115, 97);\">latest version of CKEditor</a>&nbsp;plugin. Extract the downloaded archive and place it in the root of your web application directory. You don’t need to download the CKEditor separately, all the required files are included in our source code.</p><h2 style=\"font-family: &quot;Open Sans&quot;, sans-serif; font-weight: bold; line-height: 1.2em; color: rgb(51, 51, 51); margin: 25px 0px 15px; font-size: 20px;\">Add CKEditor to Textarea</h2>";

            if (TempData["HtmlData"] != null)
            {
                ViewBag.HtmlData = TempData["HtmlData"] as string;
            }

            return View();
        }

        [HttpPost]
        public ActionResult UploadComment(int D_ID)
        {
            dynamic obj1 = new ExpandoObject();


            try
            {
                List<dynamic> objectprop = new List<dynamic>();
                objectprop = obj.ReadobjectProps(D_ID);
                if (objectprop != null && objectprop.Count > 0)
                {
                    obj1 = objectprop[0];
                    TempData["HtmlData"] = obj1.Data;
                    Session["CommentID"] = obj1.ID;
                    TempData["name"] = obj1.Obj_name;
                }
                //string ID = TempData["ID"].ToString();
                TempData["ID"] = D_ID;
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, message = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveCommentToMfile(string Message, string name, string lpassword, string ID, string Fname)
        {
            try
            {
                if (ID == "0")
                {
                    return Json(new { success = false, message = "No Object Was Selected" }, JsonRequestBehavior.AllowGet);
                }
                Session["HtmlData"] = null;
                //GET Token
                string token = Mfiles.AuthToken();



                //Get Signature Image
                //string image = CreateLogFiles.Test_image;
                string image = "";
               
                image = Mfiles.Signed(token, name, lpassword).Result;
                


                //System.Drawing.Image imageq = obj.DownloadImageFromUrl(image);
                if (image == "403")
                {
                    ViewBag.Title = "Invalid UserName / Login Password";
                    return Json(new { success = false, message = "Invalid UserName / Login Password" }, JsonRequestBehavior.AllowGet);

                }



                if (image != "")
                {
                    //int paraCount = CreateParaCount(int.Parse(ID), obj);
                    int paraCount = 0;
                    if (comments !=null)
                    {
                        paraCount = comments.ParaCount;
                    }
                    paraCount++;
                    #region making HTML1
                    string img = "";
                    
                   
                        img = "<img src='data:image/jpeg;base64," + image + "' alt='Sign Image' width='40%' height='100%'>";
                    
                  
                    
                    string signdate = "<p>Signed Date : " + DateTime.Now.ToString("dd-MM-yyy HH:mm:ss") + "</p>";
                    string Html = @"
                     <table class='customers' id='PARA" + paraCount + @"'>
                            <tr><th>PARA" + paraCount +"-"+name+ @"</th></tr>
                            <tr><td>" + Message + @"</td></tr>
                            <tr><td>" + img + @"</td></tr>
                            <tr><td>" + signdate + @"</td></tr>
                           </table>";

                    #endregion

                   
                    #region Making HTML2
                    string html1 = @"
                    <!doctype html>
                      <html lang='en'>
                         <head>
                            <style>
                                #customers {
                                  font-family: Arial, Helvetica, sans-serif;
                                  border-collapse: collapse;
                                  width: 100%;
                                }

                                p {
                                  font-weight: lighter;
                                }
                                #customers td, #customers th {
                                  border: 1px solid #ddd;
                                  padding: 8px;
                                }

                                

                                #sigimage{
                                    width : 40%;
                                    height : 35%;
                                }
                            
                                </style>
                            <!--Required meta tags -->
                            <meta charset = 'utf-8'>
                            <meta name = 'viewport' content = 'width=device-width, initial-scale=1, shrink-to-fit=no'>
                            <!--Bootstrap CSS-->
                        <link href='~/Content/bootstrap1.min.css' rel='stylesheet' />   
                        <title> Hello, world! </title>
                         </head>
                            <body style='margin: 30px'>

                       <div style='margin: 16px'>
                        <img style='width: 20%;' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAASwAAADWCAYAAABrA7++AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAFi0SURBVHhe7Z0HvBTFtu6v77573j1e7/F4jjl7jJhRMB4RVDAgSFBAUTKoBMmggAQVUBCUJIjkoGQRMJIzSM4SJIOIRAXFcE69+td0bdtm9UzP3jOzZ8/u7/f7ZsPe3dU1M9Vfr1q1aq3/CBHCjQeLF2/cqGHD6QUKFHhM//c/I78NESJEiDTCKaec8sCrr766Yv/+/Qps375Nvf32W5vvueeuZvrPf48cFSJEiBC5i3MqV37yvVkzZxqhAv/+97+dfyl14Lvv1MABA/aVLv1oZ33spZFTQoQIESLFuO6aa2q++26/vb/88osjTxGxctPixx9/VKM++ODYM8883V+felOkhRAhQoRIPm5q2rjx1E0bNzpydLJQeenGlCmT/1W3bt3xup17I82FCBEiROLx54dKlHht/PjxPzvaI4pTNLoxb9489dJLL84498wzyzrthwgRIkTOcfpppz3apcsb6w4dOuTITfxi5aYba9asUZ06dVx+ww3X1dSXOjVyxRAhQoSIHxdVr159xMIFCxx5yZlQeenGjh07VK8ePb4uWqRIC33dsyKXDxEiRIgAuLVgwfqDBw064BYWr+AkkhYHDx5UgwYN2l+mdOkuuhuXR3oTIkSIEDJuf+mlF+ds37bNkZDkCpWXFj/99JMaPXr08apVqw7QfSoY6VqIECFCRPCXxx4r9eakSZP+5WiGKCipohuffPKxeqF+/Q91H++LdDVEiBD5Fuefc84Tb7/11pYffvjBkYjcFSs33ViwYIFq1eqlWbq/j+tunxLpfYgQIfILrnjuuWfHLV++3JGE9BEqL91Yt26der1z55U33XRTHf0e/ifyVkKECJGpOOWuu+5qNmLE8KOOBogikY50Y9eunapXr57b7ita9CX9ns6JvLUQIUJkEu5t367d4j179ji3fd4RKy8tDh8+rIYMHvxduTJluun3d0XkbYYIESIv4+8VKjze54svPndu87wrVF5anDhxQo0bO/bH6tWrDtbvt1DkbYcIESJP4fLLL3+mT5/eO0+c+Mm5tTNHrNx047PPPlMNG77wEalvnI8hRIgQaY5rGzZsMGXd2rXObZyZQuWlG4sWLVJtWreee9FF51XQn8f/iXwsIUKESCf86b777nt59OjRPzr3rXhjZzrd2LBhg3rjjddX33LLzc/rz+e0yMcUIkSIXMWf/vSnEh07vrbqu+++c27V/ClWbrqxZ/du9U7v3juK339/a/1xnRf51EKECJFqnPfMM5UHzZkz27k1Q6GSaHHkyBE1dOiQg4+XL/+W/uyuinyEIUKESDpuuOHa2u/177/v119/dW7HUKxi0YKMqePHjztRq0aNYfqjvC3yiYYIESIZKNi8ebNpmzdvdm6/UKjipRtffPGFaty48WQ9rX7Q+XxDhAiRAJxa8uGHO3344YSshOrSzRgyON348ssvVdu2bef945JLKunP+v9GPvIQIULEjb/97a+l3+zadQM+GAvpBgyZPbqxceNG1bVLl3W33XprPf3R/yXyDYQIESIILqlVs+b7ixcvcm6nUKiSSTe+2btX9e37zs4SJUq01d/D+ZGvI0SIECIKFSr0wpAhQ35PqK4h3WQhk0OLo0ePquHDhh2u+MQTPfTXck3k2wkRIoTFna1atZpHvnML6YZKKJ3rWIjH5FNasBo7YcKEn2vXqjVCf0d3RL6qECHyL04vW/axtz6eMsW5RVIjHBYkyhv1wQfmxrSQjs9NWkh/SzbdmD5tmmrapMknp5166sPOdxciRP7BBRecX7FHj7e3Hj92zLklkn9TWuzfv599d+ofl12qzjn7TPXwQw+qcePGqd9++805IveFy2LjkSNqi56iWUjHJptuLFu2VLVv127hFVdcVll/jf8V+TZDhMhcXFXv+ec/XLlyhXMLpE6owLBhw9RthQupc885WxW45mp1/XXXqssuuVidf965quQjjzAFUv/6V1a6d7G9ZFK/mOse01Zf36++UkU++0wV+/xz1WXtWrU9l1M7u7F58ybVrVvX9bffXugF/Z3+NfLVhgiROfjPe+65u+X7I0dm3XXSTZFoWixcuFCVL1dWnXfu2eqKy/9hhOq6awv8gZc6wlXq0ZJq4sSJfzjf224yaDF9715VcfZsdaueKiNWsJD+9/1ffJF2wrVv3z7Vr2/fPQ89VKKD/o4vjHzVIULkbRTr0KH90m++2esM8+TfaBZ79c3fokVzddmll6iLL7rwJJGSiHBdcP55qnTpUmrSpI+cliKQrpVTWmzVQtRy6VJ128cfq7s/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' />
                        </div>
                   
                          <br>
                           <div class='container'>
        
  
        <div class='row border'>
            <div class='col-3 border'><h1> PARA1</h1></div>


            <div class='col border-bottom'><p>" + Message + @"</p></div>


                        <div class='col border-bottom'>

                        " + img + @"
                         </div>


                        <div class='col '>" + signdate + @"</div>
                        </div>
  
                          </div>
                        </body>
                        </html> ";



                    var data1 = HttpUtility.HtmlDecode(html1);
                    #endregion


                    
                    #region
                    string paras = Html;
                   
                    if (comments !=null)
                    {
                        paras = comments.Data + "<br>" + paras;
                    }

                    var res = obj.InsertCombineComments(paras, int.Parse(ID), Fname, paraCount);

                    if (res != "true")
                    {
                        return Json(new { success = false, message = "Check Sql Server" }, JsonRequestBehavior.AllowGet);
                    }

                    string CombineHtml = @"
                    <!doctype html>
                      <html lang='en'>
                         <head>
                            <style>
                                .customers {
                                  font-family: Arial, Helvetica, sans-serif;
                                  border-collapse: collapse;
                                  width: 100%;
                                }
                                div.c {
                                  text-align: right;
                                } 
                                .customers td, #customers th {
                                  border: 1px solid #ddd;
                                  padding: 8px;
                                }

                                //#customers tr:nth-child(even){background-color: #f2f2f2;}

                                //#customers tr:hover {background-color: #ddd;}

                                .customers th {
                                  padding-top: 12px;
                                  padding-bottom: 12px;
                                  text-align: left;
                                  background-color: #58508e;
                                  color: white;
                                }
                                </style>
                            <!--Required meta tags -->
                            <meta charset = 'utf-8'>
                            <meta name = 'viewport' content = 'width=device-width, initial-scale=1, shrink-to-fit=no'>
                            <!--Bootstrap CSS-->
                             <link href='~/Content/bootstrap2.min.css' rel='stylesheet' />  
                               <title> Hello, world! </title>
                         </head>
                            <body style='margin: 30px'>
                           <div style='margin: 16px'>
                        <img style='width: 20%;' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAASwAAADWCAYAAABrA7++AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAFi0SURBVHhe7Z0HvBTFtu6v77573j1e7/F4jjl7jJhRMB4RVDAgSFBAUTKoBMmggAQVUBCUJIjkoGQRMJIzSM4SJIOIRAXFcE69+td0bdtm9UzP3jOzZ8/u7/f7ZsPe3dU1M9Vfr1q1aq3/CBHCjQeLF2/cqGHD6QUKFHhM//c/I78NESJEiDTCKaec8sCrr766Yv/+/Qps375Nvf32W5vvueeuZvrPf48cFSJEiBC5i3MqV37yvVkzZxqhAv/+97+dfyl14Lvv1MABA/aVLv1oZ33spZFTQoQIESLFuO6aa2q++26/vb/88osjTxGxctPixx9/VKM++ODYM8883V+felOkhRAhQoRIPm5q2rjx1E0bNzpydLJQeenGlCmT/1W3bt3xup17I82FCBEiROLx54dKlHht/PjxPzvaI4pTNLoxb9489dJLL84498wzyzrthwgRIkTOcfpppz3apcsb6w4dOuTITfxi5aYba9asUZ06dVx+ww3X1dSXOjVyxRAhQoSIHxdVr159xMIFCxx5yZlQeenGjh07VK8ePb4uWqRIC33dsyKXDxEiRIgAuLVgwfqDBw064BYWr+AkkhYHDx5UgwYN2l+mdOkuuhuXR3oTIkSIEDJuf+mlF+ds37bNkZDkCpWXFj/99JMaPXr08apVqw7QfSoY6VqIECFCRPCXxx4r9eakSZP+5WiGKCipohuffPKxeqF+/Q91H++LdDVEiBD5Fuefc84Tb7/11pYffvjBkYjcFSs33ViwYIFq1eqlWbq/j+tunxLpfYgQIfILrnjuuWfHLV++3JGE9BEqL91Yt26der1z55U33XRTHf0e/ifyVkKECJGpOOWuu+5qNmLE8KOOBogikY50Y9eunapXr57b7ita9CX9ns6JvLUQIUJkEu5t367d4j179ji3fd4RKy8tDh8+rIYMHvxduTJluun3d0XkbYYIESIv4+8VKjze54svPndu87wrVF5anDhxQo0bO/bH6tWrDtbvt1DkbYcIESJP4fLLL3+mT5/eO0+c+Mm5tTNHrNx047PPPlMNG77wEalvnI8hRIgQaY5rGzZsMGXd2rXObZyZQuWlG4sWLVJtWreee9FF51XQn8f/iXwsIUKESCf86b777nt59OjRPzr3rXhjZzrd2LBhg3rjjddX33LLzc/rz+e0yMcUIkSIXMWf/vSnEh07vrbqu+++c27V/ClWbrqxZ/du9U7v3juK339/a/1xnRf51EKECJFqnPfMM5UHzZkz27k1Q6GSaHHkyBE1dOiQg4+XL/+W/uyuinyEIUKESDpuuOHa2u/177/v119/dW7HUKxi0YKMqePHjztRq0aNYfqjvC3yiYYIESIZKNi8ebNpmzdvdm6/UKjipRtffPGFaty48WQ9rX7Q+XxDhAiRAJxa8uGHO3344YSshOrSzRgyON348ssvVdu2bef945JLKunP+v9GPvIQIULEjb/97a+l3+zadQM+GAvpBgyZPbqxceNG1bVLl3W33XprPf3R/yXyDYQIESIILqlVs+b7ixcvcm6nUKiSSTe+2btX9e37zs4SJUq01d/D+ZGvI0SIECIKFSr0wpAhQ35PqK4h3WQhk0OLo0ePquHDhh2u+MQTPfTXck3k2wkRIoTFna1atZpHvnML6YZKKJ3rWIjH5FNasBo7YcKEn2vXqjVCf0d3RL6qECHyL04vW/axtz6eMsW5RVIjHBYkyhv1wQfmxrSQjs9NWkh/SzbdmD5tmmrapMknp5166sPOdxciRP7BBRecX7FHj7e3Hj92zLklkn9TWuzfv599d+ofl12qzjn7TPXwQw+qcePGqd9++805IveFy2LjkSNqi56iWUjHJptuLFu2VLVv127hFVdcVll/jf8V+TZDhMhcXFXv+ec/XLlyhXMLpE6owLBhw9RthQupc885WxW45mp1/XXXqssuuVidf965quQjjzAFUv/6V1a6d7G9ZFK/mOse01Zf36++UkU++0wV+/xz1WXtWrU9l1M7u7F58ybVrVvX9bffXugF/Z3+NfLVhgiROfjPe+65u+X7I0dm3XXSTZFoWixcuFCVL1dWnXfu2eqKy/9hhOq6awv8gZc6wlXq0ZJq4sSJfzjf224yaDF9715VcfZsdaueKiNWsJD+9/1ffJF2wrVv3z7Vr2/fPQ89VKKD/o4vjHzVIULkbRTr0KH90m++2esM8+TfaBZ79c3fokVzddmll6iLL7rwJJGSiHBdcP55qnTpUmrSpI+cliKQrpVTWmzVQtRy6VJ128cfq7s/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' />
                        </div>
                            <br>
                         " + paras + @"
                    <br>


                           </body>
                      </html> ";
                    #endregion




                    byte[] fileBytes = RichTextEditorViewModel.GeneratePDF(CombineHtml, data1);



                   

                    MemoryStream stream = new MemoryStream(fileBytes);



                    var createdObject = new ObjectVersion();
                    if (comments!= null && comments.ParaCount > 0)
                    {
                        createdObject = Mfiles.UpdateComments(stream, token, ID).Result;
                    }
                    else
                    {
                     createdObject = Mfiles.AddFilesToMF_Object(stream, token, ID, "Comments").Result;

                    }
                    
                    var res1 = obj.DeleteobjectProps(int.Parse(ID));
                    

                }
                else
                {
                    ViewBag.Title = "Invalid UserName / Login Password";
                    return Json(new { success = false, message = "Invalid Signature Password" }, JsonRequestBehavior.AllowGet);
                }



            }
            catch (Exception ex)
            {
                CreateLogFiles.ErrorLog(ex.Message + " " + ex.StackTrace);

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, message = "Comment Save. You can't Change It Now" }, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        public ActionResult SaveCommentToMfile_Live(string name, string Image)
        {
            try
            {
              string ID= TempData["DocID"].ToString();
                string Fname= TempData["DocName"].ToString();
                string Message = TempData["Comment"].ToString();

                if (ID == "0")
                {
                    return Json(new { success = false, message = "No Object Was Selected" }, JsonRequestBehavior.AllowGet);
                }
                Session["HtmlData"] = null;
                //GET Token
                string token = Mfiles.AuthToken();



                //Get Signature Image
                //string image = CreateLogFiles.Test_image;
                string image = "";
                if (Image != "")
                {
                    image = Image;
                }
                else
                {

                    //image = Mfiles.Signed(token, name, lpassword).Result;
                }


                //System.Drawing.Image imageq = obj.DownloadImageFromUrl(image);
                if (image == "")
                {
                    ViewBag.Title = "Invalid UserName / Login Password";
                    return Json(new { success = false, message = "Invalid UserName / Login Password" }, JsonRequestBehavior.AllowGet);

                }



                if (image != "")
                {
                    //int paraCount = CreateParaCount(int.Parse(ID), obj);
                    int paraCount = 0;
                    if (comments != null)
                    {
                        paraCount = comments.ParaCount;
                    }
                    paraCount++;
                    #region making HTML1
                    string img = "";
                    if (Image != "")
                    {
                        img = image;
                    }
                    else
                    {
                        img = "<img src='data:image/jpeg;base64," + image + "' alt='Sign Image' width='40%' height='100%'>";
                    }



                    string signdate = "<p>Signed Date : " + DateTime.Now.ToString("dd-MM-yyy") + "</p>";
                    string Html = @"
                     <table class='customers' id='PARA" + paraCount + @"'>
                            <tr><th>PARA" + paraCount + "-" + name + @"</th></tr>
                            <tr><td>" + Message + @"</td></tr>
                            <tr><td>" + img + @"</td></tr>
                            <tr><td>" + signdate + @"</td></tr>
                           </table>";

                    #endregion


                    #region Making HTML2
                    string html1 = @"
                    <!doctype html>
                      <html lang='en'>
                         <head>
                            <style>
                                #customers {
                                  font-family: Arial, Helvetica, sans-serif;
                                  border-collapse: collapse;
                                  width: 100%;
                                }

                                p {
                                  font-weight: lighter;
                                }
                                #customers td, #customers th {
                                  border: 1px solid #ddd;
                                  padding: 8px;
                                }

                                

                                #sigimage{
                                    width : 40%;
                                    height : 35%;
                                }
                            
                                </style>
                            <!--Required meta tags -->
                            <meta charset = 'utf-8'>
                            <meta name = 'viewport' content = 'width=device-width, initial-scale=1, shrink-to-fit=no'>
                            <!--Bootstrap CSS-->
                        <link href='~/Content/bootstrap1.min.css' rel='stylesheet' />   
                        <title> Hello, world! </title>
                         </head>
                            <body style='margin: 30px'>

                       <div style='margin: 16px'>
                        <img style='width: 20%;' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAASwAAADWCAYAAABrA7++AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAFi0SURBVHhe7Z0HvBTFtu6v77573j1e7/F4jjl7jJhRMB4RVDAgSFBAUTKoBMmggAQVUBCUJIjkoGQRMJIzSM4SJIOIRAXFcE69+td0bdtm9UzP3jOzZ8/u7/f7ZsPe3dU1M9Vfr1q1aq3/CBHCjQeLF2/cqGHD6QUKFHhM//c/I78NESJEiDTCKaec8sCrr766Yv/+/Qps375Nvf32W5vvueeuZvrPf48cFSJEiBC5i3MqV37yvVkzZxqhAv/+97+dfyl14Lvv1MABA/aVLv1oZ33spZFTQoQIESLFuO6aa2q++26/vb/88osjTxGxctPixx9/VKM++ODYM8883V+felOkhRAhQoRIPm5q2rjx1E0bNzpydLJQeenGlCmT/1W3bt3xup17I82FCBEiROLx54dKlHht/PjxPzvaI4pTNLoxb9489dJLL84498wzyzrthwgRIkTOcfpppz3apcsb6w4dOuTITfxi5aYba9asUZ06dVx+ww3X1dSXOjVyxRAhQoSIHxdVr159xMIFCxx5yZlQeenGjh07VK8ePb4uWqRIC33dsyKXDxEiRIgAuLVgwfqDBw064BYWr+AkkhYHDx5UgwYN2l+mdOkuuhuXR3oTIkSIEDJuf+mlF+ds37bNkZDkCpWXFj/99JMaPXr08apVqw7QfSoY6VqIECFCRPCXxx4r9eakSZP+5WiGKCipohuffPKxeqF+/Q91H++LdDVEiBD5Fuefc84Tb7/11pYffvjBkYjcFSs33ViwYIFq1eqlWbq/j+tunxLpfYgQIfILrnjuuWfHLV++3JGE9BEqL91Yt26der1z55U33XRTHf0e/ifyVkKECJGpOOWuu+5qNmLE8KOOBogikY50Y9eunapXr57b7ita9CX9ns6JvLUQIUJkEu5t367d4j179ji3fd4RKy8tDh8+rIYMHvxduTJluun3d0XkbYYIESIv4+8VKjze54svPndu87wrVF5anDhxQo0bO/bH6tWrDtbvt1DkbYcIESJP4fLLL3+mT5/eO0+c+Mm5tTNHrNx047PPPlMNG77wEalvnI8hRIgQaY5rGzZsMGXd2rXObZyZQuWlG4sWLVJtWreee9FF51XQn8f/iXwsIUKESCf86b777nt59OjRPzr3rXhjZzrd2LBhg3rjjddX33LLzc/rz+e0yMcUIkSIXMWf/vSnEh07vrbqu+++c27V/ClWbrqxZ/du9U7v3juK339/a/1xnRf51EKECJFqnPfMM5UHzZkz27k1Q6GSaHHkyBE1dOiQg4+XL/+W/uyuinyEIUKESDpuuOHa2u/177/v119/dW7HUKxi0YKMqePHjztRq0aNYfqjvC3yiYYIESIZKNi8ebNpmzdvdm6/UKjipRtffPGFaty48WQ9rX7Q+XxDhAiRAJxa8uGHO3344YSshOrSzRgyON348ssvVdu2bef945JLKunP+v9GPvIQIULEjb/97a+l3+zadQM+GAvpBgyZPbqxceNG1bVLl3W33XprPf3R/yXyDYQIESIILqlVs+b7ixcvcm6nUKiSSTe+2btX9e37zs4SJUq01d/D+ZGvI0SIECIKFSr0wpAhQ35PqK4h3WQhk0OLo0ePquHDhh2u+MQTPfTXck3k2wkRIoTFna1atZpHvnML6YZKKJ3rWIjH5FNasBo7YcKEn2vXqjVCf0d3RL6qECHyL04vW/axtz6eMsW5RVIjHBYkyhv1wQfmxrSQjs9NWkh/SzbdmD5tmmrapMknp5166sPOdxciRP7BBRecX7FHj7e3Hj92zLklkn9TWuzfv599d+ofl12qzjn7TPXwQw+qcePGqd9++805IveFy2LjkSNqi56iWUjHJptuLFu2VLVv127hFVdcVll/jf8V+TZDhMhcXFXv+ec/XLlyhXMLpE6owLBhw9RthQupc885WxW45mp1/XXXqssuuVidf965quQjjzAFUv/6V1a6d7G9ZFK/mOse01Zf36++UkU++0wV+/xz1WXtWrU9l1M7u7F58ybVrVvX9bffXugF/Z3+NfLVhgiROfjPe+65u+X7I0dm3XXSTZFoWixcuFCVL1dWnXfu2eqKy/9hhOq6awv8gZc6wlXq0ZJq4sSJfzjf224yaDF9715VcfZsdaueKiNWsJD+9/1ffJF2wrVv3z7Vr2/fPQ89VKKD/o4vjHzVIULkbRTr0KH90m++2esM8+TfaBZ79c3fokVzddmll6iLL7rwJJGSiHBdcP55qnTpUmrSpI+cliKQrpVTWmzVQtRy6VJ128cfq7s/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' />
                        </div>
                   
                          <br>
                           <div class='container'>
        
  
        <div class='row border'>
            <div class='col-3 border'><h1> PARA1</h1></div>


            <div class='col border-bottom'><p>" + Message + @"</p></div>


                        <div class='col border-bottom'>

                        " + img + @"
                         </div>


                        <div class='col '>" + signdate + @"</div>
                        </div>
  
                          </div>
                        </body>
                        </html> ";



                    var data1 = HttpUtility.HtmlDecode(html1);
                    #endregion



                    #region
                    string paras = Html;

                    if (comments != null)
                    {
                        paras = comments.Data + "<br>" + paras;
                    }

                    obj.InsertCombineComments(paras, int.Parse(ID), Fname, paraCount);



                    string CombineHtml = @"
                    <!doctype html>
                      <html lang='en'>
                         <head>
                            <style>
                                .customers {
                                  font-family: Arial, Helvetica, sans-serif;
                                  border-collapse: collapse;
                                  width: 100%;
                                }
                                div.c {
                                  text-align: right;
                                } 
                                .customers td, #customers th {
                                  border: 1px solid #ddd;
                                  padding: 8px;
                                }

                                //#customers tr:nth-child(even){background-color: #f2f2f2;}

                                //#customers tr:hover {background-color: #ddd;}

                                .customers th {
                                  padding-top: 12px;
                                  padding-bottom: 12px;
                                  text-align: left;
                                  background-color: #58508e;
                                  color: white;
                                }
                                </style>
                            <!--Required meta tags -->
                            <meta charset = 'utf-8'>
                            <meta name = 'viewport' content = 'width=device-width, initial-scale=1, shrink-to-fit=no'>
                            <!--Bootstrap CSS-->
                             <link href='~/Content/bootstrap2.min.css' rel='stylesheet' />  
                               <title> Hello, world! </title>
                         </head>
                            <body style='margin: 30px'>
                           <div style='margin: 16px'>
                        <img style='width: 20%;' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAASwAAADWCAYAAABrA7++AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAFi0SURBVHhe7Z0HvBTFtu6v77573j1e7/F4jjl7jJhRMB4RVDAgSFBAUTKoBMmggAQVUBCUJIjkoGQRMJIzSM4SJIOIRAXFcE69+td0bdtm9UzP3jOzZ8/u7/f7ZsPe3dU1M9Vfr1q1aq3/CBHCjQeLF2/cqGHD6QUKFHhM//c/I78NESJEiDTCKaec8sCrr766Yv/+/Qps375Nvf32W5vvueeuZvrPf48cFSJEiBC5i3MqV37yvVkzZxqhAv/+97+dfyl14Lvv1MABA/aVLv1oZ33spZFTQoQIESLFuO6aa2q++26/vb/88osjTxGxctPixx9/VKM++ODYM8883V+felOkhRAhQoRIPm5q2rjx1E0bNzpydLJQeenGlCmT/1W3bt3xup17I82FCBEiROLx54dKlHht/PjxPzvaI4pTNLoxb9489dJLL84498wzyzrthwgRIkTOcfpppz3apcsb6w4dOuTITfxi5aYba9asUZ06dVx+ww3X1dSXOjVyxRAhQoSIHxdVr159xMIFCxx5yZlQeenGjh07VK8ePb4uWqRIC33dsyKXDxEiRIgAuLVgwfqDBw064BYWr+AkkhYHDx5UgwYN2l+mdOkuuhuXR3oTIkSIEDJuf+mlF+ds37bNkZDkCpWXFj/99JMaPXr08apVqw7QfSoY6VqIECFCRPCXxx4r9eakSZP+5WiGKCipohuffPKxeqF+/Q91H++LdDVEiBD5Fuefc84Tb7/11pYffvjBkYjcFSs33ViwYIFq1eqlWbq/j+tunxLpfYgQIfILrnjuuWfHLV++3JGE9BEqL91Yt26der1z55U33XRTHf0e/ifyVkKECJGpOOWuu+5qNmLE8KOOBogikY50Y9eunapXr57b7ita9CX9ns6JvLUQIUJkEu5t367d4j179ji3fd4RKy8tDh8+rIYMHvxduTJluun3d0XkbYYIESIv4+8VKjze54svPndu87wrVF5anDhxQo0bO/bH6tWrDtbvt1DkbYcIESJP4fLLL3+mT5/eO0+c+Mm5tTNHrNx047PPPlMNG77wEalvnI8hRIgQaY5rGzZsMGXd2rXObZyZQuWlG4sWLVJtWreee9FF51XQn8f/iXwsIUKESCf86b777nt59OjRPzr3rXhjZzrd2LBhg3rjjddX33LLzc/rz+e0yMcUIkSIXMWf/vSnEh07vrbqu+++c27V/ClWbrqxZ/du9U7v3juK339/a/1xnRf51EKECJFqnPfMM5UHzZkz27k1Q6GSaHHkyBE1dOiQg4+XL/+W/uyuinyEIUKESDpuuOHa2u/177/v119/dW7HUKxi0YKMqePHjztRq0aNYfqjvC3yiYYIESIZKNi8ebNpmzdvdm6/UKjipRtffPGFaty48WQ9rX7Q+XxDhAiRAJxa8uGHO3344YSshOrSzRgyON348ssvVdu2bef945JLKunP+v9GPvIQIULEjb/97a+l3+zadQM+GAvpBgyZPbqxceNG1bVLl3W33XprPf3R/yXyDYQIESIILqlVs+b7ixcvcm6nUKiSSTe+2btX9e37zs4SJUq01d/D+ZGvI0SIECIKFSr0wpAhQ35PqK4h3WQhk0OLo0ePquHDhh2u+MQTPfTXck3k2wkRIoTFna1atZpHvnML6YZKKJ3rWIjH5FNasBo7YcKEn2vXqjVCf0d3RL6qECHyL04vW/axtz6eMsW5RVIjHBYkyhv1wQfmxrSQjs9NWkh/SzbdmD5tmmrapMknp5166sPOdxciRP7BBRecX7FHj7e3Hj92zLklkn9TWuzfv599d+ofl12qzjn7TPXwQw+qcePGqd9++805IveFy2LjkSNqi56iWUjHJptuLFu2VLVv127hFVdcVll/jf8V+TZDhMhcXFXv+ec/XLlyhXMLpE6owLBhw9RthQupc885WxW45mp1/XXXqssuuVidf965quQjjzAFUv/6V1a6d7G9ZFK/mOse01Zf36++UkU++0wV+/xz1WXtWrU9l1M7u7F58ybVrVvX9bffXugF/Z3+NfLVhgiROfjPe+65u+X7I0dm3XXSTZFoWixcuFCVL1dWnXfu2eqKy/9hhOq6awv8gZc6wlXq0ZJq4sSJfzjf224yaDF9715VcfZsdaueKiNWsJD+9/1ffJF2wrVv3z7Vr2/fPQ89VKKD/o4vjHzVIULkbRTr0KH90m++2esM8+TfaBZ79c3fokVzddmll6iLL7rwJJGSiHBdcP55qnTpUmrSpI+cliKQrpVTWmzVQtRy6VJ128cfq7s/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' />
                        </div>
                            <br>
                         " + paras + @"
                    <br>


                           </body>
                      </html> ";
                    #endregion




                    byte[] fileBytes = RichTextEditorViewModel.GeneratePDF(CombineHtml, data1);





                    MemoryStream stream = new MemoryStream(fileBytes);



                    var createdObject = new ObjectVersion();
                    if (comments != null && comments.ParaCount > 0)
                    {
                        createdObject = Mfiles.UpdateComments(stream, token, ID).Result;
                    }
                    else
                    {
                        createdObject = Mfiles.AddFilesToMF_Object(stream, token, ID, "Comments").Result;

                    }

                    obj.DeleteobjectProps(int.Parse(ID));

                }
                else
                {
                    ViewBag.Title = "Invalid UserName / Login Password";
                    return Json(new { success = false, message = "Invalid Signature Password" }, JsonRequestBehavior.AllowGet);
                }



            }
            catch (Exception ex)
            {
                CreateLogFiles.ErrorLog(ex.Message + " " + ex.StackTrace);

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return View("UploadComment");

        }


        [HttpPost]
        public ActionResult SaveCommentToDB(string Message, string ID, string Fname, int code)
        {
            try
            {


                Session["HtmlData"] = null;
                int CommentID = Convert.ToInt16(ID);

                TempData["ID"] = ID;
                TempData["name"] = Fname;
                if (code == 1)
                {
                   var res = obj.InsertobjectProps(Message, CommentID, Fname);
                    if (res != "true")
                    {
                        CreateLogFiles.ErrorLog("Exception" + " " + res);

                        return Json(new { success = false, message = res }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {


                    List<dynamic> objectprop = new List<dynamic>();
                    objectprop = obj.ReadobjectProps(Convert.ToInt16(ID));
                    if (objectprop.Count > 0)
                    {
                        return Json(new { success = true, message = "duplicate" }, JsonRequestBehavior.AllowGet);


                    }
                    else
                    {
                       var res = obj.InsertobjectProps(Message, CommentID, Fname);
                        if (res != "true")
                        {
                            CreateLogFiles.ErrorLog("Exception" + " " + res);

                            return Json(new { success = false, message = res }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                CreateLogFiles.ErrorLog("Exception" + " " + ex.Message + " " + ex.StackTrace);

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, message = "Comment Save. You can Change It anytime" }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult SaveToSession(string Message, string ID, string Fname)
        {
            try
            {

                TempData["ID"] = ID;
                TempData["name"] = Fname;
                Session["HtmlData"] = Message;



            }
            catch (Exception ex)
            {
                CreateLogFiles.ErrorLog("Exception" + " " + ex.Message + " " + ex.StackTrace);

                return Json(new { success = true, message = "" }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { success = true, message = "" }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult AddBoomark(string Message, string ID, string Fname,string B_ID,string text)
        {
            try
            {
                string ancor = "<a href='#"+B_ID+"'>"+ text + "</a>";
                Message += ancor;
                TempData["ID"] = ID;
                TempData["name"] = Fname;
                Session["HtmlData"] = Message;

                TempData["HtmlData"] = Message;


            }
            catch (Exception ex)
            {
                CreateLogFiles.ErrorLog("Exception" + " " + ex.Message + " " + ex.StackTrace);

                return Json(new { success = true, message = "" }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { success = true, message = "" }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult DownloadFile(string url)
        {

            System.IO.Stream stream = new System.IO.MemoryStream();
            string AuthenticationToken = Mfiles.AuthToken();


            // Create the web request.

            WebRequest request = WebRequest.Create(url);

            // Fill the authentication information.
            request.Headers["X-Authentication"] = AuthenticationToken;

            // Receive the response.
            var response = request.GetResponse();
            var fileName = response.Headers["Content-Disposition"].Split(';')[1].Split('=')[1].Trim('\"');



            stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string text = reader.ReadToEnd();

            return Json(new { success = true, message = text }, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult Contact()
        {

           
            return View();
        }


        public static int CreateParaCount(int ID, ObjectProps obj)
        {
            int commentsCount = 0;
            try
            {
                
                dynamic previousobject = new ExpandoObject();
                previousobject = obj.ReadCombineComments(ID);
                

                if (previousobject!=null)
                {
                    commentsCount = previousobject.ParaCount;
                }

                commentsCount++;
            }
            catch (Exception ex)
            {

               
            }
            return commentsCount;
        }

        [HttpPost]
        public ActionResult Image(string image)
        {
            ViewBag.Message = "Your contact page.";

            return Json(new { success = true, message = "Comment Save. You can Change It anytime" }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult About(string Message, string ID, string Fname)
        {
            TempData["DocID"] = ID;
            TempData["DocName"] = Fname;
            TempData["Comment"] = Message;


            return Json(new { success = true, message = "Comment Save. You can Change It anytime" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult About()
        {
            // Create the cookie
            HttpCookie sameSiteCookie = new HttpCookie("SameSite");

            // Set a value for the cookieSite none.
            // Note this will also require you to be running on HTTPS
            sameSiteCookie.Value = "none";

            // Set the secure flag, which Chrome's changes will require for Same
            sameSiteCookie.Secure = true;

            // Set the cookie to HTTP only which is good practice unless you really do need
            // to access it client side in scripts.
            sameSiteCookie.HttpOnly = true;

            // Add the SameSite attribute, this will emit the attribute with a value of none.
            // To not emit the attribute at all set the SameSite property to -1.
            //sameSiteCookie.Value = SameSiteMode.None;

            // Add the cookie to the response cookie collection
            Response.Cookies.Add(sameSiteCookie);

            return View();

        }

        public static async Task<int> Create_Empty_MultiFile_Object(string code)
        {
            // Create a HttpClient.
            var client = new System.Net.Http.HttpClient();

            // Authenticate using a token that we already have (not shown here).
            client.DefaultRequestHeaders.Add("X-Authentication", code);

            // Build up information for the new object.
            // Note: all mandatory properties must be set, but this is likely to be, at a minimum,
            // the class (property 100) and the name or title (typically property 0).
            var objectCreationInfo = new ObjectCreationInfo()
            {
                PropertyValues = new[]
                {
                    new PropertyValue()
                    {
                        PropertyDef = 100, // The built-in "Class" property Id.
						TypedValue = new TypedValue()
                        {
                            DataType = MFDataType.Lookup,
                            Lookup = new Lookup()
                            {
                                Item = 0, // The built-in "Other Document" class Id.
								Version = -1 // Work around the bug detailed below.
							}
                        }
                    },
                    new PropertyValue()
                    {
                        PropertyDef = 0, // The built-in "Name or Title" property Id.
						TypedValue =  new TypedValue()
                        {
                            DataType = MFDataType.Text,
                            Value = "Comments"
                        }
                    }
    }
            };

            // Serialise using JSON.NET (use Nuget to add a reference if needed).
            var stringContent = Newtonsoft.Json.JsonConvert.SerializeObject(objectCreationInfo);

            // Create the content for the web request.
            var content = new System.Net.Http.StringContent(stringContent, Encoding.UTF8, "application/json");

            // We are creating a document (multi-file-document with no files).
            const int documentObjectTypeId = 0; //

            // Execute the POST.
            // NOTE: http://developer.m-files.com/APIs/REST-API/#iis-compatibility
            var httpResponseMessage = await client.PostAsync(new Uri(domain + "REST/objects/" + documentObjectTypeId + ".aspx"), content).ConfigureAwait(false);

            // Extract the value.
            var objectVersion = Newtonsoft.Json.JsonConvert.DeserializeObject<ObjectVersion>(await httpResponseMessage.Content.ReadAsStringAsync());

            // Output the response.
            System.Console.WriteLine("New object Id was: " + objectVersion.ObjVer.ID);

            return objectVersion.ObjVer.ID;

        }




    }

}



















//                    ,

//                    new PropertyValue()
//{
//    PropertyDef = 0, // The built-in "Keywords" property Id.
//			            TypedValue = new TypedValue()
//                        {
//                            DataType = MFDataType.Text,
//                            Value = "Object Oriented"
//                        }
//                    }
//                    ,

//                    new PropertyValue()
//{
//    PropertyDef = 0, // The built-in "Proposal Number" property Id.
//			            TypedValue = new TypedValue()
//                        {
//                            DataType = MFDataType.Integer,
//                            Value = 123
//                        }
//                    }
//                    ,

//                    new PropertyValue()
//{
//    PropertyDef = 0, // The built-in "Invoice Title" property Id.
//			            TypedValue = new TypedValue()
//                        {
//                            DataType = MFDataType.Text,
//                            Value = "My OOP"
//                        }
//}