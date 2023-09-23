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
    const string BasePathTemplate = @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\ItemTemplates\{0}\Code\1033";
    string[] folders = { "AspNetCore", "CSharp" };

    string classPathTemplate = Path.Combine(BasePathTemplate, "Class", "Class.cs");
    string interfacePathTemplate = Path.Combine(BasePathTemplate, "Interface", "Interface.cs");

    foreach (string folder in folders)
    {
        using (FileStream classFileStream = File.Open(string.Format(classPathTemplate, folder), FileMode.Truncate, FileAccess.Write))
        {
            classFileStream.Write(classTemplateBytes);
        }

        using (FileStream interfaceFileStream = File.Open(string.Format(interfacePathTemplate, folder), FileMode.Truncate, FileAccess.Write))
        {
            interfaceFileStream.Write(interfaceTemplateBytes);
        }
    }
}
