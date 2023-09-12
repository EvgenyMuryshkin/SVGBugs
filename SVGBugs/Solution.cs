namespace SVGBugs
{
    public class Solution
    {
        public static string SolutionPath
        {
            get
            {
                var path = Directory.GetCurrentDirectory();

                while (path != null && !Directory.EnumerateFiles(path, "*.sln").Any())
                {
                    path = Path.GetDirectoryName(path);
                }

                return path;
            }
        }
    }
}