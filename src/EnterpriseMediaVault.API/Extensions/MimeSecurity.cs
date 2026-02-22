namespace EnterpriseMediaVault.API.Extensions;

public static class MimeSecurity
{
    private static readonly HashSet<string> Allowed =
    [
        "application/pdf",
        "image/jpeg",
        "image/png",
        "image/webp",
        "audio/mpeg",
        "video/mp4",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        "application/vnd.openxmlformats-officedocument.presentationml.presentation",
        "application/zip",
        "application/x-zip-compressed",
        "application/octet-stream"
    ];

    public static bool IsAllowedMime(string mimeType) => Allowed.Contains(mimeType.ToLowerInvariant());
}
