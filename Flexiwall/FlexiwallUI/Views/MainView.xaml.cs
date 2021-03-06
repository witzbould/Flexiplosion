﻿using System.Windows;
using System.Windows.Input;
using CommonClassesLib.Model;
using FlexiWallUI.Commands;
using FlexiWallUI.ViewModels;

namespace FlexiWallUI.Views
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private MainViewModel _dataContext;
        private Point _position;
        private int _delta;

        public MainView()
        {
            InitializeComponent();

            _dataContext = DataContext as MainViewModel;
            _position = new Point();
            _delta = 0;

            Log.LogMessage("Application Initialized");

            Closing += delegate { ((MainViewModel)DataContext).SaveSettingsCmd.Execute("Save"); };

            Closed += delegate { Application.Current.Shutdown(); };
        }

        #region UI-Events

        private void StartPushing(object sender, MouseButtonEventArgs e)
        {
            var args = new FlexiWallActionArguments
            {
                Position = e.GetPosition(sender as IInputElement),
                State = FlexiWallActionState.Start,
                Type = FlexiWallActionType.Push
            };
            _dataContext.ActionCmd.Execute(args);
        }

        private void EndPushing(object sender, MouseButtonEventArgs e)
        {
            var args = new FlexiWallActionArguments
            {
                Position = e.GetPosition(sender as IInputElement),
                State = FlexiWallActionState.Stop,
                Type = FlexiWallActionType.Push
            };
            _dataContext.ActionCmd.Execute(args);
        }

        private void StartPulling(object sender, MouseButtonEventArgs e)
        {
            var args = new FlexiWallActionArguments
            {
                Position = e.GetPosition(sender as IInputElement),
                State = FlexiWallActionState.Start,
                Type = FlexiWallActionType.Pull
            };
            _dataContext.ActionCmd.Execute(args);
        }

        private void EndPulling(object sender, MouseButtonEventArgs e)
        {
            var args = new FlexiWallActionArguments
            {
                Position = e.GetPosition(sender as IInputElement),
                State = FlexiWallActionState.Stop,
                Type = FlexiWallActionType.Pull
            };
            _dataContext.ActionCmd.Execute(args);
        }

        private void MoveOnCanvas(object sender, MouseEventArgs e)
        {
            var args = new FlexiWallActionArguments
            {
                Position = e.GetPosition(sender as IInputElement),
                State = FlexiWallActionState.Continue,
                Type = FlexiWallActionType.PushOrPull
            };
            _dataContext.ActionCmd.Execute(args);
        }

        #endregion

        private void MoveWindow(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void FlexiWallMainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            _position = e.GetPosition(sender as IInputElement);
            _dataContext.FlexiWall.RaiseInteractionEvent(_position, 0);
        }

        private void FlexiWallMainWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            _delta = e.Delta;
            _dataContext.FlexiWall.RaiseInteractionEvent(_position, _delta);
        }
    }
}
