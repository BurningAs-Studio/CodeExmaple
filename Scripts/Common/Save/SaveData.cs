using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System;

namespace FAS.Save
{
	public static class SaveData
	{
		public static bool IsRegistrationDateSet => PlayerPrefs.HasKey(SaveDataKeys.REGISTRATION_DATE);

		public static event Action OnCurrentLevelChanged;
		public static event Action OnCoinsAmountChanged;
		public static event Action OnNoAdsPurchased;
		
		public static bool HasKey(string key)
		{
			return PlayerPrefs.HasKey(key);
		}
		
		public static void SetInt(string key, int value)
		{
			PlayerPrefs.SetInt(key, value);
		}
		
		public static int GetInt(string key, int defaultValue = 0)
		{
			return PlayerPrefs.GetInt(key, defaultValue);
		}
		
		public static void SetDateTime(string key, DateTime date)
		{
			PlayerPrefs.SetString(key, date.ToString("o"));
		}
		
		public static DateTime GetDateTime(string key, DateTime defaultValue)
		{
			var dateString = PlayerPrefs.GetString(key, string.Empty);
			return string.IsNullOrEmpty(dateString) ? defaultValue :
				DateTime.Parse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
		}
		
		public static DateTime GetLastRewardDate(string productId)
		{
			var key = $"{SaveDataKeys.LAST_REWARD_DATE_PREFIX}{productId}";
			return GetDateTime(key, DateTime.MinValue);
		}
		
		public static void SetLastRewardDate(string productId, DateTime date)
		{
			var key = $"{SaveDataKeys.LAST_REWARD_DATE_PREFIX}{productId}";
			SetDateTime(key, date);
		}
		
		public static void SetPurchasedStatus(string productId, bool isPurchased)
		{
			var key = $"{SaveDataKeys.PURCHASED_STATUS_PREFIX}{productId}";
			SetInt(key, isPurchased ? 1 : 0);
		}
		
		public static int RewardVideoWatched
		{
			get => PlayerPrefs.GetInt(SaveDataKeys.REWARD_VIDEO_WATCHED, 0);
			set
			{
				PlayerPrefs.SetInt(SaveDataKeys.REWARD_VIDEO_WATCHED, value);
				//AnalyticEvents.CheckAndSendRewardEvent(value);
			}
		}
		
		public static int CoinsAmount
		{
			get => PlayerPrefs.GetInt(SaveDataKeys.COINS_AMOUNT, SaveDataKeys.DEFAULT_COINS_AMOUNT);
			set
			{
				PlayerPrefs.SetInt(SaveDataKeys.COINS_AMOUNT, value);
				OnCoinsAmountChanged?.Invoke();
			}
		}
		
		public static bool IsProductPurchasedLocally(string productId)
		{
			var key = $"{SaveDataKeys.PURCHASED_STATUS_PREFIX}{productId}";
			return GetInt(key, 0) == 1;
		}
		
		public static DateTime RegistrationDate
		{
			get
			{
				var dateString = PlayerPrefs.GetString(SaveDataKeys.REGISTRATION_DATE, string.Empty);
				return string.IsNullOrEmpty(dateString) ? DateTime.MinValue :
					DateTime.Parse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
			}
			set => PlayerPrefs.SetString(SaveDataKeys.REGISTRATION_DATE, value.ToString("o"));
		}
		
		public static bool IsAppleATTWindowShown
		{
			get => PlayerPrefs.GetInt(SaveDataKeys.IS_ATT_WINDOW_SHOWN, 0) > 0;
			set => PlayerPrefs.SetInt(SaveDataKeys.IS_ATT_WINDOW_SHOWN, value ? 1 : 0);
		}
		
		public static bool IsGoogleCmpShown
		{
			get => PlayerPrefs.GetInt(SaveDataKeys.IS_CMP_WINDOW_SHOWN, 0) > 0;
			set => PlayerPrefs.SetInt(SaveDataKeys.IS_CMP_WINDOW_SHOWN, value ? 1 : 0);
		}
		
		public static int TotalPlayTimeInMinutes
		{
			get => PlayerPrefs.GetInt(SaveDataKeys.TOTAL_PLAY_TIME_MINUTES, 0);
			set => PlayerPrefs.SetInt(SaveDataKeys.TOTAL_PLAY_TIME_MINUTES, value);
		}
		
		public static int LastRetentionDaySent
		{
			get => PlayerPrefs.GetInt(SaveDataKeys.LAST_RETENTION_DAY_SENT, 0);
			set => PlayerPrefs.SetInt(SaveDataKeys.LAST_RETENTION_DAY_SENT, value);
		}
		
		public static int CurrentLevel
		{
			get => PlayerPrefs.GetInt(SaveDataKeys.CURRENT_LEVEL, SaveDataKeys.START_LEVEL_INDEX);
			set
			{
				PlayerPrefs.SetInt(SaveDataKeys.CURRENT_LEVEL, value);
				OnCurrentLevelChanged?.Invoke();
			}
		}
		
		public static int DaysSinceRegistration
		{
			get => PlayerPrefs.GetInt(SaveDataKeys.DAYS_SINCE_REGISTRATION, 0);
			set => PlayerPrefs.SetInt(SaveDataKeys.DAYS_SINCE_REGISTRATION, value);
		}
		
		public static int CurrentSession
		{
			get => PlayerPrefs.GetInt(SaveDataKeys.CURRENT_SESSION, 0);
			set => PlayerPrefs.SetInt(SaveDataKeys.CURRENT_SESSION, value);
		}

		public static bool IsNoAdsPurchased
		{
			get => PlayerPrefs.GetInt(SaveDataKeys.NO_ADS_PURCHASE_STATE, 0) == 1;
			set
			{
				PlayerPrefs.SetInt(SaveDataKeys.NO_ADS_PURCHASE_STATE, value ? 1 : 0);
				OnNoAdsPurchased?.Invoke();
			}
		}
		
		public static int InterstitialVideoWatched
		{
			get => PlayerPrefs.GetInt(SaveDataKeys.INTERSTITIAL_VIDEO_WATCHED, 0);
			set
			{
				PlayerPrefs.SetInt(SaveDataKeys.INTERSTITIAL_VIDEO_WATCHED, value);
				//AnalyticEvents.CheckAndSendInterstitialEvent(value);
			}
		}
		
		public static void SaveTriggeredEvents(string key, HashSet<string> triggeredEvents)
		{
			var eventsString = string.Join(",", triggeredEvents);
			PlayerPrefs.SetString(key, eventsString);
		}
		
		private static HashSet<string> LoadTriggeredEvents(string key)
		{
			var eventsString = PlayerPrefs.GetString(key, string.Empty);
			var eventsArray = eventsString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			return new HashSet<string>(eventsArray);
		}

		public static HashSet<string> LoadTriggeredRewardEvents()
		{
			return LoadTriggeredEvents(SaveDataKeys.TRIGGERED_REWARD_EVENTS);
		}

		public static HashSet<string> LoadTriggeredInterstitialEvents()
		{
			return LoadTriggeredEvents(SaveDataKeys.TRIGGERED_INTERSTITIAL_EVENTS);
		}

		public static HashSet<string> LoadTriggeredTimeSpendEvents()
		{
			return LoadTriggeredEvents(SaveDataKeys.TRIGGERED_TIME_SPEND_EVENTS);
		}
	}
}