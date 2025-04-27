﻿namespace Helios.Game
{
    public class UserGroup
    {
        #region Properties

        public string Name { get; set; }
        public string InternalName { get; internal set; }
        public int Rank { get; set; }
        public List<string> DefaultPermissions { get; set; }
        public List<string> InheritsGroups { get; set; }
        public List<string> ExcludesPermissions { get; set; }
        public HashSet<string> Permissions { get; set; }

        #endregion

        #region Constructor

        public UserGroup()
        {
            DefaultPermissions = new List<string>();
            InheritsGroups = new List<string>();
            ExcludesPermissions = new List<string>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Build permissions by taking into account the default perms, the inherits and excludes.
        /// </summary>
        public void BuildPermissions(UserGroup[] groups)
        {
            Permissions = [.. DefaultPermissions];

            foreach (var userGroup in groups)
                foreach (var inheritGroup in InheritsGroups)
                {
                    if (inheritGroup == userGroup.InternalName)
                    {
                        foreach (var permission in userGroup.DefaultPermissions)
                            Permissions.Add(permission);
                    }
                }

            foreach (var excludePermission in ExcludesPermissions)
                Permissions.Remove(excludePermission);
        }

        /// <summary>
        /// Get if the group has permission
        /// </summary>
        public bool HasPermission(string permission)
        {
            if (Permissions == null) 
                return false;

            if (Permissions.Contains("root"))
                return true;

            return Permissions.Contains(permission);
        }

        #endregion
    }
}
