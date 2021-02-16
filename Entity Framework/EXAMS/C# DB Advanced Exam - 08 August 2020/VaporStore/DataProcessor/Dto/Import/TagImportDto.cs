namespace VaporStore.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;
    public class TagImportDto
    {
        [Required]
        public string Name { get; set; }
    }
}
