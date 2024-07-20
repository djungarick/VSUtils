using System.Runtime.InteropServices;
using System.Text;

string classTemplate = File.ReadAllText("Templates/Class.cs");
byte[] classTemplateBytes = Encoding.UTF8.GetBytes(classTemplate);

string interfaceTemplate = File.ReadAllText("Templates/Interface.cs");
byte[] interfaceTemplateBytes = Encoding.UTF8.GetBytes(interfaceTemplate);

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    ContinueUnderWindows();
else
    throw new NotSupportedException("Only OS 'Windows' is supported currently.");

void ContinueUnderWindows()
{
    string[] interfacePaths =
    {
        @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\ItemTemplates\AspNetCore\Code\1033\Interface\Interface.cs",
        @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\ItemTemplates\CSharp\Code\1033\Interface\Interface.cs"
    };
    string[] classPaths =
    {
        @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\ItemTemplates\AspNetCore\Code\1033\Class\Class.cs",
        @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\ItemTemplates\CSharp\Code\1033\Class\Class.cs"
    };

    foreach (string classPath in classPaths)
    {
        using FileStream classFileStream = File.Open(classPath, FileMode.Truncate, FileAccess.Write);
        classFileStream.Write(classTemplateBytes);
    }

    foreach (string interfacePath in interfacePaths)
    {
        using FileStream classFileStream = File.Open(interfacePath, FileMode.Truncate, FileAccess.Write);
        classFileStream.Write(interfaceTemplateBytes);
    }
}
