﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using static System.Web.UI.HtmlControls.HtmlGenericControl;
using System.Web.Services;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Services.Protocols;

/// <summary>
/// Summary description for DepartmentOfEducation
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class DepartmentOfEducation : System.Web.Services.WebService
{

    public DepartmentOfEducation()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string SendIt(int id)
    {
        
        string ReciverMail, returnstr="";
        SqlConnection c = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Omer\\Dropbox\\school\\UpGradeProject\\DepartmentOfEducation\\App_Data\\Students.mdf;Integrated Security=True");
        SqlCommand cmd = new SqlCommand("SELECT MailAddress FROM Students Where StudentId = @ID", c);
        cmd.Parameters.AddWithValue("ID", id);
        c.Open();
        ReciverMail = (string)cmd.ExecuteScalar();
        c.Close();
        MailMessage msg = new MailMessage();
        msg.From = new MailAddress("Support@UpGrade.com");
        msg.To.Add(ReciverMail);
        msg.Subject = "SignUpInfo";
        msg.Body = "Hello, welcome to your Account info,\nYour ID is " + id + " and your temporary password is also " + id + ".\n Your account type is Student so make sure you select it at login page.";
        SmtpClient client = new SmtpClient();
        client.UseDefaultCredentials = true;
        client.Host = "smtp.gmail.com";
        client.Port = 587;
        client.EnableSsl = true;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.Credentials = new NetworkCredential("Sup.UpGrade@gmail.com", "Arik2005");
        client.Timeout = 20000;
        try
        {
            client.Send(msg);
            returnstr = "Mail Sent";
        }
        catch
        {
            returnstr = "Error, No ID in database, try again later";
        }
        msg.Dispose();
        return returnstr;

        }


}
