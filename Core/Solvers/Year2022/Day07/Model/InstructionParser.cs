namespace AdventOfCode.Core.Solvers.Year2022.Day07.Model;

internal class InstructionParser
{
    internal static Inode ReadTerminalOutput(string[] lines)
    {
        Inode rootNode = new(null, "/");
        Inode currentDir = rootNode;

        foreach (var line in lines)
        {
            if (line.StartsWith("$ cd"))
            {
                var targetDirectory = line.Split(' ')[2];
                if (targetDirectory == "/")
                {
                    // Skip as the root node has already been created
                    continue;
                }
                else if (targetDirectory == "..")
                {
                    currentDir = currentDir.Parent!;
                }
                else
                {
                    currentDir = currentDir.Children
                        .Single(c => c.Name == targetDirectory);
                }
            }
            else if (line == "$ ls")
            {
                // The next lines will be files or directories
                continue;
            }
            else
            {
                if (line.StartsWith("dir"))
                {
                    var dirName = line.Split(' ')[1];

                    var child = new Inode(currentDir, name: dirName);
                    currentDir.Children.Add(child);
                }
                else
                {
                    var fileParts = line.Split(' ');

                    var child = new Inode(currentDir, name: fileParts[1], size: int.Parse(fileParts[0]));
                    currentDir.Children.Add(child);
                }
            }
        }

        return rootNode;
    }
}
