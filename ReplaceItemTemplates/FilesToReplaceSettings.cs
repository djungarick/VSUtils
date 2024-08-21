using Microsoft.Extensions.Options;

namespace ReplaceItemTemplates;

internal sealed class FilesToReplaceSettings
{
    [Required]
    [ValidateEnumeratedItems]
    public FileToReplace[] List { get; set; } = null!;
}
