# UnityMonstackContentLoader

Loads content from Resources in either JSON or XML format

# Table of Contents

* [Install](#install)
  * [Dependencies](#dependencies)
* [Getting started](#getting-started)
* * [Creating a JSON file](#creating-a-json-file)
* * [Creating a mapping object](#creating-a-mapping-object)
* * [Creating a repository](#creating-a-repository)
* * [Getting entities from repository](#getting-entities-from-repository)

# Install


You can either just put the files into `Assets/Plugins/UnityMonstackContentLoader` or use it as a submodule:
```sh
git submodule add https://github.com/actionk/UnityMonstackContentLoader.git Assets/Plugins/UnityMonstackContentLoader
```

## Dependencies

The library depends on:
* [UnityMonstackCore](https://github.com/actionk/UnityMonstackCore)
* [Newtonsoft JSON.NET](https://www.newtonsoft.com/json)

These are required dependencies

# Getting started

For initializing the files (loading those into the memory), do this on project start or later (as you need it).

```cs
DependencyProvider.Resolve<ContentRepositoryService>().Reload();
```

`Reload` can be executed multiple times to reload the content.

## Creating a JSON file

First of all, create a `Content` folder in `Resources` folder.

Then, create a JSON file in `Resources/Content/` folder. For example:

```json
{
    "entries": [
        {
            "id": 1,
            "title": "Rock",
            "type": "GATHERABLE",
            "gather": {
                "lootId": 1
            },
            "prefab": "Environment/Earth/Prop_Rock_1",
            "geometry": {
                "size": {
                    "x": 2,
                    "y": 2
                }
            },
        }
    ]
}
```

As you can see, the JSON is a simple object with only one field `entries`. This is an array of objects you want to load.

## Creating a mapping object

Create a class that will represent an entity from your JSON:

```cs
[Serializable]
public class ContentObject
{
    public enum Type
    {
        GATHERABLE,
        STORAGE,
        ELECTRICITY_PRODUCER,
        ELECTRICITY_PRESERVER,
        ELECTRICITY_HUB,
        CRAFTING_STATION
    }

    public int id;
    public string title;
    public Type type;

    public Geometry geometry;

    public string prefab;
    
    [Serializable]
    public struct Geometry
    {
        public int2 size;

        public int2 SizeOrDefault => size.Equals(int2.zero) ? new int2(1, 1) : size;

        public static readonly Geometry DEFAULT = new Geometry {size = new int2(1, 1)};
    }
 }
```

As you can see, it can even map embedded objects from JSON. Don't forget to mark your classes with `[Serializable]`.

### Creating a repository

Now we need to create a mapper. Create a injectable class for a repository:

```cs
[Inject]
public class ContentObjectRepository : AbstractJSONContentListRepository<int, ContentObject>
{
    public override FileSourceType FileSource => FileSourceType.Resources;

    public ContentObjectRepository() : base("Content/Objects")
    {
    }

    protected override int GetEntityID(ContentObject entity)
    {
        return entity.id;
    }
}
```

### Getting entities from repository

```cs
var contentObject = DependencyProvider.Resolve<ContentObjectRepository>().GetByKey(objectId);
```

```cs
var contentObjects = DependencyProvider.Resolve<ContentObjectRepository>().GetAll();
```
