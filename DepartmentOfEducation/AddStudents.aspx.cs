﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

public partial class AddStudents : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Insert_Click(object sender, EventArgs e)
    {
        SqlConnection c = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Omer\\Dropbox\\school\\DepartmentOfEducation\\App_Data\\Students.mdf;Integrated Security=True");
        SqlCommand cmd = new SqlCommand("INSERT INTO Students (DOB, FirstName, LastName, SchoolID, PhoneNumber, MailAddress) VALUES(@DOB, @FirstName, @LastName, @SchoolID, @PhoneNumber, @MailAddress);", c);
        cmd.Parameters.AddWithValue("FirstName", FirstName.Text);
        cmd.Parameters.AddWithValue("LastName", LastName.Text);
        cmd.Parameters.AddWithValue("PhoneNumber", PhoneNumber.Text);
        cmd.Parameters.AddWithValue("MailAddress", MailAddress.Text);
        cmd.Parameters.AddWithValue("SchoolID", SchoolID.Text);
        cmd.Parameters.AddWithValue("DOB", Convert.ToDateTime(DOB.Text));
        try{
            c.Open();
            cmd.ExecuteNonQuery();
            c.Close();
        }
        catch
        {
            FirstName.Text = "FAILED!!!";
        }

    }
    public void sendit(object sender ,EventArgs e)
    {
        string ReciverMail;
        SqlConnection c = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Omer\\Dropbox\\school\\DepartmentOfEducation\\App_Data\\Students.mdf;Integrated Security=True");
        SqlCommand cmd = new SqlCommand("SELECT MailAddress FROM Students Where StudentId = @ID", c);
        try
        {
            c.Open();
            ReciverMail = (string)cmd.ExecuteScalar();
            c.Close();
            Response.Redirect("http://www.google.com");
        }
        catch
        {

            ReciverMail = "No mail Detected";
        }


        cmd.Parameters.AddWithValue("ID", IdTextBox.Text);
        MailMessage msg = new MailMessage();
        msg.From = new MailAddress("Sup.UpGrade@gmail.com");
        if (ReciverMail == "No mail Detected")
            Response.Redirect("http://NOMAILDETECTED.com");
        else
        {
            msg.To.Add(ReciverMail);
            msg.Subject = "SignUpInfo";
            msg.Body = "Hello, welcome to your Account info,\nYour ID is "+IdTextBox.Text+" and your temporary password is also "+IdTextBox.Text+ ".\n Your account type is Student so make sure you select it at login page.";
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
                Response.Redirect("https://www.google.com");//return success
            }
            catch (Exception ex)
            {
                Response.Redirect("https://www.bing.com");//return fail
            }
            finally
            {
                msg.Dispose();
            }
        }

}
}