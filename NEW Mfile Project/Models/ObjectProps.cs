using MFaaP.MFWSClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace NEW_Mfile_Project.Models
{
    public class ObjectProps
    {
        DBConnect obj = new DBConnect();
        public string fileName { set; get; }
        public int Obj_Type { set; get; }
        public int Obj_ID { set; get; }
        public int Obj_Version { set; get; }
        public int ObjectVersionID { set; get; }

        string connetionString = @"Data Source=DESKTOP-5GTHSVS\SQLEXPRESS;Initial Catalog=M-FilesWeb;Integrated Security=True";
        //string connetionString = @"Data Source=.;Initial Catalog=M-FilesWeb;Integrated Security=True";


        public string InsertobjectProps(string fileName, int type, int ID, int Version, int VersionID)
        {
            string messages = "";
            try
            {

                SqlConnection con = new SqlConnection(connetionString);
                string procedure = "SaveObjectProperties_sp";         //Stored Procedure name   
                SqlCommand com = new SqlCommand(procedure, con);  //creating  SqlCommand  object  
                com.CommandType = CommandType.StoredProcedure;  //here we declaring command type as stored Procedure  


                com.Parameters.AddWithValue("@FileName", fileName);
                com.Parameters.AddWithValue("@oType", type);
                com.Parameters.AddWithValue("@oID", ID);
                com.Parameters.AddWithValue("@oVersion ", Version);
                com.Parameters.AddWithValue("@OVersionID", VersionID);
                con.Open();
                int res = com.ExecuteNonQuery();
                con.Close();
                if (res > 0)
                {
                    messages = "File Added To Mfiles";
                }
            }
            catch (Exception ex)
            {

                messages = "Error Occured" + ex.Message;
                CreateLogFiles.ErrorLog("Exception" + " " + ex.Message + " " + ex.StackTrace);

            }
            return messages;
        }

        public string InsertobjectProps(string Data, int ID,string name)
        {
            string messages = "Insert Error";
            try
            {

                SqlConnection con = new SqlConnection(connetionString);
                string procedure = "SaveCommets_sp";         //Stored Procedure name   
                SqlCommand com = new SqlCommand(procedure, con);  //creating  SqlCommand  object  
                com.CommandType = CommandType.StoredProcedure;  //here we declaring command type as stored Procedure  


                com.Parameters.AddWithValue("@Data", Data);
                com.Parameters.AddWithValue("@Obj_ID", ID); 
                com.Parameters.AddWithValue("@Obj_name", name);
                con.Open();
                int res = com.ExecuteNonQuery();
                con.Close();
                if (res > 0)
                {
                    messages = "true";
                }
            }
            catch (Exception ex)
            {

                messages = "Error Occured" + ex.Message;
                CreateLogFiles.ErrorLog("Exception" + " " + ex.Message + " " + ex.StackTrace);

            }
            return messages;
        }


        public string InsertCombineComments(string Data, int Obj_ID, string Obj_name,int paracount)
        {
            string messages = "";
            try
            {

                SqlConnection con = new SqlConnection(connetionString);
                string procedure = "SaveCombineComments_sp";         //Stored Procedure name   
                SqlCommand com = new SqlCommand(procedure, con);  //creating  SqlCommand  object  
                com.CommandType = CommandType.StoredProcedure;  //here we declaring command type as stored Procedure  


                com.Parameters.AddWithValue("@html", Data);
                com.Parameters.AddWithValue("@Obj_ID", Obj_ID);
                com.Parameters.AddWithValue("@Obj_name", Obj_name);
                com.Parameters.AddWithValue("@ParaCount", paracount);

                con.Open();
                int res = com.ExecuteNonQuery();
                con.Close();
                if (res > 0)
                {
                    messages = "true";
                }
            }
            catch (Exception ex)
            {

                messages = "Error Occured" + ex.Message;
                CreateLogFiles.ErrorLog("Exception" + " " + ex.Message + " " + ex.StackTrace);

            }
            return messages;
        }

        public dynamic ReadCombineComments(int ID)
        {
            
            dynamic obj = null;
            try
            {

                using (SqlConnection con = new SqlConnection(connetionString))
                {

                    //set stored procedure name
                    string spName = "GetCombineComments_sp";

                    //define the SqlCommand object
                    SqlCommand cmd = new SqlCommand(spName, con);

                    //open connection
                    con.Open();

                    //set the SqlCommand type to stored procedure and execute
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Obj_ID", ID);
                   
                    SqlDataReader dr = cmd.ExecuteReader();

                    //check if there are records
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            obj = new ExpandoObject();
                            obj.Data = dr["html"].ToString();
                            obj.ID = Convert.ToInt16(dr["Obj_ID"].ToString());
                            obj.Obj_name = dr["Obj_name"].ToString();
                            obj.ParaCount = Convert.ToInt16(dr["ParaCount"].ToString());


                            //objectprop.Add(obj);
                        }
                    }


                    //close data reader
                    dr.Close();

                    //close connection
                    con.Close();
                }
            }
            catch (Exception ex)
            {

               
                CreateLogFiles.ErrorLog("Exception" + " " + ex.Message + " " + ex.StackTrace);

            }
            return obj;
        }
        public List<ObjectProps> ReadobjectProps1(int ID)
        {
            string messages = "";
            List<ObjectProps> objectprop = new List<ObjectProps>();
            try
            {

                using (SqlConnection con = new SqlConnection(connetionString))
                {

                    //set stored procedure name
                    string spName = "GetObjectProperties_sp";

                    //define the SqlCommand object
                    SqlCommand cmd = new SqlCommand(spName, con);
                    cmd.Parameters.AddWithValue("@ID", ID);

                    //open connection
                    con.Open();

                    //set the SqlCommand type to stored procedure and execute
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();

                    //check if there are records
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ObjectProps obj = new ObjectProps();
                            obj.fileName = dr["FileName"].ToString();
                            obj.Obj_Type = Convert.ToInt16(dr["Obj_Type"].ToString());
                            obj.Obj_ID = Convert.ToInt16(dr["Obj_ID"].ToString());
                            obj.Obj_Version = Convert.ToInt16(dr["Obj_Version"].ToString());
                            obj.ObjectVersionID = Convert.ToInt32(dr["ObjectVersionID"].ToString());

                            objectprop.Add(obj);
                        }
                    }


                    //close data reader
                    dr.Close();

                    //close connection
                    con.Close();
                }
            }
            catch (Exception ex)
            {

                messages = "Error Occured" + ex.Message;
                CreateLogFiles.ErrorLog("Exception" + " " + ex.Message + " " + ex.StackTrace);

            }
            return objectprop;
        }

        public List<dynamic> ReadobjectProps(int ID)
        {
            string messages = "";
            List<dynamic> objectprop = new List<dynamic>();
            try
            {

                using (SqlConnection con = new SqlConnection(connetionString))
                {

                    //set stored procedure name
                    string spName = "GetCommet_sp";

                    //define the SqlCommand object
                    SqlCommand cmd = new SqlCommand(spName, con);

                    //open connection
                    con.Open();

                    //set the SqlCommand type to stored procedure and execute
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Obj_ID", ID);
                    SqlDataReader dr = cmd.ExecuteReader();

                    //check if there are records
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            dynamic obj = new ExpandoObject();
                            obj.Data = dr["Data"].ToString();
                            
                            obj.ID = Convert.ToInt16(dr["Obj_ID"].ToString());
                            obj.Obj_name = dr["Obj_name"].ToString();


                            objectprop.Add(obj);
                        }
                    }


                    //close data reader
                    dr.Close();

                    //close connection
                    con.Close();
                }
            }
            catch (Exception ex)
            {

                CreateLogFiles.ErrorLog("Exception" + " " + ex.Message + " " + ex.StackTrace);

            }
            return objectprop;
        }
        public string DeleteobjectProps(int id)
        {
            string messages = "";
            try
            {

                SqlConnection con = new SqlConnection(connetionString);
                string procedure = "DeleteObjectProperties_sp";         //Stored Procedure name   
                SqlCommand com = new SqlCommand(procedure, con);  //creating  SqlCommand  object  
                com.Parameters.AddWithValue("@ID",id);
                com.CommandType = CommandType.StoredProcedure;  //here we declaring command type as stored Procedure  


                con.Open();
                int res = com.ExecuteNonQuery();
                con.Close();
                if (res > 0)
                {
                    messages = "true";
                }
            }
            catch (Exception ex)
            {

                messages = "Error Occured" + ex.Message;
                CreateLogFiles.ErrorLog("Exception" + " " + ex.Message + " " + ex.StackTrace);

            }
            return messages;
        }

        public System.Drawing.Image DownloadImageFromUrl(string imageUrl)
        {
            System.Drawing.Image image = null;

            try
            {
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;

                System.Net.WebResponse webResponse = webRequest.GetResponse();

                System.IO.Stream stream = webResponse.GetResponseStream();


                // Convert byte[] to Base64 String
                //string base64String = Convert.ToBase64String(imageBytes);
            }
            catch (Exception ex)
            {
                return null;
            }

            return image;
        }
        public static ObjectVersion[] UpdateObject(string fileName)
        {
            //    Username = "mfile",
            //            Password = "abc123",
            //            VaultGuid = "{F36D4276-EE11-4767-80EE-0FB0DFED0112}"
            var client = new MFWSClient("http://localhost/");
            client.AuthenticateUsingCredentials(Guid.Parse("{F36D4276-EE11-4767-80EE-0FB0DFED0112}"), "mfile", "abc123");

            var results = client.ObjectSearchOperations.SearchForObjectsByString(fileName);

            return results;
        }

    }
}
