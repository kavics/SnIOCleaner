using SenseNet.IO;

namespace SnIOCleaner;

internal class CleanContent : IContent
{
    private readonly IContent _underlyingContent;
    private readonly List<string> _fieldNames;

    public string[] FieldNames => _fieldNames.ToArray();

    public object this[string fieldName]
    {
        get => _underlyingContent[fieldName];
        set => _underlyingContent[fieldName] = value;
    }

    public string Name
    {
        get => _underlyingContent.Name;
        set => _underlyingContent.Name = value;
    }
    public string Path => _underlyingContent.Path;
    public string Type => _underlyingContent.Type;
    public PermissionInfo Permissions
    {
        get => _underlyingContent.Permissions;
        set => _underlyingContent.Permissions = value;
    }

    public bool IsFolder => true;
    public bool HasData => _underlyingContent.HasData;

    public CleanContent(IContent content)
    {
        _underlyingContent = content;
        _fieldNames = _underlyingContent.FieldNames.ToList();
    }

    public Task<Attachment[]> GetAttachmentsAsync() => _underlyingContent.GetAttachmentsAsync();

    public void RemoveField(params string[] fieldName)
    {
        foreach (var item in fieldName)
            _fieldNames.Remove(item);
    }
}