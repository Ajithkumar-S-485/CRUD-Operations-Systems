using System.ComponentModel.DataAnnotations;

namespace Demo.WebApi.Models
{
    public class ModelBase
    {
        [Key]
        public int Id { get; set; }
    }
}