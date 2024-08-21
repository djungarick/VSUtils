namespace ReplaceItemTemplates;

internal sealed class FileToReplace
{
    [Required]
    public string Path { get; set; } = null!;

    [Required]
    public string Template { get; set; } = null!;
}
