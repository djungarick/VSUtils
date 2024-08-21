using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ReplaceItemTemplates;

const string TemplatesFolder = "Templates";

const int SpaceLength = 1;

IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", true, true);

IConfiguration configuration = configurationBuilder.Build();

IConfigurationSection filesToReplaceSettingsConfigurationSection = configuration.GetSection(nameof(FilesToReplaceSettings));
if (!filesToReplaceSettingsConfigurationSection.Exists())
{
    Console.WriteLine($"The '{nameof(FilesToReplaceSettings)}' configuration section was not found.");
    _ = Console.ReadKey();

    return;
}

IServiceCollection serviceCollection = new ServiceCollection();

serviceCollection.AddOptions<FilesToReplaceSettings>()
    .Bind(filesToReplaceSettingsConfigurationSection)
    .ValidateDataAnnotations();

ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

IOptions<FilesToReplaceSettings> fileToReplaceOptions = serviceProvider.GetRequiredService<IOptions<FilesToReplaceSettings>>();
FileToReplace[] filesToReplace;

try
{
    filesToReplace = fileToReplaceOptions.Value.List;
}
catch (OptionsValidationException ex)
{
    WriteErrorMessage(ex.Message);
    _ = Console.ReadKey();

    return;
}

int maxPathLength = filesToReplace.Max(static fileToReplace => fileToReplace.Path.Length);
int maxMessageLength = typeof(Messages)
    .GetFields(BindingFlags.Public | BindingFlags.Static)
    .Where(static fieldInfo => fieldInfo.IsLiteral && !fieldInfo.IsInitOnly)
    .Select(static fieldInfo => fieldInfo.GetValue(null) as string)
    .Where(static fieldValue => fieldValue is not null)
    .Max(static fieldValue => fieldValue!.Length);

Console.WindowWidth = maxPathLength + SpaceLength + maxMessageLength;

foreach (IGrouping<string, FileToReplace> filesToReplaceGroupedByTemplate in filesToReplace.GroupBy(
    static fileToReplace => fileToReplace.Template))
{
    WriteTemplateName(filesToReplaceGroupedByTemplate.Key);

    string templateName = Path.Combine(TemplatesFolder, filesToReplaceGroupedByTemplate.Key);
    if (!File.Exists(templateName))
    {
        WriteErrorMessage("TEMPLATE NOT FOUND");

        continue;
    }

    string templateContent = File.ReadAllText(templateName, Encoding.UTF8);

    foreach (FileToReplace fileToReplace in filesToReplaceGroupedByTemplate)
    {
        Console.Write(fileToReplace.Path);
        Console.SetCursorPosition(maxPathLength + SpaceLength, Console.CursorTop);

        if (!File.Exists(fileToReplace.Path))
        {
            WriteNotFound();

            continue;
        }

        if (File.ReadAllText(fileToReplace.Path, Encoding.UTF8) == templateContent)
        {
            WriteAlreadyReplaced();

            continue;
        }

        using (StreamWriter fileStream = new(File.Open(fileToReplace.Path, FileMode.Truncate, FileAccess.Write)))
        {
            fileStream.Write(templateContent);
        }

        WriteReplaced();
    }
}

_ = Console.ReadKey();

static void WriteReplaced()
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(Messages.Replaced);
    Console.ResetColor();
}

static void WriteNotFound()
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(Messages.NotFound);
    Console.ResetColor();
}

static void WriteAlreadyReplaced()
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(Messages.AlreadyReplaced);
    Console.ResetColor();
}

static void WriteTemplateName(string templateName)
{
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.Write(templateName);
    Console.WriteLine(':');
    Console.ResetColor();
}

static void WriteErrorMessage(string errorMessage)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(errorMessage);
    Console.ResetColor();
}
