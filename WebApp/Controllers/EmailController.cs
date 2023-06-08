using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using WebApp.Data;
using WebApp.Models;

public class EmailController : Controller
{
    private readonly ApplicationDbContext _Context;

    public EmailController(ApplicationDbContext dbContext)
    {
        _Context = dbContext;
    }

    // Action to send feedback emails
    public IActionResult SendFeedbackEmails()
    {
        List<Customer> customers = _Context.Customers.ToList();

        foreach (Customer customer in customers)
        {
            // Generate a unique feedback URL for each customer
            string feedbackUrl = $"https://localhost:7129/rating/{customer.Id}";

            // Send email with feedback link to the customer's email
            SendEmail(customer.Email, "Feedback Request", $"Please provide your feedback <a href='{feedbackUrl}'>here</a>.");

            // You can save the feedback URL in the database if needed
            // customer.FeedbackUrl = feedbackUrl;
        }
        TempData["success"] = "Email send Successfully";
        return RedirectToAction("index", "Home");
    }

    private void SendEmail(string recipient, string subject, string body)
    {
        string senderEmail = "ahmedqureshdummy@gmail.com"; // Replace with your sender email address
        string senderPassword = "wzintsgkzsbubcdd"; // Replace with your sender email password

        var mailMessage = new MailMessage(senderEmail, recipient, subject, body);
        mailMessage.IsBodyHtml = true;

        var smtpClient = new SmtpClient("smtp.gmail.com", 587); // Replace with your SMTP server address and port
        smtpClient.EnableSsl = true;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

        try
        {
            smtpClient.Send(mailMessage);
        }
        catch (SmtpException ex)
        {
            // Handle the exception or log the error
            // For simplicity, rethrowing the exception here
            throw ex;
        }
        finally
        {
            // Dispose of the SmtpClient and MailMessage objects
            smtpClient.Dispose();
            mailMessage.Dispose();
        }
    }
}