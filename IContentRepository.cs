using Plugins.Shared.UnityMonstackContentLoader.JSON.Converter;
using UnityEngine;

namespace Plugins.Shared.UnityMonstackContentLoader
{
    public interface IContentRepository
    {
        int Count { get; }
        int Priority { get; }
        void Reload();
    }
}