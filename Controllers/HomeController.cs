using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpendSmart.Models;

namespace SpendSmart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SpendSmartDbContext _context;

        public HomeController(ILogger<HomeController> logger, SpendSmartDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Expenses()
        {
            var allExpenses = _context.Expenses.ToList(); // Fetch all expenses from the database 

            var totalExpenses = allExpenses.Sum(x => x.Value); // Calculate the total expenses 

            ViewBag.Expenses = totalExpenses; // Pass the total expenses to the view 
            return View(allExpenses);
        }

        public IActionResult CreateEditExpense(int? id)
        {
            if(id != null)
            {
                var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
                return View(expenseInDb);

            }
            return View();
        }


        public IActionResult DeleteExpense(int id)
        {
            var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id); // Find the expense by ID 
            _context.Expenses.Remove(expenseInDb); // Remove the expense from the context 
            _context .SaveChanges(); // Save changes to the database 
            return RedirectToAction("Expenses"); 
        }

        public IActionResult CreateEditExpenseForm(Expense model)
        {
            if(model.Id == 0)
            {
                _context.Expenses.Add(model);
            }
            else
            {
                _context.Expenses.Update(model);
            }

            _context.SaveChanges(); 

            return RedirectToAction("Expenses");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
