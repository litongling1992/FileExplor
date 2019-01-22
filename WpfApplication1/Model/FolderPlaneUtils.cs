using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;

namespace WpfApplication1.Model
{
    public static class FolderPlaneUtils
    {
        public static bool IsFolder(string path)
        {
            bool isFolder;
            isFolder = System.IO.Directory.Exists(path);
            return isFolder;
        }

        public static bool IsDrive(string path)
        {
            bool isDrive = false;
            // path here X: ; str X:// 
            foreach (string str in Directory.GetLogicalDrives())
            {
                if (str.Contains(path)) { isDrive = true; }
            }
            return isDrive;
        }

        public static bool IsLink(string path)
        {
            bool isLink = false;

            string ext = Path.GetExtension(path);
            ext.ToLower();

            isLink = (ext == ".lnk");
            return isLink;
        }


        public static bool hasWriteAccessToFolder(string folderPath)
        {
            // from http://stackoverflow.com/questions/1410127/c-sharp-test-if-user-has-write-access-to-a-folder

            try
            {
                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do not have access to view the permissions. 
                System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(folderPath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public static string ResolveIfShortCut(string path)
        {
            if (path == "") return "";
            if (FolderPlaneUtils.IsLink(path))
            {
                // First working solution chosen to resolve link
                // For using IWshRuntimeLibrary add reference, Com tab, choose Microsoft Shell Controls and Automation
                // Question: must we dispose shell, I assume this is a Managed wrapper around com
                // question: does this also works on non W7 systems?

                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                IWshRuntimeLibrary.IWshShortcut link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(path);

                string target = link.TargetPath;
                return target;
            }

            // If not found, keep original path
            return path;
        }

        public static string FolderUp(string path)
        {
            string[] folders = path.Split('\\');

            if (folders.Length <= 1)
            {
                // the sky is the limit
                return path;
            }
            else
            {
                // Remove from path tailer = "//" + folders[folders.Length - 1]
                // for (int i = 0; i <= folders.Length - 3; i++) { result = result + folders[i] + "\\"; }
                // result = result + folders[folders.Length - 2];

                string result = path.Remove(path.Length - (2 + folders[folders.Length - 1].Length) + 1);
                return result;
            }
        }


        // Some hack/heuristics to some word wrapping/insert newlines in Friendlyname, about max 3 lines
        // Note: Another trick to limit width Tabs might be MaxWidth dependant selection/hoover in View
        // to do: some testing or better: make a decent Algorithm, try all/more posibilities and find "optimal" solution 
        // according a criterium

        public static string MyShortFriendlyName(string text)
        {
            String str = "";
            // 1) Split the text, refinement: also possible on aA, a1, use dates as one word etc.
            string[] words = text.Split(new char[] { ' ', '-', '_', '.' });

            // 2) Heuristically compute Nr lines and optimal NrCharLine
            // Estimator max 3 lines however given the current algorithm we might get aditional lines
            int NrChar = text.Length;
            int NrLines = (NrChar <= 10) ? NrLines = 1 : (NrChar <= 40) ? NrLines = 2 : NrLines = 3; //so not Delphi
            int NrCharLine = NrChar / NrLines;

            // refinement: If really long and no splits force split and/or work with dots ....

            if ((NrLines == 1) || (words.Count() == 1)) return text;


            // some administration, indices [0..L-1] 
            int indexInTextSeparator = -1;
            int startInLineCurrentWord = 0;
            int endInLineCurrentWord = 0;
            int lengthWord = 0;

            // Add word for word to a line, use a test to determine to add the word to the current or to a new line
            for (int iWord = 0; iWord <= words.Length - 1; iWord++)
            {
                lengthWord = words[iWord].Length;
                endInLineCurrentWord = startInLineCurrentWord + lengthWord - 1;

                // 3 Examine all words, add to new line under certain conditions 
                // refinement: best fit || ( NrCharLine-nrCurrent< nrNext-NrCharLine)
                if (((endInLineCurrentWord + 1 <= 1.05 * (NrCharLine)) || (startInLineCurrentWord <= 0.2 * NrCharLine)))
                {
                    // add to oldline 
                    if ((iWord != 0) && (indexInTextSeparator < text.Length - 1))
                    {
                        str = str + text[indexInTextSeparator];
                    };
                    str = str + words[iWord];
                    startInLineCurrentWord = endInLineCurrentWord + 1;
                }
                else
                {
                    //add to newline (space replaced by NewLine)
                    str = str + System.Environment.NewLine + words[iWord];
                    startInLineCurrentWord = lengthWord;
                }

                indexInTextSeparator = indexInTextSeparator + (lengthWord + 1);
            }

            return str;
        }
    }

}
