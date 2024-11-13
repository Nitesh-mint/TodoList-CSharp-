using Microsoft.AspNetCore.Mvc;
using TodoList.Data;
using TodoList.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace TodoList.Controllers{
    public class TodoController : Controller{
        private readonly TodoContext _context;
        public TodoController(TodoContext context){
            _context = context;
        }

        //GET : Todo 
        public async Task<IActionResult> Index(){
            return View(await _context.TodoItems.ToListAsync());
        }      

        // Get: Todo/Create 
        public IActionResult Create(){
            return View();
        }

        // POST: Todo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoItem todoItem){
            if(ModelState.IsValid){
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