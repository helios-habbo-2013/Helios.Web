﻿using Helios.Web.Storage.Models.Avatar;
using Helios.Web.Storage.Models.Catalogue;
using System.Collections.Generic;
using System.Linq;

namespace Helios.Web.Storage.Access
{
    public class CurrencyDao
    {
        /// <summary>
        /// Get currency data for user, if doesn't exist, create rows in database for each currency
        /// </summary>
        public static List<CurrencyData> GetCurrencies(int avatarId)
        {
            List<CurrencyData> currencyList = new List<CurrencyData>();

            using (var context = new StorageContext())
            {
                currencyList = context.CurrencyData.Where(x => x.AvatarId == avatarId).ToList();

                if (!currencyList.Any())
                {
                    currencyList = new List<CurrencyData>();
                    currencyList.Add(new CurrencyData { AvatarId = avatarId, SeasonalType = SeasonalCurrencyType.PUMPKINS, Balance = 0 });
                    currencyList.Add(new CurrencyData { AvatarId = avatarId, SeasonalType = SeasonalCurrencyType.PEANUTS, Balance = 0 });
                    currencyList.Add(new CurrencyData { AvatarId = avatarId, SeasonalType = SeasonalCurrencyType.STARS, Balance = 0 });
                    currencyList.Add(new CurrencyData { AvatarId = avatarId, SeasonalType = SeasonalCurrencyType.CLOUDS, Balance = 0 });
                    currencyList.Add(new CurrencyData { AvatarId = avatarId, SeasonalType = SeasonalCurrencyType.DIAMONDS, Balance = 0 });
                    currencyList.Add(new CurrencyData { AvatarId = avatarId, SeasonalType = SeasonalCurrencyType.DUCKETS, Balance = 0 });
                    currencyList.Add(new CurrencyData { AvatarId = avatarId, SeasonalType = SeasonalCurrencyType.LOYALTY_POINTS, Balance = 0 });

                    context.AddRange(currencyList);
                    context.SaveChanges();
                }
            }

            /*using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            {
                CurrencyData currencyDataAlias = null;

                currencyList = session.QueryOver(() => currencyDataAlias)
                    .Where(() => currencyDataAlias.avatarId == avatarId)
                    .List() as List<CurrencyData>;

                if (!currencyList.Any())
                {
                    currencyList = new List<CurrencyData>();
                    currencyList.Add(new CurrencyData { avatarId = avatarId, SeasonalType = SeasonalCurrencyType.PUMPKINS, Balance = 0 });
                    currencyList.Add(new CurrencyData { avatarId = avatarId, SeasonalType = SeasonalCurrencyType.PEANUTS, Balance = 0 });
                    currencyList.Add(new CurrencyData { avatarId = avatarId, SeasonalType = SeasonalCurrencyType.STARS, Balance = 0 });
                    currencyList.Add(new CurrencyData { avatarId = avatarId, SeasonalType = SeasonalCurrencyType.CLOUDS, Balance = 0 });
                    currencyList.Add(new CurrencyData { avatarId = avatarId, SeasonalType = SeasonalCurrencyType.DIAMONDS, Balance = 0 });
                    currencyList.Add(new CurrencyData { avatarId = avatarId, SeasonalType = SeasonalCurrencyType.DUCKETS, Balance = 0 });
                    currencyList.Add(new CurrencyData { avatarId = avatarId, SeasonalType = SeasonalCurrencyType.LOYALTY_POINTS, Balance = 0 });

                    using (var transaction = session.BeginTransaction())
                    {
                        try
                        {
                            foreach (var currencyData in currencyList)
                                session.Save(currencyData);

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            transaction.Rollback();

                        }

                    }
                }
            }*/

            return currencyList;
        }

        /// <summary>
        /// Get singular currency for user straight from database
        /// </summary>
        public static CurrencyData GetCurrency(int avatarId, SeasonalCurrencyType seasonalCurrencyType)
        {
            using (var context = new StorageContext())
            {
                return context.CurrencyData.SingleOrDefault(x => x.AvatarId == avatarId && x.SeasonalType == seasonalCurrencyType);

            }
            /*using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            {
                CurrencyData currencyDataAlias = null;

                return session.QueryOver(() => currencyDataAlias).Where(() => currencyDataAlias.avatarId == avatarId && currencyDataAlias.SeasonalType == seasonalCurrencyType).SingleOrDefault();
            }*/
        }

        /// <summary>
        /// Save all currencies for user
        /// </summary>
        public static void SaveCurrencies(List<CurrencyData> currencyList)
        {
            using (var context = new StorageContext())
            {
                context.CurrencyData.UpdateRange(currencyList);
                context.SaveChanges();

            }

            /*using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        foreach (var currencyData in currencyList)
                            session.Update(currencyData);

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
        /// Retrives adjusts the credits to change from database and saves it again, then returns it
        /// </summary>
        public static int SaveCredits(int avatarId, int creditsChanged)
        {
            using (var context = new StorageContext())
            {
                var avatarData = context.AvatarData.FirstOrDefault(x => x.Id == avatarId);
                avatarData.Credits += creditsChanged;

                context.AvatarData.Update(avatarData);
                context.SaveChanges();

                return context.AvatarData.FirstOrDefault(x => x.Id == avatarId).Credits;
                /*using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
                {
                    session.Query<AvatarData>().Where(x => x.Id == avatarId).Update(x => new AvatarData { Credits = x.Credits + creditsChanged });
                    return session.QueryOver<AvatarData>().Where(x => x.Id == avatarId).Select(x => x.Credits).SingleOrDefault<int>();
                }*/

            }
        }
    }
}
