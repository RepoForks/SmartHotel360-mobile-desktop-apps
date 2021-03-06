﻿using SmartHotel.Clients.Core.Models;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Settings
{
    public class SettingsService : BaseSettingsLoader<RemoteSettings>, ISettingsService<RemoteSettings>
    {
        public string RemoteFileUrl { get => AppSettings.SettingsFileUrl; set => AppSettings.SettingsFileUrl = value; }

        public Task<RemoteSettings> LoadSettingsAsync()
        {
            RemoteSettings settings = new RemoteSettings();
            settings.Urls.Bookings = AppSettings.BookingEndpoint;
            settings.Urls.Hotels = AppSettings.HotelsEndpoint;
            settings.Urls.Suggestions = AppSettings.SuggestionsEndpoint;
            settings.Urls.Notifications = AppSettings.NotificationsEndpoint;
            settings.Urls.ImagesBaseUri = AppSettings.ImagesBaseUri;
            settings.Tokens.Bingmaps = AppSettings.BingMapsApiKey;
            settings.B2c.Client = AppSettings.B2cClientId;
            settings.B2c.Tenant = AppSettings.B2cTenant;
            settings.B2c.Policy = AppSettings.B2cPolicy;
            settings.Analytics.Android = AppSettings.MobileCenterAnalyticsAndroid;
            settings.Analytics.Ios = AppSettings.MobileCenterAnalyticsIos;
            settings.Analytics.Uwp = AppSettings.MobileCenterAnalyticsWindows;
            settings.Others.FallbackMapsLocation = AppSettings.FallbackMapsLocation;
            settings.Bot.FacebookId = AppSettings.FacebookBotId;
            settings.Bot.SkypeId = AppSettings.SkypeBotId;

            return Task.FromResult(settings);
        }

        public Task PersistRemoteSettingsAsync(RemoteSettings remote)
        {
            AppSettings.BookingEndpoint = remote.Urls.Bookings;
            AppSettings.HotelsEndpoint = remote.Urls.Hotels;
            AppSettings.SuggestionsEndpoint = remote.Urls.Suggestions;
            AppSettings.NotificationsEndpoint = remote.Urls.Notifications;
            AppSettings.ImagesBaseUri = remote.Urls.ImagesBaseUri;
            AppSettings.BingMapsApiKey = remote.Tokens.Bingmaps;
            AppSettings.B2cClientId = remote.B2c.Client;
            AppSettings.B2cTenant = remote.B2c.Tenant;
            AppSettings.B2cPolicy = remote.B2c.Policy;
            AppSettings.MobileCenterAnalyticsAndroid = remote.Analytics.Android;
            AppSettings.MobileCenterAnalyticsIos = remote.Analytics.Ios;
            AppSettings.MobileCenterAnalyticsWindows = remote.Analytics.Uwp;
            AppSettings.FallbackMapsLocation = remote.Others.FallbackMapsLocation;
            AppSettings.FacebookBotId = remote.Bot.FacebookId;
            AppSettings.SkypeBotId = remote.Bot.SkypeId;

            return Task.FromResult(false);
        }
    }
}
