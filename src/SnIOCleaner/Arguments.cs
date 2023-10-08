namespace SnIOCleaner;

internal class Arguments
{
    public string SourcePath { get; set; }
    public string TargetPath { get; set; }

    public static Arguments Parse(string[] args)
    {
        if (args.Length == 0)
            throw new ArgumentException("Invalid args: source path is required.");

        var targetPath = Path.GetDirectoryName(args[0]);

        if (args.Length == 3)
        {
            if (args[1].Equals("--target", StringComparison.OrdinalIgnoreCase) ||
                args[1].Equals("-t", StringComparison.OrdinalIgnoreCase))
            {
                targetPath = args[2];
            }
        }

        return new Arguments {SourcePath = args[0], TargetPath = targetPath };
    }

}