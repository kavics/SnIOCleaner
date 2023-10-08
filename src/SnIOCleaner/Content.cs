//using Newtonsoft.Json;

//namespace SnIOCleaner;

//// Copied from SnIO

//public class PermissionInfo
//{
//    public bool IsInherited { get; set; }
//    public AceInfo[] Entries { get; set; }
//}

//public class AceInfo
//{
//    public string Identity { get; set; }
//    public bool LocalOnly { get; set; }
//    public Dictionary<string, string> Permissions { get; set; }
//}

//public class Attachment
//{
//    /// <summary>
//    /// Gets or sets the name of the field that stores the stream.
//    /// </summary>
//    public string FieldName { get; set; }
//    /// <summary>
//    /// Gets or sets the original file name (without any path segment)
//    /// </summary>
//    public string FileName { get; set; }
//    /// <summary>
//    /// Gets or sets the mime type
//    /// </summary>
//    public string ContentType { get; set; }
//    /// <summary>
//    /// Gets or sets the stream
//    /// </summary>
//    public Stream Stream { get; set; }
//}

//public interface IContent
//{
//    string[] FieldNames { get; }
//    object this[string fieldName] { get; set; }

//    public string Name { get; set; }
//    public string Path { get; }
//    public string Type { get; }
//    public PermissionInfo Permissions { get; set; }

//    public bool IsFolder { get; }
//    public bool HasData { get; }

//    Task<Attachment[]> GetAttachmentsAsync();
//}

//internal class Content
//{
//    private string ToJson(IContent content)
//    {
//        var fields = content.FieldNames
//            .Where(fieldName => content[fieldName] != null)
//            .ToDictionary(fieldName => fieldName, fieldName => content[fieldName]);

//        var model = new
//        {
//            ContentType = content.Type,
//            ContentName = content.Name,
//            Fields = fields,
//            Permissions = content.Permissions
//        };

//        var writer = new StringWriter();
//        JsonSerializer.Create(new JsonSerializerSettings
//        {
//            Formatting = Formatting.Indented,
//            NullValueHandling = NullValueHandling.Ignore
//        }).Serialize(writer, model);

//        return writer.GetStringBuilder().ToString();
//    }

//}