using System.Runtime.InteropServices;
using System.Text;

const string ClassTemplate = """
    namespace $rootnamespace$;

    internal sealed class $safeitemrootname$
    {
        
    }

    """;
const string InterfaceTemplate = """
    namespace $rootnamespace$;

    public interface $safeitemrootname$
    {
        
    }

    """;

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    ContinueUnderWindows();
else
    throw new NotSupportedException("Only OS 'Windows' is supported currently.");

static void ContinueUnderWindows()
{
    const string BasePathTemplate = @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\ItemTemplates\{0}\Code\1033";
    string[] folders = { "AspNetCore", "CSharp" };

    string classPathTemplate = Path.Combine(BasePathTemplate, "Class", "Class.cs");
    string interfacePathTemplate = Path.Combine(BasePathTemplate, "Interface", "Interface.cs");

    foreach (string folder in folders)
    {
        using (FileStream classFileStream = File.Open(string.Format(classPathTemplate, folder), FileMode.Truncate, FileAccess.Write))
        {
            classFileStream.Write(Encoding.UTF8.GetBytes(ClassTemplate));
        }

        using (FileStream interfaceFileStream = File.Open(string.Format(interfacePathTemplate, folder), FileMode.Truncate, FileAccess.Write))
        {
            interfaceFileStream.Write(Encoding.UTF8.GetBytes(InterfaceTemplate));
        }
    }
}
