﻿using Helios.Web.Storage.Models.Effect;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Helios.Web.Storage.Access
{
    public class EffectDao
    {
        /// <summary>
        /// Create items and refresh it with their filled in database ID's
        /// </summary>
        public static void SaveEffects(List<EffectData> items)
        {
            /*using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        foreach (var itemData in items)
                            session.SaveOrUpdate(itemData);

                        transaction.Commit();

                        foreach (var itemData in items)
                            session.Refresh(itemData);
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }*/
        }

        /// <summary>
        /// Get list of all effects for user
        /// </summary>
        public static List<EffectData> GetUserEffects(int avatarId)
        {
            /*using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            {
                return session.QueryOver<EffectData>().Where(x => x.avatarId == avatarId).List() as List<EffectData>;
            }*/
            return new List<EffectData>();
        }

        /// <summary>
        /// Get list of all effects for user
        /// </summary>
        public static List<EffectSettingData> GetEffectSettings()
        {
            /*using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            {
                return session.QueryOver<EffectSettingData>().List() as List<EffectSettingData>;
            }*/
            return new List<EffectSettingData>();
        }

        /// <summary>
        /// Update effect instance
        /// </summary>
        public static void UpdateEffect(EffectData effectData)
        {
            /*using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Update(effectData);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }*/
        }

        /// <summary>
        /// Delete effect
        /// </summary>
        /// <param name="effectId"></param>
        public static void DeleteEffect(EffectData effectData)
        {
            /*using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            {
                session.Query<EffectData>().Where(x => x.EffectId == effectData.EffectId && x.avatarId == effectData.avatarId).Delete();
            }*/
        }
    }
}
