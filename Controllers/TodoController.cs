using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using TodoList.Data;
using TodoList.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace TodoList.Controllers{
    public class TodoController : Controller{
        private readonly TodoContext _context;
        public TodoController(TodoContext context){
            _context = context;
        }

        //GET : Todo 
        public async Task<IActionResult> Index(string searchString, string sortOrder, int? pageNumber)
        {
            ViewData["DateSortParm"] = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                pageNumber = 1;
            }
           
            var tasks = from t in _context.TodoItems select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                tasks = tasks.Where(t => t.Task.ToLower().Contains(searchString.ToLower()));
                //counting and passing the number of search result to the view.
                int numTasks = tasks.Count();
                ViewData["numTasks"] = numTasks;
            }

            switch (sortOrder)
            {
                case "Date":
                    tasks = tasks.OrderBy(t => t.CreatedDate);
                    break;
                case "Date_desc":
                    tasks = tasks.OrderByDescending(t => t.CreatedDate);
                    break;
            }
            
            var expiredTasks = tasks.Where(t => t.DueDate.Value.Date < DateTime.Now.Date).ToList();
            ViewData["expiredTasks"] = expiredTasks;
            Console.Write("Expired:"+ expiredTasks.Count());
            
            
            int pageSize = 5;
            
            
            return View(await PaginatedList<TodoItem>.CreateAsync(tasks.AsNoTracking(), pageNumber ?? 1, pageSize));
        }      

        // Get: Todo/Create 
        public IActionResult Create(){
            return View();
        }

        // POST: Todo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoItem todoItem){
            if(ModelState.IsValid)
            {
                todoItem.CreatedDate = DateTime.Now;
                _context.Add(todoItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todoItem);
        }

        //Post : Todo/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id){
            var todoItem = await _context.TodoItems.FindAsync(id);
            if(todoItem != null){
                _context.TodoItems.Remove(todoItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index)); 
        }

        //Post : Todo/
        [HttpGet]
        public async Task<IActionResult> Update(int id){
            var todoItem = await _context.TodoItems.FindAsync(id);
            if(todoItem == null){
                return NotFound();
            }
            return View(todoItem);
        }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, TodoItem todoItem)
    {
        Console.WriteLine("Due Date: " + todoItem.DueDate);
        if (id != todoItem.ID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(todoItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.TodoItems.Any(e => e.ID == todoItem.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(todoItem);
    }
    }
    
}