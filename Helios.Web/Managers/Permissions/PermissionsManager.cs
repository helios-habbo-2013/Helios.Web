using YamlDotNet.Serialization;

namespace Helios.Game
{
    public class PermissionsManager
    {
        #region Properties

        public Dictionary<int, UserGroup> Ranks;

        public UserGroup this[int index]
        {
            get
            {
                if (this.Ranks.TryGetValue(index, out UserGroup? value))
                {
                    return value;
                }
                else
                {
                    return new UserGroup();
                }
            }
        }

        #endregion

        #region Constructors

        public PermissionsManager()
        {
            {
                Ranks = [];

                var input = new StringReader(File.ReadAllText("permissions.yml"));
                var deserializer = new DeserializerBuilder().Build();

                var config = deserializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, dynamic>>>>(input);
                var groups = config["groups"];

                foreach (var kvp in groups)
                {
                    var groupName = kvp.Key;
                    var groupData = kvp.Value;

                    var rank = int.Parse(groupData["rank"]);
                    var userGroup = new UserGroup
                    {
                        Name = groupData["name"],
                        InternalName = kvp.Key,
                        Rank = rank,
                    };


                    if (groupData.ContainsKey("permissions"))
                    {
                        var permissions = groupData["permissions"];

                        if (permissions != null)
                        {
                            foreach (var permission in permissions)
                                userGroup.DefaultPermissions.Add(permission);
                        }
                    }

                    if (groupData.ContainsKey("inherits"))
                    {
                        var inherits = groupData["inherits"];

                        if (inherits != null)
                        {
                            foreach (var inheritsGroup in inherits)
                                userGroup.InheritsGroups.Add(inheritsGroup);
                        }
                    }

                    if (groupData.ContainsKey("excludes"))
                    {
                        var excludes = groupData["excludes"]; ;

                        if (excludes != null)
                        {
                            foreach (var excludesGroup in excludes)
                                userGroup.ExcludesPermissions.Add(excludesGroup);
                        }
                    }

                    Ranks[rank] = userGroup;
                }

                foreach (var rank in Ranks.Values.ToArray())
                {
                    rank.BuildPermissions([.. Ranks.Values]);
                }

                //Log.ForContext<PermissionsManager>().Information("Loaded {Count} of Ranks", Ranks.Count);
            }

            #endregion

            #region Methods



            #endregion
        }
    }
}
