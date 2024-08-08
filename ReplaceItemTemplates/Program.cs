using System.Runtime.InteropServices;
using System.Text;

const string MessageReplaced = "REPLACED";
const string MessageNotFound = "NOT FOUND";

const int SpaceLength = 1;

string classTemplate = File.ReadAllText("Templates/Class.cs");
byte[] classTemplateBytes = Encoding.UTF8.GetBytes(classTemplate);

string interfaceTemplate = File.ReadAllText("Templates/Interface.cs");
byte[] interfaceTemplateBytes = Encoding.UTF8.GetBytes(interfaceTemplate);

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    ContinueUnderWindows();
else
    throw new NotSupportedException("Only OS 'Windows' is supported currently.");

static void WriteReplacedSuccessfully()
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(MessageReplaced);
    Console.ResetColor();
}

static void WriteNotFound()
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(MessageNotFound);
    Console.ResetColor();
}

void ContinueUnderWindows()
{
    string[] interfacePaths =
    [
        @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\ItemTemplates\AspNetCore\Code\1033\Interface\Interface.cs",
        @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\ItemTemplates\CSharp\Code\1033\Interface\Interface.cs",
        @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\ItemTemplates\CSharp\Code\1049\Interface\Interface.cs"
    ];
    string[] classPaths =
    [
        @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\ItemTemplates\AspNetCore\Code\1033\Class\Class.cs",
        @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\ItemTemplates\CSharp\Code\1033\Class\Class.cs",
        @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\ItemTemplates\CSharp\Code\1049\Class\Class.cs"
    ];

    int maxPathLength = new string[][] { interfacePaths, classPaths }
        .SelectMany(static _ => _.Select(static _ => _.Length))
        .Max();
    int maxMessageLength = new string[] { MessageReplaced, MessageNotFound }
        .Select(static _ => _.Length)
        .Max();

    Console.WindowWidth = maxPathLength + SpaceLength + maxMessageLength;

    foreach (string classPath in classPaths)
    {
        Console.Write(classPath);
        Console.SetCursorPosition(maxPathLength + SpaceLength, Console.CursorTop);

        if (File.Exists(classPath))
        {
            using FileStream classFileStream = File.Open(classPath, FileMode.Truncate, FileAccess.Write);
            classFileStream.Write(classTemplateBytes);

            WriteReplacedSuccessfully();
        }
        else
        {
            WriteNotFound();
        }
    }

    foreach (string interfacePath in interfacePaths)
    {
        Console.Write(interfacePath);
        Console.SetCursorPosition(maxPathLength + SpaceLength, Console.CursorTop);

        if (File.Exists(interfacePath))
        {
            using FileStream classFileStream = File.Open(interfacePath, FileMode.Truncate, FileAccess.Write);
            classFileStream.Write(interfaceTemplateBytes);

            WriteReplacedSuccessfully();
        }
        else
        {
            WriteNotFound();
        }
    }
}
