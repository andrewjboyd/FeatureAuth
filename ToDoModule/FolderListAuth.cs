using FeatureAuth;

namespace ToDoModule;

[ClaimType("folderList")]
internal enum FolderListAuth
{
    Read = 1,
}
