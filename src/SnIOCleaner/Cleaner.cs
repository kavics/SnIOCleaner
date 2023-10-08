using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SenseNet.IO;
using SenseNet.IO.Implementations;

namespace SnIOCleaner;

internal class Cleaner
{
    private readonly Arguments _arguments;
    private readonly ILogger<FsWriter> _logger;

    public Cleaner(Arguments arguments, ILogger<FsWriter> logger)
    {
        _arguments = arguments;
        _logger = logger;
    }

    public async Task RunAsync(CancellationToken cancel)
    {
        var reader = new FsReader(Options.Create(new FsReaderArgs { Path = _arguments.SourcePath }));
        var writer = new FsWriter(Options.Create(new FsWriterArgs {Path = _arguments.TargetPath}),
            _logger);

        while (await reader.ReadAllAsync(Array.Empty<string>(), cancel))
        {
            var content = Clean(reader.Content);
            await writer.WriteAsync(reader.RelativePath, content, cancel);
            Console.WriteLine(reader.Content.Path);
        }
    }

    private IContent Clean(IContent readerContent)
    {
        var content = new CleanContent(readerContent);

        foreach (var fieldName in readerContent.FieldNames)
        {
            if (fieldName == "DisplayName" && (string)content[fieldName] == content.Name)
                content.RemoveField(fieldName);
            if (fieldName == "Version" && ((string)content[fieldName]).Equals("V1.0.A", StringComparison.OrdinalIgnoreCase))
                content.RemoveField(fieldName);
            if (_skipEmptyFields.Contains(fieldName) && string.IsNullOrEmpty((string)content[fieldName]))
                content.RemoveField(fieldName);
            if (_unnecessaryFields.Contains(fieldName))
                content.RemoveField(fieldName);
            if (fieldName == "TrashDisabled" && (bool)content[fieldName] == false)
                content.RemoveField(fieldName);
            if (fieldName == "Index" && (long)content[fieldName] == 0)
                content.RemoveField(fieldName);
            if (fieldName == "EnableLifespan" && (bool)content[fieldName] == false)
                content.RemoveField("EnableLifespan", "ValidFrom", "ValidTill");
            if (_skipIsDefaultOption.Contains(fieldName))
                if(content[fieldName] is JArray array)
                    if(array.Count == 1 && array[0].ToString() == "0")
                        content.RemoveField(fieldName);
            if(_arguments.Minimal)
                if (_skipIfMinimal.Contains(fieldName))
                    content.RemoveField(fieldName);
        }
        return content;
    }

    private readonly string[] _unnecessaryFields =
    {
        "Name", "Id", "VersionId", "Type", "Path", "Depth", "IsSystemContent", "IsFolder", "Hidden", "Locked",
        "Approvable", "IsTaggable", "Publishable", "Versions", "Workspace"
    };
    private readonly string[] _skipEmptyFields =
    {
        "BrowseUrl", "Sharing", "SharedWith", "SharedBy", "SharingMode", "SharingLevel"
    };
    private readonly string[] _skipIsDefaultOption =
    {
        "PreviewEnabled", "VersioningMode", "InheritableVersioningMode", "ApprovingMode", "InheritableApprovingMode"
    };
    private readonly string[] _skipIfMinimal =
    {
        "Owner", "CreatedBy", "VersionCreatedBy", "CreationDate", "VersionCreationDate",
        "ModifiedBy", "VersionModifiedBy", "ModificationDate", "VersionModificationDate",
    };
}