﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Signal.database;
using Signal.database.loaders;
using Signal.Model;
using Signal.ViewModel.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextSecure.database;
using Windows.UI.Xaml.Controls;

namespace Signal.ViewModel
{
    public class ThreadViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;

        public ThreadViewModel(IDataService service, INavigationService navService)
        {
            _dataService = service;
            _navigationService = navService;
            Threads = new ThreadCollection(service);

            Messenger.Default.Register<AddThreadMessage>(
                this,
                message =>
                {
                    var thread = DatabaseFactory.getThreadDatabase().Get(message.ThreadId);
                    //SelectedThread = message.NewValue;
                    Threads.Add(thread);
                    Debug.WriteLine("ThreadViewModel got new Thread");
                }
            );
        }

        ObservableCollection<Thread> _Threads;
        public ObservableCollection<Thread> Threads
        {
            get { return _Threads ?? (Threads = new ThreadCollection(_dataService)); }
            set
            {
                _Threads = value;
                RaisePropertyChanged("Threads");
            }
        }

        private RelayCommand _addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new RelayCommand(
                  () =>
                {
                    _navigationService.NavigateTo("DirectoryPageKey");
                   
                },
                    () => true));
            }
        }


        private RelayCommand<Thread> _deleteCommand;
        public RelayCommand<Thread> DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand = new RelayCommand<Thread>(
                  thread =>
                  {
                      Debug.WriteLine($"Should delete {thread.Recipients.ShortString}");

                      DatabaseFactory.getThreadDatabase().DeleteConversation(thread.ThreadId);
                      //deleteConversations(selectedConversations);

                      /*var menuFlyoutItem = sender as MenuFlyoutItem;
                      if (menuFlyoutItem != null)
                      {
                          var thread = menuFlyoutItem.DataContext as Thread;
                          if (thread != null)
                          {
                              Debug.WriteLine($"Should delete {thread.Recipients.ShortString}");

                          }
                      }*/


                  },
                   obj => true));
            }
        }

        private RelayCommand _flyoutCommand;
        public RelayCommand FlyoutCommand
        {
            get
            {
                return _flyoutCommand ?? (_flyoutCommand = new RelayCommand(
                  () =>
                  {
                      Debug.WriteLine($"Show flyout");

                  },
                   () => true));
            }
        }



        /* private void ShowThread(ThreadDatabase.Thread thread)
         {
             if (!SimpleIoc.Default.ContainsCreated<MessageViewModel>("Thread-" + thread._id))
             {
                 SimpleIoc.Default.Register(() => new MessageViewModel(_dataService, _navigationService) { ThreadId = thread._id.Value }, "Thread-" + thread._id, true);
             }

             _navigationService.NavigateTo("Thread-" + thread._id, "test");              
         } 

         public RelayCommand<ThreadDatabase.Thread> NavigateCommand { get; private set; }*/
    }
}
