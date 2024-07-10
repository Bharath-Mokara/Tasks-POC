using System.ComponentModel.DataAnnotations;

namespace ToolsAppApi.Models
{
    public class Template
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name {get; set;}
        public string? Content { get; set; }
    }
}