using System.ComponentModel.DataAnnotations;

namespace TodoList.Models{
    public class TodoItem{
        public int ID {get;set;}
        [Required]
        public string? Task {get;set;}
        public bool IsComplete {get;set;}
        
        public DateTime? DueDate {get;set;}
        
        public DateTime? CreatedDate {get;set;}
    }
}