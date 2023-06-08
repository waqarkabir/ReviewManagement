using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class RatingController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public RatingController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            // Retrieve the customer by ID
            Customer customer = _dbContext.Customers.Find(id);

            if (customer != null)
            {
                // Pass the customer model to the feedback view
                return View(customer);
            }

            // Handle the case when customer is not found
            return NotFound();
        }

        [HttpPost]
        public IActionResult Feedback(int id, int rating)
        {
            // Retrieve the customer by ID
            Customer customer = _dbContext.Customers.Find(id);

            if (customer != null)
            {
                // Save the feedback to the customer's record in the database
                customer.Rating = rating;
                _dbContext.Update(customer);
                _dbContext.SaveChanges();

                // Redirect to a thank you page or perform any desired action
                return RedirectToAction("ThankYou");
            }

            // Handle the case when customer is not found
            return NotFound();
        }

        public IActionResult ThankYou()
        {
            return View();
        }

    }
}
