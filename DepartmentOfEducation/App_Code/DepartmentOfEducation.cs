using System;
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

    //a method which sends an email in database from ID
    //method sends mail and returns a string of an error message according to the errors in method
    [WebMethod]
    public string SendIt(string idstr)
    {

        if (!CheckForInternetConnection())
            return "Cannot connect to Email service, connect to the internet and try again";
        if (!isStringValidInt(idstr))
            return "ID is not a valid id number, please try again";
        string pass = "";
        int id = int.Parse(idstr);
        string ReciverMail, returnstr = "";
        SqlConnection c = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Omer\\Dropbox\\school\\UpGradeProject\\DepartmentOfEducation\\App_Data\\Students.mdf;Integrated Security=True");
        SqlCommand cmd = new SqlCommand("SELECT MailAddress FROM Students Where StudentId = @ID", c);
        cmd.Parameters.AddWithValue("ID", id);
        c.Open();
        ReciverMail = (string)cmd.ExecuteScalar();
        if (ReciverMail == null)
            return "Error, ID not in database, contact School Admin";
        c.Close();
        MailMessage msg = new MailMessage();
        msg.From = new MailAddress("Support@UpGrade.com");
        msg.To.Add(ReciverMail);
        msg.Subject = "SignUpInfo";
        string msgString = "";
        msgString += "Hello, This mail was sent from the UpGrade Support team";
        msgString += "/nYou've clicked the 'Forgot Password' on our site, if you ment to do sothe password is " + pass;
        msg.Body = "Hello, welcome to your Account info,\nYour ID is " + id + " and your temporary password is also " + id + ".\n Your account type is Student so make sure you select it at login page.";
        SmtpClient client = new SmtpClient();
        client.UseDefaultCredentials = true;
        client.Host = "smtp.gmail.com";
        client.Port = 587;
        client.EnableSsl = true;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.Credentials = new NetworkCredential("Sup.UpGrade@gmail.com", "Arik2005");
        client.Timeout = 20000;
        client.Send(msg);
        returnstr = "Mail Sent";
        msg.Dispose();
        return returnstr;



        }


    //a method which returns a boolean whether there is or isnt an internet connection
    //used when sending confirmation mail to file out error messages
    [WebMethod]
    public bool CheckForInternetConnection()
    {
        try
        {
            using (var client = new WebClient())
            {
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
        }
        catch
        {
            return false;
        }
    }

    [WebMethod]
    public bool idExistInDatabase(int id)
    {
        SqlConnection c = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Omer\\Dropbox\\school\\UpGradeProject\\DepartmentOfEducation\\App_Data\\Students.mdf;Integrated Security=True");
        SqlCommand cmd = new SqlCommand("Select * FROM Students WHERE StudentId = @id", c);
        cmd.Parameters.AddWithValue("id", id);
        c.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
            return true;
        else
            return false;

    } 

    public bool isStringValidInt(string str)
    {
        int a;
        try
        {
            a = int.Parse(str);
        }
        catch
        {
            return false;
        }
        return true;
    }

}

