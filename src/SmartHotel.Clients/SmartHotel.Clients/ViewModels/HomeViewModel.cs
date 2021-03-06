﻿using SmartHotel.Clients.Core.Models;
using SmartHotel.Clients.Core.Services.Authentication;
using SmartHotel.Clients.Core.Services.Booking;
using SmartHotel.Clients.Core.Services.Chart;
using SmartHotel.Clients.Core.Services.Notification;
using SmartHotel.Clients.Core.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class HomeViewModel : ViewModelBase, IHandleViewAppearing, IHandleViewDisappearing
    {
        private bool _hasBooking;
        private Microcharts.Chart _temperatureChart;
        private Microcharts.Chart _greenChart;
        private ObservableCollection<Notification> _notifications;

        private readonly INotificationService _notificationService;
        private readonly IChartService _chartService;
        private readonly IBookingService _bookingService;
        private readonly IAuthenticationService _authenticationService;

        public HomeViewModel(
            INotificationService notificationService,
            IChartService chartService,
            IBookingService bookingService,
            IAuthenticationService authenticationService)
        {
            _notificationService = notificationService;
            _chartService = chartService;
            _bookingService = bookingService;
            _authenticationService = authenticationService;
        }

        public bool HasBooking
        {
            get { return _hasBooking; }
            set
            {
                _hasBooking = value;
                OnPropertyChanged();
            }
        }

        public Microcharts.Chart TemperatureChart
        {
            get { return _temperatureChart; }

            set
            {
                _temperatureChart = value;
                OnPropertyChanged();
            }
        }

        public Microcharts.Chart GreenChart
        {
            get { return _greenChart; }

            set
            {
                _greenChart = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Notification> Notifications
        {
            get { return _notifications; }

            set
            {
                _notifications = value;
                OnPropertyChanged();
            }
        }

        public ICommand NotificationsCommand => new Command(OnNotifications);

        public ICommand OpenDoorCommand => new Command(OpenDoorAsync);

        public ICommand BookRoomCommand => new Command(BookRoomAsync);

        public ICommand SuggestionsCommand => new Command(SuggestionsAsync);

        public ICommand BookConferenceCommand => new Command(BookConferenceAsync);

        public ICommand GoMyRoomCommand => new Command(GoMyRoomAsync);

        public override async Task InitializeAsync(object navigationData)
        {
            try
            {
                IsBusy = true;

                HasBooking = AppSettings.HasBooking;

                TemperatureChart = await _chartService.GetTemperatureChartAsync();
                GreenChart = await _chartService.GetGreenChartAsync();

                var authenticatedUser = _authenticationService.AuthenticatedUser;
                var notifications = await _notificationService.GetNotificationsAsync(3, authenticatedUser.Token);
                Notifications = new ObservableCollection<Models.Notification>(notifications);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Home] Error: {ex}");
                await DialogService.ShowAlertAsync(Resources.ExceptionMessage, Resources.ExceptionTitle, Resources.DialogOk);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public Task OnViewAppearingAsync(VisualElement view)
        {
            MessagingCenter.Subscribe<Booking>(this, MessengerKeys.BookingRequested, OnBookingRequested);
            MessagingCenter.Subscribe<CheckoutViewModel>(this, MessengerKeys.CheckoutRequested, OnCheckoutRequested);

            return Task.FromResult(true);
        }

        public Task OnViewDisappearingAsync(VisualElement view)
        {
            return Task.FromResult(true);
        }

        private async void OnNotifications()
        {
            await NavigationService.NavigateToAsync(typeof(NotificationsViewModel), Notifications);
        }

        private async void OpenDoorAsync()
        {
            await NavigationService.NavigateToPopupAsync<OpenDoorViewModel>(true);
        }

        private async void BookRoomAsync()
        {
            await NavigationService.NavigateToAsync<BookingViewModel>();
        }

        private async void SuggestionsAsync()
        {
            await NavigationService.NavigateToAsync<SuggestionsViewModel>();
        }

        private async void BookConferenceAsync()
        {
            await NavigationService.NavigateToAsync<BookingViewModel>();
        }

        private async void GoMyRoomAsync()
        {
            if (HasBooking)
            {
                await NavigationService.NavigateToAsync<MyRoomViewModel>();
            }
        }

        private void OnBookingRequested(Booking booking)
        {
            if (booking == null)
            {
                return;
            }

            HasBooking = true;
        }

        private void OnCheckoutRequested(object args)
        {
            HasBooking = false;
        }
    }
}